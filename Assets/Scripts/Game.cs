using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
	public bool DoneInit { get; private set; }

	[Header("Time")]
	[SerializeField] private float dayLengthInSeconds;
	[SerializeField] private float nightLengthInSeconds;
	[SerializeField] private int startOfficeHour;
	[SerializeField] private int endOfficeHour;
	[SerializeField] private int speedChangeDelay;

	private static readonly DateTime foundingDate = new(2017, 3, 6, 8, 0, 0);

	public DateTime Time { get; private set; }
	public bool IsOfficeTime => (Time.Hour >= startOfficeHour && Time.Hour < endOfficeHour);
	public DateTime StartOfficeTime => Time.Subtract(Time.TimeOfDay).Add(TimeSpan.FromHours(startOfficeHour));
	public DateTime EndOfficeTime => Time.Subtract(Time.TimeOfDay).Add(TimeSpan.FromHours(endOfficeHour));
	public TimeSpan DailyOfficeTime => EndOfficeTime - StartOfficeTime;

	public TimeSpan CompanyAge => Time.Subtract(foundingDate);

	private bool skipToNextDay;


	[Header("Money")]
	private int money;
	public int Money
	{
		get => money;
		set
		{
			moneyText.text = value + " $";
			money = value;
		}
	}
	private float reputation = 1f;
	public float Reputation
	{
		get => reputation;
		private set
		{
			reputation = Mathf.Clamp(value, 1f, 10f); // from 1 - 10
			ReputationStars.i.UpdateStars();
		}
	}

	// [Header("Orders")]
	public List<Order> CurrentOrders { get; private set; }
	public List<Order> AvailableOrders { get; private set; }

	// [Header("Employees")]
	public List<Employee> Employees { get; private set; }
	public List<Employee> JobApplications { get; private set; }

	public int DesksOwned { get; private set; }

	[Header("Office")]
	public OfficeManager officeManager;
	public OfficeType OfficeType => officeManager.officeType;

	private int currentOffice;
	public int[] officePrices;

	public int NextOfficePrice => officePrices[currentOffice + 1];

	[Header("UI")]
	[SerializeField] private TMP_Text timeText;
	[SerializeField] private TMP_Text dateText;
	[SerializeField] private TMP_Text moneyText;
	[SerializeField] private TMP_Text currentOrdersText;

	[Header("Icon Buttons")]
	[SerializeField] private IconButton hirePanelButton;
	[SerializeField] private IconButton employeesPanelButton;
	[SerializeField] private IconButton officePanelButton;
	[SerializeField] private IconButton incommingOrdersPanelButton;
	[SerializeField] private IconButton currentOrdersPanelButton;


	[Header("Other")]
	public string companyName;

	public static Game i;

	[Header("Incoming Requests")]
	private int nextOrderChance;
	private int nextJobApplicationChance;

	[SerializeField] private int jobApplicationFrequency;
	[SerializeField] private int orderFrequency;

	void Awake()
	{
		DoneInit = false;

		if (i == null) i = this;
		DontDestroyOnLoad(this);

		SkillInfo.Init();

		Time = foundingDate;
		Money = 10000;
		currentOffice = 0;

		Employees = new();

		var you = Employee.You;
		you.salary = you.requestedSalary;
		you.employedSince = Time;

		Employees.Add(you);

		JobApplications = new List<Employee>
		{
			Employee.Generate(),
			Employee.Generate(),
			Employee.Generate(),
		};

		AvailableOrders = new() { new Order(OrderList.list[0]) };

		AvailableOrders.Add(Order.Generate());
		AvailableOrders.Add(Order.Generate());

		CurrentOrders = new List<Order>
		{

		};

		nextJobApplicationChance = jobApplicationFrequency;
		nextOrderChance = orderFrequency;

		SetTimeText();
		SetDateText();
	}

	void Start()
	{
		DoneInit = true;
	}

	void Update()
	{
		// TIME
		var previousTime = Time;

		var daySpeed = (endOfficeHour - startOfficeHour) * 60 * 60 / dayLengthInSeconds;
		var nightSpeed = (24 - (endOfficeHour - startOfficeHour)) * 60 * 60 / nightLengthInSeconds;


		// Update Time
		var isDay = Time > StartOfficeTime.Subtract(TimeSpan.FromMinutes(speedChangeDelay)) && Time < EndOfficeTime.Add(TimeSpan.FromMinutes(speedChangeDelay));

		float timeSpeed;
		if (!isDay || skipToNextDay)
		{
			timeSpeed = nightSpeed;
			if (!isDay) skipToNextDay = false;
		}
		else
			timeSpeed = daySpeed;

		Time = Time.AddSeconds(UnityEngine.Time.deltaTime * timeSpeed);
		if (Time.DayOfWeek == DayOfWeek.Saturday) Time = Time.AddDays(2);

		// Do stuff if time changes
		SetTimeText();

		if (previousTime.Day < Time.Day) DayPassed();
		if (previousTime.Month < Time.Month) MonthPassed();
		if (previousTime.Year < Time.Year) YearPassed();

		// Employees
		foreach (var employee in Employees)
		{
			if (!employee.hasDesk)
			{
				var employeeSpaces = officeManager.desks.Where(g => g.activeSelf).Select(g => g.GetComponentInChildren<EmployeeRender>()).ToArray();

				var emptySpace = employeeSpaces.FirstOrDefault(e => e.employee == null);
				if (emptySpace != null)
				{
					employee.hasDesk = true;
					emptySpace.employee = employee;
					employee.employeeRender = emptySpace;
				}
			}
		}

		// Orders
		if (IsOfficeTime)
		{
			var workedTime = TimeSpan.FromSeconds(UnityEngine.Time.deltaTime * timeSpeed);

			foreach (var order in new List<Order>(CurrentOrders))
			{
				var workedPoints = (float)workedTime.TotalHours * order.GetWorkingSpeed();
				order.workedPoints += workedPoints;

				if (order.workedPoints > 0) order.timeSpent += workedTime;

				if (order.Completion > 1f)
				{
					CompleteOrder(order);
				}
			}
		}

		SetOrderText();
	}

	private void DayPassed()
	{
		SetDateText();

		if (Random.Range(0, nextOrderChance) == 0)
		{
			AvailableOrders.Add(Order.Generate());
			nextOrderChance = orderFrequency;
			incommingOrdersPanelButton.EnableDot();
		}
		if (Random.Range(0, nextJobApplicationChance) == 0)
		{
			JobApplications.Add(Employee.Generate());
			nextJobApplicationChance = jobApplicationFrequency;
			hirePanelButton.EnableDot();
		}

		AvailableOrdersPanel.i.UpdatePanel();
		HirePanel.i.UpdatePanel();

		nextOrderChance--;
		nextJobApplicationChance--;

		foreach (var employee in Employees)
		{
			Money -= employee.salary / 22;
			employee.requestedSalary += 1;
		}

		foreach (var employee in new List<Employee>(Employees))
		{
			if (employee.Happiness < Multipliers.i.minHappiness)
			{
				Fire(employee);
				reputation -= .5f;

				PopupManager.i.AddPopup(new Popup(employee.name_ + " quit because of low happiness.", () => EmployeesPanel.i.gameObject.SetActive(true)));
				employeesPanelButton.EnableDot();
			}
		}
	}

	private void YearPassed()
	{

	}

	private void MonthPassed()
	{

	}

	public void TakeOrder(Order order)
	{
		AvailableOrders.Remove(order);
		CurrentOrders.Add(order);

		CurrentOrdersPanel.i.UpdatePanel();
		AvailableOrdersPanel.i.UpdatePanel();
	}

	public void CompleteOrder(Order order)
	{
		CurrentOrders.Remove(order);
		CurrentOrdersPanel.i.UpdatePanel();

		if (order.Completion < 1f)
		{
			Money -= 100;

			Reputation -= .1f;

			return;
		}

		var daysOverDeadline = (order.deadline > Time) ? 0f : (float)(order.deadline - Time).TotalDays;
		var timeFee = Mathf.RoundToInt(Mathf.Ceil(daysOverDeadline) * order.difficultyMultiplier * -100);

		var moneyChanges = new MoneyChange[]
		{
			new MoneyChange { amount = order.Payment, cause = "Payment" },
			new MoneyChange { amount = timeFee, cause = "Deadline" },
		};

		float reputationChange = 0;
		reputationChange -= daysOverDeadline * 0.05f;
		reputationChange += .5f;

		OrderFinishedPanel.i.OpenPanel(order, reputationChange, moneyChanges);

		Money += moneyChanges.Sum(ch => ch.amount);
		Reputation += reputationChange;
	}

	public void Hire(Employee employee)
	{
		if (DesksOwned <= Employees.Count) return;

		employee.salary = employee.requestedSalary;
		employee.employedSince = Time;

		JobApplications.Remove(employee);
		HirePanel.i.UpdatePanel();

		Employees.Add(employee);
		EmployeesPanel.i.UpdatePanel();
	}

	public void Reject(Employee employee)
	{
		JobApplications.Remove(employee);
		HirePanel.i.UpdatePanel();
	}

	public void Fire(Employee employee)
	{
		if (employee.name_ == "You") return;

		employee.employeeRender.employee = null;
		employee.assignedOrder?.assignedEmployees.Remove(employee);

		Employees.Remove(employee);
		EmployeesPanel.i.UpdatePanel();
	}

	public void BuyNewOffice()
	{
		if (currentOffice + 1 >= officePrices.Length) return;
		if (Money < NextOfficePrice) return;

		Money -= NextOfficePrice;
		currentOffice++;

		foreach (var employee in Employees)
		{
			employee.hasDesk = false;
			employee.employeeRender = null;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void OfficeIsLoaded()
	{
		DesksOwned = 0;
		BuyDeskFree();
		
		OfficeManagementPanel.i.gameObject.SetActive(false);
	}
 
	public void BuyDesk()
	{
		var price = 100;
		if (Money < price) return;
		if (officeManager.desks.Length <= DesksOwned) return;

		Money -= price;
		BuyDeskFree();
	}
	public void BuyDeskFree()
	{
		if (officeManager.desks.Length <= DesksOwned) return;

		DesksOwned++;

		foreach (var desk in officeManager.desks)
		{
			if (desk.activeSelf) continue;

			desk.SetActive(true);
			return;
		}

		throw new Exception("more desks active than bought");
	}

	private void SetTimeText()
	{
		/* Format:
		 * <mspace=mspace=38>13:39</mspace>
		 */

		var text = "<mspace=mspace=38>" + Time.ToString("HH:") + TimeSpan.FromMinutes(Time.Minute / 15 * 15).ToString("mm") + "</mspace>";

		timeText.text = text;
	}

	private void SetDateText()
	{
		/* Format:
		 * November 14<sup>th</sup>
		 * 2020
		 * Friday
		 */

		var text = "";
		text += Time.ToString("MMMM d") + "<sup>th</sup>";
		text += "\n" + Time.Year;
		text += "\n" + Time.DayOfWeek.ToString();

		dateText.text = text;
	}

	private void SetOrderText()
	{
		var text = "";

		foreach (var order in CurrentOrders)
		{
			if (text != "") text += "\n";

			var workingSpeed = order.GetWorkingSpeed();

			var remainingPoint = order.orderDescription.workPoints - order.workedPoints;
			var remainingHours = remainingPoint / workingSpeed;

			text += Math.Floor(order.Completion * 100) + "% - " + order.orderDescription.name +
				" - Time left: " + (remainingHours == float.PositiveInfinity ? "N/A" : Mathf.RoundToInt(remainingHours) + " hr");
		}

		currentOrdersText.text = text;
	}

	public void SkipToNextDay()
	{
		skipToNextDay = !skipToNextDay;
	}
}

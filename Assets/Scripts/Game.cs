using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
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
	public float Reputation { get => reputation; private set { reputation = Mathf.Clamp(value, 1f, 10f); } } // from 1-10

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

	[Space(5)]
	[SerializeField] private GameObject deskContainer;

	[Header("UI")]
	[SerializeField] private TMP_Text timeText;
	[SerializeField] private TMP_Text dateText;
	[SerializeField] private TMP_Text moneyText;
	[SerializeField] private TMP_Text currentOrdersText;

	public string companyName;

	public static Game i;

	[Header("Incoming Requests")]
	private int nextOrderChance;
	private int nextJobApplicationChance;

	[SerializeField] private int jobApplicationFrequency;
	[SerializeField] private int orderFrequency;

	void Awake()
	{
		if (i == null) i = this;

		DontDestroyOnLoad(this);

		Time = foundingDate;
		DesksOwned = 0;
		Money = 10000;

		Employees = new List<Employee>
		{
			Employee.You
		};

		JobApplications = new List<Employee>
		{
			Employee.Generate(),
			Employee.Generate(),
			Employee.Generate(),
		};

		BuyDeskFree();

		AvailableOrders = new List<Order>
		{
			new Order(OrderList.list[0]),
			Order.Generate(),
			Order.Generate(),
		};

		CurrentOrders = new List<Order>
		{

		};

		nextJobApplicationChance = jobApplicationFrequency;
		nextOrderChance = orderFrequency;
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
				var employeeSpaces = deskContainer.GetComponentsInChildren<EmployeeRender>();

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
		}
		if (Random.Range(0, nextJobApplicationChance) == 0)
		{
			JobApplications.Add(Employee.Generate());
			nextJobApplicationChance = jobApplicationFrequency;
		}

		AvailableOrdersPanel.i.UpdatePanel();
		HirePanel.i.UpdatePanel();

		nextOrderChance--;
		nextJobApplicationChance--;
	}

	private void YearPassed()
	{

	}

	private void MonthPassed()
	{
		foreach (var employee in Employees)
		{
			Money -= employee.salary;
		}
	}

	public void TakeOrder(Order order)
	{
		AvailableOrders.Remove(order);
		CurrentOrders.Add(order);

		AvailableOrdersPanel.i.UpdatePanel();
	}

	private void CompleteOrder(Order order)
	{
		if (order.Completion < 1f)
		{
			Money -= 100;
			CurrentOrders.Remove(order);

			Reputation -= .1f;

			return;
		}

		var timeFee = (order.deadline > Time) ? 0f : (float)(order.deadline - Time).TotalDays;

		Reputation -= timeFee * 0.05f;
		Reputation += .5f;

		Money += order.Payment + (int)timeFee * -100;

		CurrentOrders.Remove(order);
	}

	public void Hire(Employee employee)
	{
		if (DesksOwned <= Employees.Count) return;

		JobApplications.Remove(employee);
		HirePanel.i.UpdatePanel();

		Employees.Add(employee);
		EmployeesPanel.i.UpdatePanel();
	}

	public void Fire(Employee employee)
	{
		employee.employeeRender.employee = null;

		Employees.Remove(employee);
		EmployeesPanel.i.UpdatePanel();
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

		var text = "<mspace=mspace=38>" + Time.ToString("HH:mm") + "</mspace>";

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

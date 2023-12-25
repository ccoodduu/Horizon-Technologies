using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
	[Header("Time")]
	[SerializeField] private float dayLengthInSeconds;
	[SerializeField] private float nightLengthInSeconds;
	[SerializeField] private int startOfficeHour;
	[SerializeField] private int endOfficeHour;
	[SerializeField] private int speedChangeDelay;

	private static DateTime foundingDate = new DateTime(2000, 1, 1, 8, 0, 0);

	public DateTime time { get; private set; }
	public bool IsOfficeTime => (time.Hour >= startOfficeHour && time.Hour < endOfficeHour);
	public DateTime startOfficeTime => time.Subtract(time.TimeOfDay).Add(TimeSpan.FromHours(startOfficeHour));
	public DateTime endOfficeTime => time.Subtract(time.TimeOfDay).Add(TimeSpan.FromHours(endOfficeHour));
	public TimeSpan dailyOfficeTime => endOfficeTime - startOfficeTime;

	public TimeSpan companyAge => time.Subtract(foundingDate);

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
	private float reputation = 1; // from 1-10

	// [Header("Orders")]
	public List<Order> currentOrders { get; private set; }
	public List<Order> availableOrders { get; private set; }

	// [Header("Employees")]
	public List<Employee> employees { get; private set; }
	public List<Employee> jobApplications { get; private set; }

	public int desksOwned { get; private set; }

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
	void Awake()
	{
		if (i == null) i = this;

		DontDestroyOnLoad(this);

		time = foundingDate;
		desksOwned = 0;
		Money = 10000;

		employees = new List<Employee>
		{
			Employee.You
		};

		jobApplications = new List<Employee>
		{
			Employee.Generate(),
			Employee.Generate(),
			Employee.Generate(),
			Employee.Generate(),
			Employee.Generate(),
		};

		BuyDeskFree();

		availableOrders = new List<Order>
		{
			new Order(OrderList.list[20]),
			new Order(OrderList.list[21]),
			new Order(OrderList.list[22])
		};

		currentOrders = new List<Order>
		{
			new Order(OrderList.list[23]),
			new Order(OrderList.list[24])
		};

		currentOrders[0].assignedEmployees.Add(employees[0]);
	}

	void Update()
	{
		// TIME
		var previousTime = time;

		var daySpeed = (endOfficeHour - startOfficeHour) * 60 * 60 / dayLengthInSeconds;
		var nightSpeed = (24 - (endOfficeHour - startOfficeHour)) * 60 * 60 / nightLengthInSeconds;


		// Update Time
		var isDay = time > startOfficeTime.Subtract(TimeSpan.FromMinutes(speedChangeDelay)) && time < endOfficeTime.Add(TimeSpan.FromMinutes(speedChangeDelay));

		float timeSpeed;
		if (!isDay || skipToNextDay)
		{
			timeSpeed = nightSpeed;
			if (!isDay) skipToNextDay = false;
		}
		else
			timeSpeed = daySpeed;

		time = time.AddSeconds(Time.deltaTime * timeSpeed);
		if (time.DayOfWeek == DayOfWeek.Saturday) time = time.AddDays(2);

		// Do stuff if time changes
		SetTimeText();

		if (previousTime.Day < time.Day) DayPassed();
		if (previousTime.Month < time.Month) MonthPassed();
		if (previousTime.Year < time.Year) YearPassed();

		// Employees
		foreach (var employee in employees)
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
			var workedTime = TimeSpan.FromSeconds(Time.deltaTime * timeSpeed);

			foreach (var order in new List<Order>(currentOrders))
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
	}

	private void YearPassed()
	{

	}

	private void MonthPassed()
	{
		foreach (var employee in employees)
		{
			Money -= employee.salary;
		}
	}

	public void TakeOrder(Order order)
	{
		availableOrders.Remove(order);
		currentOrders.Add(order);
	}

	private void CompleteOrder(Order order)
	{
		if (order.Completion < 1f)
		{
			Money -= 100;
			currentOrders.Remove(order);

			return;
		}

		var timeFee = (order.deadline > time) ? 0f : (float)(order.deadline - time).TotalDays;

		Money += order.Payment + (int)timeFee * -100;

		currentOrders.Remove(order);
	}

	public void Hire(Employee employee)
	{
		if (desksOwned <= employees.Count) return;

		jobApplications.Remove(employee);
		HirePanel.i.UpdatePanel();

		employees.Add(employee);
		EmployeesPanel.i.UpdatePanel();
	}

	public void Fire(Employee employee)
	{
		employee.employeeRender.employee = null;

		employees.Remove(employee);
		EmployeesPanel.i.UpdatePanel();
	}

	public void BuyDesk()
	{
		var price = 100;
		if (Money < price) return;
		if (officeManager.desks.Length <= desksOwned) return;

		Money -= price;
		BuyDeskFree();
	}
	public void BuyDeskFree()
	{
		if (officeManager.desks.Length <= desksOwned) return;

		desksOwned++;

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

		var text = "<mspace=mspace=38>" + time.ToString("HH:mm") + "</mspace>";

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
		text += time.ToString("MMMM d") + "<sup>th</sup>";
		text += "\n" + time.Year;
		text += "\n" + time.DayOfWeek.ToString();

		dateText.text = text;
	}

	private void SetOrderText()
	{
		var text = "";

		foreach (var order in currentOrders)
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

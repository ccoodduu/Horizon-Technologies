using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
	public int money;
	public DateTime time { get; private set; }
	public List<Employee> employees { get; private set; }
	public List<Employee> availableEmployees { get; private set; }

	public int desksOwned { get; private set; }
	public OfficeType OfficeType => officeManager.officeType;
	public OfficeManager officeManager;

	[Header("UI")]
	[SerializeField] private TMP_Text timeText;
	[SerializeField] private TMP_Text dateText;

	public static Game i;
	void Awake()
	{
		if (i == null) i = this;

		DontDestroyOnLoad(this);

		time = new DateTime(2000, 1, 1, 8, 0, 0);
	}

	void Update()
	{
		// TIME
		var previousTime = time;

		// Update Time
		time = time.AddMinutes(Time.deltaTime);
		if (time.Hour > 16) time = time.AddHours(24 - 16 + 8);
		if (time.DayOfWeek == DayOfWeek.Saturday) time = time.AddDays(2);

		// Do stuff if time changes
		SetTimeText();

		if (previousTime.Day < time.Day) DayPassed();
		if (previousTime.Month < time.Month) MonthPassed();
		if (previousTime.Year < time.Year) YearPassed();

		
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
			money -= employee.salary;
		}
	}

	public void Hire(Employee employee)
	{
		if (desksOwned <= employees.Count) return;

		availableEmployees.Remove(employee);
		employees.Add(employee);


	}

	private void SetTimeText()
	{
		/* Format:
		 * 13:39
		 */

		var text = time.ToString("HH:mm");

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
}

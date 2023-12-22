using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
	public TimeSpan companyAge => time.Subtract(foundingDate);


	private bool skipToNextDay;


	[Header("Money")]
	public int money;

	// [Header("Employees")]
	public List<Employee> employees { get; private set; }
	public List<Employee> availableEmployees { get; private set; }

	public int desksOwned { get; private set; }

	[Header("Office")]
	public OfficeManager officeManager;
	public OfficeType OfficeType => officeManager.officeType;

	[Header("UI")]
	[SerializeField] private TMP_Text timeText;
	[SerializeField] private TMP_Text dateText;

	public static Game i;
	void Awake()
	{
		if (i == null) i = this;

		DontDestroyOnLoad(this);

		time = foundingDate;
		desksOwned = 1;
		employees = new List<Employee>();

		employees.Add(new Employee());
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

	public void SkipToNextDay()
	{
		skipToNextDay = !skipToNextDay;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeUI : MonoBehaviour
{
	[Header("Text")]
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private TMP_Text ageText;
	[SerializeField] private TMP_Text experienceText;
	[SerializeField] private TMP_Text salaryText;
	[SerializeField] private TMP_Text skillText;

	[Header("Image")]
	[SerializeField] private EmployeeImage employeeImage;

	[Header("Buttons")]
	[SerializeField] private Button hireButton;
	[SerializeField] private Button fireButton;

	public Employee employee { get; private set; }
	
	private EmployeeUIType type;

	public void SetParameters(Employee employee, EmployeeUIType type)
	{
		this.employee = employee;
		this.type = type;

		// text
		nameText.text = employee.name_;
		ageText.text = employee.Age + "y/o";
		experienceText.text = "Exp: " + ((int)employee.Experience.TotalDays / 365) + "y";
		salaryText.text = employee.salary + "$/m";
		skillText.text = employee.skills.ToSkillString();

		// image
		employeeImage.SetImage(employee);

		// button
		switch (type)
		{
			case EmployeeUIType.Hire:
				hireButton.gameObject.SetActive(true);
				fireButton.gameObject.SetActive(false);
				break;

			case EmployeeUIType.Fire:
				hireButton.gameObject.SetActive(false);
				fireButton.gameObject.SetActive(true);
				break;

			case EmployeeUIType.Show:
				hireButton.gameObject.SetActive(false);
				fireButton.gameObject.SetActive(false);
				break;
		}
	}

	public void Hire()
	{
		if (employee == null) return;

		Game.i.Hire(employee);
	}

	public void Fire()
	{
		if (employee == null) return;

		Game.i.Fire(employee);
	}

	public void OpenFullPanel()
	{
		if (employee == null) return;
		if (type is EmployeeUIType.Hire) return;

		EmployeeViewPanel.i.Open(employee);
	}
}


public enum EmployeeUIType
{
	Hire,
	Fire,
	Show,
}
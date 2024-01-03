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
	[SerializeField] private GameObject hireButtons;
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
		salaryText.text = (type is EmployeeUIType.Hire ? employee.requestedSalary : employee.salary) + "$/m";
		skillText.text = employee.skills.ToSkillString();

		// image
		employeeImage.SetImage(employee);

		// button
		switch (type)
		{
			case EmployeeUIType.Hire:
				hireButtons.SetActive(true);
				fireButton.gameObject.SetActive(false);
				break;

			case EmployeeUIType.Fire:
				hireButtons.SetActive(false);
				fireButton.gameObject.SetActive(true);
				break;

			case EmployeeUIType.Show:
				hireButtons.SetActive(false);
				fireButton.gameObject.SetActive(false);
				break;
		}

		if (employee.name_ == "You")
		{
			fireButton.gameObject.SetActive(false);
		}
	}

	public void Hire()
	{
		if (employee == null) return;

		Game.i.Hire(employee);
	}

	public void Reject()
	{
		if (employee == null) return;

		Game.i.Reject(employee);
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
using System.Collections;
using System.Collections.Generic;
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

	[Header("Image")]
	[SerializeField] private Image hairImage;
	[SerializeField] private Image headImage;
	[SerializeField] private Image shirtImage;

	[Header("Buttons")]
    [SerializeField] private Button hireButton;
	[SerializeField] private Button fireButton;

	public Employee employee {  get; private set; }

	public void SetParameters(Employee employee, EmployeeUIType type)
    {
        this.employee = employee;

		// text
        nameText.text = employee.name_;
		ageText.text = "Age: " + employee.age;
        experienceText.text = "Exp: " + ((int)employee.experience.TotalDays / 365) + "y";
        salaryText.text = "Salary: " + employee.salary + "$/m";

		// image
		hairImage.color = employee.looks.hairColor;
		headImage.color = employee.looks.skinColor;
		shirtImage.color = employee.looks.shirtColor;

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
}


public enum EmployeeUIType
{
    Hire,
    Fire,
    Show,
}
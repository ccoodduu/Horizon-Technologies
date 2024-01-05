using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class EmployeeViewPanel : MonoBehaviour
{
	public static EmployeeViewPanel i;

	private Employee employee;

	[SerializeField] private GameObject panel;

	[Header("UI")]
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private TMP_Text aboutText;
	[SerializeField] private TMP_Text statsText;
	[SerializeField] private EmployeeImage image;
	[SerializeField] private TMP_Dropdown assignedTaskDropdown;
	[SerializeField] private TMP_InputField newSalaryInputField;

	public bool IsOpen => panel.activeSelf;

	void Awake()
	{
		i = this;
	}

	private void Update()
	{
		if (!Game.i.Employees.Contains(employee))
		{
			panel.SetActive(false);
		}
	}

	public void DontBeNegative(string value)
	{
		if (value == "-") newSalaryInputField.text = "";
	}

	public void SetSalary(string value)
	{
		employee.salary = int.Parse(value);

		UpdatePanel();
		EmployeesPanel.i.UpdatePanel();
	}

	public void ChangeAssignedTask(int option)
	{
		if (option == 0) employee.SetAssignedOrder(null);
		employee.SetAssignedOrder(Game.i.CurrentOrders[option - 1]);
	}

	public void Open(Employee employee)
	{
		panel.SetActive(true);

		this.employee = employee;

		UpdatePanel();
	}

	public void UpdatePanel()
	{
		image.SetImage(employee);

		nameText.text = employee.name_;
		aboutText.text = "Age: " + employee.Age + " y/o";

		var statsString = new StringBuilder();

		statsString.AppendLine("Salary: " + employee.salary + " $");
		statsString.AppendLine("Experience: " + (int)employee.Experience.TotalDays / 365 + " y");
		statsString.AppendLine("Employed since: " + employee.employedSince.ToString("dd/MM/yyyy"));
		statsString.AppendLine("Happiness: " + Mathf.RoundToInt(employee.Happiness * 100) + "%");
		statsString.AppendLine("Skills: " + employee.skills.ToSkillString());
		statsString.AppendLine("Working speed: " + Mathf.Round(employee.WorkingSpeed * 100) / 100 + "x");

		statsText.text = statsString.ToString();

		assignedTaskDropdown.ClearOptions();
		assignedTaskDropdown.AddOptions(new List<string> { "None " });
		assignedTaskDropdown.AddOptions(Game.i.CurrentOrders.Select(i => i.orderDescription.name).ToList());

		assignedTaskDropdown.value = Game.i.CurrentOrders.IndexOf(employee.assignedOrder) + 1;

		newSalaryInputField.text = "";
		newSalaryInputField.gameObject.SetActive(employee.name_ != "You");
	}
}

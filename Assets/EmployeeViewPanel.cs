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

	void Start()
	{
	    i = this;
	}

	public void ChangeAssignedTask(int option)
    {
        if (option == 0) employee.SetAssignedOrder(null);
        employee.SetAssignedOrder(Game.i.currentOrders[option - 1]);
    }

	public void Open(Employee employee)
    {
        panel.SetActive(true);

        this.employee = employee;

        image.SetImage(employee);

        nameText.text = employee.name_;
        aboutText.text = "Age: " + employee.age + " y/o";

        var statsString = new StringBuilder();

        statsString.AppendLine("Salary: " + employee.salary + " $");
		statsString.AppendLine("Experience: " + (int)employee.experience.TotalDays / 365 + " y");
		statsString.AppendLine("Employed since: " + employee.employedSince.ToString("dd/MM/yyyy"));

        var skillString = string.Join(", ", employee.skills.Select(s => s.ToDisplayText()));
        statsString.AppendLine("Skills: " + skillString);

        statsText.text = statsString.ToString();

        assignedTaskDropdown.ClearOptions();
        assignedTaskDropdown.AddOptions(new List<string> { "None "});
        assignedTaskDropdown.AddOptions(Game.i.currentOrders.Select(i => i.orderDescription.name).ToList());

        assignedTaskDropdown.value = Game.i.currentOrders.IndexOf(employee.assignedOrder) + 1;
	}
}

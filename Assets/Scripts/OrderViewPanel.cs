using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class OrderViewPanel : MonoBehaviour
{
	[SerializeField] private GameObject panel;

	[Header("Text")]
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private TMP_Text statsText;
	[SerializeField] private TMP_Text aboutText;
	[SerializeField] private TMP_Text employeesText;

	public static OrderViewPanel i;
	public Order order { get; private set; }

	void Start()
	{
		i = this;
	}

	void Update()
	{
		if (panel.activeSelf) UpdatePanel();
	}

	public void OpenOrder(Order order)
	{
		this.order = order;

		panel.SetActive(true);

		UpdatePanel();
	}

	void UpdatePanel()
	{
		nameText.text = order.orderDescription.name;
		aboutText.text = "Owner name: " + order.ownerName;

		var statsString = new StringBuilder();

		statsString.AppendLine("Payment: " + order.Payment + " $");
		statsString.AppendLine("Progress: " + Mathf.RoundToInt(order.Completion * 100) + "%");

		var estimatedDone = order.EstimatedDone();
		statsString.AppendLine("Estimated done: " + ((estimatedDone == DateTime.MaxValue) ? "N/A" : estimatedDone.ToString("dd/MM/yyyy")));
		statsString.AppendLine("Deadline: " + order.deadline.ToString("dd/MM/yyyy"));
		statsString.AppendLine("Required skills: " + order.orderDescription.skills.ToSkillString());

		statsText.text = statsString.ToString();

		aboutText.text = "";

		var employeesString = "";
		foreach (var employee in order.assignedEmployees)
		{
			employeesString += " - " + employee.name_ + " (" + employee.skills.ToSkillString() + ")" + "\n";
		}

		employeesText.text = employeesString;
	}

	public void OpenEmail()
	{
		var body = order.FormatAsEmail();

		EmailViewPanel.i.OpenEmail(new Email()
		{
			subject = order.orderDescription.name,
			body = body,
			sender = order.ownerName,
		}, new EmailActionButton[0]);
	}

	public void AssignAllEmployees()
	{
		foreach (var employee in Game.i.Employees)
		{
			employee.SetAssignedOrder(order);
		}
	}

	public void CancelOrder()
	{
		Game.i.CompleteOrder(order);

		panel.SetActive(false);
	}
}

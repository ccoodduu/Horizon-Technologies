using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class OrderFinishedPanel : MonoBehaviour
{
	public static OrderFinishedPanel i;

	[SerializeField] private GameObject panel;

	[Header("UI")]
	[SerializeField] private TMP_Text statsText;
	[SerializeField] private TMP_Text moneyLeftText;
	[SerializeField] private TMP_Text moneyRightText;

	public bool IsOpen => panel.activeSelf;

	void Awake()
	{
		i = this;
	}

	public void OpenPanel(Order order, float reputationChange, MoneyChange[] economy)
	{
		panel.SetActive(true);

		var daysOverDeadline = Mathf.CeilToInt((float)(Game.i.Time - order.deadline).TotalDays);

		// set texts
		var statsString = new StringBuilder();

		statsString.AppendLine($"<size=36>{order.orderDescription.name}</size><line-height=10>");
		statsString.AppendLine($"<size=18>{order.orderDescription.skills.ToSkillString()}</size><line-height=100%>");
		statsString.AppendLine($"Deadline: {order.deadline.ToString("dd/MM/yyyy")} ({(daysOverDeadline > 0 ? $"{Mathf.Abs(daysOverDeadline)}d ago" : $"in {Mathf.Abs(daysOverDeadline)}d")})");
		statsString.AppendLine($"Hours spent: {Mathf.RoundToInt((float)order.timeSpent.TotalHours)} hr");
		statsString.AppendLine($"Reputation {(reputationChange > 0 ? "+" : "-") + Math.Round((decimal)reputationChange, 1)}");

		statsText.text = statsString.ToString();

		var total = economy.Sum(ch => ch.amount);

		var moneyLeftString = new StringBuilder();

		moneyLeftString.AppendLine($"Payment:<line-height=150%>");
		moneyLeftString.Append($"<line-height=100%>");
		foreach (var change in economy)
		{
			moneyLeftString.AppendLine($" {(change.amount > 0 ? "+" : "-")} {change.amount} $");
		}
		moneyLeftString.AppendLine();
		moneyLeftString.AppendLine($" = {total} $");
		moneyLeftString.AppendLine($" = {Mathf.RoundToInt(total / (float)order.timeSpent.TotalHours)} $/hr");

		moneyLeftText.text = moneyLeftString.ToString();

		var moneyRightString = new StringBuilder();

		moneyRightString.AppendLine("<line-height=150%>");
		moneyRightString.Append("<line-height=100%>");
		foreach (var change in economy)
		{
			moneyRightString.AppendLine(change.cause);
		}
		moneyRightString.AppendLine();
		moneyRightString.AppendLine("Total");

		moneyRightText.text = moneyRightString.ToString();
	}
}

public struct MoneyChange
{
	public int amount;
	public string cause;
}
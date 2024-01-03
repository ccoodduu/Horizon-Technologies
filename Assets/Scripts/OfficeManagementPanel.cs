using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeManagementPanel : MonoBehaviour
{
	public static OfficeManagementPanel i;

	[SerializeField] private TMP_Text deskStatsText;
	[SerializeField] private TMP_Text newOfficeStatsText;

	[SerializeField] private Button newOfficeButton;
	[SerializeField] private Button buyDeskButton;


	void Start()
	{
		i = this;

		gameObject.SetActive(false);

		UpdatePanel();
	}

	public void UpdatePanel()
	{
		var deskStatsString = new StringBuilder();

		deskStatsString.AppendLine($"Used desks: {Mathf.Min(Game.i.Employees.Count, Game.i.DesksOwned)}/{Game.i.DesksOwned}");
		deskStatsString.AppendLine($"Owned desks: {Game.i.DesksOwned}/{Game.i.officeManager.desks.Length}");
		deskStatsString.AppendLine($"Desk price: {Game.i.DeskPrice} $");

		deskStatsText.text = deskStatsString.ToString();

		buyDeskButton.interactable = Game.i.DesksOwned < Game.i.officeManager.desks.Length;


		if (Game.i.NextOffice.price == 0)
		{
			newOfficeStatsText.text = "Max level office reached!";
			newOfficeButton.gameObject.SetActive(false);

			return;
		}

		var newOfficeStatsString = new StringBuilder();

		newOfficeStatsString.AppendLine($"{Game.i.NextOffice.desks} Desks");
		newOfficeStatsString.AppendLine($"Happiness bonus: +{Mathf.RoundToInt(Game.i.NextOffice.happinessBonus * 100)}%");
		newOfficeStatsString.AppendLine($"Reputation: +{Game.i.NextOffice.reputationBonus}");
		newOfficeStatsString.AppendLine(Game.i.NextOffice.look);
		newOfficeStatsString.AppendLine("");

		newOfficeStatsString.AppendLine($"Price: {Game.i.NextOffice.price} $");

		newOfficeStatsText.text = newOfficeStatsString.ToString();

	}
}
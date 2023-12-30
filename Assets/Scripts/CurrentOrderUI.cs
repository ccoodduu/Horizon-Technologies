using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentOrderUI : MonoBehaviour
{
	public Order order { get; private set; }

	[Header("Text")]
	[SerializeField] private TMP_Text titleText;
	[SerializeField] private TMP_Text progressText;
	[SerializeField] private TMP_Text paymentText;
	[SerializeField] private TMP_Text skillText;

	[Header("Image")]
	[SerializeField] private EmailImage image;

	public void SetParameters(Order order)
	{
		this.order = order;

		UpdateUI();
	}

	public void UpdateUI()
	{
		// Text
		titleText.text = order.orderDescription.name;
		progressText.text = Mathf.RoundToInt(order.Completion * 100) + "%";
		paymentText.text = order.Payment + " $";
		skillText.text = order.orderDescription.skills.ToSkillString();

		// Image
		image.SetImage(order.ownerName);
	}

	public void OpenOrderViewPanel()
	{
		OrderViewPanel.i.OpenOrder(order);
	}
}

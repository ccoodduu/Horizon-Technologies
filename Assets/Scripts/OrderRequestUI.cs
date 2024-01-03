using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OrderRequestUI : MonoBehaviour
{
	public Order order { get; private set; }

	[Header("Text")]
	[SerializeField] private TMP_Text titleText;
	[SerializeField] private TMP_Text pointsText;
	[SerializeField] private TMP_Text paymentText;
	[SerializeField] private TMP_Text skillText;

	[Header("Image")]
	[SerializeField] private EmailImage image;

	public void SetParameters(Order order)
	{
		this.order = order;

		// Text
		titleText.text = order.orderDescription.name;
		pointsText.text = order.orderDescription.workPoints + " wp";
		paymentText.text = order.payment + " $";

		skillText.text = order.orderDescription.skills.ToSkillString();

		// Image
		image.SetImage(order.ownerName);
	}

	public void Reject()
	{
		Game.i.AvailableOrders.Remove(order);
		AvailableOrdersPanel.i.UpdatePanel();
	}

	public void Accept()
	{
		Game.i.TakeOrder(order);
	}

	public void OpenEmail()
	{
		var body = order.FormatAsEmail();

		EmailViewPanel.i.OpenEmail(new Email()
		{
			subject = order.orderDescription.name,
			body = body,
			sender = order.ownerName,
		}, new EmailActionButton[] {
			new EmailActionButton("Accept", () => Accept(), true),
			new EmailActionButton("Reject", () => Reject(), true),
		});
	}
}

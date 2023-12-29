using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OrderUI : MonoBehaviour
{
	public Order order { get; private set; }

	[Header("Text")]
	[SerializeField] private TMP_Text titleText;
	[SerializeField] private TMP_Text pointsText;
	[SerializeField] private TMP_Text paymentText;
	[SerializeField] private TMP_Text skillText;

	[Header("Image")]
	[SerializeField] private TMP_Text imageLetterText;
	[SerializeField] private Image image;

	public void SetParameters(Order order)
	{
		this.order = order;

		// Text
		titleText.text = order.orderDescription.name;
		pointsText.text = order.orderDescription.workPoints + " wp";
		paymentText.text = order.orderDescription.payment + " $";

		var skillsTextString = "";
		foreach (var skill in order.orderDescription.skills)
		{
			if (skillsTextString != "") skillsTextString += ", ";
			skillsTextString += skill.ToDisplayText();
		}

		skillText.text = skillsTextString;

		// Image
		imageLetterText.text = order.ownerName[0].ToString();
		Random.InitState(order.ownerName.ToCharArray().Sum(i => (byte)i));
		var color = Random.ColorHSV();
		image.color = color;
		imageLetterText.color = (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f) > (186f / 255f) ? Color.black : Color.white;
	}

	public void Reject()
	{
		Game.i.availableOrders.Remove(order);
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

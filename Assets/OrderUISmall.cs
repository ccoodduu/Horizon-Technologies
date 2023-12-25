using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUISmall : MonoBehaviour
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

	public void OpenEmail()
	{
		var body = order.FormatAsEmail();

		EmailViewPanel.i.OpenEmail(new Email()
		{
			subject = order.orderDescription.name,
			body = body,
			sender = order.ownerName,
		});
	}
}

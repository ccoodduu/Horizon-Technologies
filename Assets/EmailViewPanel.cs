using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailViewPanel : MonoBehaviour
{
	public static EmailViewPanel i;

	[SerializeField] private GameObject panel;

	[Header("Text")]
    [SerializeField] private TMP_Text subjectText;
	[SerializeField] private TMP_Text bodyText;

	[Header("Image")]
	[SerializeField] private TMP_Text imageLetterText;
	[SerializeField] private Image image;

	void Start()
	{
		i = this;
	}

	public void OpenEmail(Email email)
    {
		panel.SetActive(true);

		subjectText.text = email.subject;
		bodyText.text = email.body;

		// Image
		imageLetterText.text = email.sender[0].ToString();
		Random.InitState(email.sender.ToCharArray().Sum(i => (byte)i));
		var color = Random.ColorHSV();
		image.color = color;
		imageLetterText.color = (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f) > (186f / 255f) ? Color.black : Color.white;
	}
}

public struct Email
{
    public string sender;
    public string subject;
    public string body;
}

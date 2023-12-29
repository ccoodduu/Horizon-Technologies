using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

	[Header("Buttons")]
	[SerializeField] private Button[] buttons;

	[Header("Scroll View")]
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private ScrollRect scrollRect;

	private ContentSizeFitter contentSizeFitter;


	void Start()
	{
		i = this;
		contentSizeFitter = bodyText.GetComponent<ContentSizeFitter>();
	}

	private bool skip = true;
	void Update()
	{
		if (contentSizeFitter.verticalFit is ContentSizeFitter.FitMode.PreferredSize)
		{
			if (skip) { skip = false; return; }
			skip = true;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
		}
	}

	public void OpenEmail(Email email, EmailActionButton[] buttons)
	{
		panel.SetActive(true);

		scrollbar.value = 1;

		subjectText.text = email.subject;
		bodyText.text = email.body;
		contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		// Image
		imageLetterText.text = email.sender[0].ToString();
		Random.InitState(email.sender.ToCharArray().Sum(i => (byte)i));
		var color = Random.ColorHSV();
		image.color = color;
		imageLetterText.color = (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f) > (186f / 255f) ? Color.black : Color.white;

		foreach (var button in this.buttons) button.gameObject.SetActive(false);

		for (int i = 0; i < buttons.Length; i++)
		{
			var actionButton = buttons[i];
			var buttonElement = this.buttons[i];

			buttonElement.gameObject.SetActive(true);
			buttonElement.GetComponentInChildren<TMP_Text>().text = actionButton.text;

			buttonElement.onClick.RemoveAllListeners();

			var action = new UnityAction(actionButton.closeAfter ?
				(() => { actionButton.action.Invoke(); panel.SetActive(false); })
				:
				actionButton.action
			);

			buttonElement.onClick.AddListener(action);
		}
	}
}

public struct Email
{
	public string sender;
	public string subject;
	public string body;
}

public struct EmailActionButton
{
	public string text;
	public Action action;
	public bool closeAfter;

	public EmailActionButton(string text, Action action, bool closeAfter = false)
	{
		this.text = text;
		this.action = action;
		this.closeAfter = closeAfter;
	}
}

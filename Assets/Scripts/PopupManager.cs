using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager i;

	private List<Popup> popups = new();

	[Header("Time")] 
	[SerializeField] private float timePerPopup;
	[SerializeField] private float hideTime;
	[SerializeField] private float showTime;

	[Header("UI")]
	[SerializeField] private TMP_Text text;
	[SerializeField] private Button button;
	[SerializeField] private CanvasGroup canvasGroup;
	[Space(5)]
	[SerializeField] private RectTransform textTransform;
	[SerializeField] private RectTransform backgroundTransform;


	private float time = float.MaxValue;
	private bool hide = true;

	void Start()
	{
		i = this;	
	}

	void Update()
	{
		time += Time.deltaTime;

		backgroundTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textTransform.rect.height);

		if (time > timePerPopup && !hide)
		{
			HidePopup();
		}

		if (hide)
		{
			var progress = Mathf.Clamp01((time - startHideTime) / hideTime);

			canvasGroup.alpha = 1 - progress;

			if (progress == 1 && popups.Count > 0) ShowPopup();
		} 
		else
		{
			var progress = Mathf.Clamp01(time / showTime);

			canvasGroup.alpha = progress;
		}

	}

	public void AddPopup(Popup popup)
	{
		popups.Add(popup);
	}

	public void ShowPopup()
	{
		time = 0;
		hide = false;

		var popup = popups[0];

		text.text = popup.text;
		button.onClick.RemoveAllListeners();

		button.interactable = popup.onClick != null;
		if (popup.onClick != null) button.onClick.AddListener(new UnityAction(popup.onClick));
	}

	private float startHideTime;
	public void HidePopup()
	{
		startHideTime = time;

		hide = true;

		popups.RemoveAt(0);
	}
}

public struct Popup
{
	public string text;
	public Action onClick;

	public Popup(string text, Action onClick)
	{
		this.text = text;
		this.onClick = onClick;
	}
}
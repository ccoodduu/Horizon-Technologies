using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOnePanelOpen : MonoBehaviour
{
    private bool isStarted;

	private void Start()
	{
		isStarted = true;
	}

	private void OnEnable()
	{
        if (!isStarted) return;

        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        foreach (var panel in canvas.GetComponentsInChildren<OnlyOnePanelOpen>())
        {
            if (panel == this) continue;
            panel.gameObject.SetActive(false);
        }

		foreach (var panel in canvas.GetComponentsInChildren<AutoClosePanel>())
		{
			if (panel.gameObject == gameObject) continue;
			panel.gameObject.SetActive(false);
		}
	}
}

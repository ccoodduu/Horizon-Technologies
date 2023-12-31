using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconButton : MonoBehaviour
{
	[SerializeField] private GameObject target;
	[SerializeField] private Image dot;

	public void Toggle()
	{
		target.SetActive(!target.activeSelf);

		dot.enabled = false;
	}

	public void EnableDot()
	{
		if (target.activeSelf) return;
		dot.enabled = true;
	}
}

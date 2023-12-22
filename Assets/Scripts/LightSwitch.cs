using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
	private bool wasOn = false;
	[SerializeField] private int minuteOffset;

	private void Start()
	{
		
		TurnOf();
	}

	private void Update()
	{
		var offset = TimeSpan.FromMinutes(minuteOffset);

		var isOn = Game.i.time > Game.i.startOfficeTime.Subtract(offset) && Game.i.time < Game.i.endOfficeTime.Add(offset);

		if (wasOn && !isOn) TurnOf();
		if (!wasOn && isOn) TurnOn();

		wasOn = isOn;
	}
	private void TurnOn()
	{
		foreach (var light in GetComponentsInChildren<Light>())
		{
			light.enabled = true;
		}
	}
	private void TurnOf()
	{
		foreach (var light in GetComponentsInChildren<Light>())
		{
			light.enabled = false;
		}
	}
}

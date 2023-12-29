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

		var isOn = Game.i.Time > Game.i.StartOfficeTime.Subtract(offset) && Game.i.Time < Game.i.EndOfficeTime.Add(offset);

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

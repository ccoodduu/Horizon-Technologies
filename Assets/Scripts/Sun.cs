using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
	void Update()
	{
		var timeOfDay = Game.i.Time.TimeOfDay / TimeSpan.FromDays(1);
		var angle = (float)timeOfDay * 360f - 90f;

		transform.localRotation = Quaternion.Euler(angle, 0, 0);
	}
}

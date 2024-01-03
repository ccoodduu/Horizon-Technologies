using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = System.Random;

public class EmailImage : MonoBehaviour
{
	[SerializeField] private TMP_Text imageLetterText;
	[SerializeField] private Image image;

	public void SetImage(string name)
	{
		Random random = new Random(name.ToCharArray().Sum(i => (byte)i));

		imageLetterText.text = name[0].ToString();
		var color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
		image.color = color;
		imageLetterText.color = (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f) > (186f / 255f) ? Color.black : Color.white;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailImage : MonoBehaviour
{
	[SerializeField] private TMP_Text imageLetterText;
	[SerializeField] private Image image;

	public void SetImage(string name)
	{
		imageLetterText.text = name[0].ToString();
		Random.InitState(name.ToCharArray().Sum(i => (byte)i));
		var color = Random.ColorHSV();
		image.color = color;
		imageLetterText.color = (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f) > (186f / 255f) ? Color.black : Color.white;
	}
}

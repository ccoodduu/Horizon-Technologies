using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeImage : MonoBehaviour
{
	[SerializeField] private Image hairImage;
	[SerializeField] private Image headImage;
	[SerializeField] private Image shirtImage;
	

	public void SetImage(Employee employee)
	{
		// image
		hairImage.color = employee.looks.hairColor;
		headImage.color = employee.looks.skinColor;
		shirtImage.color = employee.looks.shirtColor;
	}
}

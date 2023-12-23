using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EmployeeRender : MonoBehaviour
{
	[SerializeField] MeshRenderer hairMeshRenderer;
	[SerializeField] MeshRenderer skinMeshRenderer;
	[SerializeField] MeshRenderer shirtMeshRenderer;
	[SerializeField] MeshRenderer pantsMeshRenderer;
	[SerializeField] MeshRenderer shoesMeshRenderer;

	public Employee employee;

	private TimeSpan offset;
	
	private bool wasInOffice = false;

	private void Start()
	{
		offset = TimeSpan.FromMinutes(Random.Range(-10, 10));
		ExitOffice();
	}

	private void Update()
	{
		if (employee != null)
		{
			hairMeshRenderer.material.color = employee.looks.hairColor;
			skinMeshRenderer.material.color = employee.looks.skinColor;
			shirtMeshRenderer.material.color = employee.looks.shirtColor;
			pantsMeshRenderer.material.color = employee.looks.pantsColor;
			shoesMeshRenderer.material.color = employee.looks.shoesColor;
		}


		var isInOffice = Game.i.time > Game.i.startOfficeTime.Add(offset) && Game.i.time < Game.i.endOfficeTime.Add(offset);
		isInOffice = isInOffice && employee != null;

		if (wasInOffice && !isInOffice) ExitOffice();
		if (!wasInOffice && isInOffice) EnterOffice();

		wasInOffice = isInOffice;
	}
	private void EnterOffice()
	{
		foreach (var mr in GetComponentsInChildren<MeshRenderer>())
		{
			mr.enabled = true;
		}
	}
	private void ExitOffice()
	{
		foreach (var mr in GetComponentsInChildren<MeshRenderer>())
		{
			mr.enabled = false;
		}
	}	
}

public struct EmployeeLooks
{
	public Color hairColor;
	public Color skinColor;
	public Color shirtColor;
	public Color pantsColor;
	public Color shoesColor;

	public EmployeeLooks(Color hairColor, Color skinColor, Color shirtColor, Color pantsColor, Color shoesColor)
	{
		this.hairColor = hairColor;
		this.skinColor = skinColor;
		this.shirtColor = shirtColor;
		this.pantsColor = pantsColor;
		this.shoesColor = shoesColor;
	}

	public static EmployeeLooks RandomLooks()
	{
		var hairColor = Color.HSVToRGB(Random.Range(15f, 50f) / 360f, Random.Range(30f, 90f) / 100f, Random.Range(10f, 75f) / 100f);
		var shirtColor = Color.HSVToRGB(Random.Range(0f, 360f) / 360f, Random.Range(10f, 90f) / 100f, Random.Range(10f, 90f) / 100f);
		var pantsColor = Color.HSVToRGB(Random.Range(0f, 360f) / 360f, Random.Range(10f, 90f) / 100f, Random.Range(10f, 90f) / 100f);
		var shoesColor = Color.HSVToRGB(Random.Range(190f, 260f) / 360f, Random.Range(0f, 50f) / 100f, Random.Range(0f, 100f) / 100f);

		var s = Random.value;
		var v = s * new Vector3(241f, 212f, 175f) + (1 - s) * new Vector3(20f, 10f, 4f);
		var skinColor = new Color(v.x / 255f, v.y / 255f, v.z / 255f);

		return new EmployeeLooks(hairColor, skinColor, shirtColor, pantsColor, shoesColor);
	}
}

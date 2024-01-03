using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
	public Vector3 officeSize;
	public OfficeType officeType;
	public GameObject[] desks;

	void Start()
	{
		Game.i.officeManager = this;

		Game.i.OfficeIsLoaded();
	}
}

[System.Serializable]
public struct Office
{
	public int price;
	public int desks;
	public float happinessBonus;
	public float reputationBonus;
	public string look;
}
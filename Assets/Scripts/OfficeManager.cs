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
	}
}

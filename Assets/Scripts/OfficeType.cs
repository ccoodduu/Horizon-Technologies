using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OfficeType", menuName = "Office Type", order = 0)]
public class OfficeType : ScriptableObject
{
	public Mesh[] tables;
	public Mesh[] chairs;
	public Mesh[] walls;
}

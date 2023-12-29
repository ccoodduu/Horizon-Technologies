using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskRender : MonoBehaviour
{
    [SerializeField] private MeshFilter tableMesh;
	[SerializeField] private MeshFilter chairMesh;
	[SerializeField] private MeshFilter wallMesh;

	void Start()
    {
		tableMesh.mesh = Game.i.OfficeType.tables.Random();
		chairMesh.mesh = Game.i.OfficeType.chairs.Random();
		wallMesh.mesh = Game.i.OfficeType.walls.Random();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskRender : MonoBehaviour
{
    [SerializeField] MeshFilter tableMesh;
	[SerializeField] MeshFilter chairMesh;
	[SerializeField] MeshFilter wallMesh;

	void Start()
    {
		tableMesh.mesh = Game.i.OfficeType.tables.Random();
		chairMesh.mesh = Game.i.OfficeType.chairs.Random();
		wallMesh.mesh = Game.i.OfficeType.walls.Random();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizer : MonoBehaviour
{
	void Update()
	{
		var officeSize = Game.i.officeManager.officeSize;

		var floorHeight = Mathf.Sqrt(officeSize.x * officeSize.x + officeSize.z * officeSize.x);
		var wallHeight = officeSize.y;
		var height = floorHeight + wallHeight;

		var size = height * Mathf.Sin(transform.rotation.eulerAngles.x * Mathf.Deg2Rad);

		foreach (var camera in GetComponentsInChildren<Camera>())
		{
			camera.orthographicSize = size / 2 + 1;
		}
		transform.position = new Vector3(height, height + wallHeight / 2, height);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeRender : MonoBehaviour
{
	[SerializeField] MeshRenderer hairMeshRenderer;
	[SerializeField] MeshRenderer skinMeshRenderer;
	[SerializeField] MeshRenderer shirtMeshRenderer;
	[SerializeField] MeshRenderer pantsMeshRenderer;
	[SerializeField] MeshRenderer shoesMeshRenderer;

	void Start()
	{
		hairMeshRenderer.material.color = Color.HSVToRGB(Random.Range(15f, 50f) / 360f, Random.Range(30f, 90f) / 100f, Random.Range(10f, 75f) / 100f);
		shirtMeshRenderer.material.color = Color.HSVToRGB(Random.Range(0f, 360f) / 360f, Random.Range(10f, 90f) / 100f, Random.Range(10f, 90f) / 100f);
		pantsMeshRenderer.material.color = Color.HSVToRGB(Random.Range(0f, 360f) / 360f, Random.Range(10f, 90f) / 100f, Random.Range(10f, 90f) / 100f);
		shoesMeshRenderer.material.color = Color.HSVToRGB(Random.Range(190f, 260f) / 360f, Random.Range(0f, 50f) / 100f, Random.Range(0f, 100f) / 100f);

		var s = Random.value;
		var v = s * new Vector3(241f, 212f, 175f) + (1 - s) * new Vector3(20f, 10f, 4f);
		skinMeshRenderer.material.color = new Color(v.x / 255f, v.y / 255f, v.z / 255f);

	}
}

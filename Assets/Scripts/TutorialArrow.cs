using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    public string Name;

	private Vector3 originalPosition;
	void Start()
	{
		originalPosition = transform.localPosition;
	}

	private float time = 0;
	void Update()
	{
		time += Time.deltaTime;

		var translate = new Vector3(Mathf.Sin(time * 4) * 10 + 10, 0, 0);
		var rotated = transform.rotation * translate;

		transform.localPosition = originalPosition - rotated;
	}
}

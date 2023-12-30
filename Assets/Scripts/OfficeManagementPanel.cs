using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManagementPanel : MonoBehaviour
{
	public static OfficeManagementPanel i;

	void Start()
	{
		i = this;

		gameObject.SetActive(false);
	}
}
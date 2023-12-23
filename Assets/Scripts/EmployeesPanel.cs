using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmployeesPanel : MonoBehaviour
{
	public static EmployeesPanel i;

	[SerializeField] private GameObject employeeUIContainer;
	[SerializeField] private GameObject employeeUIPrefab;

	void Start()
	{
		i = this;

		UpdatePanel();
	}

	public void UpdatePanel()
	{
		var currentShown = GetComponentsInChildren<EmployeeUI>();

		foreach (var e in Game.i.employees)
			if (!currentShown.Any(a => a.employee == e)) SpawnEmployeeUI(e);

		foreach (var a in currentShown)
			if (!Game.i.employees.Any(e => a.employee == e)) Destroy(a.gameObject);
	}

	private void SpawnEmployeeUI(Employee employee)
	{
		var gameObject = Instantiate(employeeUIPrefab, employeeUIContainer.transform);

		var eUI = gameObject.GetComponent<EmployeeUI>();
		eUI.SetParameters(employee, EmployeeUIType.Fire);
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrentOrdersPanel : MonoBehaviour
{
	public static CurrentOrdersPanel i;

    [SerializeField] private GameObject OrderUIPrefab;
    [SerializeField] private GameObject OrderUIContainer;

	void Start()
	{
		i = this;

		UpdatePanel();
		gameObject.SetActive(false);
	}

	public void UpdatePanel()
	{
		var currentShown = GetComponentsInChildren<OrderUI>();

		foreach (var o in Game.i.AvailableOrders)
			if (!currentShown.Any(a => a.order == o)) SpawnOrderUI(o);

		foreach (var a in currentShown)
			if (!Game.i.AvailableOrders.Any(o => a.order == o)) Destroy(a.gameObject);
	}

	private void SpawnOrderUI(Order order)
	{
		var gameObject = Instantiate(OrderUIPrefab, OrderUIContainer.transform);

		var oUI = gameObject.GetComponent<OrderUI>();
		oUI.SetParameters(order);
	}
}

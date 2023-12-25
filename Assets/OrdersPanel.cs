using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrdersPanel : MonoBehaviour
{
	public static OrdersPanel i;

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
		var currentShown = GetComponentsInChildren<OrderUISmall>();

		foreach (var o in Game.i.availableOrders)
			if (!currentShown.Any(a => a.order == o)) SpawnOrderUI(o);

		foreach (var a in currentShown)
			if (!Game.i.availableOrders.Any(o => a.order == o)) Destroy(a.gameObject);
	}

	private void SpawnOrderUI(Order order)
	{
		var gameObject = Instantiate(OrderUIPrefab, OrderUIContainer.transform);

		var oUI = gameObject.GetComponent<OrderUISmall>();
		oUI.SetParameters(order);
	}
}

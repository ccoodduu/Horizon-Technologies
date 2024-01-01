using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvailableOrdersPanel : MonoBehaviour
{
	public static AvailableOrdersPanel i;

    [SerializeField] private GameObject orderRequestUIPrefab;
    [SerializeField] private GameObject orderUIContainer;

	void Awake()
	{
		i = this;
		gameObject.SetActive(false);
	}

	void Start()
	{
		UpdatePanel();
	}

	public void UpdatePanel()
	{
		var currentShown = GetComponentsInChildren<OrderRequestUI>();

		foreach (var o in Game.i.AvailableOrders)
			if (!currentShown.Any(a => a.order == o)) SpawnOrderUI(o);

		foreach (var a in currentShown)
			if (!Game.i.AvailableOrders.Any(o => a.order == o)) Destroy(a.gameObject);
	}

	private void SpawnOrderUI(Order order)
	{
		var gameObject = Instantiate(orderRequestUIPrefab, orderUIContainer.transform);

		var oUI = gameObject.GetComponent<OrderRequestUI>();
		oUI.SetParameters(order);
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CurrentOrdersPanel : MonoBehaviour
{
	public static CurrentOrdersPanel i;

    [SerializeField] private GameObject OrderUIPrefab;
    [SerializeField] private GameObject OrderUIContainer;

	void Awake()
	{
		i = this;
		gameObject.SetActive(false);
	}

	void Start()
	{
		UpdatePanel();
	}

	void Update()
	{
		var currentShown = GetComponentsInChildren<CurrentOrderUI>();

		foreach (var oUI in currentShown)
			oUI.UpdateUI();
	}

	public void UpdatePanel()
	{
		var currentShown = GetComponentsInChildren<CurrentOrderUI>();

		foreach (var o in Game.i.CurrentOrders)
			if (!currentShown.Any(a => a.order == o)) SpawnOrderUI(o);

		foreach (var a in currentShown)
			if (!Game.i.CurrentOrders.Any(o => a.order == o)) Destroy(a.gameObject);
	}

	private void SpawnOrderUI(Order order)
	{
		var gameObject = Instantiate(OrderUIPrefab, OrderUIContainer.transform);

		var oUI = gameObject.GetComponent<CurrentOrderUI>();
		oUI.SetParameters(order);
	}
}

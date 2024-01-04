using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private Transform parent;
	[SerializeField] private float zoomFactor = 1.0f;

	[SerializeField] private float maxSize;
	[SerializeField] private float minSize;

	[SerializeField] private float maxX;
	[SerializeField] private float maxY;

	private Vector3 dragPos;

	private Camera[] cameras;

	void Start()
	{
		cameras = GetComponentsInChildren<Camera>();

		ResetCamera();
	}

	void Update()
	{
		if (Game.i.isPaused) return;

		var c = Camera.main;

		var ratio = (Camera.main.ScreenToWorldPoint(new Vector3(1, 0, 0)) - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0))).magnitude;

		// Panning
		if (Input.GetMouseButtonDown(2))
		{
			dragPos = Input.mousePosition;
		}

		if (Input.GetMouseButton(2))
		{
			Vector3 diff = dragPos - Input.mousePosition;
			diff.z = 0.0f;
			diff *= ratio;

			transform.localPosition += diff;

			dragPos = Input.mousePosition;
		}

		// Zooming
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			var s = Input.GetAxis("Mouse ScrollWheel");

			var size = c.orthographicSize * Mathf.Pow(zoomFactor, -s);
			foreach (var camera in cameras)
			{
				camera.orthographicSize = Mathf.Clamp(size, minSize, maxSize);
			}
		}

		// Clamp Pos
		var max = new Vector2(maxX - c.orthographicSize * c.aspect, maxY - c.orthographicSize);

		transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -max.x, max.x), Mathf.Clamp(transform.localPosition.y, -max.y, max.y), 0);
	}

	public void ResetCamera()
	{
		var officeSize = Game.i.officeManager.officeSize;

		var floorHeight = Mathf.Sqrt(officeSize.x * officeSize.x + officeSize.z * officeSize.x);
		var wallHeight = officeSize.y;
		var height = floorHeight + wallHeight;

		var size = height * Mathf.Sin(parent.rotation.eulerAngles.x * Mathf.Deg2Rad);

		foreach (var camera in cameras)
		{
			camera.orthographicSize = size / 2 + 1;
		}
		parent.position = new Vector3(50, 50 + wallHeight / 2, 50);
	}
}

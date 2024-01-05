using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Toggle();
		}	
	}

	public void Toggle()
	{
		if (Game.i.isPaused) Resume();
		else Pause();
	}

	public void Pause()
	{
		Game.i.isPaused = true;
		panel.SetActive(true);
	}

	public void Resume()
	{
		Game.i.isPaused = false;
		panel.SetActive(false);
	}

	public void Exit()
	{
		var globalParent = Game.i.gameObject.transform.parent;
		foreach (var ddol in FindObjectsOfType<DontDestroyOnLoad>(true))
		{
			Destroy(ddol.gameObject);
		}
		
		SceneManager.LoadScene(0);
	}
}

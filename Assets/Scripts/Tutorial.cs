using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private Button nextButton;
	[SerializeField] private TMP_Text tutorialText;

	[HideInInspector] public bool isDoneWithTutorial;

	private TutorialArrow[] arrows;

	private GameObject GetArrow(string name) => arrows.First(a => a.Name == name).gameObject;

	private int step;
	private TutorialStep[] steps;

	private TutorialStep CurrentStep => steps[step];

	void Awake()
	{
		arrows = FindObjectsOfType<TutorialArrow>(true);

		steps = new TutorialStep[]
		{
			new TutorialStep(
				"Hello! :D \n" +
				"I am the tutorial, and I will give you a quick guide on how to manage your newly founded company. ",
				GetArrow("Tutorial")
			),
			new TutorialStep(
				"This is the Info Panel \n" +
				"Here you can see the time, your money, and your reputation (the stars).",
				GetArrow("Info Panel")
			),
			new TutorialStep(
				"You can also skip to the next morning if time is moving too slowly. ",
				GetArrow("Skip Day")
			),
			new TutorialStep(
				"Lets take our first order! \n" +
				"Open the Order Request Panel.",
				() => AvailableOrdersPanel.i.gameObject.activeSelf,
				GetArrow("Incoming Orders Button")
			),
			new TutorialStep(
				"Here you can see your available orders. \n" +
				"New orders will pop up while you are playing. "
			),
			new TutorialStep(
				"Try opening the first order by clicking on it. ",
				() => EmailViewPanel.i.isOpen,
				GetArrow("First Available Order")
			)
		};


		step = -1;
	}

	void Start()
	{
		NextStep();
	}

	void Update()
	{
		if (isDoneWithTutorial) return;

		if (CurrentStep.isDone.Invoke())
			NextStep();
	}

	public void NextStep()
	{
		if (step != -1)
		{
			CurrentStep.gameObject?.SetActive(false);
			nextButton.gameObject.SetActive(false);
		}

		step++;

		if (step >= steps.Length)
		{
			isDoneWithTutorial = true;
			panel.SetActive(false);
			return;
		}

		if (step == 0) GetComponent<Image>().enabled = true;
		else GetComponent<Image>().enabled = false;

		CurrentStep.gameObject?.SetActive(true);
		nextButton.gameObject.SetActive(CurrentStep.nextButton);
		tutorialText.text = CurrentStep.text;
	}

	public void NextButtonPressed() { isNextButtonPressed = true; }

	private static bool isNextButtonPressed;
	public static bool PressedNextButton()
	{
		if (isNextButtonPressed)
		{
			isNextButtonPressed = false;
			return true;
		}
		return false;
	}
}

public struct TutorialStep
{
	public string text;
	public Func<bool> isDone;
	public GameObject gameObject;
	public bool nextButton;

	public TutorialStep(string text) : this()
	{
		this.text = text;
		isDone = () => Tutorial.PressedNextButton();
		gameObject = null;
		nextButton = true;
	}

	public TutorialStep(string text, GameObject gameObject) : this()
	{
		this.text = text;
		isDone = () => Tutorial.PressedNextButton();
		this.gameObject = gameObject;
		nextButton = true;
	}

	public TutorialStep(string text, Func<bool> isDone) : this()
	{
		this.text = text;
		this.isDone = isDone;
		gameObject = null;
		nextButton = false;
	}

	public TutorialStep(string text, Func<bool> isDone, GameObject gameObject) : this()
	{
		this.text = text;
		this.isDone = isDone;
		this.gameObject = gameObject;
		nextButton = false;
	}
}

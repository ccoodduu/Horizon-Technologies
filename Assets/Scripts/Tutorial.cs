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
	[SerializeField] private Button skipTutorialButton;
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
				() => EmailViewPanel.i.IsOpen,
				GetArrow("First Available Order")
			),
			new TutorialStep(
				"Here you can see the order request. \n" +
				"At the bottom you can see some stats. \n" +
				"You can see Payment, Skills, Work Points, and Deadline."
			),
			new TutorialStep(
				"The Required Skills are what skills your employees need to complete the task without a severe speed decrease. "
			),
			new TutorialStep(
				"The Work Points are how long an order will take. On average, 1 wp is 1 hour of work. So this order will on average take 80 hours for 1 employee. "
			),
			new TutorialStep(
				"At last the deadline is the last date you can finish the order, before getting payed less and getting a worse review. "
			),
			new TutorialStep(
				"Now press accept to claim the order. ",
				() => Game.i.CurrentOrders.Count > 0
			),
			new TutorialStep(
				"Great! \n" +
				"Now to begin working on an order, you need to assign some employees to the task."
			),
			new TutorialStep(
				"Go to the employees panel.",
				() => EmployeesPanel.i.gameObject.activeSelf,
				GetArrow("Employees Panel Button")
			),
			new TutorialStep(
				"Click on your employee (yourself)",
				() => EmployeeViewPanel.i.IsOpen,
				GetArrow("First Employee")
			),
			new TutorialStep(
				"And set your employees assigned task to the order.",
				() => Game.i.Employees[0].assignedOrder != null,
				GetArrow("Assign Task Dropdown")
			),
			new TutorialStep(
				"Awesome! \n" +
				"Now you can see in the top left that the order is being worked on."
			),
			new TutorialStep(
				"Let's hire another employee to help you with your order. "
			),
			new TutorialStep(
				"First we need a desk ready for our future employee. \n" +
				"Open the Office Management Panel.",
				() => OfficeManagementPanel.i.gameObject.activeSelf,
				GetArrow("Office Management Button")
			),
			new TutorialStep(
				"Click Buy Desk. ",
				() => Game.i.DesksOwned > 1,
				GetArrow("Buy Desk")
			),
			new TutorialStep(
				"Great! \n" +
				"Now lets hire the employee. \n" +
				"Open the Hire Panel. ",
				() => HirePanel.i.gameObject.activeSelf,
				GetArrow("Hire Panel Button")
			),
			new TutorialStep(
				"Now hire the person you like. \n" +
				"(I can recommend the first one)",
				() => Game.i.Employees.Count > 1
			),
			new TutorialStep(
				"Fantastic! \n" +
				"Now assign them to the current order.",
				() => Game.i.Employees[1].assignedOrder != null
			),
			new TutorialStep(
				"Good! \n" +
				"Now we have another employee working in our company."
			),
			new TutorialStep(
				"Now one last thing. \n" +
				"Try opening the Current Orders Pane.l",
				() => CurrentOrdersPanel.i.gameObject.activeSelf,
				GetArrow("Current Orders Button")
			),
			new TutorialStep(
				"Here you can see and manage all your current orders. \n" +
				"Try clicking on your order",
				() => OrderViewPanel.i.IsOpen
			),
			new TutorialStep(
				"Here you can see stats about this order. \n" +
				"You can also quickly assign all employees to this order."
			),
			new TutorialStep(
				"Now to finish the tutorial lets get this order finished! \n" +
				"(Remember you can speed up time)",
				() => OrderFinishedPanel.i.IsOpen
			),
			new TutorialStep(
				"Congrats! \n" +
				"You finished your first order and you are now done with the tutorial. \n" +
				"Good luck!"
			),
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

		if (step == 0)
		{
			GetComponent<Image>().enabled = true;
			skipTutorialButton.gameObject.SetActive(true);
		}
		else
		{
			GetComponent<Image>().enabled = false;
			skipTutorialButton.gameObject.SetActive(false);
		}

		CurrentStep.gameObject?.SetActive(true);
		nextButton.gameObject.SetActive(CurrentStep.nextButton);
		tutorialText.text = CurrentStep.text;

		if (step == steps.Length - 1)
		{
			nextButton.GetComponentInChildren<TMP_Text>().text = "Close";
		}
	}

	public void SkipTutorial()
	{
		NextStep();
		CurrentStep.gameObject?.SetActive(false);
		nextButton.gameObject.SetActive(false);

		step = steps.Length - 1;
		NextStep();
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

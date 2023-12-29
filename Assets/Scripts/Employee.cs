using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class Employee
{
	public int salary;
	public string name_;
	private DateTime birthday;
	public DateTime employedSince;
	public DateTime workedSince; // experience
	public Skill[] skills;

	public EmployeeLooks looks;

	public Order assignedOrder;
	public int age => ((int)(Game.i.time - birthday).TotalDays / 365);
	public TimeSpan experience => Game.i.time.Subtract(workedSince);

	public bool hasDesk;
	public EmployeeRender employeeRender;

	public bool canWork => hasDesk;

	public static Employee You => new Employee("You", 0, 22, TimeSpan.Zero, new Skill[] { Skill.HTML, Skill.JavaScript, Skill.CSS });

	public static Employee Generate()
	{
		var maxFator = GetOverallFactorFromReputation(Game.i.Reputation);
		float overallFactor = Random.Range(maxFator * .7f, maxFator);

		int baseSalary = Random.Range(6000, 10000);
		int adjustedSalary = Mathf.RoundToInt(baseSalary * overallFactor);

		int baseAge = Random.Range(22, 40);
		int adjustedAge = Mathf.RoundToInt(baseAge * overallFactor);

		TimeSpan baseExperience = TimeSpan.FromDays(Random.Range(0, 1000 * overallFactor));
		TimeSpan adjustedExperience = TimeSpan.FromDays(baseExperience.TotalDays * overallFactor);

		Employee employee = new Employee(NameGenerator.GenerateName(), adjustedSalary, adjustedAge, adjustedExperience, GenerateRandomSkills(overallFactor));

		return employee;
	}

	private static float GetOverallFactorFromReputation(float reputation)
	{
		float baseFactor = 1f;

		float overallFactor = Mathf.Clamp(baseFactor + reputation * 0.1f, 1f, 2.0f);

		return overallFactor;
	}

	private static Skill[] GenerateRandomSkills(float overallFactor)
	{
		List<Skill> allSkills = new List<Skill>((Skill[])Enum.GetValues(typeof(Skill)));
		List<Skill> randomSkills = new List<Skill>();

		// Adjust the number of skills based on the overallFactor
		int numSkills = Mathf.RoundToInt(Random.Range(1f, 3f) * overallFactor);

		// Define skill categories (you can customize this based on your skills)
		Dictionary<Skill, string[]> skillCategories = new Dictionary<Skill, string[]>
	{
		{ Skill.CSharp, new string[] { "Programming", "DotNet", "Backend", "Object-Oriented" } },
		{ Skill.CPP, new string[] { "Programming", "Backend", "Object-Oriented" } },
		{ Skill.C, new string[] { "Programming", "Backend" } },
		{ Skill.Java, new string[] { "Programming", "Backend", "Object-Oriented" } },
		{ Skill.JavaScript, new string[] { "Programming", "Frontend", "Web Development" } },
		{ Skill.HTML, new string[] { "Web Development", "Frontend" } },
		{ Skill.CSS, new string[] { "Web Development", "Frontend" } },
		{ Skill.Python, new string[] { "Programming", "Backend", "Scripting" } },
		{ Skill.PHP, new string[] { "Web Development", "Backend", "Scripting" } },
		{ Skill.SQL, new string[] { "Database", "Backend" } },
		{ Skill.dotNET, new string[] { "DotNet", "Backend" } },
		{ Skill.React, new string[] { "Web Development", "Frontend", "JavaScript" } },
		{ Skill.Angular, new string[] { "Web Development", "Frontend", "JavaScript" } },
		{ Skill.Nodejs, new string[] { "Web Development", "Backend", "JavaScript" } },
		{ Skill.DevOps, new string[] { "DevOps", "Infrastructure" } },
		{ Skill.Docker, new string[] { "DevOps", "Containers" } },
	};

		// Use a HashSet to track selected skills
		HashSet<Skill> selectedSkills = new HashSet<Skill>();

		// Iterate through skill categories to bias the selection
		foreach (var category in skillCategories.Values.Distinct())
		{
			List<Skill> categorySkills = skillCategories.Where(pair => pair.Value.Intersect(category).Any()).Select(pair => pair.Key).ToList();

			// Calculate the number of skills to select from this category
			int categorySkillsCount = Mathf.RoundToInt(numSkills * Random.Range(0.2f, 0.8f));

			// Ensure the total number of skills doesn't exceed the desired numSkills
			categorySkillsCount = Mathf.Min(categorySkillsCount, numSkills - randomSkills.Count);

			// Randomly select skills from the current category
			for (int i = 0; i < categorySkillsCount && categorySkills.Count > 0; i++)
			{
				int randomIndex = Random.Range(0, categorySkills.Count);
				Skill selectedSkill = categorySkills[randomIndex];

				// Check if the skill has already been selected
				while (selectedSkills.Contains(selectedSkill))
				{
					randomIndex = Random.Range(0, categorySkills.Count);
					selectedSkill = categorySkills[randomIndex];
				}

				randomSkills.Add(selectedSkill);
				selectedSkills.Add(selectedSkill);

				categorySkills.RemoveAt(randomIndex);
			}
		}

		// Fill in the remaining skills with random ones
		for (int i = randomSkills.Count; i < numSkills; i++)
		{
			int randomIndex = Random.Range(0, allSkills.Count);
			Skill selectedSkill = allSkills[randomIndex];

			// Check if the skill has already been selected
			while (selectedSkills.Contains(selectedSkill))
			{
				randomIndex = Random.Range(0, allSkills.Count);
				selectedSkill = allSkills[randomIndex];
			}

			randomSkills.Add(selectedSkill);
			selectedSkills.Add(selectedSkill);
		}

		return randomSkills.ToArray();
	}
	public Employee(string name, int salary, int age, TimeSpan experience, Skill[] skills)
	{
		name_ = name;
		this.salary = salary;
		this.birthday = Game.i.time - TimeSpan.FromDays(age * 365) + TimeSpan.FromDays(Random.Range(0, 364));
		workedSince = Game.i.time.Subtract(experience);
		this.skills = skills;

		looks = EmployeeLooks.RandomLooks();
	}

	public void SetAssignedOrder(Order order)
	{
		if (assignedOrder != null) assignedOrder.assignedEmployees.Remove(this);

		if (order != null) order.assignedEmployees.Add(this);

		assignedOrder = order;
	}
}

public static class NameGenerator
{
	public static string RandomFirstName() => firstName[Random.Range(0, firstName.Length)];

	public static string RandomLastName() => lastName[Random.Range(0, lastName.Length)];


	public static string GenerateName()
	{
		string firstName = RandomFirstName();
		string lastName = RandomLastName();

		return firstName + " " + lastName;
	}


	public static string[] firstName = new string[]
	{
		"Alice", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Ivy", "Jack",
		"Leo", "Mia", "Noah", "Olivia", "Quinn", "Samuel", "Taylor",
		"Ava", "Benjamin", "Chloe", "Daniel", "Eva", "Felix", "Georgia", "Harrison", "Isabel", "Jacob", "Jonathan",
		"Liam", "Madison", "Nathan", "Oscar", "Penelope", "Rebecca", "Sebastian", "Stephen", "Tara",
		"Brandon", "Cassandra", "Derek", "Eliza", "Fiona", "Gavin", "Heather", "Isaiah", "Jasmine",
		"Kevin", "Luna", "Mason", "Natalie", "Olive", "Patrick", "Quincy", "Riley", "Samantha", "Trevor",
		"Violet", "Wyatt", "Zion"
	};

	public static string[] lastName = new string[]
	{
		"Anderson", "Andersen", "Brown", "Carter", "Davis", "Evans", "Fisher", "Garcia", "Hill", "Irwin", "Johnson",
		"Keller", "Lopez", "Miller", "Nelson", "Owens", "Perez", "Quinn", "Reyes", "Smith", "Taylor",
		"Underwood", "Vargas", "Williams", "Young", "Zimmerman", "Adams", "Baker", "Clark", "Diaz",
		"Edwards", "Fletcher", "Gomez", "Hall", "Ingram", "Jenkins", "Kim", "Lambert", "Morgan", "Nguyen",
		"O'Brien", "Parker", "Quigley", "Reid", "Stewart", "Tucker", "Underhill", "Valentine", "Woods", "Archer",
		"Bennett", "Cunningham", "Duncan", "Emerson", "Floyd", "Galloway", "Hawkins", "Jefferson", "Kendrick",
		"Montgomery", "Nash", "Ortega", "Pierce", "Roth", "Sullivan", "Thompson", "Vance", "Webster", "York"
	};
}
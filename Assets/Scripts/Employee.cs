using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class Employee
{
	public int requestedSalary;
	public int salary;
	public string name_;
	private DateTime birthday;
	public DateTime employedSince;
	public DateTime workedSince; // experience
	public Skill[] skills;

	public EmployeeLooks looks;

	public Order assignedOrder;
	public int Age => (int)(Game.i.Time - birthday).TotalDays / 365;
	public TimeSpan Experience => Game.i.Time.Subtract(workedSince);

	public bool hasDesk;
	public EmployeeRender employeeRender;

	public bool CanWork => hasDesk;

	public static Employee You => new("You", 0, 22, TimeSpan.Zero, new Skill[] { Skill.HTML, Skill.JavaScript, Skill.CSS });

	// 0.0 - 1.0
	public float Happiness
	{
		get
		{
			if (name_ == "You") return 1;

			var happiness = Multipliers.i.baseHappiness;

			happiness *= ((float)salary / (float)requestedSalary) * Multipliers.i.salaryHappinessMultiplier;

			if (assignedOrder != null)
				happiness *= Mathf.Pow(Multipliers.i.skillHappinessMultiplier, assignedOrder.orderDescription.skills.Select(s => skills.Contains(s)).Count() - 1);

			happiness += Game.i.CurrentOffice.happinessBonus;

			return Mathf.Clamp01(happiness);
		}
	}

	public float WorkingSpeed
	{
		get
		{
			if (!CanWork) return 0;

			var speed = Multipliers.i.baseSpeed;

			speed *= (Happiness + (1 - Multipliers.i.baseHappiness)) * Multipliers.i.happinessSpeedMultiplier;

			speed *= Mathf.Pow(Multipliers.i.experienceSpeedMultiplier, ((float)Experience.TotalDays / 365f));
			speed *= Mathf.Pow(Multipliers.i.employmentTimeSpeedMultiplier, ((float)Game.i.Time.Subtract(employedSince).TotalDays / 365f));

			speed *= Mathf.Pow(Multipliers.i.ageSpeedMultiplier, (Age - 30));

			return speed;
		}
	}

	public void SetAssignedOrder(Order order)
	{
		assignedOrder?.assignedEmployees.Remove(this);
		order?.assignedEmployees.Add(this);
		assignedOrder = order;
	}

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

		var employee = new Employee(NameGenerator.GenerateName(), adjustedSalary, adjustedAge, adjustedExperience, GenerateRandomSkills(overallFactor));

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
		var allSkills = new List<Skill>((Skill[])Enum.GetValues(typeof(Skill)));

		// Adjust the number of skills based on the overallFactor
		int numSkills = Mathf.RoundToInt(Random.Range(1f, 3f) * overallFactor);

		var selectedSkills = new HashSet<Skill>();

		while (selectedSkills.Count() < numSkills)
		{
			var weightedSkills = new Dictionary<Skill, float>();

			foreach (var skill in allSkills)
				if (!selectedSkills.Contains(skill))
					weightedSkills[skill] = 0.1f;

			foreach (var skill in selectedSkills)
			{
				foreach (var category in skill.Info().categories)
				{
					foreach (var s in allSkills.Where(i => i.Info().categories.Contains(category)))
					{
						if (weightedSkills.ContainsKey(s))
							weightedSkills[s] += 1f;
					}
				}
			}

			var skills = weightedSkills.Keys.ToArray();
			var weights = weightedSkills.Values.ToArray();
			var cumulative = weights.Select((w, i) => weights.Take(i + 1).Sum()).ToArray();

			var random = Random.Range(0f, cumulative.Last());

			int i = 0;
			while (true)
			{
				if (cumulative[i] > random) break;
				i++;
			}

			selectedSkills.Add(skills[i]);
		}

		return selectedSkills.ToArray();
	}

	public Employee(string name, int salary, int age, TimeSpan experience, Skill[] skills)
	{
		name_ = name;
		this.requestedSalary = salary;
		this.birthday = Game.i.Time - TimeSpan.FromDays(age * 365) + TimeSpan.FromDays(Random.Range(0, 364));
		workedSince = Game.i.Time.Subtract(experience);
		this.skills = skills;

		looks = EmployeeLooks.RandomLooks();
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
		"Violet", "Wyatt", "Zion", "Jimmy", "Jamie", "Malcolm", "Bruce", "Dewey", "Francis", "Morty",
		"Janett", "Cornelius", "Arthur", "Connan", "Jerry", "Karl", "Kim", "Andrea", "Rosa", "Linguine",
		"James", "Mario", "Luigi", "Maurice", "King", "Jeff", "Mike", "Andy", "Joe", "Ludwig",
		"Eleanor", "Tom", "Sophie", "Ryan", "Charlotte", "Max", "Avery", "Isaac", "Lucy", "Nolan",
		"Leah", "Elijah", "Hannah", "Caleb", "Audrey", "Gabriel", "Stella", "Jordan", "Nora", "Evan",
		"Alexis", "Isaiah", "Lily", "Nicholas", "Grace", "Victor", "Maya", "Tristan", "Scarlett", "Dylan",
		"Aiden", "Peyton", "Isabelle", "Alex", "Xavier", "Sophia", "Jaxon", "Aria", "Carter", "Mila",
		"Morgan", "Quinn", "Landon", "Eva", "Piper", "Gideon", "Zara", "Colton", "Aurora", "Jaden",
		"Nova", "Cameron", "Delilah", "Owen", "Willow", "Ezekiel", "Hazel", "Gemma", "Bentley", "Isla",
		"Xander", "Daisy", "Maxwell", "Nina", "Ezra", "Sadie", "Kai", "Lila", "Brayden", "Evelyn"
	};
	public static string[] lastName = new string[]
	{
		"Anderson", "Andersen", "Brown", "Carter", "Davis", "Evans", "Fisher", "Garcia", "Hill", "Irwin", "Johnson",
		"Keller", "Lopez", "Miller", "Nelson", "Owens", "Perez", "Quinn", "Reyes", "Smith", "Taylor",
		"Underwood", "Vargas", "Williams", "Young", "Zimmerman", "Adams", "Baker", "Clark", "Diaz",
		"Edwards", "Fletcher", "Gomez", "Hall", "Ingram", "Jenkins", "Kim", "Lambert", "Morgan", "Nguyen",
		"O'Brien", "Parker", "Quigley", "Reid", "Stewart", "Tucker", "Underhill", "Valentine", "Woods", "Archer",
		"Bennett", "Cunningham", "Duncan", "Emerson", "Floyd", "Galloway", "Hawkins", "Jefferson", "Kendrick",
		"Montgomery", "Nash", "Ortega", "Pierce", "Roth", "Sullivan", "Thompson", "Vance", "Webster", "York",
		"Mama", "Vanderbilt", "Harrington", "Hendricks", "Winston", "Lawson", "Hudson", "Higgins", "Hendrix", "Thornton",
		"Barlow", "Beaumont", "Livingston", "Ramsey", "Winters", "Dalton", "Everett", "Wilder", "Fitzgerald", "Hayes",
		"Gallagher", "Lancaster", "Shepherd", "Langley", "Mercer", "Carmichael", "Dalton", "Fitzpatrick", "Hensley", "Sawyer",
		"McCarthy", "Callahan", "Monroe", "Morrison", "O'Donnell", "Farrell", "Quinn", "Redmond", "Sheridan", "Vaughn",
		"Wainwright", "Hartman", "Holloway", "Baxter", "Wynn", "McKenzie", "Fleming", "Tate", "Wolfe", "Irving",
		"Gibson", "Stevens", "Rowland", "Browning", "Gilliam", "Walsh", "Ward", "Hammond", "Wheeler", "Garrison"
	};
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum Skill
{
	CSharp,
	CPP,
	C,
	Java,
	JavaScript,
	HTML,
	CSS,
	Python,
	PHP,
	SQL,
	dotNET,
	React,
	Angular,
	Nodejs,
	DevOps,
	Docker,
}

public class SkillInfo
{
	public Skill skill;
	public float demand; // amount of orders with skill
	public string[] categories;
	public float supply; // chance of workers getting skill


	public float AveragedValue
	{
		get
		{
			var x = (demand / supply) / infos.Values.Average(i => i.Value);
			return Mathf.Pow(x - 1, 3) * 0.5f + 1; // clamp in weird way
		}
	}

	public float Value => demand / supply;

	public static Dictionary<Skill, SkillInfo> infos = new();

	public static void Init()
	{
		infos.Clear();
		foreach (var skill in (Skill[])Enum.GetValues(typeof(Skill)))
		{
			infos.Add(skill, new SkillInfo(skill));
		}
	}

	public SkillInfo(Skill skill)
	{
		this.skill = skill;

		categories = skillCategories[skill];

		demand = OrderList.list.Where(l => l.skills.Contains(skill)).Sum(l => l.workPoints);

		supply = categories.Sum(c => skillCategories.Values.Count(v => v.Contains(c)));
	}

	public static Dictionary<Skill, string[]> skillCategories = new()
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
}

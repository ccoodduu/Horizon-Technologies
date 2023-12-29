using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
	public static T Random<T>(this IEnumerable<T> e) => e.ElementAt(UnityEngine.Random.Range(0, e.Count()));

	public static string ToDisplayText(this Skill skill)
	{
		return skill switch
		{
			Skill.CSharp => "C#",
			Skill.CPP => "C++",
			Skill.dotNET => ".NET",
			Skill.Nodejs => "Node.js",
			_ => skill.ToString(),
		};
	}

	public static string ToSkillString( this IEnumerable<Skill> skills) => string.Join(", ", skills.Select(s => s.ToDisplayText()));
}

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
		switch (skill)
		{
			case Skill.CSharp:
				return "C#";
			case Skill.CPP:
				return "C++";
			case Skill.dotNET:
				return ".NET";
			case Skill.Nodejs:
				return "Node.js";
			default:
				return skill.ToString();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multipliers : MonoBehaviour
{
    [Header("Employee")]
    public float baseHappiness = 0.8f;
    public float salaryHappinessMultiplier = 1f;
	public float skillHappinessMultiplier = 1.2f;
    [Space(5)]
    public float baseSpeed = 1f;
    public float happinessSpeedMultiplier = 1f;
	public float experienceSpeedMultiplier = 1.1f;
	public float employmentTimeSpeedMultiplier = 1.1f;
	public float ageSpeedMultiplier = 0.99f;
    [Space(5)]
    public float minHappiness = 0.5f;

	//[Header("Order")]

	public static Multipliers i;

    void Awake()
    {
        i = this;
    }
}

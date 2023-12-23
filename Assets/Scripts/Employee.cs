using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee
{
    public int salary;
    public string name_;
    public int age;
    public DateTime employedSince;
    public DateTime workedSince; // experience
	public TimeSpan experience => Game.i.time.Subtract(workedSince);

    public bool hasDesk;

	public EmployeeLooks looks;

    public static Employee You => new Employee("You", 0, 21, TimeSpan.Zero);
    
    public static Employee Generate()
    {
        return You;
    }

	public Employee(string name, int salary, int age, TimeSpan experience)
	{
        name_ = name;
        this.salary = salary;
        this.age = age;
        workedSince = Game.i.time.Subtract(experience);

        looks = EmployeeLooks.RandomLooks();
	}
}

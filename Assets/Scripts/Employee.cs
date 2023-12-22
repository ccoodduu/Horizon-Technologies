using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public int salary;
    public string name_;
    public int age;
	public int yearsOfExperience;

	public EmployeeLooks looks;

    public static Employee You => new Employee("You", 0, 21);
    
    public Employee() 
    { 
        
    }

	public Employee(string name, int salary, int age)
	{
        
	}
}

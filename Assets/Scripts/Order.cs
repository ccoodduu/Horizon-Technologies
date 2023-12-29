using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Order
{
	public float difficultyMultiplier; // 1 - 5

	public OrderDescription orderDescription;
	public string ownerName;
	public DateTime deadline;
	public List<Employee> assignedEmployees;
	public float workedPoints;

	public int Payment => Mathf.RoundToInt(orderDescription.payment * ((difficultyMultiplier - 1) * 2 + 1));

	public float Completion => workedPoints / orderDescription.workPoints;

	public static Order Generate()
	{
		var order = OrderList.list.Random();

		var difficulty = Random.Range(1f, Game.i.Reputation / 2);

		return new Order(order, difficulty);
	}

	public Order(OrderDescription orderDescription, float difficultyMultiplier = 1.0f)
	{
		this.difficultyMultiplier = difficultyMultiplier;
		this.orderDescription = orderDescription;
		ownerName = NameGenerator.GenerateName();
		deadline = Game.i.time + TimeSpan.FromHours(
			(orderDescription.workPoints / Game.i.dailyOfficeTime.TotalHours * 24) / 5 * 7 / difficultyMultiplier + 24 * 5
		); // Five days after a single employee could have finished the job in normal conditions
		deadline = deadline.Add(TimeSpan.FromDays(1) - deadline.TimeOfDay); // Round to midnight

		assignedEmployees = new List<Employee>();
		workedPoints = 0;
	}

	// Get the amount of workPoints pr. hour
	public float GetWorkingSpeed()
	{
		var skillCounts = new Dictionary<Skill, int>();
		foreach (var skill in (Skill[])Enum.GetValues(typeof(Skill))) skillCounts.Add(skill, 0);

		foreach (var employee in assignedEmployees)
		{
			if (!employee.canWork) continue;
			foreach (var skill in employee.skills) skillCounts[skill] += 1;
		}

		var speedMultiplier = 1.0f;

		foreach (var neededSkill in orderDescription.skills)
		{
			var count = skillCounts[neededSkill];

			if (count == 0) speedMultiplier *= .6f;
			speedMultiplier *= Mathf.Pow(1.2f, count - 1);
		}

		return speedMultiplier * assignedEmployees.Count;
	}

	public string FormatAsEmail()
	{
		var skillsTextString = "";
		foreach (var skill in orderDescription.skills)
		{
			if (skillsTextString != "") skillsTextString += ", ";
			skillsTextString += skill.ToDisplayText();
		}

		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine(orderDescription.longDescription);
		stringBuilder.Replace("[Order Requester's Name]", this.ownerName);
		stringBuilder.Replace("[Your Company]", Game.i.companyName);

		stringBuilder.AppendLine("");
		stringBuilder.AppendLine("Payment: " + orderDescription.payment + " $");
		stringBuilder.AppendLine("Required skills: " + skillsTextString);
		stringBuilder.AppendLine("Work points: " + orderDescription.workPoints + " wp");
		stringBuilder.AppendLine("Deadline: " + deadline.ToString("dd/MM/yyyy"));

		return stringBuilder.ToString();
	}
}

public struct OrderDescription
{
	public string name;
	public string shortDescription;
	public string longDescription;
	public Skill[] skills;
	public int workPoints; // About 1 hr.
	public int payment;
}

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

public static class OrderList
{
	public static Dictionary<int, int> frequencies = new Dictionary<int, int>();

	public static OrderDescription[] list = new OrderDescription[]
	{
		new OrderDescription
		{
			name = "Enterprise Resource Planning (ERP) System",
			shortDescription = "Develop a user-friendly ERP system integrating HR, finance, and inventory for streamlined internal processes.",
			longDescription = "Subject: Request for ERP System Development\n\nDear [Your Company],\n\nWe are in need of a skilled development team to create a user-friendly Enterprise Resource Planning (ERP) system. The system should seamlessly integrate Human Resources (HR), finance, and inventory modules to enhance our internal processes. The goal is to streamline operations, improve efficiency, and ensure a smooth workflow across departments. We look forward to your proposal and estimated timeline for this project.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.CSharp, Skill.SQL, Skill.dotNET, Skill.JavaScript },
			workPoints = 500,
			payment = 13000
		},
		new OrderDescription
		{
			name = "Customer Relationship Management (CRM) Software",
			shortDescription = "Build a customizable CRM system to track interactions, manage leads, and provide insightful analytics for enhanced sales processes.",
			longDescription = "Subject: Request for CRM Software Development\n\nDear [Your Company],\n\nWe are seeking expertise to build a customizable Customer Relationship Management (CRM) system. The system should be capable of efficiently tracking customer interactions, managing leads, and providing insightful analytics to enhance our sales processes. Customization options for specific business needs are a priority. We kindly request your team's proposal and timeline estimate for this project.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Java, Skill.SQL, Skill.HTML, Skill.CSS },
			workPoints = 300,
			payment = 9000
		},
		new OrderDescription
		{
			name = "Online Learning Platform",
			shortDescription = "Create an interactive learning platform with seamless course management, enrollment, and progress tracking.",
			longDescription = "Subject: Request for Online Learning Platform Development\n\nDear [Your Company],\n\nWe are interested in developing an interactive online learning platform. The platform should feature seamless course management, easy enrollment processes, and robust progress tracking capabilities. We are excited to receive your proposal and learn more about your team's approach to this project.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 400,
			payment = 8000
		},
		new OrderDescription
		{
			name = "Health Records Management System",
			shortDescription = "Develop a secure and compliant Health Records Management System with a user-friendly interface for healthcare professionals.",
			longDescription = "Subject: Request for Health Records Management System Development\n\nDear [Your Company],\n\nWe are reaching out to your team for the development of a secure and compliant Health Records Management System. The system should feature a user-friendly interface tailored to the needs of healthcare professionals. Data security and regulatory compliance are of utmost importance. We look forward to reviewing your proposal and discussing the project details.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Angular, Skill.dotNET, Skill.SQL },
			workPoints = 250,
			payment = 7000
		},
		new OrderDescription
		{
			name = "E-commerce Website",
			shortDescription = "Design a visually appealing e-commerce website with features for product listings, shopping cart, and secure payment processing.",
			longDescription = "Subject: Request for E-commerce Website Design and Development\n\nDear [Your Company],\n\nWe are looking for a skilled web development team to design and develop a visually appealing e-commerce website. The website should include features for product listings, a shopping cart, and secure payment processing. We are eager to see your creative approach to this project and request your proposal with estimated timelines.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.PHP, Skill.SQL, Skill.HTML, Skill.CSS },
			workPoints = 350,
			payment = 10000
		},
		new OrderDescription
		{
			name = "Instant Messaging Application",
			shortDescription = "Build a real-time messaging app with private chats, group conversations, and multimedia sharing for efficient team communication.",
			longDescription = "Subject: Request for Instant Messaging Application Development\n\nDear [Your Company],\n\nWe are interested in developing a real-time messaging application for efficient team communication. The application should include features such as private chats, group conversations, and multimedia sharing. Your team's expertise in creating intuitive and robust communication tools is crucial for the success of this project. We look forward to receiving your proposal.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 200,
			payment = 6000
		},
		new OrderDescription
		{
			name = "Microservices Migration",
			shortDescription = "Migrate our monolithic app to a microservices architecture using containerization for scalability and maintenance.",
			longDescription = "Subject: Request for Microservices Migration\n\nDear [Your Company],\n\nWe are seeking expertise in migrating our existing monolithic application to a microservices architecture. The migration should incorporate containerization for scalability and ease of maintenance. We believe your team's experience in this area will greatly benefit our organization. We look forward to your proposal and discussing the details further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Docker, Skill.Nodejs, Skill.Python },
			workPoints = 450,
			payment = 12000
		},
		new OrderDescription
		{
			name = "Personal Portfolio Website Redesign",
			shortDescription = "Redesign a personal portfolio website for a modern and professional look, emphasizing skills, projects, and achievements.",
			longDescription = "Subject: Request for Personal Portfolio Website Redesign\n\nDear [Your Company],\n\nWe are looking for a creative team to redesign a personal portfolio website. The redesign should give the website a modern and professional look, with a focus on highlighting skills, projects, and achievements. We are excited to see your design concepts and hear more about your approach to this project.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.HTML, Skill.CSS, Skill.JavaScript },
			workPoints = 60,
			payment = 4000
		},
		new OrderDescription
		{
			name = "Inventory Management System",
			shortDescription = "Develop a comprehensive inventory management system to track and manage products, orders, and stock levels.",
			longDescription = "Subject: Request for Inventory Management System Development\n\nDear [Your Company],\n\nWe are in need of a comprehensive inventory management system to track and manage products, orders, and stock levels. The system should be efficient and user-friendly, providing insights into our inventory processes. We look forward to your proposal and discussing the details of this project.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.CSharp, Skill.SQL, Skill.dotNET },
			workPoints = 300,
			payment = 8000
		},
		new OrderDescription
		{
			name = "Task Management App",
			shortDescription = "Build a task management application with features for creating, assigning, and tracking tasks within a team.",
			longDescription = "Subject: Request for Task Management App Development\n\nDear [Your Company],\n\nWe are interested in developing a task management application with features for creating, assigning, and tracking tasks within a team. The application should enhance collaboration and productivity. We eagerly await your proposal and look forward to discussing the project in more detail.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 180,
			payment = 6000
		},
		new OrderDescription
		{
			name = "Travel Booking Platform",
			shortDescription = "Design and implement a travel booking platform allowing users to search, book, and manage their travel plans.",
			longDescription = "Subject: Request for Travel Booking Platform Development\n\nDear [Your Company],\n\nWe are looking for a skilled team to design and implement a travel booking platform. The platform should allow users to search, book, and manage their travel plans seamlessly. Your expertise in creating user-friendly travel solutions is crucial for the success of this project. We look forward to your proposal.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Java, Skill.HTML, Skill.CSS },
			workPoints = 400,
			payment = 9000
		},
		new OrderDescription
		{
			name = "Financial Analytics Platform",
			shortDescription = "Develop a financial analytics platform with advanced reporting and data visualization capabilities for informed decision-making.",
			longDescription = "Subject: Request for Financial Analytics Platform Development\n\nDear [Your Company],\n\nWe are seeking expertise in developing a financial analytics platform. The platform should have advanced reporting and data visualization capabilities to support informed decision-making. We believe your team's experience in this area will greatly benefit our organization. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Java, Skill.JavaScript, Skill.SQL, Skill.React },
			workPoints = 350,
			payment = 10000
		},
		new OrderDescription
		{
			name = "Social Media Integration Module",
			shortDescription = "Integrate a social media module into an existing platform, allowing users to connect and share content seamlessly.",
			longDescription = "Subject: Request for Social Media Integration Module\n\nDear [Your Company],\n\nWe are interested in integrating a social media module into our existing platform. The module should allow users to connect and share content seamlessly. Your team's expertise in social media integration is essential for the success of this project. We look forward to your proposal and discussing the project details.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.PHP, Skill.JavaScript, Skill.CSS, Skill.SQL },
			workPoints = 200,
			payment = 6000
		},
		new OrderDescription
		{
			name = "Smart Home Automation System",
			shortDescription = "Create a smart home automation system with features for controlling lights, temperature, and security devices remotely.",
			longDescription = "Subject: Request for Smart Home Automation System Development\n\nDear [Your Company],\n\nWe are seeking expertise to develop a smart home automation system. The system should have features for controlling lights, temperature, and security devices remotely. Your team's experience in creating innovative and reliable home automation solutions is crucial for the success of this project. We look forward to your proposal and discussing the project details.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.CSharp, Skill.React, Skill.Nodejs, Skill.Docker },
			workPoints = 450,
			payment = 12000
		},
		new OrderDescription
		{
			name = "E-learning Content Authoring Tool",
			shortDescription = "Build an e-learning content authoring tool to create interactive and engaging educational materials for online courses.",
			longDescription = "Subject: Request for E-learning Content Authoring Tool Development\n\nDear [Your Company],\n\nWe are interested in developing an e-learning content authoring tool. The tool should enable the creation of interactive and engaging educational materials for online courses. Your team's expertise in e-learning solutions is crucial for the success of this project. We look forward to your proposal and discussing the project details.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.JavaScript, Skill.React, Skill.Nodejs, Skill.CSS },
			workPoints = 300,
			payment = 8000
		},
		new OrderDescription
		{
			name = "Event Management App",
			shortDescription = "Develop an event management application for planning, organizing, and tracking events with features for attendee registration.",
			longDescription = "Subject: Request for Event Management App Development\n\nDear [Your Company],\n\nWe are looking for a skilled team to develop an event management application. The application should assist in planning, organizing, and tracking events, with features for attendee registration. Your expertise in creating intuitive and robust event management solutions is crucial for the success of this project. We look forward to your proposal.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 280,
			payment = 7000
		},
		new OrderDescription
		{
			name = "Real-time Collaboration Platform",
			shortDescription = "Build a real-time collaboration platform that enables users to work together on documents, projects, and tasks seamlessly.",
			longDescription = "Subject: Request for Real-time Collaboration Platform Development\n\nDear [Your Company],\n\nWe have an exciting project that requires your expertise in developing a real-time collaboration platform. The platform should empower users to seamlessly collaborate on documents, projects, and tasks. Your proficiency in Java, JavaScript, React, Node.js, and CSS is essential for the success of this project. We eagerly await your proposal and look forward to discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Java, Skill.JavaScript, Skill.React, Skill.Nodejs, Skill.CSS },
			workPoints = 400,
			payment = Random.Range(7000, 9000)
		},
		new OrderDescription
		{
			name = "AI-Powered Chatbot",
			shortDescription = "Develop an AI-powered chatbot to enhance customer support services by providing automated and intelligent responses.",
			longDescription = "Subject: Request for AI-Powered Chatbot Development\n\nDear [Your Company],\n\nWe have an intriguing project that requires your expertise in developing an AI-powered chatbot. The chatbot aims to enhance our customer support services by providing automated and intelligent responses. Your skills in Python, JavaScript, and Node.js are crucial for this task. We anticipate your proposal and are excited to delve into further discussions.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Python, Skill.JavaScript, Skill.Nodejs },
			workPoints = 240,
			payment = Random.Range(5000, 7000)
		},
		new OrderDescription
		{
			name = "Supply Chain Management System",
			shortDescription = "Design and implement a supply chain management system to optimize logistics, inventory, and order fulfillment processes.",
			longDescription = "Subject: Request for Supply Chain Management System Development\n\nDear [Your Company],\n\nWe have an exciting project that requires a skilled developer to design and implement a cutting-edge supply chain management system. Your expertise in C#, SQL, and .NET will play a crucial role in optimizing logistics, inventory, and order fulfillment processes. With a substantial project scope of 500 work points and a payment ranging from $11000 to $13000, we eagerly await your proposal and look forward to discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.CSharp, Skill.SQL, Skill.dotNET },
			workPoints = 500,
			payment = Random.Range(11000, 13000)
		},
		new OrderDescription
		{
			name = "Mobile Health App",
			shortDescription = "Create a mobile health application with features for tracking fitness activities, monitoring health metrics, and setting wellness goals.",
			longDescription = "Subject: Mobile Health App Development\n\nDear [Your Company],\n\nWe are in need of a skilled developer to create a mobile health application. The app should include features for tracking fitness activities, monitoring health metrics, and setting wellness goals. Your expertise in React, Node.js, JavaScript, and CSS is crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.CSS },
			workPoints = 350,
			payment = 10000
		},
		new OrderDescription
		{
			name = "Automated Testing Framework",
			shortDescription = "Develop an automated testing framework to streamline the testing process and ensure the reliability of software applications.",
			longDescription = "Subject: Request for Automated Testing Framework Development\n\nDear [Your Company],\n\nWe are reaching out to enlist your expertise in developing an automated testing framework. The framework should streamline our testing process and ensure the reliability of our software applications. Your skills in Java, JavaScript, Python, and CSS are critical for this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.Java, Skill.JavaScript, Skill.Python, Skill.CSS },
			workPoints = 200,
			payment = 7000
		},
		new OrderDescription
		{
			name = "Task Management App",
			shortDescription = "Build a task management app with features for creating, assigning, and tracking tasks.",
			longDescription = "Subject: Task Management App Development\n\nDear [Your Company],\n\nWe are excited to invite your team to develop a task management app for us. The app should have features for creating, assigning, and tracking tasks efficiently. Your expertise in React, Node.js, JavaScript, HTML, and CSS is crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 180,
			payment = 6000
		},
		new OrderDescription
		{
			name = "Personal Blog Platform",
			shortDescription = "Create a simple and intuitive platform for users to start and manage personal blogs.",
			longDescription = "Subject: Personal Blog Platform Development\n\nDear [Your Company],\n\nWe are in need of a developer to create a personal blog platform. The platform should provide users with a simple and intuitive way to start and manage their personal blogs. Your skills in PHP, HTML, and CSS are essential for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.PHP, Skill.HTML, Skill.CSS },
			workPoints = 120,
			payment = 5000
		},
		new OrderDescription
		{
			name = "Portfolio Website",
			shortDescription = "Develop a sleek and modern portfolio website to showcase skills and projects.",
			longDescription = "Subject: Portfolio Website Development\n\nDear [Your Company],\n\nWe are seeking a talented developer to redesign and develop a sleek and modern portfolio website. The website should effectively showcase skills and projects. Your expertise in HTML, CSS, and JavaScript is crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.HTML, Skill.CSS, Skill.JavaScript },
			workPoints = 60,
			payment = 4000
		},
		new OrderDescription
		{
			name = "Social Media Feed Widget",
			shortDescription = "Design and implement a widget for displaying a social media feed on external websites.",
			longDescription = "Subject: Social Media Feed Widget Development\n\nDear [Your Company],\n\nWe have a task that requires your expertise in designing and implementing a widget for displaying a social media feed on external websites. Your skills in JavaScript, CSS, and HTML are crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.JavaScript, Skill.CSS, Skill.HTML },
			workPoints = 80,
			payment = 5000
		},
		new OrderDescription
		{
			name = "Simple E-commerce Module",
			shortDescription = "Add a basic e-commerce module to an existing website for product listing and checkout.",
			longDescription = "Subject: E-commerce Module Development\n\nDear [Your Company],\n\nWe are reaching out to request your expertise in adding a basic e-commerce module to an existing website. The module should handle product listing and checkout. Your skills in PHP, SQL, HTML, and CSS are crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.PHP, Skill.SQL, Skill.HTML, Skill.CSS },
			workPoints = 150,
			payment = 6000
		},
		new OrderDescription
		{
			name = "Contact Form Integration",
			shortDescription = "Integrate a contact form into a website to enable user inquiries and feedback.",
			longDescription = "Subject: Contact Form Integration\n\nDear [Your Company],\n\nWe have a task that requires your expertise in integrating a contact form into a website. The form should enable user inquiries and feedback. Your skills in PHP, HTML, and CSS are crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.PHP, Skill.HTML, Skill.CSS },
			workPoints = 40,
			payment = 2000
		},
		new OrderDescription
		{
			name = "Weather App",
			shortDescription = "Build a simple weather application displaying current weather conditions and forecasts.",
			longDescription = "Subject: Weather App Development\n\nDear [Your Company],\n\nWe are seeking a skilled developer to build a simple weather application. The app should display current weather conditions and forecasts. Your skills in JavaScript, HTML, and CSS are crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 100,
			payment = 4000
		},
		new OrderDescription
		{
			name = "Survey Generator",
			shortDescription = "Develop a tool for creating and conducting online surveys with customizable questions.",
			longDescription = "Subject: Survey Generator Tool Development\n\nDear [Your Company],\n\nWe are in need of a developer to create a tool for creating and conducting online surveys with customizable questions. Your skills in React, Node.js, JavaScript, and CSS are crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.CSS },
			workPoints = 120,
			payment = 5000
		},
		new OrderDescription
		{
			name = "Basic Content Management System (CMS)",
			shortDescription = "Build a lightweight CMS for managing and updating website content.",
			longDescription = "Subject: Basic CMS Development\n\nDear [Your Company],\n\nWe are reaching out to request your expertise in building a lightweight Content Management System (CMS). The CMS should be capable of managing and updating website content. Your skills in PHP, SQL, HTML, and CSS are crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.PHP, Skill.SQL, Skill.HTML, Skill.CSS },
			workPoints = 200,
			payment = 6000
		},
		new OrderDescription
		{
			name = "Team Collaboration Board",
			shortDescription = "Create a collaborative board for teams to organize tasks and share updates.",
			longDescription = "Subject: Team Collaboration Board Development\n\nDear [Your Company],\n\nWe are excited to invite your team to develop a collaborative board for teams. The board should facilitate organizing tasks and sharing updates. Your expertise in React, Node.js, JavaScript, HTML, and CSS is crucial for the success of this project. We look forward to your proposal and discussing the project further.\n\nBest regards,\n[Order Requester's Name]",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			workPoints = 90,
			payment = 5000
		}
	};
}
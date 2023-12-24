using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OrderDescription
{
    public string name;
    public string description;
    public Skill[] skills;
    public int duration;
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
	public static OrderDescription[] list = new OrderDescription[]
	{
		new OrderDescription
		{
			name = "Enterprise Resource Planning (ERP) System",
			description = "Develop a user-friendly ERP system integrating HR, finance, and inventory for streamlined internal processes.",
			skills = new Skill[] { Skill.CSharp, Skill.SQL, Skill.dotNET, Skill.JavaScript },
			duration = 500
		},
		new OrderDescription
		{
			name = "Customer Relationship Management (CRM) Software",
			description = "Build a customizable CRM system to track interactions, manage leads, and provide insightful analytics for enhanced sales processes.",
			skills = new Skill[] { Skill.Java, Skill.SQL, Skill.HTML, Skill.CSS },
			duration = 300
		},
		new OrderDescription
		{
			name = "Online Learning Platform",
			description = "Create an interactive learning platform with seamless course management, enrollment, and progress tracking.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 400
		},
		new OrderDescription
		{
			name = "Health Records Management System",
			description = "Develop a secure and compliant Health Records Management System with a user-friendly interface for healthcare professionals.",
			skills = new Skill[] { Skill.Angular, Skill.dotNET, Skill.SQL },
			duration = 250
		},
		new OrderDescription
		{
			name = "E-commerce Website",
			description = "Design a visually appealing e-commerce website with features for product listings, shopping cart, and secure payment processing.",
			skills = new Skill[] { Skill.PHP, Skill.SQL, Skill.HTML, Skill.CSS },
			duration = 350
		},
		new OrderDescription
		{
			name = "Instant Messaging Application",
			description = "Build a real-time messaging app with private chats, group conversations, and multimedia sharing for efficient team communication.",
			skills = new Skill[] { Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 200
		},
		new OrderDescription
		{
			name = "Microservices Migration",
			description = "Migrate our monolithic app to a microservices architecture using containerization for scalability and maintenance.",
			skills = new Skill[] { Skill.Docker, Skill.Nodejs, Skill.Python },
			duration = 450
		},
		new OrderDescription
		{
			name = "Personal Portfolio Website Redesign",
			description = "Redesign a personal portfolio website for a modern and professional look, emphasizing skills, projects, and achievements.",
			skills = new Skill[] { Skill.HTML, Skill.CSS, Skill.JavaScript },
			duration = 60
		},
		new OrderDescription
		{
			name = "Inventory Management System",
			description = "Develop a comprehensive inventory management system to track and manage products, orders, and stock levels.",
			skills = new Skill[] { Skill.CSharp, Skill.SQL, Skill.dotNET },
			duration = 300
		},
		new OrderDescription
		{
			name = "Task Management App",
			description = "Build a task management application with features for creating, assigning, and tracking tasks within a team.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 180
		},
		new OrderDescription
		{
			name = "Travel Booking Platform",
			description = "Design and implement a travel booking platform allowing users to search, book, and manage their travel plans.",
			skills = new Skill[] { Skill.Java, Skill.HTML, Skill.CSS },
			duration = 400
		},
		new OrderDescription
		{
			name = "Financial Analytics Platform",
			description = "Develop a financial analytics platform with advanced reporting and data visualization capabilities for informed decision-making.",
			skills = new Skill[] { Skill.Java, Skill.JavaScript, Skill.SQL, Skill.React },
			duration = 350
		},
		new OrderDescription
		{
			name = "Social Media Integration Module",
			description = "Integrate a social media module into an existing platform, allowing users to connect and share content seamlessly.",
			skills = new Skill[] { Skill.PHP, Skill.JavaScript, Skill.CSS, Skill.SQL },
			duration = 200
		},
		new OrderDescription
		{
			name = "Smart Home Automation System",
			description = "Create a smart home automation system with features for controlling lights, temperature, and security devices remotely.",
			skills = new Skill[] { Skill.CSharp, Skill.React, Skill.Nodejs, Skill.Docker },
			duration = 450
		},
		new OrderDescription
		{
			name = "E-learning Content Authoring Tool",
			description = "Build an e-learning content authoring tool to create interactive and engaging educational materials for online courses.",
			skills = new Skill[] { Skill.JavaScript, Skill.React, Skill.Nodejs, Skill.CSS },
			duration = 300
		},
		new OrderDescription
		{
			name = "Event Management App",
			description = "Develop an event management application for planning, organizing, and tracking events with features for attendee registration.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 280
		},
		new OrderDescription
		{
			name = "Real-time Collaboration Platform",
			description = "Build a real-time collaboration platform that enables users to work together on documents, projects, and tasks seamlessly.",
			skills = new Skill[] { Skill.Java, Skill.JavaScript, Skill.React, Skill.Nodejs, Skill.CSS },
			duration = 400
		},
		new OrderDescription
		{
			name = "AI-Powered Chatbot",
			description = "Develop an AI-powered chatbot to enhance customer support services by providing automated and intelligent responses.",
			skills = new Skill[] { Skill.Python, Skill.JavaScript, Skill.Nodejs },
			duration = 240
		},
		new OrderDescription
		{
			name = "Supply Chain Management System",
			description = "Design and implement a supply chain management system to optimize logistics, inventory, and order fulfillment processes.",
			skills = new Skill[] { Skill.CSharp, Skill.SQL, Skill.dotNET },
			duration = 500
		},
		new OrderDescription
		{
			name = "Mobile Health App",
			description = "Create a mobile health application with features for tracking fitness activities, monitoring health metrics, and setting wellness goals.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.CSS },
			duration = 350
		},
		new OrderDescription
		{
			name = "Automated Testing Framework",
			description = "Develop an automated testing framework to streamline the testing process and ensure the reliability of software applications.",
			skills = new Skill[] { Skill.Java, Skill.JavaScript, Skill.Python, Skill.CSS },
			duration = 200
		},
		new OrderDescription
		{
			name = "Task Management App",
			description = "Build a task management app with features for creating, assigning, and tracking tasks.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 180
		},
		new OrderDescription
		{
			name = "Personal Blog Platform",
			description = "Create a simple and intuitive platform for users to start and manage personal blogs.",
			skills = new Skill[] { Skill.PHP, Skill.HTML, Skill.CSS },
			duration = 120
		},
		new OrderDescription
		{
			name = "Portfolio Website",
			description = "Develop a sleek and modern portfolio website to showcase skills and projects.",
			skills = new Skill[] { Skill.HTML, Skill.CSS, Skill.JavaScript },
			duration = 60
		},
		new OrderDescription
		{
			name = "Social Media Feed Widget",
			description = "Design and implement a widget for displaying a social media feed on external websites.",
			skills = new Skill[] { Skill.JavaScript, Skill.CSS, Skill.HTML },
			duration = 80
		},
		new OrderDescription
		{
			name = "Simple E-commerce Module",
			description = "Add a basic e-commerce module to an existing website for product listing and checkout.",
			skills = new Skill[] { Skill.PHP, Skill.SQL, Skill.HTML, Skill.CSS },
			duration = 150
		},
		new OrderDescription
		{
			name = "Contact Form Integration",
			description = "Integrate a contact form into a website to enable user inquiries and feedback.",
			skills = new Skill[] { Skill.PHP, Skill.HTML, Skill.CSS },
			duration = 40
		},
		new OrderDescription
		{
			name = "Weather App",
			description = "Build a simple weather application displaying current weather conditions and forecasts.",
			skills = new Skill[] { Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 100
		},
		new OrderDescription
		{
			name = "Survey Generator",
			description = "Develop a tool for creating and conducting online surveys with customizable questions.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.CSS },
			duration = 120
		},
		new OrderDescription
		{
			name = "Basic Content Management System (CMS)",
			description = "Build a lightweight CMS for managing and updating website content.",
			skills = new Skill[] { Skill.PHP, Skill.SQL, Skill.HTML, Skill.CSS },
			duration = 200
		},
		new OrderDescription
		{
			name = "Team Collaboration Board",
			description = "Create a collaborative board for teams to organize tasks and share updates.",
			skills = new Skill[] { Skill.React, Skill.Nodejs, Skill.JavaScript, Skill.HTML, Skill.CSS },
			duration = 90
		},
	};
}
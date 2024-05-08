# Courier Company Management System with Drones

## Overview
This C# project simulates software for managing a product courier company utilizing drones for deliveries. It includes features such as drone management (battery percentages, locations, carrying capacity), docking stations for charging, customer management (locations), and package management.

## Technologies Used
### Tech Stack: C#, .NET Framework, WPF, XAML, and more.
### Usage of:
- Object-Oriented Programming (OOP): Utilized to encapsulate behavior within objects and promote code reuse and maintainability.
- Multithreading: Employed to enhance application responsiveness and efficiency, enabling concurrent execution of tasks such as drone management and package delivery.
- Three-tier Architecture: Implemented to ensure separation of concerns between presentation, business logic, and data access layers, enhancing modularity and scalability.
- SOLID Principles: Adhered to principles such as Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion to foster robust, maintainable, and extensible code.
- Design Patterns:
    - Singleton: Applied to ensure a single instance of critical classes such as drone management and docking stations, promoting resource efficiency and consistency.
    - Factory: Utilized to abstract the creation of drone objects, facilitating flexible object creation and decoupling client code from concrete implementations.
- Design by Contract: Implemented to establish clear specifications and expectations for class behaviors, enhancing code reliability and facilitating easier maintenance.

## Installation
1. Clone the repository to your local machine.
2. Open the solution file (`CourierCompanyManagement.sln`) in Visual Studio.
3. Build the solution to restore packages and compile the project.

## Usage
1. Launch the application.
2. Use the provided GUI to navigate through the different functionalities:
    - Manage drones: View drone status, locations, and battery percentages.
    - Manage docking stations: Monitor charging status and available capacity.
    - Manage customers: Add, remove, or update customer information.
    - Manage packages: Assign packages to drones for delivery.
3. Interact with the application according to the on-screen instructions.

## Credits
- Developed by Shimon Zitrinboim 
- Special thanks to Yair Busso

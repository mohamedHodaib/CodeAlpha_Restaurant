# CodeAlpha_Restaurant

## Overview

CodeAlpha_Restaurant is a backend solution for restaurant management, developed in C#. It provides a comprehensive set of APIs and business logic for handling core restaurant operations such as reservations, inventory, menu management, table management, ordering, and customer data. The project is organized into multiple layers including API, Business Logic, and Data Access.

## Features

- **Menu Management:**  
  - Add, update, delete, and search menu items.
  - Filter menu items by category or availability.

- **Inventory Management:**  
  - Track and manage stock levels for ingredients and supplies.
  - Update supplier information, units, minimum stock levels, and quantities.
  - Identify low stock items and search inventory by name, supplier, or unit.

- **Reservation System:**  
  - Handle customer reservations.
  - Manage table assignments and availability.

- **Order Processing:**  
  - Manage customer orders and their status.
  - Integrate with inventory to update stock accordingly.

- **Table Management:**  
  - Track table availability.
  - Assign and update table statuses.

- **Customer Management:**  
  - Store and manage customer information for reservations and orders.

- **API with Swagger:**  
  - RESTful API endpoints exposed for all major functionalities.
  - Integrated Swagger UI for easy API testing and documentation.

## Technologies Used

- **Language:** C#
- **Framework:** ASP.NET Core Web API
- **Database:** (Assumed SQL Server based on Microsoft.Data.SqlClient usage)
- **Dependency Injection:** ASP.NET Core built-in DI
- **API Documentation:** Swagger / OpenAPI
- **Logging:** Console Logging via Microsoft.Extensions.Logging

## Project Structure

- `Restaurant.Api/`  
  ASP.NET Core Web API project exposing endpoints for restaurant operations.

- `Restaurant.Business/`  
  Business logic for handling menu, inventory, reservations, tables, and orders.

- `Restaurant.DataAccess/`  
  Data access layer for interacting with the database, handling CRUD operations for all entities.

- `Restaurant.DataAccess.DTOs/`  
  Data Transfer Objects used between data access and business layers.

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/mohamedHodaib/CodeAlpha_Restaurant.git
   ```

2. **Restore NuGet packages and build the project:**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Configure your connection string:**  
   Update the connection string in `appsettings.json` to point to your SQL Server instance.

4. **Run the API:**
   ```bash
   cd Restaurant.Api
   dotnet run
   ```

5. **Access Swagger UI:**  
   Navigate to `https://localhost:<port>/swagger` in your browser for API documentation and testing.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request. For major changes, open an issue first to discuss what you would like to change.

## License

This project is currently unlicensed. Please contact the repository owner for details if you wish to use this project.

## Author

- [mohamedHodaib](https://github.com/mohamedHodaib)

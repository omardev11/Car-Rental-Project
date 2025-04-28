# Car Rental Management System

## **Overview**
This project is a server-side car rental management system designed to manage vehicle bookings, customer accounts, staff operations, and payment workflows. It is structured with a 3-tier architecture and follows SOLID principles to ensure a clean and maintainable codebase.

## **Features**
### **Back-End**
- Built with **C#**, **ASP.NET Core**, and **SQL Server**.
- 3-tier architecture:
  - **Data Layer**: Handles database operations using **ADO.NET**.
  - **Business Layer**: Manages application logic.
  - **Presentation Layer**: Exposes a RESTful API.
- Implements Dependency Injection and SOLID principles:
  - Each service has an interface injected into the classes.
  - Services are registered in the Dependency Injection (DI) container.
- Provides 85+ API endpoints for:
  - Vehicle management
  - Customer and staff accounts
  - Booking and rental processes
  - Payment methods and transaction records
  - Admin approval workflows

### **API Documentation**
- Fully documented with **Swagger** for easy testing and integration.

## **Key Functionalities**
1. **Vehicle Management**:
   - Add, update, delete, and list vehicles.
2. **Booking and Rental Operations**:
   - Customers can book and rent vehicles, with admin approval and return processes.
3. **User Management**:
   - Customer and staff registration and login functionality.
4. **Payment Processing**:
   - Manage customer cards, payments, and view transaction histories.

## **Installation**
### Prerequisites
- Visual Studio
- SQL Server
- .NET SDK (for ASP.NET Core)
- A web browser (for Swagger UI testing)

## **Acknowledgment**
All architecture, API development, and database design were developed from scratch, focusing on clean code practices, scalability, and maintainability.

## **Feedback and Contributions**
Suggestions and improvements are always welcome! Feel free to fork the repository or open an issue.

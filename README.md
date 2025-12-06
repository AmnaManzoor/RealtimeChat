Chatly 💬
A modern, real-time chat application built with .NET Core (Clean Architecture) and Angular 18.
 Features
•	Real-time Messaging: Instant message delivery using SignalR.
•	Secure Authentication: User registration and login with JWT (JSON Web Tokens) and ASP.NET Core Identity.
•	Clean Architecture: Robust and scalable backend structure separating Domain, Application, Infrastructure, and API.
•	Modern UI: Responsive design built with Angular 18 and Tailwind CSS.
•	Swagger Documentation: Interactive API documentation for easy testing.
Tech Stack
Backend
•	.NET 10 (Web API)
•	Entity Framework Core (SQL Server)
•	SignalR (Real-time communication)
•	ASP.NET Core Identity (Authentication)
•	Clean Architecture pattern

Frontend
•	Angular 18 (Standalone Components)
•	Tailwind CSS (Styling)
•	SignalR Client
•	RxJS

Prerequisites

Before you begin, ensure you have the following installed:
•	[.NET SDK](https://dotnet.microsoft.com/download) (Version 10 or compatible)
•	[Node.js](https://nodejs.org/) (LTS version)
•	[SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or SQL Express)
Installation & Setup

 1. Clone the Repository
bash
git clone https://github.com/yourusername/chatly.git
cd chatly

2. Backend Setup
Navigate to the API directory:
    bash
    cd ChatlyApp.API
2.  Configure Database: Update the ‘ConnectionStrings’ in ‘appsettings.json’ if your SQL Server instance is different.
    json
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ChatlyDb;Trusted_Connection=True;MultipleActiveResultSets=true"

3.  Apply Migrations: Create the database and tables.
    bash
    dotnet ef database update --project ../ChatlyApp.Infrastructure --startup-project .
   
4.  Run the API:
    bash
    dotnet run --launch-profile http
    API URL: ‘http://localhost:5100’
    Swagger UI: ‘http://localhost:5100/swagger’
3. Frontend Setup
1.  Open a new terminal and navigate to the frontend directory:
    bash
    cd frontend
2.  Install Dependencies:
    ```bash
    npm install
    ```
3.  Start the App:
    bash
    npm start
4.  Open your browser and visit ‘http://localhost:4200’.

Usage
1.  Register: Create a new account on the registration page.
2.  Login: Sign in with your credentials.
3.  Chat: Start sending messages in real-time! Open the app in multiple tabs or browsers to test the real-time functionality.
Architecture Overview
The solution follows Clean Architecture principles to ensure separation of concerns and maintainability:
•	ChatlyApp.Domain: Contains enterprise logic and entities (e.g., ‘User’, ‘Message’). It has no dependencies.
•	ChatlyApp.Application: Contains business logic, DTOs, and interfaces. It depends only on the Domain layer.
•	ChatlyApp.Infrastructure: Implements interfaces (e.g., ‘AppDbContext’, Identity). It depends on the Application layer.
•	ChatlyApp.API: The entry point (Controllers, Hubs). It depends on Application and Infrastructure layers.



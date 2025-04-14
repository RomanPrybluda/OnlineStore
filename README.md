# Craft Sweets Online Store 

[Link to swagger page on test site](http://craft-sweets.runasp.net/swagger/index.html)

[Link to Kanban](https://github.com/users/gentlenestle/projects/1)

## Description

**Craft Sweets** is a RESTful API for an online sweets store, built with **ASP.NET Core 8.0**, **Entity Framework Core**, and **Identity**. The solution is structured with three main layers: `WebAPI`, `Domain`, and `DAL`.

---

## 🚀 Getting Started

### ✅ Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or any IDE with .NET support
- [Git](https://git-scm.com/)

---

### 📦 Installation

1. **Clone the repository:**
```bash
   git clone https://github.com/your-username/craft-sweets.git
   cd craft-sweets
```

2. Configure the connection string:
Set your local database connection string in either appsettings.json or using the Secret Manager:
```
dotnet user-secrets set "ConnectionStrings:LocalConnectionString" "Server=YOUR_SERVER_NAME;Database=CraftSweetDB;Trusted_Connection=True;Encrypt=False;"
```

3. Run the application:
```
dotnet run --project WebAPI
```
The application will automatically:
- Apply pending EF Core migrations
- Seed initial data for products, categories, and users

4. Open Swagger UI:
Visit https://localhost:5001/swagger to test API endpoints.

### 🧱 Project Structure

craft-sweets/
├── WebAPI/        # Web API project (controllers, middleware, services)
├── Domain/        # Domain models and business logic
├── DAL/           # Data Access Layer (DbContext, Migrations)
└── README.md      # Project documentation

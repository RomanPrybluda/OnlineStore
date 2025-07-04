# Craft Sweets Web API

[Link to swagger page on test site](http://craft-sweets.runasp.net/swagger/index.html)

[Link to Kanban](https://github.com/users/gentlenestle/projects/1)

[Link to Software Requirements Specification](https://docs.google.com/document/d/1hcFULwdvsAFvCo67wSuyIMmszhERiu9VX_tDqdbu9A0/edit?tab=t.0#heading=h.37fyoutx2o50)

[Site link](https://sweet-online-store.vercel.app/)

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
   ```

2. **Go to the project directory:**

   ```bash   
   cd craft-sweets
   ```

3. **Configure the connection string:**

   Set your local database connection string in either `appsettings.json` or using the [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

   ```bash
   dotnet user-secrets set "ConnectionStrings:LocalConnectionString" "Server=YOUR_SERVER_NAME;Database=CraftSweetDB;Trusted_Connection=True;Encrypt=False;"
   ```

4. **Run the application:**

   ```bash
   dotnet run --project WebAPI
   ```

   The application will automatically:
   - Apply pending EF Core migrations
   - Seed initial data for products, categories, and users

5. **Open Swagger UI:**

   Visit [https://localhost:5001/swagger](https://localhost:5001/swagger) to test API endpoints.

---

## 🧱 Project Structure

```
craft-sweets/
├── WebAPI/        # Web API project (controllers, middleware, services)
├── Domain/        # Domain models and business logic
├── DAL/           # Data Access Layer (DbContext, Migrations)
└── README.md      # Project documentation
```

---

## 🛠️ Built With

- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Swagger / Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Bogus](https://github.com/bchavez/Bogus) for generating fake seed data
- [Newtonsoft.Json](https://www.newtonsoft.com/json)

---

## 🔧 Configuration

You can configure the app using `user-secrets`. Example connection string:

```json
{
  "ConnectionStrings:LocalConnectionString": "Server=YOUR_SERVER;Database=CraftSweetDB;Trusted_Connection=True;Encrypt=False;"
}
```

---

## 🧪 Testing

There are currently no automated tests, but the project is structured to allow easy integration of:

- **xUnit** or **NUnit** for unit testing
- **Moq** for mocking dependencies
- **Testcontainers** for integration testing with SQL Server

---

## 📌 Useful Commands

- Add a migration:

  ```bash
  dotnet ef migrations add YourMigrationName --project DAL --startup-project WebAPI
  ```

- Apply migrations to the database:

  ```bash
  dotnet ef database update --project DAL --startup-project WebAPI
  ```

---

## 📖 Best Practices Followed

- Layered architecture (WebAPI / Domain / DAL)
- Clean separation of concerns
- Secure configuration via `user-secrets`
- Exception handling via middleware
- Swagger documentation for API
- CORS policy enabled for cross-origin requests
- Enum serialization using `JsonStringEnumConverter`
- Auto-detection of pending migrations on startup

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙋‍♂️ Author

**Your Name** — [@RomanPrybluda](https://github.com/RomanPrybluda)

Feel free to open issues or submit pull requests 💡

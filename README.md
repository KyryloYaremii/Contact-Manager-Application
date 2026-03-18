# Contact-Manager-Application

An application for managing employee data with the ability to upload, edit, and filter from CSV files.

## Tech Stack
- **Backend:** .NET 8, ASP.NET Core Web API / MVC
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Web)
- **Database:** Entity Framework Core (SQL Server / SQLite)
- **Frontend:** HTML5, CSS3 (Bootstrap 5), Vanilla JavaScript
- **Libraries:** CsvHelper (for parsing CSV), Serilog (logging)

## Features
- **Data Upload:** Import employee data from CSV files with automatic column mapping.
- **Record Management (CRUD):** View, edit, and delete employee data.
- **Interactive table:** - Live search by any column (name, date, marriage status, salary).
- Client-side data validation during editing.
- **Architecture:** Clear separation of responsibilities between the UI (MVC) and API layers.

 ## Installation and Run

1. **Clone the repository:**
```bash
git clone <url-to-your-repository>
```
2. **Set up the database:**
Check the connection string in appsettings.json and apply migrations:
```
Bash
dotnet ef database update --project src/ContactManagerApp.Infrastructure --startup-project src/ContactManagerApp.Web
```
3. **Run the project:**
```
Bash
dotnet run --project src/ContactManagerApp.Web
```
**CSV File Format**
The file should contain the following headers: Name, Date of birth, Married, Phone, Salary.
Recommended date format: yyyy-MM-dd.

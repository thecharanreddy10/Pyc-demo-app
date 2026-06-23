# PayrollApp

PayrollApp is an ASP.NET Core 8 Razor Pages payroll management application. It provides employee management, payroll processing, and payroll history storage using Entity Framework Core with MySQL.

## Features

- Add, edit, view, and delete employee records
- Process payroll for salaried and hourly employees
- Automatically calculate gross pay, tax, social security, health insurance, total deductions, and net pay
- Store payroll records in a MySQL database
- Seed sample employee data on first run

## Prerequisites

- .NET 8 SDK
- MySQL server
- A MySQL user that can create databases and tables

## Configuration

The application reads the MySQL connection string from `appsettings.json`.

Default connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=PayrollDB;User=payroll_app;Password=PayrollApp@2026;"
}
```

Update the values for your local environment if needed.

## Setup

1. Open a terminal in the `sample-pay` folder.
2. Restore NuGet packages:

```powershell
dotnet restore
```

3. Ensure your MySQL server is running and the configured user can connect.

4. Run the app:

```powershell
dotnet run
```

The app will automatically create the `PayrollDB` database if it does not already exist and seed initial employee data.

## Usage

- Open the application URL shown in the terminal, typically `https://localhost:5001` or `http://localhost:5000`.
- Navigate to the Employees page to manage employee information.
- Use the Payroll Process page to generate payroll records for selected employees.
- View payroll history in the Payroll History page.

## Project Structure

- `Program.cs` - Application startup and service configuration
- `Data/ApplicationDbContext.cs` - EF Core database context
- `Data/DbInitializer.cs` - Database creation and seed data logic
- `Models/Employee.cs` - Employee entity
- `Models/PayrollRecord.cs` - Payroll record entity
- `Services/PayrollCalculationService.cs` - Payroll calculation logic
- `Pages/Employees` - Employee management Razor Pages
- `Pages/Payroll` - Payroll processing and history pages

## Notes

- The app uses `Pomelo.EntityFrameworkCore.MySql` for MySQL database integration.
- Salaried employees are paid monthly as `AnnualSalary / 12`.
- Hourly employees use the entered `HoursWorked` value to calculate pay.
- The payroll calculation includes a fixed health insurance deduction of $200.

## Troubleshooting

- If the app cannot connect to MySQL, verify the connection string and that the server is reachable.
- Confirm the MySQL user has permission to create databases and tables.
- Check the terminal output for any EF Core or startup exceptions.

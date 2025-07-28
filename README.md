# 📊 RepoGen

A lightweight, developer-friendly Blazor Server application built with **Vertical Slice Architecture** to simplify the creation and management of internal SQL-based reports in corporate environments.

## ✨ Key Features

- 🔍 Custom SQL report execution using **Dapper**
- 📊 Responsive UI with **Radzen components** (including `DataGrid`)
- 🧩 Clean architecture via **Vertical Slice Architecture**
- 🔐 Authentication & Authorization with **Cookies**
- 🔑 Secure password hashing using **BCrypt**
- 🧾 Report export to `.xlsx` via **DocumentFormat.OpenXml**
- 📦 CQRS support using **MediatR**
- ✅ End-to-end testing with **xUnit**

---

## ⚙️ Tech Stack

| Layer                  | Technology                       |
|-----------------------|----------------------------------|
| Frontend              | Blazor Server + Radzen UI        |
| Backend               | .NET 8, Dapper, EF Core, MediatR |
| Auth                  | Cookie Authentication + BCrypt   |
| Reporting             | SQL (Dapper) + OpenXML (Export)  |
| Tests                 | xUnit                            |

---

## 🏗 Project Structure

/Context
- AppDbContext

/Entities
- Entity, EnumStatus, LogContent, Permission, User

/Interface
/Common
- Menus, Navs, Buttons, Layouts, Grids
/EntitiesSlices
- Auth
- User Management
- Permission Management
- Log Management
/ReportsSlices
- FakeModel

---

## 🚀 Adding a New Report

To create a new report:

0. **FakeModel** as an example on Project
1. **Add a button** to `ReportsMenu.razor` linking to the new report page.
2. Create a new folder under `/Interface/ReportsSlices`:
   - Add a `.razor` page (UI).
   - Add a handler (`IRequestHandler`) for SQL execution using Dapper.
3. Add tests in `/IntegrationTest` to validate the report.

---

## 🔐 Authentication & Security

- Login is handled via **Cookie Authentication**.
- Passwords are hashed securely with **BCrypt**.
- Authorization logic can be extended per report or role.

---

## 🧾 Report Export

Reports can optionally support export to Excel:

- Add an export button to the report page.
- Implement an export service using `DocumentFormat.OpenXml`.

---

## ✅ Testing

- All handlers and pages should have corresponding **integration tests**.
- Tests are written using **xUnit** for full coverage and safety.

---

## 📌 Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server or compatible RDBMS

---

## 📄 License

This project is proprietary and intended for internal corporate use only.
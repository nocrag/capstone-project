# 🧾 Capstone Project – Purchase Order & HR Management System

This is a full-stack enterprise application developed as part of my final Capstone project at NBCC. The system supports two main modules:

- **Purchase Order (PO) Management** – Developed by me
- **Human Resources (HR) Module** – Developed by my teammate

The project is designed to streamline internal workflows for submitting, approving, and managing purchase orders and employee records within a company.

---

## 🔧 Tech Stack

- **Backend:** ASP.NET Core MVC, C#
- **Database:** SQL Server (with stored procedures)
- **Frontend:** Razor Views, HTML/CSS
- **Other Tools:** ADO.NET, Git, GitHub, Visual Studio

---

## 📦 Purchase Order Module (My Contribution)

### ✅ Key Features

- Submit purchase orders with itemized details
- Approve, deny, and update PO statuses
- Edit individual PO items (quantities, descriptions, etc.)
- Search and filter by PO number and date range
- Role-based logic to list only POs created by the logged-in user
- Stored procedures for creating, retrieving, updating, and status setting
- Manual `DataRow`-to-object mapping for full control and clarity

### 💼 Responsibilities

- Designed the PO data model and relationships (ERD, UML)
- Implemented all repository, service, and controller layers
- Created and documented stored procedures
- Integrated frontend and backend using Razor Pages
- Added search, validation, and filtering logic
- Worked with a teammate using Git for version control and task coordination

---

## 📁 Folder Structure

```
CAPSTONE-PROJECT/
├── Design/
├── Mobile/
├── Naming Conventions/
│ └── README.MD
├── ProjectArtifacts/
│ ├── Sprint1/
│ │ ├── Database/
│ │ │ ├── DBCreationAndSeedScript.sql
│ │ │ ├── HR_StoredProc.sql
│ │ │ └── PurchaseOrderSPs.sql
│ │ └── Diagrams/
│ │ └── Images/
│ │ ├── Kilo_Diagrams_v1.0.vpp
│ │ ├── Kilo_Diagrams_v1.1.pdf
│ │ ├── ...
│ │ └── Kilo_ERD_v1.4.pdf
│ ├── Sprint2/
│ │ ├── Database/
│ │ │ └── DBCreationAndSeedScript.sql
│ │ └── Diagrams/
│ │ └── Images/
│ │ ├── Kilo_Diagrams_v2.0.vpp
│ │ ├── ...
│ │ └── Kilo_ERD_v2.1.pdf
│ └── Sprint3/
│ ├── Database/
│ └── Diagrams/
├── Web/
│ └── MJRecords/
│ ├── MJRecords.API/
│ ├── MJRecords.DAL/
│ ├── MJRecords.Model/
│ └── MJRecords.Repository/
```

---

## 🚀 Getting Started

Follow these steps to set up and run the project locally:

### 1. Clone the Repository

```bash
git clone https://github.com/nocrag/capstone-project
cd capstone-project
```

### 2. Open in Visual Studio

- Launch Visual Studio

- Open the solution file located under Web/MJRecords

### 3. Configure the Database

- Use the SQL scripts in ProjectArtifacts/Sprint1/Database/ to:

  - Create the database
  - Seed initial data
  - Set up stored procedures

### 4. Update appsettings.json

In the MJRecords.API project, update your appsettings.json with your local SQL Server connection string:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DB_NAME;Trusted_Connection=True;"
}
```

### 5. Run the Project

- Set MJRecords.WEB as the startup project
- Hit F5 or click Start Debugging

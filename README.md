# Full Stack Template

A reusable full-stack template built for rapid development using **ASP.NET Core**, **React + Vite**, and **PostgreSQL**.

## Backend

The backend provides a clean foundation for building RESTful APIs with ASP.NET Core.

### Features

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL support
- AutoMapper integration
- Role-based authorization for future scalability
- Cookie support
- Password hashing with BCrypt
- Swagger UI for API testing and documentation
- Organized architecture with:
  - Controllers
  - Models
  - DTOs
  - Mappers
  - Middleware

### Included Functionality

A complete User CRUD example demonstrating:

- Register
- Login
- View Users
- Delete Users

This serves as a reference implementation for building additional modules.

---

## Frontend

The frontend is built with **React**, **Vite**, and **TypeScript**, providing a scalable project structure.

### Features

- React + Vite architecture
- TypeScript support
- Integrated with the ASP.NET Core backend
- Example Login page
- API communication already configured
- Easy-to-extend folder structure for adding new pages and features

---

## Database

The project uses **PostgreSQL** running through **Docker**, allowing for a consistent development environment across different machines.

---

### Quick Start
## 1. Start PostgreSQL

Run from the project root.

```
docker compose up -d
```

Stop PostgreSQL.

```
docker compose down
```

Stop and remove the database volume (deletes all data).

```
docker compose down -v
```

Check running containers.
```
docker ps
```
View PostgreSQL logs.
```
docker compose logs -f
```

## 2. Backend

Navigate to the backend directory.
```
cd backend
```
Restore NuGet packages.
```
dotnet restore
```
Build the project.
```
dotnet build
```
Run the API.
```
dotnet run
```
Run with Hot Reload.
```
dotnet watch
```
Apply Entity Framework migrations.
```
dotnet ef database update
```
Create a new migration.
```
dotnet ef migrations add InitialCreate
```
Remove the latest migration.
```
dotnet ef migrations remove
```
Clean build artifacts.
```
dotnet clean
```

## 3. Frontend

Navigate to the frontend directory.
```
cd frontend
```
Install dependencies.
```
npm install
```
Start the development server.
```
npm run dev
```
Build for production.
```
npm run build
```
Preview the production build.
```
npm run preview
```
Lint the project.
```
npm run lint
```
### Typical Development Workflow

Start the database.
```
docker compose up -d
```
Start the backend.
```
cd backend
dotnet watch
```
In another terminal, start the frontend.
```
cd frontend
npm run dev

```
When finished, stop PostgreSQL.
```
docker compose down
```
---

## Purpose

This template is designed to eliminate repetitive project setup, allowing new projects to start with a production-ready foundation that can be easily extended for larger applications.
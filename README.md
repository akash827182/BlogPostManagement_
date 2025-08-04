# Blog Post Management API (Q3 Architecture)

## Introduction

A RESTful API for managing blog posts using **ASP.NET Core**, designed with a clean and scalable **Q3 N-Tier architecture**.

### Layers Overview
- `Q3.API`: Entry point. Controllers and middleware.
- `Q3.Business`: Business logic and service layer.
- `Q3.Data`: EF Core-based data access layer.
- `Q3.Shared`: DTOs, Interfaces, and Validators.
- `Q3.Integration`: External service logic (e.g., JWT handling).
- `Q3.AutoMapper`: Profiles for mapping between Entity ↔ DTO.

---

## Features

- ✅ JWT Authentication (Token generation in Integration Layer)
- ✅ Role-Based Access Control (User, Admin)
- ✅ CRUD operations on Blog Posts
- ✅ DTO validation using FluentValidation
- ✅ Layered separation using Q3 Architecture
- ✅ AutoMapper for object mapping
- ✅ Password hashing with BCrypt
- ✅ Global exception handling middleware
- ✅ Swagger UI for API testing

---

## Project Structure

```
BlogPostManagement/
├── Q3.API/
│   ├── Controllers/
│   ├── Middleware/
│   └── DI/
├── Q3.Business/
│   ├── BusinessServices/
│   └── IBusinessServices/
├── Q3.Data/
│   ├── Entities/
│   ├── Repository/
│   ├── IRepository/
│   └── DbContext/
├── Q3.Shared/
│   ├── DTO/
│   │   └── MainData/
│   ├── Validators/
│   │   └── MainData/
│   └── Interfaces/
├── Q3.Integration/
│   └── Services/
├── Q3.AutoMapper/
```

---

## Layer Responsibilities

| Layer          | Responsibility                                             |
|----------------|------------------------------------------------------------|
| **API**        | HTTP routing, controller logic, Swagger, middleware        |
| **Business**   | Core logic, validations, decision-making                   |
| **Data**       | Database interaction, EF repositories                      |
| **Shared**     | DTOs, interfaces, validators                               |
| **Integration**| External logic like JWT, future APIs                       |
| **AutoMapper** | Entity ↔ DTO conversions                                   |

---

## ⚙️ Setup

1. **Clone the Repo**
```bash
git clone https://yourrepo.com/project.git
cd project
```

2. **Update DB Connection in `appsettings.json`**
```json
"ConnectionStrings": {
  "BlogPostManagementDb": "Server=YOUR_SERVER;Database=BlogPostDb;Trusted_Connection=True;"
}
```

3. **Run Migrations**
```bash
dotnet ef database update --project Q3.Data --startup-project Q3.API
```

4. **Run the App**
```bash
dotnet run --project Q3.API
```

5. **Open Swagger**
```
https://localhost:<port>/swagger
```

---
## 🔐 Authentication & Authorization
|Role  |	Permissions
|------|-----------------------------------------------|
|User  |	Create blog, update/delete own posts only  |
|Admin |	Full access to all blogs                   |

## 🔐 Authentication Endpoints

| Method | Route               | Description           |
|--------|---------------------|-----------------------|
| POST   | `/api/auth/signup`  | Register new user     |
| POST   | `/api/auth/login`   | Login and get JWT     |
| GET    | `/api/auth/users`   | Get user list (JWT)   |

Add to protected routes:
```
Authorization: Bearer <your-token>
```

---

## 📑 Blog Post Endpoints

| Method | Route                    | Description            |
|--------|--------------------------|------------------------|
| GET    | `/api/blogposts`         | List all posts         |
| GET    | `/api/blogposts/{id}`    | Get single post        |
| POST   | `/api/blogposts`         | Create new post        |
| PUT    | `/api/blogposts/{id}`    | Update post            |
| DELETE | `/api/blogposts/{id}`    | Delete post            |

---

**To Add New Integration (e.g., External API or Email):**
1. Create service in `Q3.Integration`
2. Interface in `Q3.Shared.Interfaces`
3. Register in Dependency Injector

---

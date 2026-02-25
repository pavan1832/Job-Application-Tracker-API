# 📋 Job Application Tracker API

A production-ready REST API built with **ASP.NET Core 8**, **Entity Framework Core**, and **SQLite** for tracking job applications, companies, and interview rounds.

The project is live here: https://job-application-tracker-api-b9t2.onrender.com/index.html

---

## 🏗️ Architecture

```
JobTrackerAPI/
├── Controllers/          # HTTP layer — routes requests to services
├── Services/             # Business logic
│   └── Interfaces/       # Service contracts
├── Repositories/         # Data access layer (EF Core)
│   └── Interfaces/       # Repository contracts
├── Models/
│   ├── Entities/         # EF Core database entities
│   └── DTOs/             # Request/response objects
│       ├── Auth/
│       ├── Company/
│       ├── JobApplication/
│       └── Interview/
├── Mapping/              # AutoMapper profile
├── Middleware/           # Global exception handler
├── Extensions/           # JWT settings, claims helpers
├── Data/                 # DbContext
├── Migrations/           # EF Core migrations
└── Tests/                # Unit test examples
```

**Design Patterns Used:** Repository Pattern · Dependency Injection · DTO mapping with AutoMapper · Layered Architecture

---

## 🔑 Features

- **JWT Authentication** — register, login, role-based access (User / Admin)
- **Job Application CRUD** — create, read, update (PATCH), delete applications
- **Filtering & Pagination** — filter by status, search by keyword, sort, paginate
- **Company Management** — admin-managed company directory linked to applications
- **Interview Tracking** — nested interview rounds per application with type and result tracking
- **Auto-status promotion** — creating an interview round auto-updates application status to `Interviewing`
- **Global Exception Middleware** — structured JSON error responses for all exceptions
- **Swagger UI** — fully documented, interactive API explorer with JWT support
- **Health Check** — `/health` endpoint for monitoring

---

## 📡 API Endpoints

### Auth
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/auth/register` | ❌ | Register a new user |
| POST | `/api/auth/login` | ❌ | Login and get JWT token |

### Job Applications
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/jobapplications` | ✅ | Get all (paginated, filtered) |
| GET | `/api/jobapplications/{id}` | ✅ | Get by ID |
| POST | `/api/jobapplications` | ✅ | Create new |
| PATCH | `/api/jobapplications/{id}` | ✅ | Partial update |
| DELETE | `/api/jobapplications/{id}` | ✅ | Delete |

**Query Parameters for GET `/api/jobapplications`:**
```
?page=1&pageSize=10&status=Applied&searchTerm=engineer&sortBy=ApplicationDate&sortDescending=true
```
Available statuses: `Applied`, `Interviewing`, `Offer`, `Rejected`, `Withdrawn`, `Ghosted`

### Companies
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/companies` | ✅ | Get all companies |
| GET | `/api/companies/{id}` | ✅ | Get by ID |
| POST | `/api/companies` | 🔐 Admin | Create company |
| PATCH | `/api/companies/{id}` | 🔐 Admin | Update company |
| DELETE | `/api/companies/{id}` | 🔐 Admin | Delete company |

### Interview Rounds
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/jobapplications/{appId}/interviews` | ✅ | Get all rounds |
| GET | `/api/jobapplications/{appId}/interviews/{id}` | ✅ | Get by ID |
| POST | `/api/jobapplications/{appId}/interviews` | ✅ | Add interview round |
| PATCH | `/api/jobapplications/{appId}/interviews/{id}` | ✅ | Update round |
| DELETE | `/api/jobapplications/{appId}/interviews/{id}` | ✅ | Delete round |

### System
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/health` | ❌ | Health check |
| GET | `/` | ❌ | Swagger UI |

---

## 🚀 Local Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Any IDE (Visual Studio 2022, VS Code with C# extension, or Rider)

### Steps

```bash
# 1. Clone the repo
git clone https://github.com/your-org/job-tracker-api.git
cd job-tracker-api

# 2. Install dependencies
dotnet restore

# 3. Update appsettings.json (change the JWT secret for production!)
# JwtSettings:SecretKey must be at least 32 characters

# 4. Apply migrations (creates SQLite DB automatically)
dotnet ef database update

# 5. Run the API
dotnet run

# 6. Open Swagger UI
# Navigate to: http://localhost:5000
```

### Default Admin Credentials (seed data)
```
Email:    admin@jobtracker.com
Password: Admin@12345
```

### Example Workflow

```bash
# Register a new user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Jane","lastName":"Doe","email":"jane@test.com","password":"Pass@12345"}'

# Login and get token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"jane@test.com","password":"Pass@12345"}'

# Create a job application (use token from above)
curl -X POST http://localhost:5000/api/jobapplications \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "jobTitle": "Senior Backend Engineer",
    "companyName": "Acme Corp",
    "jobLocation": "Remote",
    "applicationDate": "2024-06-01T00:00:00Z",
    "status": "Applied",
    "notes": "Applied through LinkedIn"
  }'

# Add an interview round
curl -X POST http://localhost:5000/api/jobapplications/1/interviews \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "interviewDate": "2024-06-15T10:00:00Z",
    "interviewType": "Technical",
    "interviewer": "John Smith",
    "notes": "Focus on system design"
  }'
```

---

## 🐳 Docker

```bash
# Build the image
docker build -t job-tracker-api .

# Run locally
docker run -p 8080:8080 \
  -e JwtSettings__SecretKey="YourProductionSecretKeyHere!" \
  -e ConnectionStrings__DefaultConnection="Data Source=/data/JobTracker.db" \
  -v $(pwd)/data:/data \
  job-tracker-api

# Open Swagger UI at http://localhost:8080
```

---

## ☁️ Azure Deployment

### Option 1: Azure App Service

```bash
# Install Azure CLI and login
az login

# Create resource group
az group create --name rg-jobtracker --location eastus

# Create App Service plan
az appservice plan create \
  --name plan-jobtracker \
  --resource-group rg-jobtracker \
  --sku B1 \
  --is-linux

# Create Web App
az webapp create \
  --resource-group rg-jobtracker \
  --plan plan-jobtracker \
  --name app-jobtracker-api \
  --runtime "DOTNETCORE:8.0"

# Configure environment variables
az webapp config appsettings set \
  --resource-group rg-jobtracker \
  --name app-jobtracker-api \
  --settings \
    JwtSettings__SecretKey="YourStrongProductionSecret!" \
    JwtSettings__Issuer="JobTrackerAPI" \
    JwtSettings__Audience="JobTrackerClients" \
    ASPNETCORE_ENVIRONMENT="Production"

# Deploy from local (or use GitHub Actions for CI/CD)
dotnet publish -c Release -o ./publish
cd publish
zip -r ../deploy.zip .
az webapp deployment source config-zip \
  --resource-group rg-jobtracker \
  --name app-jobtracker-api \
  --src ../deploy.zip
```

### Option 2: Azure Container Apps (Docker)

```bash
# Push to Azure Container Registry
az acr create --resource-group rg-jobtracker --name acrjobtracker --sku Basic
az acr login --name acrjobtracker
docker tag job-tracker-api acrjobtracker.azurecr.io/job-tracker-api:latest
docker push acrjobtracker.azurecr.io/job-tracker-api:latest

# Create Container App
az containerapp create \
  --name app-jobtracker \
  --resource-group rg-jobtracker \
  --image acrjobtracker.azurecr.io/job-tracker-api:latest \
  --target-port 8080 \
  --ingress external \
  --env-vars JwtSettings__SecretKey="secret" JwtSettings__Issuer="JobTrackerAPI"
```

> **Production Note:** For Azure, replace SQLite with **Azure SQL Database** by changing the connection string and switching the EF provider from `UseSqlite` to `UseSqlServer` in `Program.cs`.

---

## 🧪 Running Tests

```bash
# From test project directory
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 🔒 Security Notes

- Change `JwtSettings:SecretKey` before deploying to production — use at least 32 random characters
- Store secrets in **Azure Key Vault** or environment variables — never commit them to source control
- For production, switch to **Azure SQL** instead of SQLite
- Passwords are hashed using **BCrypt** with a cost factor of 11

---

## 📦 Tech Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| ASP.NET Core | 8.0 | Web framework |
| Entity Framework Core | 8.0 | ORM |
| SQLite | - | Local dev database |
| AutoMapper | 13.x | DTO mapping |
| BCrypt.Net | 4.x | Password hashing |
| JWT Bearer | 8.0 | Authentication |
| Swashbuckle | 6.x | Swagger/OpenAPI docs |
| xUnit + Moq | - | Unit testing |

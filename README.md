# AppointmentIngestionApp

End-to-end appointment ingestion system built for a case study. Includes a .NET 8 Web API with validation and layered architecture, automated tests, and a React + TypeScript frontend for data submission and live appointment listing.

## 📋 Table of Contents

- [Architecture Overview](#architecture-overview)
- [Prerequisites](#prerequisites)
- [Backend Setup](#backend-setup)
- [Frontend Setup](#frontend-setup)
- [Running Tests](#running-tests)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Time Breakdown](#time-breakdown)

## 🏗️ Architecture Overview

**Clean Layered Architecture:**
- **API Layer** (`AppointmentIngestion.Api`): ASP.NET Core Web API with controllers and middleware
- **Services Layer** (`AppointmentIngestion.Services`): Business logic, validation, and DTOs
- **Repository Layer** (`AppointmentIngestion.Data`): Core Entities and Database implementation

**Frontend:**
- React 18 + TypeScript
- Zod for schema validation
- React Hook Form for form management
- Tailwind CSS for styling
- Custom hooks for state management

## 📦 Prerequisites

### Backend
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 / VS Code / Rider (optional)

### Frontend
- [Node.js 18+](https://nodejs.org/) and npm
- Any modern code editor

## 🚀 Backend Setup

### 1. Clone the Repository
git clone <repository-url> cd AppointmentIngestionApp
cd backend/AppointmentIngestionApp


### 2. Restore Dependencies
dotnet restore

### 3. Build the Solution
dotnet build


### 4. Run the API
cd src/AppointmentIngestion.Api
dotnet run


The API will start at:
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `http://localhost:5000/swagger`

### Alternative: Run from Visual Studio
1. Open `AppointmentIngestionApp.sln`
2. Set `AppointmentIngestion.Api` as startup project
3. Press `F5` or click "Start Debugging"

## 🎨 Frontend Setup

### 1. Navigate to Frontend Directory
cd frontend


### 2. Install Dependencies
npm install


### 3. Configure Environment Variables
Edit `.env` if you need to change the url

### 4. Run Development Server
npm run dev


The frontend will start at:
- **Local**: `http://localhost:3000`

### 5. Build for Production (Optional)


## 🧪 Running Tests

### Backend Tests

**Run All Tests:**
dotnet test

**Run with Detailed Output:**
dotnet test --logger "console;verbosity=detailed"


**Run with Code Coverage:**
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover


## 📚 API Documentation

### Endpoints

**Base URL:** `http://localhost:5000/api`

#### Create Appointment
POST /api/appointments/ingest Content-Type: application/json
{ "clientName": "John Doe", "appointmentTime": "2025-01-15T14:30:00.000Z", "serviceDurationMinutes": 60 }


**Response (201 Created):**
{ "id": 1, "message": "Appointment created successfully." }


#### Get All Appointments
GET /api/appointments

**Response (200 OK):**
[ { "id": 1, "clientName": "John Doe", "appointmentTime": "2025-01-15T14:30:00Z", "serviceDurationMinutes": 60, "status": "Scheduled" } ]


#### Get Appointment by ID
GET /api/appointments/{id}
{ "id": 1, "clientName": "John Doe", "appointmentTime": "2025-01-15T14:30:00Z", "serviceDurationMinutes": 60, "status": "Scheduled" }


## ⏱️ Time Breakdown

**Total Time: ~6 hours**

| Task | Duration | Details |
|------|----------|---------|
| **Backend Development** | 5.5 hours | - Project structure setup<br>- Domain entities and repositories<br>- Service layer with validation<br>- API controllers and error handling<br>- Problem Details integration |
| **Frontend Development** | 3 hours | - React + TypeScript setup<br>- Component architecture<br>- API integration with type safety<br>- Form validation with Zod<br>- Responsive UI with Tailwind |
| **Testing** | 2.5 hour | - Unit tests for services<br>- Controller tests with mocking<br>- Validation tests<br>- Test fixtures and data |
| **Documentation** | 0.5 hours | - README.md<br>- JUSTIFICATION.md<br>- Code comments<br>- API documentation |
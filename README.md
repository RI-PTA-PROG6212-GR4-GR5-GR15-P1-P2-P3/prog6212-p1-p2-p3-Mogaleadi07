# RaceDay - Event Management System

## Project Overview

RaceDay is a full-stack event management system purpose-built for South African road running, walking, and cycling events. It addresses the complete event lifecycle: organisers can create events of any size, define category divisions for different participant groups, manage enrolments, and record race results with participant positions and finish times. Participants benefit from a centralized platform to discover upcoming events, view event details and routes, enrol in their chosen category, and maintain a personal race history. 

The system enforces data integrity through role-based access control, ensuring event organisers cannot accidentally compete in their own races and participants cannot register twice for the same category. Furthermore, the frontend and backend are completely decoupled, with the MVC application having zero direct database access.

This repository contains both Part 1 (planning, ERD, SQL schema), Part 2 (the working ASP.NET Core Web API), and Part 3 (the ASP.NET Core MVC Frontend) integrated into a single deliverable.

---

TABLE OF CONTENTS

1. User Roles
2. Database Schema
3. Key Relationships
4. API Endpoints
5. System Architecture (MVC, API, Blob Storage)
6. Setup and Run Instructions (Local & Docker)
7. Authentication and Role-Based Access
8. CI/CD Pipeline
9. Video Walkthrough

---

## 1. User Roles

### Event Organiser
Who: A user responsible for creating and managing road events.
Tasks:
- Create new events (races, walks, cycles)
- Define event categories (age groups, distance types)
- View all participants enrolled in their events
- Record participant finish times and positions
- Upload event banner images (stored in Azure Blob Storage)

### Participant
Who: A user competing in road events.
Tasks:
- Browse all available events on the MVC frontend
- View event details, categories, and banner images
- Enrol in events and select a category
- View their own enrolment history and race results

---

## 2. Database Schema

The system uses SQL Server with Entity Framework Core (Code-First approach).

USER
Base user account with authentication credentials and role assignment. Central table linking all other entities.

EVENT_ORGANISER
Users authorized to create and manage events. Linked to USER table.

PARTICIPANT
Users competing in events. Linked to USER table and tracks all event enrolments.

EVENT
Race event details (name, location, date, distance, type, banner URL). Created by organisers.

EVENT_CATEGORIES
Event divisions for grouping participants. One event can have multiple categories.

EVENT_ENROLMENT
Links participants to event categories. Tracks signup date and status. Prevents duplicate enrolments.

ROUTE
Event-specific route information. One-to-one relationship with events.

RESULTS
Race outcome records (finish time, position, date). Linked to enrolments.

---

## 3. Key Relationships

- 1 User can be 1 EventOrganiser OR 1 Participant
- 1 EventOrganiser creates multiple Events
- 1 Event has 1 Route
- 1 Event contains multiple Categories
- Multiple Participants enrol in multiple Events via EVENT_ENROLMENT

---

## 4. API Endpoints

18 total endpoints across 7 functional areas:
- Authentication (2)
- User Profile (3)
- Events (5)
- Categories (2)
- Enrolments (3)
- Results (2)
- Route (1)

---

## 5. System Architecture

The application is split into two main projects:

1. RaceDayAPI (Backend): An ASP.NET Core Web API that handles all business logic, JWT authentication, and direct database communication via Entity Framework Core.
2. RaceDayMVC (Frontend): An ASP.NET Core MVC application that consumes the API via HTTP. It contains NO direct database access. It maintains user state using Session Management.

Image Handling:
Images uploaded by Organisers are sent from the MVC app to the API. The API uses the Azure.Storage.Blobs SDK to upload the image. For local development, Azurite (a local Azure Storage Emulator) is used.

---

## 6. Setup and Run Instructions

### Option A: Running Locally (Visual Studio)

PREREQUISITES:
- Visual Studio 2022 (17.8+)
- .NET 8.0 SDK
- SQL Server & SSMS
- Azurite (Azure Storage Emulator)

STEP 1 - CLONE THE REPOSITORY
git clone https://github.com/<your-username>/raceday-api.git
cd raceday-api

STEP 2 - OPEN IN VISUAL STUDIO
Open the solution file in Visual Studio 2022 and allow NuGet packages to restore.

STEP 3 - CONFIGURE THE CONNECTION STRINGS & JWT
In RaceDayAPI/appsettings.Development.json, verify the database connection and Azurite string:
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RACEDAYEVENT;Trusted_Connection=True;TrustServerCertificate=True;",
  "AzureStorage": "UseDevelopmentStorage=true"
}

Ensure the Jwt configuration block is present in appsettings.json.

STEP 4 - START AZURITE
Open a terminal and run:
azurite
(Leave this terminal open in the background).

STEP 5 - CREATE THE DATABASE
In the Package Manager Console (Default project: RaceDayAPI), run:
Add-Migration InitialCreate
Update-Database
(Note: The API is configured to auto-migrate on startup via Program.cs).

STEP 6 - RUN BOTH PROJECTS
Set both RaceDayAPI and RaceDayMVC as "Multiple Startup Projects" in Visual Studio.
Press F5. The API will open Swagger, and the MVC app will open in your browser.

### Option B: Running via Docker (Frontend Containerized)

The MVC frontend is containerized using Docker. It communicates with the API running on the host machine via host.docker.internal.

PREREQUISITES:
- Docker Desktop installed and running.
- The RaceDayAPI must be running locally on your machine (e.g., on http://localhost:5276).

STEP 1 - BUILD THE DOCKER IMAGE
Open a terminal in the root directory (where the Dockerfile and docker-compose.yml are located):
docker build -t raceday-mvc -f RaceDayMVC/Dockerfile .

STEP 2 - RUN THE CONTAINER
Run the container, passing the API URL via environment variables:
docker run -d -p 8080:8080 --name raceday-mvc-container -e ApiSettings__BaseUrl="http://host.docker.internal:5276/" raceday-mvc

STEP 3 - ACCESS THE APP
Open your browser and go to: http://localhost:8080

---

## 7. Authentication and Role-Based Access

- Passwords are hashed with BCrypt before storage.
- On login, the API issues a JSON Web Token (JWT) containing the user's ID and Role.
- The MVC app stores this JWT in Session.
- The MVC app sends the token in the Authorization header on every request.
- Protected API endpoints validate the token and enforce Roles (e.g., [Authorize(Roles = "Organiser")]).

---

## 8. CI/CD Pipeline

The project uses GitHub Actions for Continuous Integration. The pipeline automatically builds all projects, restores NuGet packages, and runs unit tests whenever code is pushed to the repository.

CI/CD Pipeline Screenshots:
Part 1:
<img width="1366" height="768" alt="Screenshot 2026-09-04 094638" src="https://github.com/user-attachments/assets/743c3192-f294-49a7-b2f9-47859f014d9d" />

Part 2:
<img width="706" height="188" alt="RaceDayAPI CI Screenshot" src="https://github.com/user-attachments/assets/95724a55-5fec-4f3f-a75e-24ad2f78c8b5" />

Part 3:
<img width="635" height="185" alt="image" src="https://github.com/user-attachments/assets/8755d857-bd29-4936-af05-88f5971b7dde" />


---

## 9. Video Walkthrough

Part 1: The Video Walkthrough explains the planning documents from the entity relationship diagrams (ERDs), API Endpoints and the SQL Scripts in the SQL Server Management System (SSMS). 
YouTube Link: https://youtu.be/m_7JQy6tCgQ 

Part 2: The Video Walkthrough explains the working ASP.NET Core Web API implementation.
YouTube Link: https://youtu.be/MrJm40hASVk

Part 3: The Video Walkthrough explains the MVC implementation, the organiser and participant dashboards, and how the RaceDay website works.
YouTube Link: https://youtu.be/EHZhL4nLhCY

# RaceDay - Event Management System

## Project Overview

RaceDay is a full-stack event management system purpose-built for South 
African road running, walking, and cycling events. It addresses the complete 
event lifecycle: organisers can create events of any size, define category 
divisions for different participant groups, manage enrolments, and record 
race results with participant positions and finish times. Participants 
benefit from a centralized platform to discover upcoming events, view event 
details and routes, enrol in their chosen category, and maintain a personal 
race history. The system enforces data integrity through role-based access 
control, ensuring event organisers cannot accidentally compete in their own 
races and participants cannot register twice for the same category.

This repository contains both Part 1 (planning, ERD, SQL schema) and Part 2 (the working ASP.NET Core Web API) integrated into a single deliverable.

===============================================
TABLE OF CONTENTS
===============================================

1.User Roles

2. Database Schema

3.Key Relationships

4.API Endpoints

5.Setup and Run Instructions

6.Testing the API in Swagger

7.Authentication and Role-Based Access

8.CI/CD Pipeline

9.Video Walkthrough

---

## User Roles

### 1. Event Organiser

Who: A user responsible for creating and managing road events.

The tasks the event organiser should do:
- Create new events (races, walks, cycles)
- Define event categories (age groups, distance types)
- View all participants enrolled in their events
- Record participant finish times and positions
- View event routes and participant enrolments
- Manage event details (update, delete)

Example: John Mthembu organizes the Comrades Marathon and records David's finish time as 05:47:30 (position 1).

---

### 2. Participant

Who: A user competing in road events.

The tasks the participant can do:
- Browse all available events
- View event details and categories
- Enrol in events and select a category
- View their own enrolment history
- Track their race results and performance
- View event routes and prepare for races
- Cannot create events or record results

Example: David Nkosi views all upcoming marathons, enrolls in the Comrades 87km category, and checks his finish time after the race.

## Database Schema for Section A:

8 Entities:

USER
  Base user account with authentication credentials and role assignment 
  (EventOrganiser or Participant). Central table linking all other entities.

EVENT_ORGANISER
  Users authorized to create and manage events. Linked to USER table to 
  maintain role separation and enable organiser-specific functionality.

PARTICIPANT
  Users competing in events. Linked to USER table and tracks all event 
  enrolments, categories, and race results across multiple events.

EVENT
  Race event details (name, location, date, distance, type). Created by 
  organisers and contains multiple categories, one route, and many enrolments.

EVENT_CATEGORIES
  Event divisions for grouping participants (age groups, distance types). 
  One event can have multiple categories to serve different participant groups.

EVENT_ENROLMENT
  Links participants to event categories. Tracks signup date and status. 
  Unique constraint prevents duplicate enrolments for same participant, 
  event, and category.

ROUTE
  Event-specific route information (description, distance, difficulty, 
  map URL). One-to-one relationship with events. Enables participant 
  preparation and logistics planning.

RESULTS
  Race outcome records (finish time, position, date). Linked to enrolments 
  with 0..1 cardinality to support DNF (Did Not Finish) scenarios.
  
---

## Key Relationships

- 1 User can be 1 EventOrganiser OR 1 Participant
- 1 EventOrganiser creates multiple Events
- 1 Event has 1 Route
- 1 Event contains multiple Categories
- Multiple Participants enrol in multiple Events via EVENT_ENROLMENT

---
## API Endpoints

18 total endpoints across 7 functional areas:

|Authentication |   2  |  
|User Profile   |   3  |   
|Events         |   5  |   
|Categories     |   2  |   
|Enrolments     |   3  |   
|Results        |   2  | 
|Route          |   1  |  
---
##SETUP AND RUN INSTRUCTIONS

###PREREQUISITES

Make sure you have the following installed:

Visual Studio 2022 (17.8 or newer)

.NET 8.0 SDK

SQL Server (2019 or newer)

SQL Server Management Studio (SSMS 19 or newer)

A modern browser (for Swagger)

STEP 1 - CLONE THE REPOSITORY

Open a terminal and run:

git clone https://github.com/<your-username>/raceday-api.git
cd raceday-api

STEP 2 - OPEN IN VISUAL STUDIO

Open RaceDay.sln (or RaceDayAPI.csproj) in Visual Studio 2022.

Wait for NuGet packages to restore automatically.

If they don't, right-click the solution and choose "Restore NuGet Packages".

STEP 3 - VERIFY NUGET PACKAGES

Ensure these are installed (Tools > NuGet Package Manager > Manage NuGet Packages for Solution):

Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.Authentication.JwtBearer
BCrypt.Net-Next

STEP 4 - CONFIGURE THE CONNECTION STRING

Open appsettings.json and confirm the DefaultConnection matches your local SQL Server instance:

"ConnectionStrings": {
"DefaultConnection": "Server=localhost\MSSQLSERVER01;Database=RaceDayDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

Important: The server name (localhost\MSSQLSERVER01) will differ on your machine. Check it in SSMS (the name you use to connect).

STEP 5 - CONFIGURE JWT SETTINGS

Also in appsettings.json, verify the JWT block:

"Jwt": {
"Key": "RaceDaySecretKey2024SuperSecureTokenWithManyCharactersForBetterSecurity12345",
"Issuer": "RaceDayAPI",
"Audience": "RaceDayClient",
"ExpiryMinutes": 60
}

Important: Never commit real secrets. For production, use environment variables or User Secrets.

STEP 6 - CREATE THE DATABASE

Open Tools > NuGet Package Manager > Package Manager Console.
Make sure the Default project is RaceDayAPI, then run:

Add-Migration InitialCreate
Update-Database

This will:

Create the RaceDayDb database

Create all 8 tables (Users, EventOrganisers, Participants, Events, EventCategories, Routes, EventEnrolments, Results)

Alternatively, just press F5 - the app is configured to auto-migrate on startup.

STEP 7 - VERIFY IN SSMS

Open SQL Server Management Studio.

Connect to your SQL Server instance.

Expand Databases > RaceDayDb > Tables.

Confirm all 8 tables exist.

STEP 8 - RUN THE API

Press F5 (or the green Run button) in Visual Studio.

Swagger opens automatically in your browser at https://localhost:<port>/swagger.


##AUTHENTICATION AND ROLE-BASED ACCESS

HOW AUTHENTICATION WORKS

Passwords are hashed with BCrypt before being stored - plain text passwords are never saved.

On login, the API verifies the password against the stored hash and issues a JWT.

The JWT contains claims: userId, userName, email, and role (Organiser or Participant).

The client sends the token on every request via the Authorization: Bearer <token> header.

Protected endpoints validate the token's signature, issuer, audience, and expiry.

ROLE ENFORCEMENT IN CODE

Endpoints are decorated with attributes:

[Authorize] // any authenticated user
[Authorize(Roles = "Organiser")] // Organiser only
[Authorize(Roles = "Participant")] // Participant only

If a user with the wrong role hits a protected endpoint, the API returns 403 Forbidden.
## CI/CD Pipeline
CI/CD Pipeline Screenshot Part 1:

<img width="1366" height="768" alt="Screenshot 2026-09-04 094638" src="https://github.com/user-attachments/assets/743c3192-f294-49a7-b2f9-47859f014d9d" />

CI/CD Pipeline Screenshot Part 2:

---
## The Video Walkthrough

Part 1;The Video Walkthrough explains the planning documents from the entity relationship diagrams (ERDs),
API Endpoints and the SQL Scripts in the SQL Server Management System (SSMS). 
YouTube Link for the unlisted video : https://youtu.be/m_7JQy6tCgQ 

Part 2: The Video Walkthrough explains the working ASP.NET Core Web API implementation.
YouTube Link for the unlisted video: https://youtu.be/MrJm40hASVk




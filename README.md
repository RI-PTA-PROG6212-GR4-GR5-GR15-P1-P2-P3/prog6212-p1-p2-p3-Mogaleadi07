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

## CI/CD Pipeline
CI/CD Pipeline Screenshot:

<img width="1366" height="768" alt="Screenshot 2026-09-04 094638" src="https://github.com/user-attachments/assets/743c3192-f294-49a7-b2f9-47859f014d9d" />

---
## The Video Walkthrough

The Video Walkthrough explains the planning documents from the entity relationship diagrams (ERDs),
API Endpoints and the SQL Scripts in the SQL Server Management System (SSMS).

YouTube Link for the unlisted video : https://youtu.be/m_7JQy6tCgQ 


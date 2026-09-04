# RaceDay - Event Management System

## Project Overview

RaceDay is a full-stack web-based event management system designed for South African road running, walking, and cycling events. The system enables event organisers to create and manage races, define participant categories, and record race results. Participants can browse available events, enrol in races, and track their performance history.
The RaceDay event management system  streamlines the complete event lifecycle — from event creation through result recording — with role-based access control and real-time data management.

## User Roles

### 1. Event Organiser

Who: A user responsible for creating and managing road events.

The tasks the event organiser can do:
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
- USER: Base user information
- EVENT_ORGANISER: Organiser-specific role
- PARTICIPANT: Participant-specific role
- EVENT: Race events
- EVENT_CATEGORIES: Event divisions (age/distance)
- EVENT_ENROLMENT: Participant sign-ups (3-way junction)
- ROUTE: Race route information
- RESULTS: Finish times and positions

Key Relationships:
- 1 User can be 1 EventOrganiser OR 1 Participant
- 1 EventOrganiser creates multiple Events
- 1 Event has 1 Route
- 1 Event contains multiple Categories
- Multiple Participants enrol in multiple Events via EVENT_ENROLMENT

---
## API Endpoints

18 total endpoints across 7 functional areas:

Authentication         2    
User Profile           3     
Events                 5     
Categories             2     
Enrolments             3     
Results                2   
Route                  1    

## CI/CD Pipeline

Our continuous integration and deployment workflow ensures code quality and reliable releases.

CI/CD Pipeline Screenshot is in the docs folder
---

YouTube Link for the unlisted video : 


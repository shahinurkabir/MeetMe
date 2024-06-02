# MeetMe - A Web-Based Calendar & Appointment Management Project.

## Create Events and Configure Availability Slots with Ease.

MeetMe is an application that allows you to share your calendar availability online, enabling people to book appointments based on your schedule. This concept is similar to what Calendly offers. MeetMe is a small-scale version of Calendly, aimed at creating a proof of concept during spare time.

Last year, I worked on an appointment management feature for my organization's public-facing site, where we used Calendly. One of my coworkers suggested that we develop a small appointment booking system ourselves. However, after a brief analysis, I realized it wouldn't be easy and we didn't have the time to implement additional features beyond our core business application.

A few months later, at the end of 2023, I decided to create a basic project similar to Calendly, focusing on the essential features rather than a full production application.

This project allows people to share their calendars online, enabling others to book appointments during available time slots. Some of its features include:

- Setting your availability schedule.
- Defining different event types on your calendar.
- Sharing your calendar with others to book appointments.
- Managing your profile.

## Sample Screenshots to Illustrate Functionality


## Project Overview
This project consists of two main parts. The backend application is a .NET 6 Web API, which handles all server-side processing such as managing event types, availability, appointments, and more.

The frontend is a single-page application built with Angular 14. Currently, no UI framework has been used, but we may incorporate one in the future if needed.

## Technical Stack
   - .Net 6
   - MediatR - command Query handler
   - FluendValidation - To validate input data
   - EFCore - ORM
   - Amazon DynamoDB - NoSQL support
   - Angular 14 - Frontend development framework

## Backend System 
Clean architecture is a popular design pattern for modern applications, regardless of the tech stack, whether .NET, Java, Python, or Node.js. It helps maintain complex systems by making them more maintainable and decoupling infrastructure from the core domain.

The backend API application follows the clean architecture pattern. The entire backend system is divided into several projects:

### Backend Projects
   - MeetMe.Core - Contains core entities, DTOs, interfaces, extensions, and common services.
   - MeetMe.Application - Implements all business logic through core interfaces.
   - Data Providers - Persistence layer, selectable in the program.cs file. The system performs accordingly.
      - Dataprovider.Dapper - Not yet implemented; plans to use Dapper micro ORM to persist data.
      - DataProvider.DynamoDB - Uses DynamoDB for NoSQL support, with plans to add more NoSQL tools.
      - DataProvider.EntityFramework - Used EF Core as ORM
      - DataProvider.InMemoryData  - Stores data temporarily in .NET collections at runtime, data is lost upon application closure.Its helpfull to see in action without configurting database.
   - MeetMe.API - Exposes application functionalities through APIs over HTTP protocol for external connectivity and operation.

### Backend Project - MeetMe.Application
All business logic is written here, following the mediator pattern. Each API request sends a command to the mediator. A specific handler for each command performs the appropriate action. Commands are validated before execution using FluentValidation.

## Frontend Application
This is a single-page application based on Angular 14, consisting of two main parts: 
- ### Application Core
      Contains all common controls, utilities, pipes, shared types, and interfaces. These reusable items are included in a core module named App Core Module.
   ### App-Core
   - controls
   - directives
   - gurads
   - interceptios
   - interfaces
   - pipes
   - services
   - utilities
- ### Application Features
      All main features of the application are included in this main module. Features are logically divided, but not yet into dedicated modules; this may be done later.
   ### Features
  - Account Settings - Manage user ID and base URI.
   - Availability - Configure your time availability using event type availability settings.
   - Event Types - Define your event types and time availability slots as needed.
   - Calendar - Lists all event availabilities. Share this URL to allow others to book appointments from your calendar.
   - Schedule Appointments - Filter booked appointments using various filtering options.

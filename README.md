# Getting Started

## Introduction
This project implements a backend service for managing travel experiences, where users can create trips with associated activities. The project includes validation logic, database integration with MongoDB, and unit tests to cover the core functionality requested.

## Features
- Create a travel experience (trip) with details such as title, user, start and end dates, and activities.
- Perform validation on trip details, such as ensuring the title and user ID are provided, and checking if dates are in the future.
- Handle errors and validation messages clearly when the provided data is incorrect.

## Design Decisions
- MongoDB was chosen for its flexibility in storing documents and handling dynamic schemas.
- The validation service was implemented to ensure that the trip data received is complete and correct before storing it in the database.

## Prerequisites

You'll need the following tools installed in order to run the project locally:
- Visual Studio 2022 (ASP.NET and Web Development Addon)
- MongoDB (Optional: MongoDBAtlas)

Once you have the prerequisites installed on your machine, you will need to check out the project locally by cloning the repo.

## Install dependencies
Open the project in visual studio and go to the Build tab, and select Build Solution. 

## Configure MongoDB
Make sure you have MongoDB running on your machine. Update the connection string in `appsettings.json` accordingly.

## Run the API
To run the API locally, go to the Debug tab, and select Start Debugging or Start Without Debugging.

## Running Tests
You can run the tests by going to the Test tab, and selecting Run All Tests.

## Endpoints

### POST /api/trips
- **Description**: Creates a new trip and its related activites.
- **Request Body**: The request body should be a JSON object containing the trip and its related activity details:
```json
{
  "userId": 1,
  "title": "France",
  "startDate": "2025-04-13T18:25:15.017Z",
  "endDate": "2025-04-13T18:25:15.017Z",
  "activities": [
    {
      "destinationId": 1,
      "duration": 5,
      "cost": 4
    }
  ]
}
```

- **Response**: Returns the created trip with its ID and total cost:
```json
{
  "trip": {
    "tripId": "55a83cb6-47a3-4fa7-bf6f-577fadc2d42e",
    "userId": 1,
    "title": "France",
    "startDate": "2025-04-13T18:25:15.017Z",
    "endDate": "2025-04-13T18:25:15.017Z",
    "totalCost": 4
  },
  "activities": [
    {
      "activityId": "96262c96-fe79-4b49-a96b-da93c9ae3eb4",
      "tripId": "55a83cb6-47a3-4fa7-bf6f-577fadc2d42e",
      "destinationId": 1,
      "duration": 5,
      "cost": 4
    }
  ]
}
```

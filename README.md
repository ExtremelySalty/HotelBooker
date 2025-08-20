# HotelBooker

This project has been created in order to allow users to book a hotel room. This will be to showcase use of C#, Restful API design and EF Core

## Business Rules

* Rooms have 3 possible types: Single, Double, Deluxe
* * This will be in a DB to allow future room types to exist
* Hotels all have 6 rooms
* * Only 6 rooms but allow user to set room numbers
* Rooms cannot be double booked
* * Room can only have a single booking per day. Assume that the final day is inclusive and it is only classed as "free" the day after
* Bookings should generate unique reference numbers
* * (hotel-identifier)-(timestamp)-(random-suffix)
* Room cannot be occupied past it's max capacity
* * IE if a room has space for 4 it cannot hold 5 people

## Requirements

### Functional

* Find a hotel based on name
* Find available rooms between 2 dates for a givennumber of people
* Book a room
* Find booking details based on booking reference

### Technical 
* API must be testable
* Utilise OpenAPI/Swagger
* Have ability to seed and reset data for testing purposes
* API does not need authentication

## Technologies used

* Language: C#
* ORM: EntityFrameworkCore (v8)
* Database Provider: Sql Server
* NUnit: testing framework
* TestContainers: this library is used for the testing of the Categories of Repository and Integration
* Shouldly: used for the assertions for the tests
* NSubstitute: used for mocking interfaces
* Bogus: used for generating test data in the tests as well as the seed data

## How to run

1. Clone repo
2. Add DB connection string to the appsettings
3. Press run
4. On swagger hit the api/data/seed endpoint to create and populate the DB (pass in the optional param max 60 to generate custom amount of data. Default is 10)


## Test runner

The tests are split by the following Categories:
* Unit: for purely code only testing IE using mocks and simulating
* Repository: for testing the different repositories against a DB
* Integration: using the WebApplicationFactory to test in process the project

Please be aware that in order for the Repository and Integration tests to run you will need to have either Podman or Docker installed as these use the TestContainers library to test against a Sql Server DB and not an InMemory substitute. If you do not have either of these installed unselect these Categories from the Test runner screen or they will fail.

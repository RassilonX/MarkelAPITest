# MarkelAPITest

This project uses .Net 8.0 as the SDK across all projects to make a standard API for returning claim and company data, and for updating claim data. The project is covered with unit tests that were written in xUnit. It uses a service layer to interact with a database, which has been stubbed out to return sample data. If the database were to be implemented, then it would be built in Entity Framework with a repository layer to interact with the database.

### Frameworks Used:
- .Net 8
- xUnit
- Microsoft.AspNetCore.App
- Microsoft.NETCore.App

## Running the project
The project can be run in debug or release mode by opening it in Visual Studio. This will run the API, and also open Swagger documentation of each endpoint. You can try out the API using the Swagger page.

## Running the Unit Tests
There are currently no special things to do beforehand to run the unit tests. They can be run from the test explorer in Visual Studio.

## Future Work
- Implement a database in Entity Framework with a repository
- Make the API use async
- Change the stub to use async to support the async change in the API
- Make the return strings global const strings
- Customise the Swagger documentation if possible
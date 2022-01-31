# Digital Concept Development (DCD)

### Prerequisites

-   [.NET 6.0+](https://dotnet.microsoft.com/download/dotnet/6.0)
-   [Node 16+ with npm 8+](https://github.com/nodesource/distributions/blob/master/README.md)
-   [Docker](https://docs.docker.com/engine/install/)

## Frontend

The frontend is built using TypeScript and components from the Equinor Design System ([EDS](https://eds.equinor.com/components/component-status/)).

### Run frontend

Before you can run the application, you need to copy `.env.template` (`cp .env.template .env`) and set the necessary values.

```
cd frontend
npm install
npm start
```

## Backend
The backend is built with .NET 6

### Run backend
Create a file `backend/api/Properties/launchSettings.json` with the provided
template file.

```
cd backend/api
dotnet run
```

### Testing

We use Cypress to create end-to-end tests for the application.

#### Test Strategy

We aim to create a test automation suite that will `describe and validate the systems behavior and functionality as seen by its users`.

The Cypress tests constitute a suite of automated, functional regression tests.

#### Run the tests

To run the Cypress tests locally, type

`npm run cyopen` to open the Cypress interactive runner. This requires the application to be run locally.

## Deployment
We have 4 different environments in use; dev, pr, qa and prod. Dev is built
when PR's are merged to master. The pr env is built on push to the pr branch. The
qa and prod environments are deployed with [Azure Pipeline](https://dev.azure.com/Shellvis/DCD/_build?definitionId=40)
manually and when a new tag is created.

### Team
DCD is developed by the Shellvis team in TDI EDT DSD EDB.


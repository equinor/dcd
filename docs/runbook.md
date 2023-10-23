# Runbook for Concept App

### Prerequisites

- [.NET 6.0+](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node 16+ with npm 8+](https://github.com/nodesource/distributions/blob/master/README.md)
- [Docker](https://docs.docker.com/engine/install/)

## Frontend

The frontend is built using TypeScript and components from the Equinor Design System ([EDS](https://eds.equinor.com/components/component-status/)).

### Run frontend

Before you can run the application, you need to copy `.env.template` (`cp .env.template .env`) and set the necessary values.

```cmd
cd frontend
npm install
npm start
```

## Backend

The backend is dotnet webapi built with .NET 6 which provides a REST interface for the frontend. Swagger has been installed to provide documentation for the API, and to test functions. The backend retrieves and stores data in a Azure SQL Database for each environment. 

### Run backend

Create a file `backend/api/Properties/launchSettings.json` with the provided
template file. You need to populate the app configuration connection string
(navigate to azure portal, find app configuration resource, navigate to
settings -> access keys), and choose an AppConfiguration Environment (`dev` for
local development at time of writing).

Finally, to be able to use secrets referenced in the app config, you need to
authenticate yourself on the command line. [Get a hold of the azure CLI
`az`](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) and run `az login` in the command line. NB: You will need to use a browser for the
authentication, as far as I know.

Then, to start the backend, you can run

```cmd
cd backend/api
dotnet run
```

## Deployment

We have 4 different environments in use; dev, pr, qa and prod. Dev is built
when PR's are merged to master. The pr env is built on push to the pr branch. The
qa and prod environments are deployed with [Azure Pipeline](https://dev.azure.com/Shellvis/DCD/_build?definitionId=40)
manually and when a new tag is created.
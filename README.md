# Digital Concept Development (DCD)

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/684c3f46696f49dc8b95a2d789b08daf)](https://app.codacy.com/gh/equinor/dcd?utm_source=github.com&utm_medium=referral&utm_content=equinor/dcd&utm_campaign=Badge_Grade_Settings) [![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url] [![Stargazers][stars-shield]][stars-url] [![Issues][issues-shield]][issues-url]

## Concept App, Digital Concept Application

-   Product owner: Atle Svandal
-   Business area: Early phase concept studies

## Summary Description

The application supports collecting time series values for cost profiles for offshore facilities, exploration, drilling (well)
and volume profiles for drainage stragegy and creating these as assets on business cases to be compared for projects.

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
template file. You need to populate the app configuration connection string
(navigate to azure portal, find app configuration resource, navigate to
settings -> access keys), and choose an AppConfiguration Environment (`dev` for
local development at time of writing).

Finally, to be able to use secrets referenced in the app config, you need to
authenticate yourself on the command line. [Get a hold of the azure CLI
`az`](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) and run `az login` in the command line. NB: You will need to use a browser for the
authentication, as far as I know.

Then, to start the backend, you can run

```
cd backend/api
dotnet run
```

## Deployment

We have 4 different environments in use; dev, pr, qa and prod. Dev is built
when PR's are merged to master. The pr env is built on push to the pr branch. The
qa and prod environments are deployed with [Azure Pipeline](https://dev.azure.com/Shellvis/DCD/_build?definitionId=40)
manually and when a new tag is created.

## Development

### Git Hooks

There are various git hooks provided in the `git-hooks` folder. Using these is
voluntary. Various checks are performed that are also performed in CI. The
intention is to shorten the development cycle and avoid pushing code that will
not pass CI.

To use e.g. the pre-commit hook, copy the file `pre-commit` into `.git/hooks/`
in the project root, or (more elegant, as this will automate the hook
automatically) link the script like such, running in the project root:

```
ln -s $PWD/git-hooks/pre-commit .git/hooks/pre-commit
```

The above requires having `ln` available.

#### Dependencies

The scripts rely on

```
- /bin/sh
- git
- dotnet
- basic *nix utils (grep, tail, awk, ...)
```

### Team

DCD is developed by the It's a Feature team in TDI EDT DSD EDB. Development was started by the Shellvis team. 

[contributors-shield]: https://img.shields.io/github/contributors/equinor/dcd.svg?style=for-the-badge
[contributors-url]: https://github.com/equinor/dcd/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/equinor/dcd.svg?style=for-the-badge
[forks-url]: https://github.com/equinor/dcd/network/members
[stars-shield]: https://img.shields.io/github/stars/equinor/dcd.svg?style=for-the-badge
[stars-url]: https://github.com/equinor/dcd/stargazers
[issues-shield]: https://img.shields.io/github/issues/equinor/dcd.svg?style=for-the-badge
[issues-url]: https://github.com/equinor/dcd/issues
[license-shield]: https://img.shields.io/github/license/equinor/dcd.svg?style=for-the-badge
[license-url]: https://github.com/equinor/dcd/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/othneildrew
[product-screenshot]: images/screenshot.png

## Architecture Diagrams
The following diagrams have been created using PlantUML.

### System Context Diagram
System context diagram for the DCD application.
![SysContextDiagram](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/equinor/dcd///main/PlantUMLC4L1)

### Container Diagram
Container diagram for the DCD application.
![SysContextDiagram](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/equinor/dcd///main/DCD_C4Container.iuml)

## Access to application and data - UNDER CONSTRUCTION
AD groups that can view data (AccessIT groups work in progress)
|Name|Description|User types|How to check|
|-|-|-|-|
|Project Users| Read/write access to app | Employees, external hire, consultants | [ConceptApp Users](https://portal.azure.com/#view/Microsoft_AAD_IAM/GroupDetailsMenuBlade/~/Overview/groupId/cd75d09b-5f90-4fac-be54-de4af8b5b279), [fg_2S_IAF](https://portal.azure.com/#view/Microsoft_AAD_IAM/GroupDetailsMenuBlade/~/Overview/groupId/a64069dd-12fd-422b-8c1e-2093fa32819d), [fg_PRD EP CD VALU](https://portal.azure.com/#view/Microsoft_AAD_IAM/GroupDetailsMenuBlade/~/Overview/groupId/553eada8-9205-4c81-bd32-488ebc5dc349) |
|Read Only User| Only able to read all information in app | Employees, external hire, consultants | Currently no groups |
|Admin| Set/change specific settings in app | Employees, external hire, consultants | [ConceptApp Admins](https://portal.azure.com/#view/Microsoft_AAD_IAM/GroupDetailsMenuBlade/~/Overview/groupId/196697db-1a55-4e46-8581-7f2463016e8f), [fg_2S_IAF](https://portal.azure.com/#view/Microsoft_AAD_IAM/GroupDetailsMenuBlade/~/Overview/groupId/a64069dd-12fd-422b-8c1e-2093fa32819d) |

### Admin Consent Decision Matrix
|Privilege requested|In-house developed applications|Scope|
|-|-|-|
|Application API permissions (App to App).|Application: API Owner: Team IAF, Data Owner: Atle Svandal|Sites.Read.All, user_impersonation|

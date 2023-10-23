# Digital Concept Development (DCD)

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/684c3f46696f49dc8b95a2d789b08daf)](https://app.codacy.com/gh/equinor/dcd?utm_source=github.com&utm_medium=referral&utm_content=equinor/dcd&utm_campaign=Badge_Grade_Settings) 
![Known Vulnerabilities](https://snyk.io/test/github/equinor/dcd/badge.svg)
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url] [![Stargazers][stars-shield]][stars-url] [![Issues][issues-shield]][issues-url]

## Concept App, Digital Concept Application

- Product owner: Atle Svandal
- Business area: Early phase concept studies

## Summary Description

The application supports collecting time series values for cost profiles for offshore facilities, exploration, drilling (well)
and volume profiles for drainage stragegy and creating these as assets on business cases to be compared for projects.

## Architechture

The application is split between the [frontend app](#frontend) hosted in Fusion, and the [backend app](#backend) hosted in Radix. Authentication is based on [RBAC](https://learn.microsoft.com/en-us/azure/role-based-access-control/overview), where we have different app registrations for preproduction and production with are consented to access Fusion Preprod or Fusion Prod. 

### Security

Snyk surveillance has been added to the project for continuous monitoring of the code and its dependency. 

### Azure App Config

Azure App Configuration provides a service to centrally manage application settings and feature flags. It allows us to change configuration directly in Azure for all environments. Combined with Azure Key Vault it also combines a secure place to store secrets and connection strings.

### Omnia Radix

[Omnia Radix](https://console.radix.equinor.com/applications/dcd) is a Equinor PaaS (Platform as a Service) based on AKS to build and run docker containers. You can either make Radix build your container directly, or pull the container from a container registry. For DCD the image is built in [Azure Devops](#azuredevops), and pushed to [Azure Container Registry](#azure-container-registry). Radix pulls the image corresponding to release stage.

Configuration of the required infrastructure is placed in a radixconfig.yml, which defines the different components and environments which are created. Runtime variables and secrets are also defined in radixconfig.yml. The DCD config is placed in a separate [git repo](https://github.com/equinor/dcd-radix-conf).

### Azure Container Registry

## Development

### Team
DCD is developed by the It's a Feature team in TDI EDT DSD EDB. Development was started by the Shellvis team.  As of november 2022, the team is a Sopra Steria only team. 

### Repository
The application consists of a github monorepo for frontend and backend, and a single repo for [Radix Configuration](#omnia-radix). All code changes to main branch should come as a pull request from a github fork. 

### Build and Release
There is a project in [Azure Devops](https://dev.azure.com/2S-IAF/DCD) for DCD where user stories and tasks are defined, but also build and release pipeline are hosted. 

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

## Access to application and data

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

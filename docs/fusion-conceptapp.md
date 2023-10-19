# Application Template for [Concept App](https://github.com/equinor/dcd)

## Concept App

- Product Owner: [Atle Svandal](mailto:atsv@equinor.com)
- EA Architect and Fusion Architect: [Eirik Eikeberg](mailto:eriei@equinor.com)
- Development & Ops: [Team It's A Feature](https://github.com/equinor/fusion-architecture-contracts/blob/main/teams/teamiaf.md)
- Application Users, Business Area: Early phase?
- Launch date, version in production: 28.10.2022, v1.0.2
- Are any specialties or any deviations from default worth mentioning. e.g access rights, classification of data, usage,etc
    - The user needs to be added to an approved AD group in order to access the application.
 
## Summary/Description

Concept App is a Fusion application that lets the user create and mature early phase business case concepts.  

## Documentation/Meeting Log Update

- 12.04.2022 - Initial version created for MVP
- 01.10.2022 - Soft release of version 1.0

### Actions
- [   ] Any outstanding action from review
- [ X ] A completed action


## Technical Description

- See C4 diagrams for details on technologies used, and the [repo](https://github.com/equinor/dcd) for details on build pipelines, test environments etc.

### Software Development

- Describe Branch strategy for this app
    - Fork repository. Create PR, code review, squash and merge, delete branch.
    - Merge to master deploys to CI. FQA and FPRD deploys need to be manually approved.

### Data

- Who is the data owner
    - [Cecilie Strøm](mailto:cecs@equinor.com)
- What is the classification of the data
    - Internal
- What data is used in non dev/test/qa env, where does it come from
    - Projects are created from Project Master through Fusion Integration Lib
    - Data from PROSP files retrieved from SharePoint
- Who have access to azure storage infrastructure
    - Members of the team
    - Product owner
    - Fusion core ???
- How is data extracted from storage
    - Internal data is handled by Entity Framework
- Where is the Legal Risk Analysis (LRA) for the data
    - We are looking in to this.
- Must data be kept in sync with other applications (are there the same or related data appears multiple places? If yes, how are they kept in sync (e.g. event-based, sync-based))
    - No.
- Have you evaluated how the application should handle personal data, with GDPR regulations in mind? This includes logging of information that can identify a person, like name name, userID, email, etc.
    - The application does not store user data. For troubleshooting purposes the email of the user is logged when performing actions and kept for xx days.

### Operation

- What is monitored and logged
    - User activity and error situations.
- How is the logging monitored
    - Dynatrace
- Who have the responsibility to monitor
    - The team
- Who have access to azure infrastructure
    - Members of the team
    - Product owner
- What platforms is used (Azure, Radix) etc.
    - Fusion Portal (frontend) and Radix (backend).
- What could go wrong in this solution, and how is this mitigated? 
- Where is the RunBook?
    - work in progress...
- Have you trained the support team in this app. Does the support team have any special access rights?
    - Documentation for support team will be provided at a later stage

### Disaster & Recovery

No D&R plans as of yet.

### Azure Cost

No cost key drivers identified as of yet.

### Technical debt

List known technical debt in current version and a comment on how it will be handled EXAMPLE :

#### MVP

- ~~Own table of project in solution. CommonLib not used~~ --> Use commonLib as source in next version

#### Version 1
- Created a table with persons with root access to application --> use an AccessIT group for this in next version

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

# Application Template for [insert-app-name](https://github.com/equinor/insert-app-name)

>This repo is accessable for all users in github.com/equinor organization. Please do not expose any classifyed  information in this contract.

## Title of project initiative, working title, official app title, ect

- Product Owner: [Atle Svandal](mailto:atsv@equinor.com)
- EA Architect and Fusion Architect: [Eirik Eikeberg](mailto:eriei@equinor.com)
- Application Users, Business Area: ???
- Launch date, version in production: 28.10.2022, v1.0.2
- Are any specialties or any deviations from default worth mentioning. e.g access rights, classification of data, usage,etc
    - The user needs to be added to an approved AD group in order to access the application.
- Have the default process described in team overview been overridden for some reason, e.g. extra focus on security, time to marked trumped some steps, 
    - ???
 
## Summary/Description

Description of app/solution  

## Documentation/Meeting Log Update - update every 4 months for each app

- DD.MM.YYYY - < initial version created for MVP > 
- DD.MM.YYYY - < version 1.0 of product, added admin module 

### Actions
- [   ] Any outstanding action from review
- [ X ] A completed action


## General

- Present a technical overview of solution
- C4 models at level 1 and 2. Level 3 and 4 is welcome but not a demand
- Security Risk Analysis (SRA) for the app

### Software Development

- Describe Branch strategy for this app
    - Fork repository. Create PR, code review, squash and merge, delete branch.
    - Merge to master deploys to CI. FQA and FPRD deploys need to be manually approved.

- If you are exposing APIs, have you adapted to the principles in the [Equinor API Strategy](https://github.com/equinor/api-strategy/blob/master/docs/strategy.md)?
    - The API is not exposed on https://api.equinor.com.


### Data

- Who is the data owner
    - [Cecilie Strøm](mailto:cecs@equinor.com)
- What is the classification of the data
    - Internal
- What data is used in non dev/test/qa env, where does it come from
    - Projects are created from Project Master through Fusion Integration Lib
    - Data from PROSP files retrieved from SharePoint
- Who have access to azure storage infrastructure
    - ???
    - Members of the team
    - Product owner
    - Fusion core ???
- How is data extracted from storage
    - Internal data is handled by Entity Framework
- Where is the Legal Risk Analysis (LRA) for the data
    - ???
- Must data be kept in sync with other applications (are there the same or related data appears multiple places? If yes, how are they kept in sync (e.g. event-based, sync-based)
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
    - ???
- Where is the RunBook?
    - ???
- Have you trained the support team in this app. Does the support team have any special access rights?
    - No

### Disaster & Recovery

Describe how d&r is done for this app
- Data center outage
- Infrastructure
- Storage

### Azure Cost

 What are the key cost drivers and why

### Technical debt

List known technical debt in current version and a comment on how it will be handled EXAMPLE :

#### MVP

- ~~Own table of project in solution. CommonLib not used~~ --> Use commonLib as source in next version

#### Version 1
- Created a table with persons with root access to application --> use an AccessIT group for this in next version

## Access to application and data

Insert an access matrix or similar. Also include information about who have approved the access regime. Describe the user groups that have access. Use exiting concepts and groups when granting access and discuss this with PO/SME of app.  Also make sure that the access is given on the basis of data classification referenced in [WR0158](https://docmap.equinor.com/Docmap/page/doc/dmDocIndex.html?DOCKEYID=427318)

Example of groups / constallations of people:
- User types
    - Equinor Employee - All Equinior Employee
    - Equinor Account - This includes all @equinor in Azure AD
    - Everyone in Azure AD - All of the above and includes all affiliated. We do not know how all affiliated is.
- Participant of a project - Allocated to a Poroject that have the organization chart in fusuon.
- Member of an Azure AD group - example is  "All leaders in Equinor" , "Fusion Developers", etc.
- Member of an AccessIT group - Fusion have a lsit of defined groups, if no other means of identifying the group, new groups can be created. Exiting groups can be found in [AccessIT](https://accessit.equinor.com/Search/Search?term=Fusion).


EXAMPLE

|Access|Description|Who|How to check|
|-|-|-|-|
|Read| Read all information in app| All equinor employee | AD group [Equinor All Employee](https://portal.azure.com/#blade/Microsoft_AAD_IAM/GroupDetailsMenuBlade/Overview/groupId/1db6ba0c-1d2f-4d76-9dae-0881e5913c5c) |
|Write/Update | Insert new information in app | AccessIT Role | [Developer Fusion](https://accessit.equinor.com/Search/Search?term=Developer+%28FUSION%29)|
|Delete| Delete information from app | AccessIT Role | [BOFH Fusion]() |

This access matrix is approved by Eirik Eikeberg eriei@equinor.com and Hans Dahle handah@equinor.com

## Quick facts

Update the quickfact section in fusion portal


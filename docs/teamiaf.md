# [2S-IAF](https://github.com/orgs/equinor/teams/2s-iaf)

We are a development team from [Sopra Steria](https://www.soprasteria.no/)

# Members

- [Eirik Sander Fjeld](mailto:eisande@equinor.com)(DevOps Responsible/Developer)
- [Ahmed Abdi](mailto:ahmab@equinor.com)(Team Lead/Developer)
- [Tord Austad](mailto:toaus@equinor.com)(Developer)
- [Daniel Bøhme](mailto:dboh@equinor.com)(Developer)
- [Anne Hjellbrekke Frøyen](ifro@equinor.com)(UX)

## General
- The team is located in Stavanger and Bergen, and uses Teams primarly for meeting such as
-- Daily Standup
-- Weekly status meeting with PO
-- Biweekly grooming meeting and sprint retro
- As the team is located in two different city the team mainly uses Microsoft Teams and Slack as form of communication. The team in both city have seatings in Sopra Steria offices in Stavanger and Bergen, and do uses them frequently to work together in office environment.

## Backlogs/Dev board
- [ConceptApp](https://dev.azure.com/2S-IAF/DCD/_boards/board/t/DCD%20Team/Stories)
- [BMT](https://dev.azure.com/2S-IAF/Fusion-BMT/_boards/board/t/Fusion-BMT%20Team/Stories)
- [PIA](https://dev.azure.com/2S-IAF/PIA/_workitems/recentlyupdated/)


# Applications

- [Project Information Archive](https://github.com/equinor/pia)
- [Barrier Management Tools](https://github.com/equinor/fusion-bmt)
- [ConceptApp](https://github.com/equinor/dcd)
- [FieldAp-wellpath](https://github.com/equinor/fieldap-wellpath)
- [FieldAp-statistics](https://github.com/equinor/fieldap-statistics)

# How we work

## Agile development  
The team works in sprints of two weeks.  

### Recurrent meetings

Per sprint (2 weeks): 
- Sprint planning 
- Backlog grooming 
- Retrospective 

Weekly: 
- Status meeting 

Daily: 
- Standup 


## Board status
||New|Active||Review||QA|Closed|
|-|-|-|-|-|-|-|-|
|Definition|Not picked up yet.|Actively beeing worked on.|Done, but not yet picked up for review.|Actively in review.|Approved.|Testing Quality Assurance.|Approved by PO|

### Feedback loop
- We get feedback from User Testing, PO and MS Feedback Form (linked in app). 
- Feedback is reviewed by the UX designer and Project Owner who review the feedback and design a solution. User stories are then added to the backlog. 
- The user stories are reviewed prioritized in the next backlog grooming. 

### Security
- How do you review security (min every 4 months) 
    - Automatic code scanning for vulnerabilities with [Snyk](https://app.snyk.io/org/shellvis-team)


 - How does the team conduct code reviews? 
    - All developments are done in forks, where any changes are made into PR which uses the four eyes principle.
    - Codacy is used to code scan all PRs.
 
- How is endpoints checked for security 
    - Endpoints are annotated with attributes opening the endpoint for specific roles.

 

### Testing 
 - How does the team test and protect their code and environment? 
    - Regression testing, end to end testing, and unit testing.
    - Environment are seperated into into production and preproduction.

 
- How is infrastructure as code tested  

 
- What is not automated? 

   
- Do you test code coverage 
     

- Describe how you do integration testing 
 
- What is covered with unit tests 
    - Backend(services, adapters)

 
- Happy-path vs. boundary vs. negative testing 


- Whom have access to test/dev/qa env 
    - All team members
 
- What branch protection is in place 
    - Any changing done to main needs to be approved by a team member.
    - Only collaborators are allowed to create branch in main repository

- Which linter is used 
    - ESlint 


## Tools
### Dev tools
- [Figma](https://www.figma.com/)
- [Visual Studio Codes](https://code.visualstudio.com/) 
- [Jetbrains dotUltimate](https://www.jetbrains.com/dotnet/) 
- [Github](https://github.com/)
- [Azure DevOps CI/CD](https://dev.azure.com/2S-IAF)
### Communication and documentation tools
- [Azure DevOps Board](https://dev.azure.com/2S-IAF)
- [Slack](https://equinor.slack.com/)
- [Microsoft Teams](https://www.microsoft.com/en/microsoft-teams/log-in)


## Collaboration
- The team works from two different locations, so teams is used primarly to host recurrent meetings, while Slack is used as main tool for written communication. PO is invited to join recurrent meeetings during ongoing sprints on their products.


## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# FindApprenticeship Jobs

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-findapprenticeship-jobs?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-jobs&branchName=main)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3500&repoName=SkillsFundingAgency%2Fdas-findapprenticeship-jobs&branchName=main)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-findapprenticeship-jobs&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-findapprenticeship-jobs)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This azure functions solution is part of FindApprenticeship Jobs project. Here we have background jobs in form of Azure functions that carry out periodical jobs like sending out notifications or cleaning up data.

## How It Works

The notification job uses NServiceBus protocol to send a message per notification to the notification queue. The functions connects directly with the recruit api & Azure search to get and update data.

## 🚀 Installation

### Pre-Requisites

```
* A clone of this repository
* A code editor that supports Azure functions and .NetCore 6
* An Azure Service Bus instance with a Queues
* Azure Search available either running locally or accessible in a Azure tenancy
* An Azure Active Directory account with the appropriate roles as per the [config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-findapprenticeship-jobs/SFA.DAS.FindApprenticeship.Jobs.json)
```
### Config

You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-findapprenticeship-jobs/SFA.DAS.FindApprenticeship.Jobs.json) repository.

In the SFA.DAS.FindApprenticeship.Jobs project, if not exist already, add local.settings.json file with following content:

local.settings.json file
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true;",
    "NServiceBusConnectionString": "UseLearningEndpoint=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
    "ConfigNames": "SFA.DAS.FindApprenticeship.Jobs,SFA.DAS.Encoding",
    "EnvironmentName": "LOCAL"
  }
} 
```

Azure Table Storage config

Row Key: SFA.DAS.FindApprenticeship.Jobs_1.0

Partition Key: LOCAL

Data:

```json
{
  "FindApprenticeshipJobsConfiguration": {
    "ApimKey": "test",
    "ApimBaseUrl": "https://localhost:5003/",
    "AzureSearchBaseUrl": "https://{{AZURE-SEARCH-URL}}/"
  }
}
```

## Functions

SFA.DAS.FindApprenticeship

This function is responsible for consuming events from the Recruit & FindAnApprenticeship Service. It handles the following nServiceBus events:

# Queue Trigger & Http Trigger

* GetVacanciesClosingSoonEvent
* VacancyApprovedEvent
* VacancyClosedEvent
* VacancyUpdatedEvent

### Queue Trigger

* SendApplicationReminders - Responsible for sending notification emails

### Timer Trigger & Http Trigger 

* IndexCleanup - Responsible for Azure search re-index 
* RecruitIndexer - Responsible for syncing vacancies from Recruit to Azure search index


## 🔗 External Dependencies

* The functions uses data defined in [das-recruit](https://github.com/SkillsFundingAgency/das-recruit/tree/master/src/API/Recruit.Api) as primary data source.
* The functions uses data defined in [Azure Search](https://learn.microsoft.com/en-us/azure/search/search-what-is-azure-search) as primary data source.

### 📦 Internal Package Dependencies

* SFA.DAS.NServiceBus.AzureFunction
* SFA.DAS.Api.Common
* SFA.DAS.Configuration.AzureTableStorage

## Technologies

```
* .NetCore 6.0
* Azure Functions V4
* Azure Search
* Azure Table Storage
* NServiceBus
* NUnit
* Moq
* FluentAssertions
```

## How It Works

### Running

* Open command prompt and change directory to _**/src/SFA.DAS.FindApprenticeship.Jobs/**_
* Run the web project _**/src/SFA.DAS.FindApprenticeship.Jobs.csproj**_

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

## License

Licensed under the [MIT license](LICENSE)
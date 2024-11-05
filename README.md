
# Survey App

A web-based survey application that enables administrators to create and manage surveys, while allowing users to respond and view survey results in real-time. This application supports role-based access, survey analytics, and a user-friendly interface for creating and answering surveys.

## Roles

- **Survey Coordinators**: Administrators who define and conduct surveys, manage questions, and view results. 
- **Survey Respondents**: Users who complete surveys as guests without administrative privileges.

## Key Features

### For Survey Coordinators
- **Survey Creation**: Define surveys with 1-15 multiple-choice questions, each allowing 1-10 response options.
- **Enhanced Question Types**: Create questions with selectable options or open text fields for free responses.
- **Multi-Branch Surveys**: Accommodate branching surveys with multi-step navigation.
- **Dashboard**: Overview of ongoing, closed, and recent surveys for quick access and monitoring.
- **Response Analytics**: View survey results in graphical formats (pie, bar, column charts) and export reports to Excel.
- **Survey Management**: Open, close, and manage surveys; easily access and review past surveys.

### For Survey Respondents
- **Survey Completion**: Answer questions via checkboxes or open text input.
- **Emoji Options**: Respond to option-based questions using emoji for a more engaging experience.
- **Single-Submission Protection**: Prevents respondents from completing the same survey multiple times.

### Tech Stack

- **Backend**: ASP.NET Core 8, Entity Framework 3.1.6, ASP.NET Core Identity, SQL Server
- **Frontend**: Bootstrap 4.3.1, jQuery, clipboard.js, Chart.js, Humanizer 2.8.26

### New Features (Updated)
- **Upgraded to .NET 8**
- **Dashboard**: Provides a centralized overview for Survey Coordinators.
- **Multi-Branch Survey Support**: Allows complex, branched survey designs.
- **Enhanced Question Types**: Options for both multiple-choice and open-text responses.
- **Excel Export**: Export survey results to Excel for external analysis.

## User Stories

### General
- Survey Coordinators can manage and view survey analytics.
- Survey Respondents can complete surveys as guests.

### Defining and Conducting Surveys
- Survey Coordinators can create surveys, add questions with various response types, and manage survey lifecycle.
- Survey Coordinators can access results, visualize response data, and export reports.

### Answering Surveys
- Respondents view survey titles and questions, select/check answers, and submit or cancel responses.

## Bonus Features
- Single-submission restriction per survey.
- Graphical representation of survey results in various chart formats.

## TODO
- Form validation error display improvements.

## Credits
- Illustration by [Katerina Limpitsouni (undraw.co)](https://undraw.co/).

## License
- [MIT License](https://github.com/serhatyuna/survey-app/blob/master/LICENSE) 


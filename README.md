
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


# Survey App

Users of this app are divided into two distinct roles, each having different
requirements:

-   _Survey Coordinators_ define and conduct surveys. This is an administrative
    function not available to normal users.
-   _Survey Respondents_ Complete surveys. They have no
    administrative privileges within the app.

## Tech Stack
-   ASP.NET Core 8
-   Entity Framework 3.1.6
-   ASP.NET Core Identity
-   SQL Server
-   Humanizer 2.8.26
-   Bootstrap 4.3.1
-   jQuery
-   clipboard.js
-   Chart.js

## User Stories

### General

-   [X] Survey Coordinators can define, conduct, and view surveys and survey results.
-   [X] Survey Coordinators can login to the app to access functions, like defining a survey.
-   [X] Survey Respondents can answer a survey as a guest.

### Defining a Survey

-   [X] Survey Coordinator can define a survey containing 1-15 multiple choice questions.
-   [X] Survey Coordinator can define 1-10 mutually exclusive selections to each question.
-   [X] Survey Coordinator can enter a title for the survey.
-   [X] Survey Coordinator can click a 'Cancel' button to return to the home page without saving the survey.
-   [X] Survey Coordinator can click a 'Save' button save a survey.
-   [X] After saving the survey, a link for the survey should be created. (/survey/answer/`{id}`)

### Answering a Survey
-   [X] Survey Respondents should see the title of the survey and the questions below the title.
-   [X] Survey Respondent can select responses to survey questions by clicking on a checkbox
-   [X] Survey Respondents can click a 'Submit' button submit their responses to the survey.
-   [X] Survey Respondents can click a 'Cancel' button to return to the home page without submitting the survey.

### Conducting a Survey

-   [X] Survey Coordinator can open a survey by selecting a survey from a list of previously defined surveys
-   [X] Survey Coordinators can close a survey by selecting it from a list of open surveys

### Viewing Survey Results

-   [X] Survey Coordinators can select the survey to display from a list surveys
-   [X] Survey Coordinators can view survey results as in a pie chart showing the number of responses for each of the possible selections to the questions.

## Bonus features

-   [X] Survey Respondents cannot complete the same survey more than once (maybe save the **e-mail** or ~~IP address~~ of the respondent)
-   [X] Survey Coordinators and Survey Respondents can view graphical representations of survey results (e.g. pie, bar, column, etc. charts)

## TODO

-   [ ] Display if any form validation errors.

## Credits

-    [Katerina Limpitsouni (undraw.co)](https://undraw.co/) for the survey illustration

## License

-    [MIT License](https://github.com/serhatyuna/survey-app/blob/master/LICENSE)

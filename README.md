# FlashCards

This application presents a series of flashcards to the user.  Each flashcard has a question on one side, and the answer on the other, mirroring an IRL flash card.  The flashcards are organized by topic, and are presented in a randomized, infinite list.

For a quick walkthrough of the user experience, please see: https://youtu.be/RanuGF-EJ1o

## Technologies used for this application:
- React
- .NET 8, Web API
- Entity Framework
- Cosmos DB
- Azure Container Apps
- GitHub Actions

## Solution Architecture
- flashcard.ui: 
    - React front-end
    - Deployed to Azure as an app container via a GitHub action
    - Uses the Flashcard API to get the list of topics and flashcards for a specific topic.
    - The only configuration is the base URL for the API, stored in an environmental variable called FLASHCARD_SERVICE_BASE_URL.  Locally, this is configured via the .env file, but on Azure this is defined on the app container (under Application / Containers / Environmental variables).
- Flashcard
	- C#, Web API back-end
    - Deployed to Azure as an app container via a GitHub action
    - Connects to a Cosmos DB for NoSQL database
    - Configured with a combination of
        - appsettings.json file: The environment-specific settings in the former get updated by the GitHub action.  
        - secrets manager: locally this is managed by Visual Studio.  On Azure, this is defined in the app container under Settings / Secrets.
- DataImporter:
    - C# console app
    - This is not deployed to Azure.  
    - This is designed to load up Cosmos DB with data from a flat file, data.json, that contains all of the question/answer pairs for one or more topics (a dummy version of this file is included with the project).
    - This requests two command line switches to be passed in:
        - CosmosDBSettings:PrimaryKey: The key to access Cosmos DB
        - PathToDataFile: the full path to the JSON data file to be loaded
    - Example execution:  DataImporter.exe --CosmosDBSettings:PrimaryKey abc123 --PathToDataFile ./data.json
- FlashCard.Configuration
	- C# library
	- This is used by both FlashCard and DataImporter to read the appsettings file.
- UnitTests
	- C# library
    - This contains tests for DataImport, FlashCard, and FlashCard.Configuration

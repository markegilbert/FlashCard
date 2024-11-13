using Microsoft.Azure.Cosmos;
using System.Runtime.CompilerServices;

namespace DataImporter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            String PrimaryKey;
            String PathToDataFile;
            Database CosmosDatabase;
            Container CosmosContainer;

            PrimaryKey = args.ExtractValueFor("PrimaryKey");
            PathToDataFile = args.ExtractValueFor("PathToDataFile");

            // TODO: Validate that the requirement parameters all have values

            // TODO: Move the AccountEndpoint and DatabaseName to the config file
            CosmosDatabase = await InitializeDatabase("https://localhost:8081", PrimaryKey, "flashcard");
            Console.WriteLine("Database created");
            CosmosContainer = await CosmosDatabase.CreateContainerIfNotExistsAsync(id: "flashcard", partitionKeyPath: "/id");
            Console.WriteLine("Container created");



            // ********************************************************************************
            // TODO: Remove this test data once the data file is being read in.
            var item1 = new
            {
                id = "1",
                question = "What is your name?",
                answer = "Arthur, King of the Britons",
                topic = new
                {
                    id = "1",
                    topicName = "Monty Python Questions"
                },
                createdOn = "2024-11-12"
            };
            var item2 = new
            {
                id = "2",
                question = "What is your quest?",
                answer = "To seek the grail",
                topic = new
                {
                    id = "1",
                    topicName = "Monty Python Questions"
                },
                createdOn = "2024-11-12"
            };
            var item3 = new
            {
                id = "3",
                question = "What is the airspeed velocity of an unladen swallow?",
                answer = "African or European swallow?",
                topic = new
                {
                    id = "1",
                    topicName = "Monty Python Questions"
                },
                createdOn = "2024-11-12"
            };


            await CosmosContainer.UpsertItemAsync(item1);
            Console.WriteLine("Question 1 upserted");
            await CosmosContainer.UpsertItemAsync(item2);
            Console.WriteLine("Question 1 upserted");
            await CosmosContainer.UpsertItemAsync(item3);
            Console.WriteLine("Question 1 upserted");
            // ********************************************************************************


            Console.WriteLine("Done - press any key to exit");
            Console.ReadLine();
        }


        //private static Stream FileContents(String DataFilePath)
        //{
        //    return File.OpenRead(DataFilePath);
        //}


        // Adapted from "Using the Azure Cosmos DB emulator to develop locally", by
        // The Code Wolf, https://www.youtube.com/watch?v=bCDiMBfU7UE
        private static async Task<Database> InitializeDatabase(String AccountEndpoint, String PrimaryKey, String DatabaseName)
        {
            CosmosClient Server;
            Database CosmosDatabase;

            Server = new CosmosClient(accountEndpoint: AccountEndpoint, authKeyOrResourceToken: PrimaryKey);
            CosmosDatabase = await Server.CreateDatabaseIfNotExistsAsync(id: DatabaseName, throughput: 400);

            return CosmosDatabase;
        }


    }
}

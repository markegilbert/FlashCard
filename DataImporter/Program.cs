using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;


namespace DataImporter
{
    internal class Program
    {
        private static NLog.ILogger? _LogAs;

        static async Task Main(string[] args)
        {
            String PrimaryKey;
            String PathToDataFile;
            Database CosmosDatabase;
            Container CosmosContainer;
            CosmosDBSettings Settings;

            _LogAs = NLog.LogManager.GetCurrentClassLogger();

            #region Logging
            ReportStatus("");
            ReportStatus("************************");
            ReportStatus("Application starting; reading in arguments...");
            #endregion

            try
            {
                #region Logging
                ReportStatus("Reading in command line arguments...");
                #endregion
                PrimaryKey = args.ExtractValueFor("PrimaryKey");
                PathToDataFile = args.ExtractValueFor("PathToDataFile");

                #region Logging
                ReportStatus("Loading up the config file settings...");
                #endregion
                Settings = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()
                    .GetRequiredSection(CosmosDBSettings.SettingsName)
                    .Get<CosmosDBSettings>();

                // TODO: Validate that the parameters all have values - config file and arguments


                #region Logging
                ReportStatus("Verifying database and container exist...");
                #endregion
                CosmosDatabase = await InitializeDatabase(Settings.AccountEndpoint, PrimaryKey, Settings.DatabaseName);
                #region Logging
                ReportStatus("Database was created and/or verified");
                #endregion
                CosmosContainer = await CosmosDatabase.CreateContainerIfNotExistsAsync(id: Settings.ContainerID, partitionKeyPath: Settings.ContainerPartitionKeyPath);
                #region Logging
                ReportStatus("Container was created and/or verified");
                #endregion



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
                ReportStatus("Question 1 upserted");
                await CosmosContainer.UpsertItemAsync(item2);
                ReportStatus("Question 1 upserted");
                await CosmosContainer.UpsertItemAsync(item3);
                ReportStatus("Question 1 upserted");
                // ********************************************************************************


                Console.WriteLine("Done - press any key to exit");
                Console.ReadLine();

                ReportStatus("Application exiting successfully");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                _LogAs.Error(ex);
                Console.WriteLine("Application exiting with an error; check the logs for details");
                Environment.Exit(-1);
            }

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


        private static void ReportStatus(String Message)
        {
            _LogAs?.Info(Message);
            Console.WriteLine(Message);
        }


    }
}

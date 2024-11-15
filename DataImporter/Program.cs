using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace DataImporter
{
    internal class Program
    {
        private static NLog.ILogger? _LogAs;

        static async Task Main(string[] args)
        {
            Database CosmosDatabase;
            Container FlashCardContainer, TopicContainer;
            DataImporterSettings Settings;
            String RawDataFileContents;
            int RecordCounter;

            _LogAs = NLog.LogManager.GetCurrentClassLogger();

            #region Logging
            ReportStatus("");
            ReportStatus("************************");
            ReportStatus("Application starting; reading in arguments...");
            #endregion

            try
            {
                #region Logging
                ReportStatus("Loading up the command line arguments and config file settings...");
                #endregion
                Settings = new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json")
                    .Build()
                    .Get<DataImporterSettings>()
                    .ValidateDataAnnotations<DataImporterSettings>();



                #region Logging
                ReportStatus("Verifying database and container exist...");
                #endregion
                CosmosDatabase = await InitializeDatabase(Settings.CosmosDBSettings.AccountEndpoint, Settings.CosmosDBSettings.PrimaryKey, Settings.CosmosDBSettings.DatabaseName);
                #region Logging
                ReportStatus("Database was created and/or verified");
                #endregion
                FlashCardContainer = await CosmosDatabase.CreateContainerIfNotExistsAsync(id: Settings.CosmosDBSettings.FlashCardContainer.ContainerID, partitionKeyPath: Settings.CosmosDBSettings.FlashCardContainer.PartitionKeyPath);
                TopicContainer = await CosmosDatabase.CreateContainerIfNotExistsAsync(id: Settings.CosmosDBSettings.TopicContainer.ContainerID, partitionKeyPath: Settings.CosmosDBSettings.TopicContainer.PartitionKeyPath);
                #region Logging
                ReportStatus("Container was created and/or verified");
                #endregion


                #region Logging
                ReportStatus($"Loading data from {Settings.PathToDataFile}...");
                #endregion
                RawDataFileContents = LoadDataFromFile(Settings.PathToDataFile);
                dynamic FileContentsAsJson = JsonConvert.DeserializeObject(RawDataFileContents);
                RecordCounter = 1;
                foreach (JToken CurrentItem in FileContentsAsJson)
                {
                    await FlashCardContainer.UpsertItemAsync(CurrentItem);
                    await TopicContainer.UpsertItemAsync(CurrentItem["topic"]);
                    #region Logging
                    ReportStatus($"Question #{RecordCounter} upserted");
                    #endregion

                    RecordCounter++;
                }

                Console.WriteLine("Done - press any key to exit");
                Console.ReadLine();

                ReportStatus("Application exiting successfully");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                _LogAs.Error(ex);
                Console.WriteLine("Application exiting with an error; check the logs for details.");
                Environment.Exit(-1);
            }

        }


        private static String LoadDataFromFile(String DataFilePath)
        {
            return File.ReadAllText(DataFilePath);
        }


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

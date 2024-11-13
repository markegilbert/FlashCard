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
            String PrimaryKey;
            String PathToDataFile;
            Database CosmosDatabase;
            Container CosmosContainer;
            CosmosDBSettings Settings;
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
                ReportStatus("Reading in command line arguments...");
                #endregion
                PrimaryKey = args.ExtractValueFor("PrimaryKey", ValueIsRequired: true);
                PathToDataFile = args.ExtractValueFor("PathToDataFile", ValueIsRequired: true);

                #region Logging
                ReportStatus("Loading up the config file settings...");
                #endregion
                Settings = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()
                    .GetRequiredSection(CosmosDBSettings.SettingsName)
                    .Get<CosmosDBSettings>()
                    .ValidateDataAnnotations<CosmosDBSettings>();




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


                #region Logging
                ReportStatus($"Loading data from {PathToDataFile}...");
                #endregion
                RawDataFileContents = LoadDataFromFile(PathToDataFile);
                dynamic FileContentsAsJson = JsonConvert.DeserializeObject(RawDataFileContents);
                RecordCounter = 1;
                foreach (JToken CurrentItem in FileContentsAsJson)
                {
                    await CosmosContainer.UpsertItemAsync(CurrentItem);
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

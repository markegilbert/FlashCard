using FlashCard.Configuration;


namespace DataImporter
{
    // TODO: This class needs unit tests
    public class DataImporterSettings
    {
        public String PathToDataFile { get; set; }

        public CosmosDBSettings CosmosDBSettings { get; set; }
    }
}

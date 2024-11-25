using FlashCard.Configuration;


namespace DataImporter
{
    public class DataImporterSettings
    {
        public String PathToDataFile { get; set; }

        public CosmosDBSettings CosmosDBSettings { get; set; }
    }
}

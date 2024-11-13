using System.ComponentModel.DataAnnotations;


namespace DataImporter
{
    public class CosmosDBSettings
    {
        // This value refers to the appsettings.json block where these settings reside.
        public const String SettingsName = "CosmosDBSettings";

        private String _AccountEndpoint = "";
        private String _DatabaseName = "";
        private String _ContainerID = "";
        private String _ContainerPartitionKeyPath = "";


        [Required()]
        public String AccountEndpoint 
        { 
            get { return this._AccountEndpoint; }
            set 
            {
                value = (value ?? "").Trim();
                this._AccountEndpoint = value; 
            }
        }

        [Required()]
        public String DatabaseName
        {
            get { return this._DatabaseName; }
            set 
            {
                value = (value ?? "").Trim();
                this._DatabaseName = value; 
            }
        }

        [Required()]
        public String ContainerID
        {
            get { return this._ContainerID; }
            set 
            {
                value = (value ?? "").Trim();
                this._ContainerID = value; 
            }
        }

        [Required()]
        public String ContainerPartitionKeyPath
        {
            get { return this._ContainerPartitionKeyPath; }
            set 
            {
                value = (value ?? "").Trim();
                this._ContainerPartitionKeyPath = value; 
            }
        }
    }
}

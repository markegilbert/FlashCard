using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter
{
    public class CosmosDBSettings
    {
        // This value refers to the appsettings.json block where these settings reside.
        public const String SettingsName = "CosmosDBSettings";

        private String _AccountEndpoint = "";
        public String AccountEndpoint 
        { 
            get { return this._AccountEndpoint; }
            set 
            {
                value = (value ?? "").Trim();
                this._AccountEndpoint = value; 
            }
        }

        private String _DatabaseName = "";
        public String DatabaseName
        {
            get { return this._DatabaseName; }
            set 
            {
                value = (value ?? "").Trim();
                this._DatabaseName = value; 
            }
        }

        private String _ContainerID = "";
        public String ContainerID
        {
            get { return this._ContainerID; }
            set 
            {
                value = (value ?? "").Trim();
                this._ContainerID = value; 
            }
        }

        private String _ContainerPartitionKeyPath = "";
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

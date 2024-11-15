using System.ComponentModel.DataAnnotations;

namespace FlashCard.Configuration
{
    public class CosmosDBSettings
    {
        private String _AccountEndpoint = "";
        private String _DatabaseName = "";
        private String _PrimaryKey = "";


        [Required()]
        public String PrimaryKey 
        {
            get { return this._PrimaryKey; }
            set
            {
                value = (value ?? "").Trim();
                this._PrimaryKey = value;
            }
        }

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


        public ContainerSettings FlashCardContainer { get; set; } = new ContainerSettings();
        public ContainerSettings TopicContainer { get; set; } = new ContainerSettings();
    }
}

using System.ComponentModel.DataAnnotations;


namespace FlashCard.Configuration
{
    public class ContainerSettings
    {
        private String _ContainerID = "";
        private String _PartitionKeyPath = "";



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
        public String PartitionKeyPath
        {
            get { return this._PartitionKeyPath; }
            set
            {
                value = (value ?? "").Trim();
                this._PartitionKeyPath = value;
            }
        }

    }
}

﻿namespace FlashCard.Models
{
    public class FlashCardModel
    {
        public String ID { get; set; } = "";
        public String Question { get; set; } = "";
        public String Answer { get; set; } = "";
        public TopicModel Topic { get; set; } = new TopicModel();
        public String PartitionKey { get; set; } = "";
        public DateTime CreatedOn { get; set; }
    }
}

namespace FlashCard.Models
{
    public class FlashCardModel
    {
        public int ID { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }
        public TopicModel Topic { get; set; }
        public String PartitionKey { get; set; }
        public DateOnly CreatedOn { get; set; }
    }
}

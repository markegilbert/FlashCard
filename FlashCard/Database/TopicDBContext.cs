using FlashCard.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Database
{
    public class TopicDBContext: CosmosDbContextBase
    {
        public DbSet<TopicModel> Topics { get; set; }


        public TopicDBContext(String AccountEndpoint, String AuthorizationKey, String DatabaseName, String ContainerName) 
            : base(AccountEndpoint, AuthorizationKey, DatabaseName, ContainerName) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TopicModel>()
                .ToContainer(this._ContainerName)
                .HasPartitionKey(t => t.ID)
                .HasNoDiscriminator()
                .Property(t => t.TopicName).ToJsonProperty("topicName");
        }

    }
}

using FlashCard.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FlashCard.Database
{
    public class FlashCardDBContext : CosmosDbContextBase
    {
        public DbSet<FlashCardModel> FlashCards { get; set; }


        public FlashCardDBContext(String AccountEndpoint, String AuthorizationKey, String DatabaseName, String ContainerName)
            : base(AccountEndpoint, AuthorizationKey, DatabaseName, ContainerName) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<FlashCardModel>()
                .ToContainer(this._ContainerName)
                .HasPartitionKey(t => t.PartitionKey)
                .HasNoDiscriminator();

            // TODO: Find a way to remove these explicit mappings, either by having EF generate the mappings based on the names, or through the use of JsonPropertyName attributes on the model
            // I'd rather have these get mapped purely based on their name, or through JsonPropertyName attributes added to
            // the FlashCardModel class.  However, it appears that EF for Cosmos doesn't serialize things that same way as
            // something like EF for SQL Server.  There appears to be a way around this using the Cosmos DB SDK by specifying
            // a custom serializer to use:
            //      https://www.billtalkstoomuch.com/2023/03/12/cosmosdb-vs-system-text-json/
            // But I haven't yet found a way to specify this using EF Core.
            modelBuilder.Entity<FlashCardModel>()
                .Property(t => t.PartitionKey).ToJsonProperty("partitionKey");

            modelBuilder.Entity<FlashCardModel>()
                .Property(t => t.Question).ToJsonProperty("question");

            modelBuilder.Entity<FlashCardModel>()
                .Property(t => t.Answer).ToJsonProperty("answer");

            modelBuilder.Entity<FlashCardModel>()
                .Property(t => t.CreatedOn).ToJsonProperty("createdOn");

            // TODO: Get the Topic property mapped.
        }
    }
}

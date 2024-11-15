using FlashCard.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Database
{
    public class TopicDBContext: DbContext
    {
        private String _AccountEndpoint;
        private String _AuthorizationKey;
        private String _DatabaseName;
        private String _ContainerName;

        public DbSet<TopicModel> Topics { get; set; }


        public TopicDBContext(String AccountEndpoint, String AuthorizationKey, String DatabaseName, String ContainerName)
        {
            // TODO: Validate these
            this._AccountEndpoint = AccountEndpoint;
            this._AuthorizationKey = AuthorizationKey;
            this._DatabaseName = DatabaseName;
            this._ContainerName = ContainerName;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(this._AccountEndpoint, this._AuthorizationKey,databaseName: this._DatabaseName);


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

using FlashCard.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Database
{
    public class FlashCardDBContext : DbContext
    {
        private String _AccountEndpoint;
        private String _AuthorizationKey;
        private String _DatabaseName;
        private String _ContainerName;

        public DbSet<FlashCardModel> FlashCards { get; set; }


        public FlashCardDBContext(String AccountEndpoint, String AuthorizationKey, String DatabaseName, String ContainerName)
        {
            // TODO: Validate these
            this._AccountEndpoint = AccountEndpoint;
            this._AuthorizationKey = AuthorizationKey;
            this._DatabaseName = DatabaseName;
            this._ContainerName = ContainerName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(this._AccountEndpoint, this._AuthorizationKey, databaseName: this._DatabaseName);


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Clean this up once I've fixed the "Unable to track an entity of type 'FlashCardModel' because its primary key property 'PartitionKey' is null." issue
            // **********************************************************************
            //modelBuilder.Entity<FlashCardModel>()
            //    .Property(t => t.PartitionKey).ToJsonProperty("partitionKey")
            //    .ValueGeneratedOnAdd();
            // **********************************************************************
            //modelBuilder.Entity<FlashCardModel>()
            //    .Property(t => t.PartitionKey)
            //    .ValueGeneratedOnAdd();
            // **********************************************************************
            //modelBuilder.Entity<FlashCardModel>()
            //    .ToContainer(this._ContainerName)
            //    .HasPartitionKey(t => t.PartitionKey)
            //    .HasNoDiscriminator();

            //modelBuilder.Entity<FlashCardModel>()
            //    .Property(t => t.PartitionKey).ToJsonProperty("partitionKey");
            // **********************************************************************
            modelBuilder.Entity<FlashCardModel>()
                .ToContainer(this._ContainerName)
                .HasPartitionKey(t => t.PartitionKey)
                .HasNoDiscriminator();
        }
    }
}

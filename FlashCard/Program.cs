
using FlashCard.Configuration;
using FlashCard.Database;
using NLog.Extensions.Logging;
using System.Reflection;

namespace FlashCard
{
    public class Program
    {
        private static NLog.ILogger? _LogAs;


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            _LogAs = NLog.LogManager.GetCurrentClassLogger();

            #region Logging
            _LogAs.Info("");
            _LogAs.Info("************************");
            _LogAs.Info("Program starting");
            #endregion


            // TODO: Load up the context objects
            builder.Services.AddTransient<CosmosDBSettings>((serviceProvider) => {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                    .Build()
                    .GetRequiredSection("CosmosDBSettings")
                    .Get<CosmosDBSettings>()
                    .ValidateDataAnnotations<CosmosDBSettings>();
            });

            #region Logging
            _LogAs.Info("Configuration settings loaded");
            #endregion

            builder.Services.AddSingleton<TopicDBContext>((serviceProvider) => {
                return new TopicDBContext(serviceProvider.GetService<CosmosDBSettings>().AccountEndpoint,
                    serviceProvider.GetService<CosmosDBSettings>().PrimaryKey,
                    serviceProvider.GetService<CosmosDBSettings>().DatabaseName,
                    serviceProvider.GetService<CosmosDBSettings>().TopicContainer.ContainerID);
            });
            builder.Services.AddSingleton<FlashCardDBContext>((serviceProvider) => {
                return new FlashCardDBContext(serviceProvider.GetService<CosmosDBSettings>().AccountEndpoint,
                    serviceProvider.GetService<CosmosDBSettings>().PrimaryKey,
                    serviceProvider.GetService<CosmosDBSettings>().DatabaseName,
                    serviceProvider.GetService<CosmosDBSettings>().FlashCardContainer.ContainerID);
            });

            #region Logging
            _LogAs.Info("EF Contexts loaded");
            #endregion


            // Add services to the DI container.
            builder.Logging.ClearProviders();
            builder.Logging.AddNLog();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

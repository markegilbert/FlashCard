
using FlashCard.Configuration;
using FlashCard.Database;
using NLog.Extensions.Logging;
using System.Reflection;
using System.Security;

namespace FlashCard
{
    public class Program
    {
        private static NLog.ILogger? _LogAs;
        private const String CORS_POLICY_NAME = "AllowAllOriginsPolicy";


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            _LogAs = NLog.LogManager.GetCurrentClassLogger();

            #region Logging
            _LogAs.Info("");
            _LogAs.Info("************************");
            _LogAs.Info("Program starting");
            #endregion


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
            builder.Services.AddTransient<FlashCardsRepository>((serviceProvider) =>
            { 
                return new FlashCardsRepository(serviceProvider.GetService<FlashCardDBContext>());
            });

            #region Logging
            _LogAs.Info("EF Contexts loaded");
            #endregion


            // Add logging services to the DI container.
            builder.Logging.ClearProviders();
            builder.Logging.AddNLog();

            // Configure the CORS policy
            // Source: https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY_NAME, 
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin();
                                  });
            });

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
                app.UseHttpsRedirection();
            }

            app.UseCors(CORS_POLICY_NAME);

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

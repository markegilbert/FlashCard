
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

            #region Logging
            _LogAs.Info("Logging added to the DI container");
            #endregion

            // Configure the CORS policy
            // Source: https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0
            // Source: https://dev.to/fabriziobagala/cors-in-aspnet-core-4hl2
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY_NAME, 
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .WithMethods("GET", "POST", "DELETE")
                                            .WithHeaders("Content-Type");
                                  });
            });

            #region Logging
            _LogAs.Info("CORS policy added");
            #endregion


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Logging
            _LogAs.Info("Endpoints added");
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }

            #region Logging
            _LogAs.Info("HTTP request pipeline configured");
            #endregion


            app.UseCors(CORS_POLICY_NAME);

            #region Logging
            _LogAs.Info("CORS policy configured");
            #endregion

            app.UseAuthorization();

            #region Logging
            _LogAs.Info("Authorization capabilities configured");
            #endregion

            app.MapControllers();

            #region Logging
            _LogAs.Info("Controllers were mapped");
            #endregion

            try
            {
                #region Logging
                _LogAs.Info("app.Run() is about to be invoked");
                #endregion

                app.Run();

                #region Logging
                _LogAs.Info("app.Run() was just invoked");
                #endregion
            }
            catch (Exception ex)
            {
                #region Logging
                _LogAs.Error("\t**************");
                _LogAs.Error("\tError running the app");
                _LogAs.Error(ex.Message);
                _LogAs.Error(ex.StackTrace);
                _LogAs.Error("\t**************");
                #endregion

                throw;
            }

        }
    }
}


using FlashCard.Database;
using NLog.Extensions.Logging;

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

            // TODO: Load up the application settings

            // TODO: Load up the context objects
            builder.Services.AddSingleton<TopicDBContext>(new TopicDBContext("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "flashcard", "topic"));
            

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

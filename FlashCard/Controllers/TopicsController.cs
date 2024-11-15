using Microsoft.AspNetCore.Mvc;
using FlashCard.Models;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;


namespace FlashCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private ILogger<TopicsController> _Logger;


        public TopicsController(ILogger<TopicsController> Logger) 
        {
            // TODO: Validate this
            this._Logger = Logger;
        }

        [HttpGet(Name = "GetTopics")]
        public async Task<IEnumerable<TopicModel>> Get()
        {
            #region Logging
            this._Logger.LogDebug("About to return the list of available topics");
            #endregion

            // ***************************************************************
            // TODO: Replace with the real logic
            //List<TopicModel> Topics = new List<TopicModel>();
            //Topics.Add(new TopicModel() { ID = 1, TopicName = "First" });
            //Topics.Add(new TopicModel() { ID = 2, TopicName = "Second" });
            //Topics.Add(new TopicModel() { ID = 3, TopicName = "Third" });
            //Topics.Add(new TopicModel() { ID = 4, TopicName = "Fourth" });
            //return (from t in Topics select t).OrderBy(t => t.TopicName).ToArray();
            // ***************************************************************

            // TODO: Move this logic into a new EF repository
            CosmosClient Server;
            Microsoft.Azure.Cosmos.Container FlashCardContainer;
            Microsoft.Azure.Cosmos.Container TopicContainer;

            Server = new CosmosClient(accountEndpoint: "https://localhost:8081", authKeyOrResourceToken: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            FlashCardContainer = Server.GetContainer("flashcard", "flashcard");
            TopicContainer = Server.GetContainer("flashcard", "topic");
            // *********
            //var x = (from f in FlashCardContainer.GetItemLinqQueryable<TopicModel>() select new TopicModel() { ID = f.ID, TopicName = f.TopicName}).ToList();
            //var x = (from f in FlashCardContainer.GetItemLinqQueryable<FlashCardModel>(allowSynchronousQueryExecution: true) select new TopicModel() { ID = f.Topic.ID, TopicName = f.Topic.TopicName }).ToList();
            // *********
            // This works, but only when I force FlashCards to be a List.  Keeping it as an IQueryable doesn't work - the Topics list comes out empty.
            //var FlashCards = (from f in FlashCardContainer.GetItemLinqQueryable<FlashCardModel>(allowSynchronousQueryExecution: true) select f).ToList();
            //var Topics = (from t in FlashCards select t.Topic).ToList();
            // *********
            // This works, but is there a way to extract the Topics without having to get back all FlashCard data first?
            //var FlashCards = (from f in FlashCardContainer.GetItemLinqQueryable<FlashCardModel>(allowSynchronousQueryExecution: true) select f).AsEnumerable();
            //var Topics = (from t in FlashCards select t.Topic).ToList();
            // *********
            // Adapted from: https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-dotnet-query-items
            // This works, and I only bring back the fields I care about, but it requires me to walk through the records one at a time.
            //using FeedIterator<TopicModel> feed = FlashCardContainer.GetItemQueryIterator<TopicModel>(
            //    queryText: "SELECT f.topic.id, f.topic.topicName FROM flashcard f"
            //);
            //while (feed.HasMoreResults)
            //{
            //    FeedResponse<TopicModel> response = await feed.ReadNextAsync();

            //    // Iterate query results
            //    foreach (TopicModel item in response)
            //    {
            //        Console.WriteLine($"Found item:\t{item.ID}");
            //    }
            //}
            // *********
            //using FeedIterator<TopicModel> feed = FlashCardContainer.GetItemQueryIterator<TopicModel>(
            //    queryText: "SELECT f.topic.id, f.topic.topicName FROM flashcard f"
            //);
            //while (feed.HasMoreResults)
            //{
            //    FeedResponse<TopicModel> response = await feed.ReadNextAsync();

            //    // Iterate query results
            //    foreach (TopicModel item in response)
            //    {
            //        Console.WriteLine($"Found item:\t{item.ID}");
            //    }
            //}
            // *********
            // This works, and I don't have to walk through the results creating the List one record at a time, but it's slow.  The
            // service takes 2-3 seconds every time.
            //long TimeStampNow = DateTime.Now.Ticks;
            //long TimeStampPrev = TimeStampNow;
            //using FeedIterator<TopicModel> feed = FlashCardContainer.GetItemQueryIterator<TopicModel>(
            //    queryText: "SELECT FC.topic.id, FC.topic.topicName FROM flashcard FC WHERE FC.partitionKey = 'AZ204_AzureDeveloperAssociate'"
            //);
            //TimeStampNow = DateTime.Now.Ticks;
            //this._Logger.LogInformation($"\tElapsed Time for FlashCardContainer.GetItemQueryIterator: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
            //TimeStampPrev = TimeStampNow;


            //// The FeedIterator returns the results in pages.
            //List<TopicModel> Topics;
            //if (feed.HasMoreResults)
            //{
            //    // This returns the first page of results only
            //    FeedResponse<TopicModel> response = await feed.ReadNextAsync();

            //    TimeStampNow = DateTime.Now.Ticks;
            //    this._Logger.LogInformation($"\tElapsed Time for feed.ReadNextAsync: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
            //    TimeStampPrev = TimeStampNow;

            //    Topics = (from t in response.AsEnumerable() select t).ToList();
            //    this._Logger.LogInformation($"\tRU's incurred: {response.RequestCharge}");

            //    TimeStampNow = DateTime.Now.Ticks;
            //    this._Logger.LogInformation($"\tElapsed Time for response.AsEnumerable: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
            //    TimeStampPrev = TimeStampNow;
            //}
            //else
            //{
            //    Topics = new List<TopicModel>();
            //}
            // *********
            #region Logging
            long TimeStampNow = DateTime.Now.Ticks;
            long TimeStampPrev = TimeStampNow;
            #endregion
            using FeedIterator<TopicModel> feed = TopicContainer.GetItemQueryIterator<TopicModel>(
                queryText: "SELECT T.id, T.topicName FROM topic T", requestOptions: new QueryRequestOptions() { MaxItemCount = -1 }
            );
            #region Logging
            TimeStampNow = DateTime.Now.Ticks;
            this._Logger.LogInformation($"\tElapsed Time for FlashCardContainer.GetItemQueryIterator: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
            TimeStampPrev = TimeStampNow;
            #endregion


            // The FeedIterator returns the results in pages.  Normally this would be a while loop that evaluates feed.HasMoreResults
            List<TopicModel> Topics = new List<TopicModel>();
            while (feed.HasMoreResults)
            {
                // This returns the first page of results only
                FeedResponse<TopicModel> response = await feed.ReadNextAsync();

                #region Logging
                TimeStampNow = DateTime.Now.Ticks;
                this._Logger.LogInformation($"\tElapsed Time for feed.ReadNextAsync: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
                TimeStampPrev = TimeStampNow;
                #endregion

                // Iterate query results
                foreach (TopicModel item in response)
                {
                    Topics.Add(item);
                }
                #region Logging
                TimeStampNow = DateTime.Now.Ticks;
                this._Logger.LogInformation($"\tElapsed Time for \"response\" loop: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
                TimeStampPrev = TimeStampNow;
                this._Logger.LogInformation($"\tRU's incurred: {response.RequestCharge}");
                #endregion
            }

            // *********
            //#region Logging
            //long TimeStampNow = DateTime.Now.Ticks;
            //long TimeStampPrev = TimeStampNow;
            //#endregion

            //var Topics = (from f in TopicContainer.GetItemLinqQueryable<TopicModel>(allowSynchronousQueryExecution: true) select f).ToList();

            //#region Logging
            //TimeStampNow = DateTime.Now.Ticks;
            //this._Logger.LogDebug($"\tElapsed Time for TopicContainer.GetItemLinqQueryable: {TimeSpan.FromTicks(TimeStampNow - TimeStampPrev)}");
            //TimeStampPrev = TimeStampNow;
            //#endregion

            #region Logging
            this._Logger.LogDebug("Returning the list now");
            #endregion

            return Topics;

            // TODO: The orderedlist should be cached
        }
    }
}

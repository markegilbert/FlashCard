using Microsoft.AspNetCore.Mvc;
using FlashCard.Models;


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
        public IEnumerable<TopicModel> Get()
        {
            #region Logging
            this._Logger.LogDebug("About to return the list of available topics");
            #endregion

            // TODO: Replace with the real logic
            List<TopicModel> Topics = new List<TopicModel>();
            Topics.Add(new TopicModel() { ID = 1, TopicName = "First" });
            Topics.Add(new TopicModel() { ID = 2, TopicName = "Second" });
            Topics.Add(new TopicModel() { ID = 3, TopicName = "Third" });
            Topics.Add(new TopicModel() { ID = 4, TopicName = "Fourth" });

            #region Logging
            this._Logger.LogDebug("Returning the list now");
            #endregion

            // TODO: The orderedlist should be cached
            return (from t in Topics select t).OrderBy(t => t.TopicName).ToArray();
        }
    }
}

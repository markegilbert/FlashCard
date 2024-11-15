using Microsoft.AspNetCore.Mvc;
using FlashCard.Models;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using FlashCard.Database;
using Microsoft.EntityFrameworkCore;


namespace FlashCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private ILogger<TopicsController> _Logger;
        private TopicDBContext _Context;


        public TopicsController(ILogger<TopicsController> Logger, TopicDBContext Context) 
        {
            // TODO: Validate these
            this._Logger = Logger;
            this._Context = Context;
        }

        [HttpGet(Name = "GetTopics")]
        public async Task<IEnumerable<TopicModel>> Get()
        {
            #region Logging
            this._Logger.LogDebug("About to return the list of available topics");
            #endregion

            return await this._Context.Topics.ToListAsync();
        }
    }
}

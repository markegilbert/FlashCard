using Microsoft.AspNetCore.Mvc;
using FlashCard.Database;
using FlashCard.Models;
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
            if (Logger is null) { throw new ArgumentNullException(nameof(Logger)); }
            if (Context is null) { throw new ArgumentNullException(nameof(Context)); }

            this._Logger = Logger;
            this._Context = Context;

            #region Logging
            this._Logger.LogInformation("TopicsController instantiated");
            #endregion
        }

        [HttpGet(Name = "GetTopics")]
        public async Task<IEnumerable<TopicModel>> Get()
        {
            try
            {
                #region Logging
                this._Logger.LogDebug("About to return the list of available topics");
                #endregion

                return await this._Context.Topics.ToListAsync();
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}

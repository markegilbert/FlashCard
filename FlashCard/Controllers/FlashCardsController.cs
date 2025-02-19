using Microsoft.AspNetCore.Mvc;
using FlashCard.Database;
using FlashCard.Models;


namespace FlashCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardsController : ControllerBase
    {
        private ILogger<FlashCardsController> _Logger;
        private FlashCardsRepository _Repository;


        public FlashCardsController(ILogger<FlashCardsController> Logger, FlashCardsRepository Repository)
        {
            if (Logger is null) {  throw new ArgumentNullException(nameof(Logger)); }
            if (Repository is null) { throw new ArgumentNullException(nameof(Repository)); }

            this._Logger = Logger;
            this._Repository = Repository;

            #region Logging
            this._Logger.LogInformation("FlashCardsController instantiated");
            #endregion
        }

        [HttpGet(Name = "GetFlashCards")]
        public async Task<IEnumerable<FlashCardModel>> Get([FromQuery] String TopicID, [FromQuery] int NumberOfFlashcards, [FromQuery] String? OrderBy)
        {
            try
            {
                // TODO: Modify the logging statement
                #region Logging
                this._Logger.LogDebug("About to return the list of random flash cards");
                #endregion

                // TODO: Would this switch be better handled in the repository?  Or the fact that I'm deciding between a random list and a
                //       list by specific field(s) be decided here in the controller?
                if (String.IsNullOrEmpty(OrderBy)) { return await this._Repository.GetRandomFlashCardsByTopic(TopicID, NumberOfFlashcards); }
                return await this._Repository.GetFlashCardsByTopic(TopicID, NumberOfFlashcards, OrderBy);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex.ToString());
                throw;
            }

        }
    }
}

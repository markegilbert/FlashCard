using FlashCard.Database;
using FlashCard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // TODO: Validate these
            this._Logger = Logger;
            this._Repository = Repository;
        }

        [HttpGet(Name = "GetFlashCards")]
        public async Task<IEnumerable<FlashCardModel>> Get([FromQuery] String TopicID, [FromQuery] int NumberOfFlashcards)
        {
            List<FlashCardModel> Flashcards;

            #region Logging
            this._Logger.LogDebug("About to return the list of random flash cards");
            #endregion

            return await this._Repository.GetRandomFlashCardsByTopic(TopicID, NumberOfFlashcards);
        }
    }
}

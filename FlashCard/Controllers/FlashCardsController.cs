﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<FlashCardModel>> Get([FromQuery] String TopicID, [FromQuery] int NumberOfFlashcards)
        {
            try
            {
                #region Logging
                this._Logger.LogDebug("About to return the list of random flash cards");
                #endregion

                return await this._Repository.GetRandomFlashCardsByTopic(TopicID, NumberOfFlashcards);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex.ToString());
                throw;
            }

        }
    }
}

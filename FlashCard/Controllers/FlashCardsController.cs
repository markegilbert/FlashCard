using FlashCard.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardsController : ControllerBase
    {
        private ILogger<FlashCardsController> _Logger;


        public FlashCardsController(ILogger<FlashCardsController> Logger)
        {
            // TODO: Validate this
            this._Logger = Logger;
        }

        [HttpGet(Name = "GetFlashCards")]
        public IEnumerable<FlashCardModel> Get()
        {
            #region Logging
            this._Logger.LogDebug("About to return the list of random flash cards");
            #endregion

            // TODO: Replace with the real logic
            List<FlashCardModel> FlashCards = new List<FlashCardModel>();
            FlashCards.Add(new FlashCardModel() { ID = 1, Question = "First Question", Answer = "First Answer" });
            FlashCards.Add(new FlashCardModel() { ID = 2, Question = "Second Question", Answer = "Second Answer" });
            FlashCards.Add(new FlashCardModel() { ID = 3, Question = "Third Question", Answer = "Third Answer" });
            FlashCards.Add(new FlashCardModel() { ID = 4, Question = "Fourth Question", Answer = "Fourth Answer" });

            #region Logging
            this._Logger.LogDebug("Returning the list now");
            #endregion

            return (from f in FlashCards select f).OrderBy(f => Guid.NewGuid()).ToArray();
        }
    }
}

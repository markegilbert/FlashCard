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
        private FlashCardDBContext _Context;


        public FlashCardsController(ILogger<FlashCardsController> Logger, FlashCardDBContext Context)
        {
            // TODO: Validate these
            this._Logger = Logger;
            this._Context = Context;
        }

        [HttpGet(Name = "GetFlashCards")]
        public async Task<IEnumerable<FlashCardModel>> Get()
        {
            #region Logging
            this._Logger.LogDebug("About to return the list of random flash cards");
            #endregion

            // TODO: Replace with the real logic
            //List<FlashCardModel> FlashCards = new List<FlashCardModel>();
            //FlashCards.Add(new FlashCardModel() { ID = 1, Question = "First Question", Answer = "First Answer" });
            //FlashCards.Add(new FlashCardModel() { ID = 2, Question = "Second Question", Answer = "Second Answer" });
            //FlashCards.Add(new FlashCardModel() { ID = 3, Question = "Third Question", Answer = "Third Answer" });
            //FlashCards.Add(new FlashCardModel() { ID = 4, Question = "Fourth Question", Answer = "Fourth Answer" });

            #region Logging
            this._Logger.LogDebug("Returning the list now");
            #endregion

            return await this._Context.FlashCards.ToListAsync();

            //return (from f in FlashCards select f).OrderBy(f => Guid.NewGuid()).ToArray();
        }
    }
}

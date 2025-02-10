using FlashCard.Database;
using FlashCard.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlashCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardController : ControllerBase
    {

        private ILogger<FlashCardController> _Logger;
        private FlashCardsRepository _Repository;


        public FlashCardController(ILogger<FlashCardController> Logger, FlashCardsRepository Repository)
        {
            if (Logger is null) { throw new ArgumentNullException(nameof(Logger)); }
            if (Repository is null) { throw new ArgumentNullException(nameof(Repository)); }

            this._Logger = Logger;
            this._Repository = Repository;

            #region Logging
            this._Logger.LogInformation("FlashCardController instantiated");
            #endregion
        }


        [HttpPost(Name = "InsertFlashCard")]
        public async Task<IActionResult> Post([FromBody] FlashCardModel NewFlashCard)
        {
            try
            {
                #region Logging
                this._Logger.LogDebug("Validating the new flashcard");
                #endregion

                // TODO: Validate the object.  Some of this is already being done by the framework - ID, TopicName, and PartitionKey are all required.  Make these nullable?

                // Populate the other necessary properties
                NewFlashCard.ID = Guid.NewGuid().ToString();
                NewFlashCard.PartitionKey = NewFlashCard.Topic.ID;
                NewFlashCard.CreatedOn = DateOnly.FromDateTime(DateTime.Today);

                #region Logging
                this._Logger.LogDebug("Saving the new flashcard");
                #endregion

                this._Repository.InsertFlashCard(NewFlashCard);
                await this._Repository.Save();

                #region Logging
                this._Logger.LogDebug("New flashcard has been saved");
                #endregion

                return Ok(NewFlashCard);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex.ToString());
                return BadRequest("An error occurred adding the new flashcard.");
            }
        }


        [HttpDelete(Name = "DeleteFlashCard")]
        public async Task<IActionResult> Delete([FromQuery] String PartitionKey, [FromQuery] String FlashCardID)
        {
            try
            {
                #region Logging
                this._Logger.LogDebug("Validating the requested ID");
                #endregion

                // TODO: Validate the object

                #region Logging
                this._Logger.LogDebug("Deleting the new flashcard");
                #endregion

                await this._Repository.DeleteFlashCard(PartitionKey, FlashCardID);
                int NumberRecordsAffected = await this._Repository.Save();

                #region Logging
                this._Logger.LogDebug("New flashcard has been deleted");
                #endregion

                return Ok(NumberRecordsAffected);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex.ToString());
                return BadRequest("An error occurred deleting the flashcard.");
            }
        }



    }
}

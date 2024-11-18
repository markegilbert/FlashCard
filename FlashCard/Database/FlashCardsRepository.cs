using FlashCard.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Database
{
    public class FlashCardsRepository
    {

        private FlashCardDBContext _Context;

        public FlashCardsRepository(FlashCardDBContext Context)
        {
            // TODO: Validate this
            this._Context = Context;
        }


        public async Task<ICollection<FlashCardModel>> GetRandomFlashCardsByTopic(String TopicID, int NumberOfFlashcards)
        {
            return (from f in await this._Context.FlashCards.ToListAsync() orderby Guid.NewGuid() select f)
                        .Where(f => f.PartitionKey.Equals(TopicID))
                        .Take(NumberOfFlashcards)
                        .ToList();
        }
    }
}

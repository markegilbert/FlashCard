using FlashCard.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCard.Database
{
    public class FlashCardsRepository
    {
        private FlashCardDBContext _Context;


        public FlashCardsRepository(FlashCardDBContext Context)
        {
            if (Context is null) {  throw new ArgumentNullException(nameof(Context)); }

            this._Context = Context;
        }

        public async Task<ICollection<FlashCardModel>> GetRandomFlashCardsByTopic(String TopicID, int NumberOfFlashcards)
        {
            return (from f in await this._Context.FlashCards.ToListAsync() orderby Guid.NewGuid() select f)
                        .Where(f => f.PartitionKey.Equals(TopicID))
                        .Take(NumberOfFlashcards)
                        .ToList();
        }


        public void InsertFlashCard(FlashCardModel NewFlashCard)
        {
            this._Context.FlashCards.Add(NewFlashCard);
        }


        public async Task DeleteFlashCard(String PartitionKey, String FlashCardID)
        {
            FlashCardModel? FlashCardToRemove;

            FlashCardToRemove = await this._Context.FlashCards.FindAsync(FlashCardID, PartitionKey);
            if (FlashCardToRemove != null) { this._Context.FlashCards.Remove(FlashCardToRemove); }
        }



        public async Task<int> Save()
        {
            return await this._Context.SaveChangesAsync();
        }

    }
}

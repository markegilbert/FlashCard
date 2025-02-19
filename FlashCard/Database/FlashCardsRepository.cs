using FlashCard.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<ICollection<FlashCardModel>> GetFlashCardsByTopic(String TopicID, int NumberOfFlashcards, String Sort)
        {
            IEnumerable<FlashCardModel> Query;

            Query = (from f in await this._Context.FlashCards.ToListAsync() select f)
                        .Where(f => f.PartitionKey.Equals(TopicID))
                        .Take(NumberOfFlashcards);

            // TODO: Examine Sort, and append OrderBy / OrderByDescending accordingly.
            //Query = Query.OrderByDescending(f => f.CreatedOn);
            //Query = Query.OrderByDescending(f => f.GetType().GetProperty("CreatedOn").GetValue(f, null));
            //Query = Query.OrderByDescending(f => ToSortFuncList(Sort)[0].SortFunction.GetValue(f, null));
            foreach (OrderByItem CurrentSort in this.ToOrderByFuncList<FlashCardModel>(Sort))
            {
                if (CurrentSort.Ascending)
                {
                    // TODO: Do I need to make the assignment here?
                    Query = Query.OrderBy(f => CurrentSort.SortFunction.GetValue(f, null));
                }
                else
                {
                    Query = Query.OrderByDescending(f => CurrentSort.SortFunction.GetValue(f, null));
                }
            }

            return Query.ToList();
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


        public List<OrderByItem> ToOrderByFuncList<T>(String SortString) where T: new()
        {
            List<OrderByItem> ConvertedList;

            ConvertedList = new List<OrderByItem>();

            SortString = (SortString ?? "").Trim();
            if (String.IsNullOrEmpty(SortString)) { return ConvertedList; }

            // TODO: Split the string by commas
            // TODO: Validate the fields being requested; ignore invalid ones
            // TODO: Treat a leading minus as sort descending; a leading plus or nothing should be sort ascending

            ConvertedList.Add(new OrderByItem
            {
                SortFunction = (new T()).GetType().GetProperty(SortString),
                Ascending = true
            });

            return ConvertedList;
        }

    }
}

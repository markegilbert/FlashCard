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

        public async Task<ICollection<FlashCardModel>> GetFlashCardsByTopic(String TopicID, int NumberOfFlashcards, String? OrderBy)
        {
            IEnumerable<FlashCardModel> Query;

            // If no sort is specified, return a randomized sort
            if (String.IsNullOrEmpty(OrderBy)) { return await this.GetRandomFlashCardsByTopic(TopicID, NumberOfFlashcards); }

            // Build the basic query
            Query = (from f in await this._Context.FlashCards.ToListAsync() select f)
                        .Where(f => f.PartitionKey.Equals(TopicID))
                        .Take(NumberOfFlashcards);

            // Now examine Sort, and append OrderBy / OrderByDescending accordingly.
            foreach (OrderByItem CurrentSort in this.ToOrderByFuncList<FlashCardModel>(OrderBy))
            {
                if (CurrentSort.Ascending) { Query = Query.OrderBy(f => CurrentSort.SortFunction.GetValue(f, null)); }
                else { Query = Query.OrderByDescending(f => CurrentSort.SortFunction.GetValue(f, null)); }
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


        // Adapted from: https://stackoverflow.com/questions/17738499/create-dynamic-funct-tresult-from-object
        public List<OrderByItem> ToOrderByFuncList<T>(String RawOrderBy) where T: new()
        {
            List<OrderByItem> ConvertedList;
            T EmptyModel;
            String CurrentRefinedProperty;
            String[] RawProperties;
            bool SortAscending;


            ConvertedList = new List<OrderByItem>();


            RawOrderBy = (RawOrderBy ?? "").Trim();
            if (String.IsNullOrEmpty(RawOrderBy)) { return ConvertedList; }

            EmptyModel = new T();

            RawProperties = RawOrderBy.Split(',');
            foreach (String CurrentRawProperty in RawProperties)
            {
                // The function defaults no leading character to Ascending
                SortAscending = true;

                // First, remove any extra spacing
                CurrentRefinedProperty = CurrentRawProperty.Trim();
                if (CurrentRefinedProperty.StartsWith("-"))
                {
                    SortAscending = false;
                    CurrentRefinedProperty = CurrentRefinedProperty.Replace("-", "");
                }
                else if (CurrentRefinedProperty.StartsWith("+"))
                {
                    SortAscending = true;
                    CurrentRefinedProperty = CurrentRefinedProperty.Replace("+", "");
                }

                // If the named property doesn't exist, just skip it
                if (EmptyModel.GetType().GetProperty(CurrentRefinedProperty) == null) { continue; }

                ConvertedList.Add(new OrderByItem
                {
                    SortFunction = EmptyModel.GetType().GetProperty(CurrentRefinedProperty),
                    Ascending = SortAscending
                });
            }



            return ConvertedList;
        }

    }
}

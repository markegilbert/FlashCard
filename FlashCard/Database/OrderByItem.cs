using System.Reflection;

namespace FlashCard.Database
{
    public class OrderByItem
    {
        public PropertyInfo SortFunction { get; set; }
        public bool Ascending;
    }
}

using System.Collections.Generic;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class MagicFindVisitor : VisitorBase
    {
        private const string TOKEN = "{MagicFind}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            IFilter magicFindFilter = new OrFilter(new ItemRarityFilter(), new ItemQuantityFilter());
            return current.Replace(TOKEN, runFilter(magicFindFilter, items));
        }
    }
}

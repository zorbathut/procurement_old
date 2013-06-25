using System.Collections.Generic;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class DropOnlyGemVisitor : VisitorBase
    {
        private const string TOKEN = "{DropOnlyGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<DropOnlyGemFilter>(items));
        }
    }
}

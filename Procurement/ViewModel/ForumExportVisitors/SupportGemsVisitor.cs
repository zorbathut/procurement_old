using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class SupportGemsVisitor : VisitorBase
    {
        private const string TOKEN = "{SupportGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<SupportGemsFilter>(items.OfType<Gem>().Where(g => !g.IsQuality).OrderBy(g => g.IconURL)));
        }
    }
}

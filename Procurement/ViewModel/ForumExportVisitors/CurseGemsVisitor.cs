using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class CurseGemsVisitor : VisitorBase
    {
        private const string TOKEN = "{CurseGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<CurseGemFilter>(items.OfType<Gem>().Where(g => !g.IsQuality)));
        }
    }
}

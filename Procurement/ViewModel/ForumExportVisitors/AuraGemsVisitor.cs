using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class AuraGemsVisitor : VisitorBase
    {
        private const string TOKEN = "{AuraGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<AuraGemsFilter>(items.OfType<Gem>().Where(g => !g.IsQuality)));
        }
    }
}
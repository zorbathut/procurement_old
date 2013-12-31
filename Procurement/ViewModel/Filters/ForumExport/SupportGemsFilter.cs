using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class SupportGemsFilter : IFilter
    {
        private List<string> supportGems;
        
        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public SupportGemsFilter()
        {
            supportGems = Settings.SupportGems;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Support Gems"; }
        }

        public string Help
        {
            get { return "Gems that modify skill gems they are linked to"; }
        }

        public bool Applicable(Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return item.Properties[0].Name.Contains("Support");
        }
    }
}

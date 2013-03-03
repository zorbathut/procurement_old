using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class QualityGemFilter : IFilter
    {

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Quality Gems"; }
        }

        public string Help
        {
            get { return "Quality Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return gem.IsQuality;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public class QualityFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Quality; }
        }

        private Rarity quality;
        public QualityFilter(Rarity quality)
        {
            this.quality = quality;
        }

        public string Keyword { get { return quality.ToString() + " quality"; } }
        public string Help { get { return "Returns All " + quality.ToString() + " quality items"; } }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear != null)
                return gear.Rarity == quality;

            return false;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }
    }
}
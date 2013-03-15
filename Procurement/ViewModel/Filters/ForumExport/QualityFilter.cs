using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public abstract class QualityFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Quality; }
        }

        private Quality quality;
        public QualityFilter(Quality quality)
        {
            this.quality = quality;
        }

        public string Keyword { get { return quality.ToString() + " quality"; } }
        public string Help { get { return "Returns All " + quality.ToString() + " quality items"; } }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear != null)
                return gear.Quality == quality;

            //Maps?
            return false;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }


    }
}
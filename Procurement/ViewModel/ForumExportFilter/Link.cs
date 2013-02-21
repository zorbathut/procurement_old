using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportFilter
{
    internal class Link : IFilter
    {
        private int links;
        public Link(int links)
        {
            this.links = links;
        }

        public string Keyword { get { return links.ToString() + "L"; } }
        public string Help { get { return "Returns All " + links.ToString() + " Linked items, regardless of rarity"; } }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear == null)
                return false;

            return gear.IsLinked(links);
        }
    }
}

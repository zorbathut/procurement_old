using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class DropOnlyGemFilter : IFilter
    {
        private List<string> dropOnly;
        public DropOnlyGemFilter()
        {
            //From http://en.pathofexilewiki.com/wiki/Drop_Only_Gems
            dropOnly = new List<string>();
            dropOnly.Add("Added Chaos Damage");
            dropOnly.Add("Chain");
            dropOnly.Add("Concentrated Effect");
            dropOnly.Add("Devouring Totem");
            dropOnly.Add("Elemental Hit");
            dropOnly.Add("Elemental Proliferation");
            dropOnly.Add("Elemental Weakness");
            dropOnly.Add("Greater Multiple Projectiles");
            dropOnly.Add("Knockback");
            dropOnly.Add("Punishment");
            dropOnly.Add("Temporal Chains");
            dropOnly.Add("Portal");
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Drop Only Gems"; }
        }

        public string Help
        {
            get { return "Gems only which can only be aquired through drops"; }
        }

        public bool Applicable(Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return dropOnly.Contains(gem.TypeLine);
        }
    }
}

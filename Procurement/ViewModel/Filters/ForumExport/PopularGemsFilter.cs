using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class PopularGemsFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        private List<string> popular;
        public PopularGemsFilter()
        {
            popular = new List<string>();
            popular.Add("Blood Magic");
            popular.Add("Item Quantity");
            popular.Add("Life Gain on Hit");
            popular.Add("Life Leech");
            popular.Add("Multistrike");
            popular.Add("Reduced Mana");
            popular.Add("Spell Totem");
            popular.Add("Chain");
            popular.Add("Faster Attacks");
            popular.Add("Greater Multiple Projectiles");
            popular.Add("Lesser Multiple Projectiles");
            popular.Add("Mana Leech");
            popular.Add("Added Chaos Damage");
            popular.Add("Item Rarity");
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Popular Gems"; }
        }

        public string Help
        {
            get { return "Those really popular gems"; }
        }

        public bool Applicable(Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return popular.Contains(gem.TypeLine);
        }
    }
}

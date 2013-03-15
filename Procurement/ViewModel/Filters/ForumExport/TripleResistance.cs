using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class TripleResistance : IFilter
    {
        private List<StatFilter> resistances;
        public TripleResistance()
        {
            resistances = new List<StatFilter>();
            resistances.Add(new FireResistance());
            resistances.Add(new ColdResistance());
            resistances.Add(new LightningResistance());
            resistances.Add(new ChaosResistance());
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Triple Resists"; }
        }

        public string Help
        {
            get { return "Returns items with Triple Resists"; }
        }

        public bool Applicable(Item item)
        {
            return resistances.Count(r => r.Applicable(item)) >= 3;
        }
    }
}

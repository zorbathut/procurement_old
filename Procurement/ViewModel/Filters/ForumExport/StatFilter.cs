using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal abstract class StatFilter : IFilter
    {
        private string keyword;
        private string help;
        private List<string> stats;

        public StatFilter(string keyword, string help, params string[] stats)
        {
            this.keyword = keyword;
            this.help = help;
            this.stats = new List<string>(stats);
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return keyword; }
        }

        public string Help
        {
            get { return help; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gear gear = item as Gear;
            if (gear == null)
                return false;

            List<string> pool = new List<string>(stats);
            List<string> all = new List<string>();

            if (gear.Implicitmods != null)
                all.AddRange(gear.Implicitmods.Select(s => s.ToUpper()));

            if (gear.Explicitmods != null)
                all.AddRange(gear.Explicitmods.Select(s => s.ToUpper()));

            foreach (string stat in all)
            {
                string result = pool.Find(s => stat.Contains(s));
                pool.Remove(result);
            }

            return pool.Count == 0;
        }
    }
}

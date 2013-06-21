using System;
using System.Collections.Generic;
using POEApi.Model;
using System.Linq;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class GearVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;
        public GearVisitor()
        {
            var tokensSource = from quality in Enum.GetNames(typeof(Quality))
                               from gearType in Enum.GetNames(typeof(GearType))
                               select new KeyValuePair<string, IFilter>(string.Concat("{", quality, gearType, "}"), new AndFilter(new QualityFilter(getEnum<Quality>(quality)), new GearTypeFilter(getEnum<GearType>(gearType))));

            tokens = tokensSource.ToDictionary(i => i.Key, i => i.Value);
            tokens.Add("{WhiteGear}", new WhiteQuality());
        }
        public override string Visit(IEnumerable<Item> items, string current)
        {
            string updated = current;
            var sorted = items.OrderBy(i => i.H).ThenBy(i => i.IconURL);

            foreach (var token in tokens)
            {
                if (updated.IndexOf(token.Key) < 0)
                    continue;

                updated = updated.Replace(token.Key, runFilter(token.Value, sorted));
            }

            return updated;
        }

        private T getEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }
    }
}

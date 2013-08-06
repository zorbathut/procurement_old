using System;
using System.Collections.Generic;
using Procurement.ViewModel.Filters;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;
using System.Text;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class MultipleBuyoutVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;
        private const string TOKEN = "{Buyouts}";

        public MultipleBuyoutVisitor()
        {
            tokens = Settings.Buyouts.Keys.GroupBy(k => Settings.Buyouts[k])
                                          .ToDictionary(g => string.Concat(g.Key.ToLower()), g => (IFilter)new BuyoutFilter(g.Key.ToLower()));
        }
        public override string Visit(IEnumerable<POEApi.Model.Item> items, string current)
        {
            string updated = current;
            var sorted = items.OrderBy(i => i.H).ThenBy(i => i.IconURL);

            StringBuilder builder = new StringBuilder();

            foreach (var token in tokens.OrderBy(t => t.Key, new NaiveNumberTextComparer()))
            {
                builder.AppendLine(string.Format("[spoiler=\"          ~b/o {0}          \"]", token.Key));
                builder.AppendLine(runFilter(token.Value, sorted));
                builder.AppendLine("[/spoiler]");
            }

            updated = updated.Replace(TOKEN, builder.ToString());

            return updated;
        }

        private class NaiveNumberTextComparer : IComparer<string>
        {
            private const string regex = @"^(?<number>[0-9]{1,})\s{0,}(?<currency>[a-zA-Z]{1,})";
            private Tuple<double, string> getParts(string x)
            {
                Match m = Regex.Match(x, regex);
                string number = m.Groups["number"].Value;
                string text = m.Groups["currency"].Value;

                return new Tuple<double, string>(Convert.ToDouble(number), text);
            }

            public int Compare(string x, string y)
            {
                if (!(Regex.IsMatch(x, regex) && Regex.IsMatch(y, regex)))
                    return x.CompareTo(y);

                var xPair = getParts(x);
                var yPair = getParts(y);

                if (xPair.Item2 == yPair.Item2)
                    return xPair.Item1.CompareTo(yPair.Item1);

                return xPair.Item2.CompareTo(yPair.Item2);
            }
        }
    }
}

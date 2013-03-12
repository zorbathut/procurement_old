using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    internal class CurrencyHandler
    {
        internal static double GetGCPValue(OrbType type)
        {
            if (!Settings.CurrencyRatios.ContainsKey(type))
                return 0;

            CurrencyRatio ratio = Settings.CurrencyRatios[type];

            if (ratio.GCPAmount < ratio.OrbAmount)
                return ratio.GCPAmount / ratio.OrbAmount;

            return ratio.OrbAmount * ratio.GCPAmount;
        }

        public static double GetTotalGCP(IEnumerable<Currency> currency)
        {
            double total = 0;

            foreach (var orb in currency)
                total += orb.StackInfo.Amount * orb.GCPValue;

            return total;
        }

        public static Dictionary<OrbType, double> GetTotalGCPDistribution(IEnumerable<Currency> currency)
        {
            return currency.Where(o => !o.TypeLine.Contains("Shard"))
                           .GroupBy(orb => orb.Type)
                           .Where(group => GetTotalGCP(group) > 0)
                           .Select(grp => new { Key = grp.Key, Value = GetTotalGCP(grp) })
                           .OrderByDescending(at => at.Value)
                           .ToDictionary(at => at.Key, at => at.Value);
        }
    }
}

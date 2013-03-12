using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POEApi.Model
{
    public class POEExRates
    {
        public Dictionary<OrbType, CurrencyRatio> CurrencyRatios { get; private set; }
          
        public POEExRates(POEApi.Model.JSONProxy.POEExRates proxy)
        {
            CurrencyRatios = new Dictionary<OrbType, CurrencyRatio>();
            CurrencyRatios.Add(OrbType.Alchemy, getRatio(OrbType.Alchemy, proxy.alchemy.gcp));
            CurrencyRatios.Add(OrbType.Exalted, getRatio(OrbType.Exalted, proxy.exalted.gcp));
            CurrencyRatios.Add(OrbType.Divine, getRatio(OrbType.Divine, proxy.divine.gcp));
            CurrencyRatios.Add(OrbType.Regal, getRatio(OrbType.Regal, proxy.regal.gcp));
            CurrencyRatios.Add(OrbType.Blessed, getRatio(OrbType.Blessed, proxy.blessed.gcp));
            CurrencyRatios.Add(OrbType.Regret, getRatio(OrbType.Regret, proxy.regret.gcp));
            CurrencyRatios.Add(OrbType.Chisel, getRatio(OrbType.Chisel, proxy.chisel.gcp));
            CurrencyRatios.Add(OrbType.Scouring, getRatio(OrbType.Scouring, proxy.scouring.gcp));
            CurrencyRatios.Add(OrbType.Fusing, getRatio(OrbType.Fusing, proxy.fusing.gcp));
            CurrencyRatios.Add(OrbType.Chance, getRatio(OrbType.Chance, proxy.chance.gcp));
            CurrencyRatios.Add(OrbType.Chromatic, getRatio(OrbType.Chromatic, proxy.chromatic.gcp));
            CurrencyRatios.Add(OrbType.JewelersOrb, getRatio(OrbType.JewelersOrb, proxy.jewellers.gcp));
            CurrencyRatios.Add(OrbType.Alteration, getRatio(OrbType.Alteration, proxy.alteration.gcp));
        }

        private CurrencyRatio getRatio(OrbType orbType, double gcpValue)
        {
            return new CurrencyRatio(orbType, 1, gcpValue);
        }
    }
}

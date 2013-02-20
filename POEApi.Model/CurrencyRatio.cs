namespace POEApi.Model
{
    public class CurrencyRatio
    {
        public OrbType OrbType { get; set; }
        public float OrbAmount { get; set; }
        public float GCPAmount { get; set; }

        public CurrencyRatio(OrbType orbType, int OrbAmount, int GCPAmount)
        {
            this.OrbType = orbType;
            this.OrbAmount = OrbAmount;
            this.GCPAmount = GCPAmount;
        }
    }
}

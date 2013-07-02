using System.Collections.Generic;

namespace POEApi.Model
{
    public class Map : Item
    {
        public Quality Quality { get; private set; }
        public int MapLevel { get; private set; }
        public int MapQuantity { get; private set; }

        internal Map(JSONProxy.Item item) : base(item)
        {
            this.ItemType = Model.ItemType.Gear;
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.Quality = getQuality(item);
            this.MapLevel = int.Parse(Properties.Find(p => p.Name == "Map Level").Values[0].Item1);
        }
    }
}

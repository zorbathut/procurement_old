using System.Collections.Generic;

namespace POEApi.Model
{
    public class Map : Item
    {
        public List<Property> Properties { get; set; }

        internal Map(JSONProxy.Item item) : base(item)
        {
            this.ItemType = Model.ItemType.Gear;
            this.Properties = ProxyMapper.GetProperties(item.Properties);
        }
    }
}

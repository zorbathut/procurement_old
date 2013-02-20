using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace POEApi.Model
{
    public class Gem : Item
    {
        public bool IsQuality { get; private set; }
        public int Quality { get; private set; }
        public List<Requirement> Requirements { get; set; }
        public int Socket { get; set; }
        public string Color { get; set; }

        public Gem(JSONProxy.Item item) : base(item)
        {
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.ItemType = Model.ItemType.Gem;

            this.Socket = item.Socket;
            this.Color = item.Color;
            this.Requirements = ProxyMapper.GetRequirements(item.Requirements);

            if (!Properties.Any(p => p.Name == "Quality"))
                return;

            IsQuality = true;
            Quality = ProxyMapper.GetQuality(item.Properties);
        }
    }
}

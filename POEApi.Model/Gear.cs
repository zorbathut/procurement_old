using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public class Gear : Item
    {
        public Quality Quality { get; private set; }
        public List<string> FlavourText { get; set; }
        public List<Socket> Sockets { get; set; }
        public List<Gem> SocketedItems { get; set; }
        public List<string> Implicitmods { get; set; }
        public List<Requirement> Requirements { get; set; }
        public GearType GearType { get; set; }

        internal Gear(JSONProxy.Item item) : base(item)
        {
            this.Quality = getQuality(item);
            this.FlavourText = item.FlavourText;
            this.Sockets = item.Sockets.Select(proxy => new Socket(proxy)).ToList();
            this.Explicitmods = item.ExplicitMods;
            this.SocketedItems = item.SocketedItems.Select(proxy => (Gem)ItemFactory.Get(proxy)).ToList();
            this.Implicitmods = item.ImplicitMods;
            this.Requirements = ProxyMapper.GetRequirements(item.Requirements);
            this.ItemType = Model.ItemType.Gear;
            this.GearType = GearTypeFactory.GetType(this);
        }

        private Quality getQuality(JSONProxy.Item item)
        {
            if (item.frameType <= 3)
                return (Quality)item.frameType;

            return Quality.White;
        }

        public bool IsLinked(int links)
        {
            return Sockets.GroupBy(s => s.Group).Any(g => g.Count() >= links);
        }

        public int NumberOfSockets()
        {
            return Sockets.Count();
        }
    }
}

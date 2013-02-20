using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public enum ItemType : int
    {
        UnSet,
        Gear,
        Gem,
        Currency,
    }

    public enum Quality : int
    {
        White,
        Magic,
        Rare,
        Unique
    }

    public abstract class Item
    {
        public bool Verified { get; private set; }
        public int W { get; private set; }
        public int H { get; private set; }
        public string IconURL { get; private set; }
        public string League { get; private set; }
        public string Name { get; private set; }
        public string TypeLine { get; private set; }
        public string DescrText { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public string inventoryId { get; private set; }
        public string SecDescrText { get; private set; }
        public List<string> Explicitmods { get; set; }
        public ItemType ItemType { get; set; }
        public List<Property> Properties { get; set; }

        protected Item(JSONProxy.Item item)
        {
            this.Verified = item.Verified;
            this.W = item.W;
            this.H = item.H;
            this.IconURL = item.Icon;
            this.League = item.League;
            this.Name = item.Name;
            this.TypeLine = item.TypeLine;
            this.DescrText = item.DescrText;
            this.X = item.X;
            this.Y = item.Y;
            this.inventoryId = item.InventoryId;
            this.SecDescrText = item.SecDescrText;
            this.Explicitmods = item.ExplicitMods;
            this.ItemType = Model.ItemType.UnSet;
            if (item.Properties != null)
                this.Properties = item.Properties.Select(p => new Property(p)).ToList();
        }
    }
}

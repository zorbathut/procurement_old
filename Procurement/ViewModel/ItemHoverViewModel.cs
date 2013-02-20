using System;
using System.Collections.Generic;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class ItemHoverViewModel
    {
        public Item Item { get; private set; }
        public string TypeLine { get; private set; }
        public ItemType ItemType { get; private set; }
        public List<Property> Properties { get; private set; }
        public List<Requirement> Requirements { get; private set; }
        public bool HasRequirements { get { return Requirements != null && Requirements.Count > 0; } }

        public string Name { get; private set; }
        public bool HasName { get; private set; }
        public List<string> ExplicitMods { get; private set; }
        public bool HasExplicitMods { get { return ExplicitMods != null && ExplicitMods.Count > 0; } }
        public List<string> ImplicitMods { get; private set; }
        public bool HasImplicitMods { get { return ImplicitMods != null && ImplicitMods.Count > 0; } }
        public string DescriptionText { get; private set; }
        
        public ItemHoverViewModel(Item item)
        {
            this.Item = item;
            this.Name = item.Name;
            this.TypeLine = item.TypeLine;
            this.HasName = !string.IsNullOrEmpty(item.Name);
            this.ItemType = item.ItemType;
            this.Properties = item.Properties;
            this.Requirements = new List<Requirement>();

            this.ExplicitMods = item.Explicitmods;
            this.ImplicitMods = new List<string>();
            this.DescriptionText = item.DescrText;
            if (item.SecDescrText != null)
                this.DescriptionText += Environment.NewLine + item.SecDescrText;

            Gear gear = item as Gear;
            if (gear != null)
            {
                this.Requirements = gear.Requirements;
                this.ImplicitMods = gear.Implicitmods;
            }

            Gem gem = item as Gem;
            if (gem != null)
                this.Requirements = gem.Requirements;
        }
    }
}

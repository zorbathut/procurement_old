using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel
{
    class ItemHoverViewModelFactory
    {
        internal static ItemHoverViewModel Create(Item item)
        {
            Gear gear = item as Gear;
            Nullable<Quality> q = null;

            if (gear != null)
                q = gear.Quality;

            Map map = item as Map;
            if (map != null)
                q = map.Quality;

            if (q != null)
            {
                switch (q)
                {
                    case Quality.Unique:
                        return new UniqueGearItemHoverViewModel(item);
                    case Quality.Rare:
                        return new RareGearItemHoverViewModel(item); 
                    case Quality.Magic:
                        return new MagicGearItemHoverViewModel(item);
                    case Quality.White:
                        return new WhiteGearItemHoverViewModel(item); 
                }
            }

            if (item is Gem)
                return new GemItemHoverViewModel(item);

            if (item is Currency)
                return new CurrencyItemHoverViewModel(item);

            return new ItemHoverViewModel(item);
        }
    }

    public class UniqueGearItemHoverViewModel : ItemHoverViewModel
    {
        public UniqueGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class RareGearItemHoverViewModel : ItemHoverViewModel
    {
        public RareGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class MagicGearItemHoverViewModel : ItemHoverViewModel
    {
        public MagicGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class WhiteGearItemHoverViewModel : ItemHoverViewModel
    {
        public WhiteGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class GemItemHoverViewModel : ItemHoverViewModel
    {
        public GemItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class CurrencyItemHoverViewModel : ItemHoverViewModel
    {
        public CurrencyItemHoverViewModel(Item item)
            : base(item)
        { }
    }
}

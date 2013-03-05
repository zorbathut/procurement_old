using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class ItemRarityFilter : StatFilter
    {
        public ItemRarityFilter()
            : base("Item Rarity", "Item with the Item Rarity stat", "INCREASED RARITY")
        { }
    }
}

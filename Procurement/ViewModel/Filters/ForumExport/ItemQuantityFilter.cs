using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters
{
    internal class ItemQuantityFilter : StatFilter
    {
        public ItemQuantityFilter()
            : base("Item Quantity", "Item with the Item Quantity stat", "INCREASED QUANTITY")
        { }
    }
}

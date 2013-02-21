using System.Collections.Generic;
using POEApi.Model;
namespace Procurement.ViewModel.ForumExportFilter
{
    internal interface IFilter
    {
        string Keyword { get; }
        string Help { get; }
        bool Applicable(Item item);
    }
}

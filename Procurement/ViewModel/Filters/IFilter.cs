using System.Collections.Generic;
using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    public interface IFilter
    {
        bool CanFormCategory { get; }
        string Keyword { get; }
        string Help { get; }
        bool Applicable(Item item);
    }
}

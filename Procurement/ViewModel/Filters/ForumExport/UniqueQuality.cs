using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class UniqueQuality : QualityFilter
    {
        public UniqueQuality()
            : base(Rarity.Unique)
        { }
    }
}

using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class RareQuality : QualityFilter
    {
        public RareQuality()
            : base(Quality.Rare)
        { }
    }
}

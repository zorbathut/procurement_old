using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class WhiteQuality : QualityFilter
    {
        public WhiteQuality()
            : base(Quality.White)
        { }
    }
}

using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class MagicQuality : QualityFilter
    {
        public MagicQuality()
            : base(Rarity.Magic)
        { }
    }
}

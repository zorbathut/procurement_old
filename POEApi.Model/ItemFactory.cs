using POEApi.Infrastructure;

namespace POEApi.Model
{
    internal class ItemFactory
    {
        public static Item Get(JSONProxy.Item item)
        {
            if (item.frameType == 4)
                return new Gem(item);
            
            if (item.frameType == 5)
                return new Currency(item);

            if (item.TypeLine.Contains("Map") && item.DescrText.Contains("Travel to this Map"))
                return new Map(item);

            return new Gear(item);
        }
    }
}

using System;

namespace Procurement.ViewModel.ForumExportRunners
{
    internal class ForumExportRunnerFactory
    {
        public static IForumExportRunner Create(Type type)
        {
            switch (type.Name)
            {
                case "Currency":
                    return new CurrencyRunner();
                case "Gem":
                    return new GemsRunner();
                case "Map":
                    return new MapRunner();
                case "Gear":
                    return new GearRunner();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

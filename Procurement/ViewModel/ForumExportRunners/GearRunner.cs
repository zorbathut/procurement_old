using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportRunners
{
    internal class GearRunner : ForumExportRunnerBase<Gear>
    {
        public override string GetSpoiler(IEnumerable<Gear> items)
        {
            StringBuilder builder = new StringBuilder();

            getSpoiler(builder, items.Where(i => i.Quality == Quality.Unique), "Uniques");

            getCrafting(items.Where(i => i.Quality == Quality.White && i.GearType != GearType.Flask), builder);

            builder.AppendLine("[spoiler=\"" + "Arms & Armor" + "\"]");
            
            buildGearSpoiler(items.GroupBy(gear => gear.GearType), builder);
            
            builder.AppendLine("[/spoiler]\n");

            return builder.ToString();
        }

        private void getCrafting(IEnumerable<Gear> items, StringBuilder builder)
        {
            builder.AppendLine("[spoiler=\"" + "Crafting Gear" + "\"]");
            getSpoiler(builder, items.Where(i => i.IsLinked(4)), "4 Links");
            getSpoiler(builder, items.Where(i => i.NumberOfSockets() == 6), "6 Socket");
            buildGearSpoiler(items.Where(i => !i.IsLinked(4) && i.NumberOfSockets() != 6).GroupBy(gear => gear.GearType), builder);
            builder.AppendLine("[/spoiler]\n");
        }

        private void buildGearSpoiler(IEnumerable<IGrouping<GearType, Gear>> gearByType, StringBuilder builder)
        {
            foreach (var gearGroup in gearByType)
            {
                var orderedGear = gearGroup.OrderBy(gp => gp.IconURL);

                getSpoiler(builder, orderedGear, gearGroup.Key.ToString());
            }
        }

        private void getSpoiler(StringBuilder builder, IEnumerable<Gear> gear, string name)
        {
            if (gear.Count() == 0)
                return;

            builder.AppendLine("[spoiler=\"" + name + "\"]");

            foreach (var item in gear.OrderBy(i => i.H).ThenBy(i => i.IconURL))
                builder.Append(getLinkItem(item));

            builder.AppendLine("[/spoiler]\n");
        }
    }
}

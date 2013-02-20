using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportRunners
{
    internal class GemsRunner : ForumExportRunnerBase<Gem>
    {
        public override string GetSpoiler(IEnumerable<Gem> items)
        {
            StringBuilder builder = new StringBuilder();

            var gems = items.OrderBy(i => i.inventoryId);

            getSpoilerForGroup("Quality Gems", gems.Where(i => i.IsQuality), builder);

            getSpoilerForGroup("Normal Gems", gems.ThenBy(i => i.Color).Where(i => !i.IsQuality), builder);
            
            return builder.ToString();
        }

        private static void getSpoilerForGroup(string name, IEnumerable<Gem> items, StringBuilder builder)
        {
            builder.AppendLine("[spoiler=\"" + name + "\"]");
            foreach (Item i in items)
                builder.Append(string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", i.inventoryId, ApplicationState.CurrentLeague, i.X, i.Y));

            builder.AppendLine("[/spoiler]");
        }
    }
}
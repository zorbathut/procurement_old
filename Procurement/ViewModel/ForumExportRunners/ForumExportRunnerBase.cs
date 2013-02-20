using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportRunners
{
    internal interface IForumExportRunner
    {
        string GetSpoiler(IEnumerable<Item> items);
    }
    internal abstract class ForumExportRunnerBase<T> : IForumExportRunner where T : Item
    {
        protected const string LINKITEM = "[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]";
        public abstract string GetSpoiler(IEnumerable<T> items);

        protected string getLinkItem(T item)
        {
            return string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y);
        }

        string IForumExportRunner.GetSpoiler(IEnumerable<Item> items)
        {
            return this.GetSpoiler(items.Cast<T>());
        }
    }
}

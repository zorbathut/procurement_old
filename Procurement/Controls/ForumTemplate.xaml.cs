using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class ForumTemplate : UserControl
    {
        public ForumTemplate()
        {
            InitializeComponent();

            ForumTemplateViewModel context = new ForumTemplateViewModel();
            context.Grid = this.dataGrid;
            this.DataContext = context;
            
        }
    }
}

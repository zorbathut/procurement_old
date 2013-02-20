using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.View
{
    public partial class StashView : UserControl, IView
    {
        public StashView()
        {
            InitializeComponent();
            this.DataContext = new StashViewModel(this);
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }
    }
}

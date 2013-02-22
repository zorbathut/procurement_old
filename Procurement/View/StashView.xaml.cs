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

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = this.DataContext as StashViewModel;

            if (vm == null)
                return;

            vm.SetCategoryFilter((sender as RadioButton).Content.ToString());
        }
    }
}

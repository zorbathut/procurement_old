using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class ForumTemplate : UserControl
    {
        public ForumTemplate()
        {
            InitializeComponent();
            this.DataContext = new ForumTemplateViewModel(); 
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.DataContext as ForumTemplateViewModel).Save();
        }
    }
}

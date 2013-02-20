using System.Windows.Controls;

namespace Procurement.View
{
    public partial class AboutView : UserControl, IView
    {
        public AboutView()
        {
            InitializeComponent();
            this.Version.Content = ApplicationState.Version;
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }
    }
}

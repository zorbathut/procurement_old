using System.Collections.Generic;
using System.ComponentModel;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel
{
    internal class TradingViewModel : INotifyPropertyChanged
    {
        public TradingViewModel()
        {
            Filters = CategoryManager.GetAvailableFilters();
        }

        private List<IFilter> getFilters()
        {
            throw new System.NotImplementedException();
        }

        private List<IFilter> filters;
        public List<IFilter> Filters
        {
            get { return filters; }
            set
            {
                filters = value;
                onPropertyChanged("Filters");
            }
        }

        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

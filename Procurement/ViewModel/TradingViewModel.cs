using System.Collections.Generic;
using System.ComponentModel;
namespace Procurement.ViewModel
{
    public class TradingViewModel : INotifyPropertyChanged
    {
        public class FilterInfo : INotifyPropertyChanged
        {
            public string Category { get; set; }
            public string Filters { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public TradingViewModel()
        {
            Filters = new List<FilterInfo>();
            Filters.Add(new FilterInfo() { Category = "Headshot", Filters = "4L, White" });
            Filters.Add(new FilterInfo() { Category = "Uniques Weapons", Filters = "Uniques, Weapons" });
        }

        private List<FilterInfo> filters;
        public List<FilterInfo> Filters
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

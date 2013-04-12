using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using Procurement.Controls;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel
{
    internal class ForumTemplateViewModel : INotifyPropertyChanged
    {
        public DataGrid Grid { get; set; }
        
        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ICommand AddCatagory { get; set; }
        public ICommand AddFilter { get; set; }

        public ForumTemplateViewModel()
        {
            this.AddCatagory = new DelegateCommand(addCatagory);
            this.AddFilter = new DelegateCommand(addFilter);
            this.categories = new ObservableCollection<Category>();
            this.categories.Add(new Category());
            this.Filters = CategoryManager.GetAvailableFilters();
        }

        public void addFilter(object obj)
        {
            IFilter filter = obj as IFilter;

            if (filter == null || Grid.SelectedIndex == -1)
                return;

            var currentCat = this.categories[Grid.SelectedIndex];

            if (currentCat.Filters.Contains(filter))
                return;

            currentCat.Filters.Add(filter);
            onPropertyChanged("Categories");
        }

        public void addCatagory(object obj)
        {
            DataGrid row = obj as DataGrid;
            Category c = new Category();
            this.Categories.Add(c);
        }

        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}

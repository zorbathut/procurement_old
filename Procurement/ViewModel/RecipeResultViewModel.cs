using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procurement.ViewModel.Recipes;
using System.ComponentModel;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class RecipeResultViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, List<RecipeResult>> results;
        private RecipeManager manager;

        public Dictionary<string, List<RecipeResult>> Results
        {
            get { return results; }
            set 
            { 
                results = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Results"));
            }
        }
        
        public RecipeResultViewModel()
        {
            manager = new RecipeManager();
            ApplicationState.LeagueChanged += new System.ComponentModel.PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            updateResults();
        }

        private void updateResults()
        {
            Results = manager.Run(ApplicationState.Model.GetStash(ApplicationState.CurrentLeague).Get<Item>());
        }

        void ApplicationState_LeagueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            updateResults();
        }
    }
}

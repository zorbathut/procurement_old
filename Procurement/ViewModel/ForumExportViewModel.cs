using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;
using System.Windows.Input;
using System.Windows;
using Procurement.ViewModel.ForumExportVisitors;
using System.Reflection;
using Procurement.Controls;

namespace Procurement.ViewModel
{
    public class ForumExportViewModel : INotifyPropertyChanged
    {
        public class ExportStashInfo : INotifyPropertyChanged
        {
            public string Name { get; set; }
            public decimal AvailableSpace { get; set; }
            public string Url { get; set; }
            public int ID { get; set; }
            private bool isChecked;

            public bool IsChecked
            {
                get { return isChecked; }
                set 
                { 
                    isChecked = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }

            public void FixName()
            {
                int test = -1;
                if (int.TryParse(Name, out test))
                    Name = "Tab " + Name;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private List<ExportStashInfo> stashItems;
        private List<int> selected = new List<int>();
        private string text;
        private static List<IVisitor> visitors = null;

        private DelegateCommand copyCommand;

        public DelegateCommand CopyCommand
        {
            get { return copyCommand; }
            set { copyCommand = value; }
        }
        

        public List<ExportStashInfo> StashItems
        {
            get { return stashItems; }
            set 
            { 
                stashItems = value;
                onPropertyChanged("StashItems");
            }
        }

        public string Text
        {
            get { return text; }
            set 
            { 
                text = value;
                onPropertyChanged("Text");
            }
        }
        
        public ForumExportViewModel()
        {
            copyCommand = new DelegateCommand(copy);
            updateForLeague();
            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            visitors = visitors ?? getVisitors();
        }

        private void copy(object parameter)
        {
            if (text != null)
                Clipboard.SetText(text);
        }

        void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            updateForLeague();
        }

        private void updateForLeague()
        {
            var space = ApplicationState.Stash[ApplicationState.CurrentLeague].CalculateFreeSpace();
            space.Remove("All");
            var betterSpace = space.ToDictionary(k => int.Parse(k.Key.Replace("Stash", "")) - 1, k => k.Value); ;

            var tabs = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs;

            StashItems = tabs.Where(t => betterSpace.ContainsKey(t.i)).Select(t => new ExportStashInfo() { AvailableSpace = betterSpace[t.i], Name = t.Name, Url = t.src, ID = t.i }).ToList();
            StashItems.ForEach(s => s.FixName());
        }

        public void update(int key, bool isChecked)
        {
            if (isChecked)
                selected.Add(key);
            else
                selected.Remove(key);

            Text = getSpoiler(selected.SelectMany(sid => ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(sid))
                                                              .OrderBy(id => id.Y).ThenBy(i => i.X));
        }

        private string getSpoiler(IEnumerable<Item> items)
        {
            string template = ForumExportTemplateReader.GetTemplate();

            foreach (IVisitor visitor in visitors)
                template = visitor.Visit(items, template);

            return template;
            
        }

        private static List<IVisitor> getVisitors()
        {
            Type visitorType = typeof(IVisitor);
            return Assembly.GetAssembly(visitorType).GetTypes()
                                                    .Where(t => !(t.IsAbstract || t.IsInterface) && visitorType.IsAssignableFrom(t))
                                                    .Select(t => Activator.CreateInstance(t) as IVisitor)
                                                    .ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        internal void ToggleAll(bool value)
        {
            stashItems.ForEach(si => si.IsChecked = value);
        }
    }
}

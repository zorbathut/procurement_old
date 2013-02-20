using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using POEApi.Model;

namespace Procurement.Controls
{
    public partial class CharacterSelector : UserControl
    {
        public CharacterSelector()
        {
            InitializeComponent();
        }

        public List<Character> Characters
        {
            get { return (List<Character>)GetValue(CharactersProperty); }
            set { SetValue(CharactersProperty, value); }
        }

        public static readonly DependencyProperty CharactersProperty =
            DependencyProperty.Register("Characters", typeof(List<Character>), typeof(CharacterSelector), null);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Character c = (Character)(sender as Button).Tag;
            ApplicationState.CurrentCharacter = c;
        }

    }
}

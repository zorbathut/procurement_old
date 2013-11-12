using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;

namespace Procurement
{
    public static class ApplicationState
    {
        public static string Version = "Procurement v0.0.9dev";
        public static POEModel Model = new POEModel();
        public static Dictionary<string, Stash> Stash = new Dictionary<string, Stash>();
        public static Dictionary<string, Item> Inventory = new Dictionary<string, Item>();
        public static List<Character> Characters = new List<Character>();
        public static List<string> Leagues = new List<string>();

        private static Character currentCharacter = null;

        public static Character CurrentCharacter
        {
            get { return currentCharacter; }
            set 
            { 
                currentCharacter = value;
                if (CharacterChanged != null)
                    CharacterChanged(Model, new PropertyChangedEventArgs("CurrentCharacter"));
            }
        }

        public static event PropertyChangedEventHandler LeagueChanged;
        public static event PropertyChangedEventHandler CharacterChanged;
        private static string currentLeague = string.Empty;

        public static string CurrentLeague
        {
            get { return currentLeague; }
            set 
            { 
                currentLeague = value;
                Characters = Model.GetCharacters().Where(c => c.League == value).ToList();
                CurrentCharacter = Characters.First();
                if (LeagueChanged != null)
                    LeagueChanged(Model, new PropertyChangedEventArgs("CurrentLeague"));
            }
        }

        public static void SetDefaults()
        {
            string favoriteLeague = Settings.UserSettings["FavoriteLeague"];
            if (!string.IsNullOrEmpty(favoriteLeague))
                CurrentLeague = favoriteLeague;

            string defaultCharacter = Settings.UserSettings["FavoriteCharacter"];
            if (defaultCharacter != string.Empty && Characters.Count(c => c.Name == defaultCharacter) == 1)
            {
                CurrentCharacter = Characters.First(c => c.Name == defaultCharacter);
                return;
            }

            CurrentCharacter = Characters.First();
            CurrentLeague = CurrentCharacter.League;
        }
    }
}

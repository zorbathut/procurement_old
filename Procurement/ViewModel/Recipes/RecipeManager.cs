using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class RecipeManager
    {
        private List<Recipe> known;
        public RecipeManager()
        {
            known = new List<Recipe>() { new OneChoasRecipe(), new Chromatic() };
        }

        public Dictionary<string, List<RecipeResult>> Run(IEnumerable<Item> items)
        {
            return known.SelectMany(recipe => recipe.Matches(items))
                        .GroupBy(r => r.Instance.Name)
                        .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}

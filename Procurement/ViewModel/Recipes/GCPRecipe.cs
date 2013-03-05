using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class GCPRecipe : Recipe
    {
        private class Combination
        {
            public List<Gem> Match;
            public int Total;
            public bool Perfect;
        }

        private List<Combination> combinations;
        private bool stop;
        private const int REQUIREDQUALITY = 40;
        
        public GCPRecipe()
            : base(70)
        { }

        public override string Name
        {
            get { return "1 GCP"; }
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<POEApi.Model.Item> items)
        {
            List<Gem> gems = items.OfType<Gem>().Where(g => g.IsQuality).ToList();

            bool canContinue = true;
            while (canContinue)
            {
                getCombinations(gems, REQUIREDQUALITY);
                
                Combination perfect = combinations.Find(c => c.Perfect);
                if (perfect != null)
                {
                    perfect.Match.ForEach(g => gems.Remove(g));
                    yield return getResult(perfect);
                    continue;
                }

                Combination leastOver = null;
                List<Combination> over = combinations.FindAll(c => !c.Perfect);
                if (over != null && over.Count > 0)
                    leastOver = over.OrderBy(c => c.Total).First();
                
                if (leastOver != null)
                {
                    leastOver.Match.ForEach(g => gems.Remove(g));
                    yield return getResult(leastOver);
                    continue;
                }

                if (leastOver == null)
                    canContinue = false;
            }

            Combination remaining = new Combination() { Match = gems, Total = gems.Sum(g => g.Quality), Perfect = false };
            RecipeResult leftOver = getResult(remaining);
            if (leftOver.IsMatch)
                yield return leftOver;
        }

        private void getCombinations(List<Gem> pool, int target, List<Gem> workingSet)
        {
            if (stop)
                return;

            int current = 0;
            foreach (Gem x in workingSet)
            {
                current += x.Quality;
            }
            if (current == target)
            {
                combinations.Add(new Combination() { Match = new List<Gem>(workingSet), Perfect = true, Total = 40 });
                Console.WriteLine("Perfect: sum(" + string.Join(",", workingSet.Select(n => n.Quality.ToString()).ToArray()) + ")=" + target);
                stop = true;
                return;
            }
            if (current >= target)
            {
                combinations.Add(new Combination() { Match = new List<Gem>(workingSet), Perfect = false, Total = current });
                Console.WriteLine("Over: " + current + " sum(" + string.Join(",", workingSet.Select(n => n.Quality.ToString()).ToArray()) + ")=" + target);
                return;
            }
            for (int i = 0; i < pool.Count; i++)
            {
                var remaining = new List<Gem>();
                Gem n = pool[i];
                for (int j = i + 1; j < pool.Count; j++)
                {
                    remaining.Add(pool[j]);
                }
                var workingInternal = new List<Gem>(workingSet);
                workingInternal.Add(n);
                getCombinations(remaining, target, workingInternal);
            }
        }
        private void getCombinations(List<Gem> pool, int target)
        {
            combinations = new List<Combination>();
            stop = false;
            getCombinations(pool, target, new List<Gem>());
        }

        private RecipeResult getResult(Combination currentSet)
        {
            RecipeResult result = new RecipeResult();
            result.MatchedItems = currentSet.Match.Cast<Item>().ToList();
            result.Instance = this;
            result.IsMatch = true;

            decimal total = currentSet.Total;
            decimal match = (total / REQUIREDQUALITY) * 100;

            result.IsMatch = match >= base.ReturnMatchesGreaterThan;
            result.PercentMatch = match;
            if (match < 100)
                result.Missing = new List<string>() { "Gem(s) with quality totaling " + (REQUIREDQUALITY - match).ToString() + "%"};

            return result;
        }
    }
}

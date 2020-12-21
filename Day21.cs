using System;
using System.Collections.Generic;

class Day21
{
    List<string> data;

    public Day21(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        Dictionary<string, HashSet<string>> allergensWithPossibleIngredients = new Dictionary<string, HashSet<string>>();
        List<string> fullIngredientList = new List<string>();
        foreach (string line in data)
        {
            int indexOfParens = line.IndexOf("(");
            int allergenIndex = indexOfParens + 10;
            int lengthOfIngredients = indexOfParens - 1;
            int lengthOfAllergens = line.Length - indexOfParens - 1;
            string ingredientStr = line.Substring(0, lengthOfIngredients);
            string allergenStr = line.Substring(allergenIndex);
            allergenStr = allergenStr.Substring(0, allergenStr.Length - 1); // get rid of final parentheses
            string[] ingredients = ingredientStr.Split(" ");
            string[] allergens = allergenStr.Split(", ");

            //  first, add all ingredients to the list (including duplicates)
            foreach (string ingredient in ingredients)
            {
                fullIngredientList.Add(ingredient);
            }

            //  then pair allergens with all possible ingredients that might contain it
            foreach (string allergen in allergens)
            {
                //  if it's new, create a new dictionary entry
                if (!allergensWithPossibleIngredients.ContainsKey(allergen))
                {
                    allergensWithPossibleIngredients.Add(allergen, new HashSet<string>());
                    foreach (string ingredient in ingredients)
                    {
                        allergensWithPossibleIngredients[allergen].Add(ingredient);
                    }
                }
                //  otherwise, compare foods in it
                else
                {
                    HashSet<string> newIngredients = new HashSet<string>(ingredients);
                    allergensWithPossibleIngredients[allergen].IntersectWith(newIngredients);
                }
            }
        }

        //  go back through list of possible ingredients and remove those from our full list
        foreach (var allergenInfo in allergensWithPossibleIngredients)
        {
            foreach (var ingredient in allergenInfo.Value)
            {
                fullIngredientList.RemoveAll(ingrdt => ingrdt == ingredient);
            }
        }
        Console.WriteLine($"Answer to part one: {fullIngredientList.Count}");

        //  print full allergen list and convert to a list so we can put them in order and access by index
        List<(string, List<string>)> listOfAllergens = new List<(string, List<string>)>();
        foreach (var allergenInfo in allergensWithPossibleIngredients)
        {
            List<string> ingredientList = new List<string>(allergenInfo.Value);
            listOfAllergens.Add((allergenInfo.Key, ingredientList));
        }
        //  order list by length of ingredient list
        //  cull any allergens that have only one ingredient
        listOfAllergens.Sort((x, y) => x.Item2.Count.CompareTo(y.Item2.Count)); // not necessary, but more efficient at beginning
        List<(string allergen, string ingredient)> canonicalDangerousIngredientsByAllergen = new List<(string, string)>();
        while (listOfAllergens.Count > 0)
        {
            for (int i = 0; i < listOfAllergens.Count; i++)
            {
                if (listOfAllergens[i].Item2.Count == 1)
                {
                    // Console.WriteLine($"Putting {listOfAllergens[i].Item2[0]} on the list");
                    canonicalDangerousIngredientsByAllergen.Add((listOfAllergens[i].Item1, listOfAllergens[i].Item2[0]));
                    //  remove ingredient from remaining allergens
                    for (int j = 0; j < listOfAllergens.Count; j++)
                    {
                        if (j == i) continue;
                        if (listOfAllergens[j].Item2.Contains(listOfAllergens[i].Item2[0]))
                        {
                            listOfAllergens[j].Item2.Remove(listOfAllergens[i].Item2[0]);
                        }
                    }
                    //  then remove item from list and start over
                    listOfAllergens.RemoveAt(i);
                    break;
                }
            }
        }

        Console.WriteLine($"We have culled {canonicalDangerousIngredientsByAllergen.Count} ingredients so far and have {listOfAllergens.Count} allergens left to analyze");
        canonicalDangerousIngredientsByAllergen.Sort();
        Console.WriteLine("Answer to part 2");
        foreach (var pair in canonicalDangerousIngredientsByAllergen)
        {
            Console.WriteLine($"{pair.allergen} : {pair.ingredient}");
        }
        Console.WriteLine("Answer to Part two for copy/pasting");
        foreach (var pair in canonicalDangerousIngredientsByAllergen)
        {
            Console.Write($"{pair.ingredient},");
        }


    }
}
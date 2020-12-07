using System;
using System.Collections.Generic;

class Day07
{
    private List<string> data;
    public Day07(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        Console.WriteLine($"This many data points: {data.Count}");

        //   part one
        // HashSet<string> relevantBags = new HashSet<string>();
        // getContainersForType(relevantBags, "shiny gold");
        // Console.WriteLine($"Number of bags that can contain shiny gold bags: {relevantBags.Count}");

        //  part two
        //  parse data into dictionaries for ease of reference
        Dictionary<string, string> allBags = new Dictionary<string, string>();
        foreach (string rule in data)
        {
            allBags.Add(rule.Split(" bags contain ")[0], rule.Split(" bags contain ")[1]);
        }
        Console.WriteLine($"This many unique bags: {allBags.Count}");
        int numBagsInShinyGold = getNumberOfBagsForTypeIncludingItself(allBags, "shiny gold");
        Console.WriteLine($"shiny gold bags contain {numBagsInShinyGold - 1} bags.");
    }

    private HashSet<string> getContainersForType(HashSet<string> bagTypes, string type)
    {
        for (int i = 0; i < data.Count; i++)
        {
            bool allowed = data[i].Split(" bags contain ")[1].Contains(type);
            if (allowed)
            {
                string newType = data[i].Split(" bags contain ")[0];
                // Console.WriteLine($"{newType} can contain {type}");
                bagTypes.Add(newType);
                bagTypes.UnionWith(getContainersForType(bagTypes, newType));
            }
        }

        return bagTypes;
    }

    private int getNumberOfBagsForTypeIncludingItself(Dictionary<string, string> allBags, string type)
    {
        List<(int NumBags, string BagType)> interior = parseInteriorOfBagType(allBags, type);

        //  if the bag has no bags inside itself, return 1 (indicating itself)
        if (interior.Count == 0) { return 1; }

        //  otherwise, dig down into bags to see what they contain
        int numBagsIncludingSelf = 1;
        foreach (var bag in interior)
        {
            Console.WriteLine($"\tinside: {type}, {bag.NumBags} {bag.BagType}");
            int bagsInside = bag.NumBags * getNumberOfBagsForTypeIncludingItself(allBags, bag.BagType);
            numBagsIncludingSelf += bagsInside;
        }
        return numBagsIncludingSelf;
    }

    private List<(int NumBags, string BagType)> parseInteriorOfBagType(Dictionary<string, string> allBags, string type)
    {
        List<(int NumBags, string BagType)> interiorList = new List<(int NumBags, string BagType)>();
        if (!allBags.ContainsKey(type))
        {
            Console.WriteLine($"{type} not in dictionary");
            return interiorList;
        }

        string[] parsedInterior = allBags[type].Split(", ");
        //  each bag data contains a number, a two-word description, and the word "bag" or "bags" (which can be ignored)
        foreach (string bagData in parsedInterior)
        {
            string[] individualBagData = bagData.Split(" ");
            if (individualBagData[0] == "no")
            {
                continue;
            }
            int quantity = int.Parse(individualBagData[0]);
            string bagType = individualBagData[1] + " " + individualBagData[2];
            interiorList.Add((quantity, bagType));
        }

        return interiorList;
    }
}

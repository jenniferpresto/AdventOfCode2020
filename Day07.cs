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
        HashSet<string> relevantBags = new HashSet<string>();
        getContainersForType(relevantBags, "shiny gold");
        int bagCount = relevantBags.Count;
        Console.WriteLine($"Number of bags that can contain shiny gold bags: {bagCount}");

        //  part two
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
}

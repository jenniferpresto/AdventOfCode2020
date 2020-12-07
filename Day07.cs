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
        HashSet<string> relevantBags = new HashSet<string>();
        getContainersForType(relevantBags, "shiny gold");
        int bagCount = relevantBags.Count;
        Console.WriteLine($"Number of bags that can contain shiny gold bags: {bagCount}");
    }

    private string getPrimaryBagTypeForRule(string rule)
    {
        return rule.Split(" bags contain ")[0];
    }

    private bool ruleAllowsBagType(string rule, string bagType)
    {
        return rule.Split(" bags contain ")[1].Contains(bagType);
    }

    private HashSet<string> getContainersForType(HashSet<string> bagTypes, string type)
    {
        for (int i = 0; i < data.Count; i++)
        {
            bool allowed = ruleAllowsBagType(data[i], type);
            if (allowed)
            {
                string newType = getPrimaryBagTypeForRule(data[i]);
                // Console.WriteLine($"{newType} can contain {type}");
                bagTypes.Add(newType);
                bagTypes.UnionWith(getContainersForType(bagTypes, newType));
            }
        }

        return bagTypes;
    }
}

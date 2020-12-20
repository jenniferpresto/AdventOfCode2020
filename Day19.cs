using System;
using System.Collections.Generic;
using System.Linq;
class Day19
{
    private List<string> data;
    Dictionary<string, List<string>> allRules = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> expandedRules = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> allLetterSubstitutions = new Dictionary<string, List<string>>();
    HashSet<string> allPossibilities = new HashSet<string>();
    HashSet<string> receivedMessages = new HashSet<string>();

    public Day19(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        parseData();
        doPartOne();

    }

    private void doPartOne()
    {
        //  separate the original nodes (should be two)
        separateSubstitutions();

        int numPasses = 0;
        while (!allLetterSubstitutions.ContainsKey("0"))
        {
            Console.WriteLine($"pass {numPasses}##############################, {allRules.Count} rules left");
            copyAllRulesIntoExpandedRules();
            performSubstitutionsAndPutIntoExpandedRules();
            copyExpandedRulesIntoAllRules();
            expandedRules.Clear();
            separateSubstitutions();

            numPasses++;
        }

        foreach (var value in allLetterSubstitutions["0"])
        {
            allPossibilities.Add(value.Replace(" ", ""));
            // Console.WriteLine(value.Replace(" ", ""));
        }

        HashSet<string> validReceivedMessages = new HashSet<string>(allPossibilities);
        validReceivedMessages.IntersectWith(receivedMessages);
        Console.WriteLine("These are the valid messages");
        foreach (string i in validReceivedMessages)
        {
            Console.WriteLine(i);
        }
        Console.WriteLine($"There are {validReceivedMessages.Count} valid messages");
    }

    private void separateSubstitutions()
    {
        foreach (var rule in allRules)
        {
            if (ruleStillHasNumbers(rule.Key)) { continue; }
            for (int i = 0; i < rule.Value.Count; i++)
            {
                string condensedVal = rule.Value[i].Replace(" ", "") + " ";
                rule.Value[i] = condensedVal;
            }
            allLetterSubstitutions[rule.Key] = rule.Value;
        }

        //  then we can remove it from our rules
        foreach (var substitute in allLetterSubstitutions)
        {
            if (allRules.ContainsKey(substitute.Key))
            {
                allRules.Remove(substitute.Key);
            }
        }
    }

    private bool ruleStillHasNumbers(string ruleKey)
    {
        foreach (string eachRuleValue in allRules[ruleKey])
        {
            //  see if we contain any numbers (probably a better way to do this)
            char[] allDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            if (eachRuleValue.IndexOfAny(allDigits) > -1)
            {
                return true;
            }
        }
        return false;
    }
    private void performSubstitutionsAndPutIntoExpandedRules()
    {
        foreach (var rule in allRules)
        {
            List<string> listOfNewValues = new List<string>(rule.Value);
            int valIndex = 0;
            foreach (var origValue in rule.Value)
            {
                List<string> replacementsForOrigValue = new List<string>();

                foreach (var substitute in allLetterSubstitutions)
                {
                    if (origValue.Contains(" " + substitute.Key + " "))
                    {
                        foreach (string subString in substitute.Value)
                        {
                            replacementsForOrigValue.Add(origValue.Replace(" " + substitute.Key + " ", " " + subString + " "));
                        }
                        break; // let's do this just one substitute key at a time
                    }
                }
                if (replacementsForOrigValue.Count > 0)
                {
                    listOfNewValues.Remove(origValue);
                    listOfNewValues.AddRange(replacementsForOrigValue);
                }
                valIndex++;
            }
            //  modify the expanded rules accordingly
            expandedRules.Remove(rule.Key);
            expandedRules[rule.Key] = listOfNewValues;
        }
    }

    private void printAllRules()
    {
        foreach (var rule in allRules)
        {
            Console.WriteLine($"Rule {rule.Key}");
            foreach (var eachRule in rule.Value)
            {
                Console.WriteLine($"\t{eachRule}");
            }
        }
    }

    private void printAllSubstitutions()
    {
        foreach (var substitute in allLetterSubstitutions)
        {
            Console.WriteLine($"Rule {substitute.Key}");
            foreach (var value in substitute.Value)
            {
                Console.WriteLine($"\tCan substitute: {value}");
            }
        }
    }

    private void copyExpandedRulesIntoAllRules()
    {
        //  copy the expanded dictionary into the main one
        allRules.Clear();
        foreach (var rule in expandedRules)
        {
            List<string> values = new List<string>();
            foreach (string value in rule.Value)
            {
                values.Add(value);
            }
            allRules.Add(rule.Key, values);
        }
    }

    private void copyAllRulesIntoExpandedRules()
    {
        //  copy the expanded dictionary into the main one
        expandedRules.Clear();
        foreach (var rule in allRules)
        {
            List<string> values = new List<string>();
            foreach (string value in rule.Value)
            {
                values.Add(value);
            }
            expandedRules.Add(rule.Key, values);
        }
    }

    private void parseData()
    {
        //  parse the data i
        bool parsingRules = true;
        foreach (var line in data)
        {
            // Console.WriteLine(line);
            if (line == "")
            {
                Console.WriteLine("End of rules!");
                parsingRules = false;
                continue;
            }
            if (parsingRules)
            {
                List<string> subrules = new List<string>();
                string[] splitLine = line.Split(": ");
                if (splitLine[1].Contains("\""))
                {
                    subrules.Add(splitLine[1][1].ToString());
                }
                else
                {
                    string[] splitRules = splitLine[1].Split(" | ");
                    foreach (var subrule in splitRules)
                    {
                        subrules.Add(" " + subrule + " "); // add a space to the beginning and end of every list to make it easier to substitute
                    }
                }
                allRules.Add(splitLine[0], subrules);
            }
            else
            {
                receivedMessages.Add(line);
            }
        }

    }
}
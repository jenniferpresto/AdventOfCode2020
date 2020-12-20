using System;
using System.Collections.Generic;
using System.Linq;
class Day19
{
    private List<string> data;
    Dictionary<string, List<string>> allRules = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> expandedRules = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> allLetterSubstitutions = new Dictionary<string, List<string>>();
    Dictionary<string, string> rootNodes = new Dictionary<string, string>();
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
            Console.WriteLine($"pass {numPasses}##############################");
            Console.WriteLine($"Num rules: {allRules.Count}");
            copyAllRulesIntoExpandedRules();
            Console.WriteLine("Starting substitution");
            performSubstitutionsAndPutIntoExpandedRules();
            Console.WriteLine("Finished substitution");
            copyExpandedRulesIntoAllRules();
            expandedRules.Clear();
            separateSubstitutions();

            numPasses++;
            // if (allLetterSubstitutions.ContainsKey("0")) break;
            // Console.ReadLine();
        }

        // Console.WriteLine($"allrules no longer contains key 0; it has {allLetterSubstitutions["0"].Count}");
        Console.WriteLine($"One rule left, this many substitutions: {allLetterSubstitutions.Count}");

        foreach (var value in allLetterSubstitutions["0"])
        {
            allPossibilities.Add(value.Replace(" ", ""));
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
        Console.WriteLine($"We start with this many: {allRules.Count} ");

        // bool weHit26 = false;
        foreach (var rule in allRules)
        {
            if (ruleStillHasNumbers(rule.Key)) { continue; }
            for (int i = 0; i < rule.Value.Count; i++)
            {
                // remove spaces and save in substitute list
                // string condensedVal = rule.Value[i];
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
                Console.WriteLine($"REmoving rule from allRules: {substitute.Key}; new legnth is {allRules.Count}");
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
        // Console.WriteLine($"Performing...; thiere is {allRules.Count} rule left");
        // string zeroStr = "0";
        // bool shouldPrint = false;
        // if (allRules.Count == 1)
        // {
        //     if (allRules.First().Key == "0")
        //     {
        //         // Console.WriteLine($"0 has {allRules[zeroStr].Count}, first one is {allRules[zeroStr][0]}");
        //         // Console.WriteLine($"Rule 11 has {allLetterSubstitutions[elevenStr].Count} values");
        //         Console.WriteLine($"There are {allLetterSubstitutions.Count} substitutions; allRules has 1 rule, which has {allRules.First().Value.Count} values");
        //         Console.WriteLine($"First value is {allRules.First().Value[0]}");
        //         Console.WriteLine($"Does 0 already exist in substitutes? {allLetterSubstitutions.ContainsKey(zeroStr)}");

        //         shouldPrint = true;

        //     }
        // }

        foreach (var rule in allRules)
        {
            bool shouldPrint = false;
            if (rule.Key == "0")
            {
                shouldPrint = true;
            }

            List<string> listOfNewValues = new List<string>(rule.Value);
            int valIndex = 0;
            foreach (var origValue in rule.Value)
            {
                // if (shouldPrint) { Console.WriteLine($"original value: {origValue}, {valIndex} out of {allRules.First().Value.Count}"); }
                List<string> replacementsForOrigValue = new List<string>();

                foreach (var substitute in allLetterSubstitutions)
                {
                    bool foundASubstute = false;
                    if (origValue.Contains(" " + substitute.Key + " "))
                    {
                        foundASubstute = true;
                        // if (shouldPrint) { Console.WriteLine($"Rule {substitute.Key} has {substitute.Value.Count} values"); }
                        // int rule11Index = 0;
                        foreach (string subString in substitute.Value)
                        {
                            // if (shouldPrint) Console.WriteLine($"adding substitute index {subIndex}, adding value {subString}");
                            replacementsForOrigValue.Add(origValue.Replace(" " + substitute.Key + " ", " " + subString + " "));
                            // rule11Index++;
                            // if (shouldPrint) Console.WriteLine($"Added {rule11Index}; last one was {subString}");
                            // if (rule11Index > 16380) Console.ReadLine();
                        }
                        break; // let's do this just one substitute key at a time
                    }
                    else
                    {
                        // if (shouldPrint) { Console.WriteLine($"Skipping {substitute.Key} because not in rule"); }
                    }
                    if (foundASubstute) break;
                    // subIndex++;
                    // if (subIndex > 130)
                    // {
                    //     Console.WriteLine($"Press enter {subIndex}");
                    //     Console.ReadLine();
                    // }
                }
                // if (shouldPrint)
                // {
                //     Console.WriteLine($"Press enter outside loop; looking at {origValue}");
                //     Console.ReadLine();
                // }
                if (replacementsForOrigValue.Count > 0)
                {
                    listOfNewValues.Remove(origValue);
                    listOfNewValues.AddRange(replacementsForOrigValue);
                    listOfNewValues = listOfNewValues.Distinct().ToList();
                }
                valIndex++;
            }
            //  modify the expanded rules accordingly
            Console.WriteLine($"Got through ths for loop for rule {rule.Key}");
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
                    rootNodes.Add(splitLine[0], splitLine[1][1].ToString());
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
                //  TODO: parse messages
            }
        }

    }
}
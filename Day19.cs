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
    HashSet<string> allMessages = new HashSet<string>();

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
        Console.WriteLine("Printing all rules");
        foreach (var rule in allRules)
        {
            Console.WriteLine($"Rule {rule.Key}");
            foreach (var eachRule in rule.Value)
            {
                Console.WriteLine(eachRule);
            }
        }
        //  first replace any rules that contain a root node (should be two)
        separateSubstitutions();
        Console.WriteLine("Printing substitutions after very first separation");
        foreach (var substitute in allLetterSubstitutions)
        {
            Console.WriteLine($"Rule {substitute.Key}");
            foreach (var value in substitute.Value)
            {
                Console.WriteLine($"\tCan substitute: {value}");
            }
        }

        Console.WriteLine("First pass****************************");
        copyAllRulesIntoExpandedRules();
        performSubstitutionsAndPutIntoExpandedRules();
        copyExpandedRulesIntoAllRules();
        separateSubstitutions();
        expandedRules.Clear();

        Console.WriteLine("New rules afer First Pass");
        printAllRules();
        Console.WriteLine("New substitutions after first pass");
        printAllSubstitutions();

        Console.WriteLine("Second pass****************************");
        copyAllRulesIntoExpandedRules();
        performSubstitutionsAndPutIntoExpandedRules();
        copyExpandedRulesIntoAllRules();
        separateSubstitutions();
        expandedRules.Clear();

        Console.WriteLine("New rules afer First Pass");
        printAllRules();
        Console.WriteLine("New substitutions after first pass");
        printAllSubstitutions();



        Console.WriteLine("Third pass****************************");
        copyAllRulesIntoExpandedRules();
        performSubstitutionsAndPutIntoExpandedRules();
        copyExpandedRulesIntoAllRules();
        separateSubstitutions();
        expandedRules.Clear();

        Console.WriteLine("New rules afer First Pass");
        printAllRules();
        Console.WriteLine("New substitutions after first pass");
        printAllSubstitutions();


    }

    private void separateSubstitutions()
    {
        foreach (var rule in allRules)
        {
            bool stillHasNumbers = false;
            //  see if rule has any numbers left
            foreach (string eachRuleValue in rule.Value)
            {
                //  see if we contain any numbers (probably a better way to do this)
                char[] allDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                if (eachRuleValue.IndexOfAny(allDigits) > -1)
                {
                    stillHasNumbers = true;
                    break;
                }
            }
            //  if it's purely letters, we can move it to the substitutions
            if (!stillHasNumbers)
            {
                Console.WriteLine($"Putting {rule.Key} into substitutions:");
                foreach (var value in rule.Value)
                {
                    Console.WriteLine($"\t{value}");
                }
                allLetterSubstitutions[rule.Key] = rule.Value;
            }
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

    private void performSubstitutionsAndPutIntoExpandedRules()
    {
        foreach (var rule in allRules)
        {
            List<string> listOfNewValues = new List<string>(rule.Value);
            foreach (var origValue in rule.Value)
            {
                List<string> newValues = new List<string>();
                foreach (var substitute in allLetterSubstitutions)
                {
                    if (origValue.Contains(substitute.Key + " "))
                    {
                        foreach (string subString in substitute.Value)
                        {
                            // Console.WriteLine($"in Rule {rule.Key}, replacing {substitute.Key} with {subString}");
                            newValues.Add(origValue.Replace(substitute.Key + " ", subString + " "));
                        }
                        break; // let's do this just one at a time
                    }
                }
                Console.WriteLine($"The new values after this substitution for rule {rule.Key} are ");
                foreach (string sub in newValues)
                {
                    Console.WriteLine(sub);
                }
                Console.WriteLine($"Removing {origValue} and replacing it with {newValues.Count} options");
                if (newValues.Count > 0)
                {
                    listOfNewValues.Remove(origValue);
                    listOfNewValues.AddRange(newValues);

                }
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
                    rootNodes.Add(splitLine[0], splitLine[1][1].ToString());
                    subrules.Add(splitLine[1][1].ToString());
                }
                else
                {
                    string[] splitRules = splitLine[1].Split(" | ");
                    foreach (var subrule in splitRules)
                    {
                        subrules.Add(subrule + " "); // add a space to the end of every list to make it easier to substitute
                    }
                }
                allRules.Add(splitLine[0], subrules);
            }
            else
            {
                //  TODO: parse messages
            }
        }

    }
}
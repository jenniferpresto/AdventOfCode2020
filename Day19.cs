using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
    Helpful reference here: https://dev.to/qviper/advent-of-code-2020-python-solution-day-19-4p9d
    Reference uses stacks rather than lists, which is actually cleaner
*/

class Day19
{
    List<string> data;
    Dictionary<string, List<List<string>>> allRules = new Dictionary<string, List<List<string>>>();
    List<string> receivedMessages = new List<string>();
    public Day19(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        parseData();
        //  part one
        int numValidMsgs = 0;
        foreach (string msg in receivedMessages)
        {
            bool checksOut = checkRule(msg, allRules["0"][0]);
            if (checksOut)
            {
                numValidMsgs++;
            }
        }
        Console.WriteLine($"Valid messages for part 1: {numValidMsgs}");

        //  modify rules for part two
        List<string> newAdditionToRule8 = new List<string>() { "42", "8" };
        allRules["8"].Add(newAdditionToRule8);
        List<string> newAdditionToRule11 = new List<string>() { "42", "11", "31" };
        allRules["11"].Add(newAdditionToRule11);

        numValidMsgs = 0;
        foreach (string msg in receivedMessages)
        {
            bool checksOut = checkRule(msg, allRules["0"][0]);
            if (checksOut)
            {
                numValidMsgs++;
            }
        }
        Console.WriteLine($"Valid messages for part 2: {numValidMsgs}");
    }

    private bool checkRule(string msg, List<string> rule)
    {
        // Console.WriteLine($"Starting with msg: {msg}");
        if (rule.Count > msg.Length) { return false; }
        if (rule.Count == 0 && msg.Length == 0) { return true; }
        if (rule.Count == 0) { return false; } // this does not hit with given input; would happen if started with subrule

        string ruleUnit = rule[0];
        //  if we're down to a root node, check against the first letter in our remaining message
        if (!Regex.IsMatch(ruleUnit, @"\d"))
        {
            if (msg[0].ToString() == rule[0])
            {
                string remainingMsg = msg.Substring(1); // cut off the first letter and test the next one
                List<string> remainingRule = new List<string>(rule);
                remainingRule.RemoveAt(0); // cut off first piece of rule and move to the next
                return checkRule(remainingMsg, remainingRule);
            }
            else
            {
                return false;
            }
        }
        //  we still have numbers left in the rule
        else
        {
            //  run tests for each of the possibilities the first rule splits into
            foreach (var subrule in allRules[rule[0]])
            {
                List<string> expandedRule = new List<string>();
                List<string> shortenedRuleList = new List<string>(rule);
                shortenedRuleList.RemoveAt(0);
                expandedRule.AddRange(subrule);
                expandedRule.AddRange(shortenedRuleList);

                //  continue recursing only if we make it this far (i.e., don't return false, because we need to check additional paths)
                if (checkRule(msg, expandedRule))
                {
                    return true;
                }
            }
        }

        return false;
    }


    private void parseData()
    {
        //  parse the data i
        bool parsingRules = true;
        foreach (var line in data)
        {
            if (line == "")
            {
                Console.WriteLine("End of rules!");
                parsingRules = false;
                continue;
            }
            if (parsingRules)
            {
                List<List<string>> subrules = new List<List<string>>();
                string[] splitLine = line.Split(": ");

                if (splitLine[1].Contains("\""))
                {
                    List<string> finalNode = new List<string>();
                    finalNode.Add(splitLine[1][1].ToString());
                    subrules.Add(finalNode);
                }
                else
                {
                    string[] splitRules = splitLine[1].Split(" | ");
                    foreach (var subrule in splitRules)
                    {
                        List<string> values = new List<string>();
                        foreach (var unit in subrule.Split(" "))
                        {
                            values.Add(unit);
                        }
                        subrules.Add(values);
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

    private void printRules()
    {
        foreach (var rule in allRules)
        {
            Console.WriteLine(rule.Key);
            foreach (var subrule in rule.Value)
            {
                Console.Write("\t");
                foreach (var unit in subrule)
                {
                    Console.Write(unit + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
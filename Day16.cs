using System;
using System.Collections.Generic;
using System.Linq;

class Day16
{
    private List<string> data;

    public Day16(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        // doPartOne();
        doPartTwo();
    }

    private void doPartOne()
    {
        List<(int lower, int higher)> validRanges = new List<(int, int)>();
        List<List<int>> tickets = new List<List<int>>();

        //  parse data
        bool readingRules = true;
        for (int i = 0; i < data.Count; i++)
        {
            //  add the valid ranges to the rules list
            if (readingRules)
            {
                if (data[i] == "")
                {
                    // Console.WriteLine("Hit the end of the ranges");
                    readingRules = false;
                    i += 4;
                    continue;
                }

                string[] splitLine = data[i].Split(": ")[1].Split(' ', '-');
                // foreach (string datapoint in splitLine)
                // {
                //     Console.WriteLine(datapoint);

                // }
                validRanges.Add((int.Parse(splitLine[0]), int.Parse(splitLine[1])));
                validRanges.Add((int.Parse(splitLine[3]), int.Parse(splitLine[4])));
                // foreach (var range in validRanges)
                // {
                //     Console.WriteLine(range);
                // }
            }
            //  add values of other tickets to other list
            else
            {
                List<int> ticketValues = new List<int>();
                foreach (string ticketVal in data[i].Split(','))
                {
                    ticketValues.Add(int.Parse(ticketVal));
                }
                tickets.Add(ticketValues);
            }
        }

        //  compare values in each ticket
        int totalInvalidValues = 0;
        foreach (var ticket in tickets)
        {
            foreach (int ticketValue in ticket)
            {
                bool validValue = false;
                foreach (var range in validRanges)
                {
                    if (ticketValue >= range.lower && ticketValue <= range.higher)
                    {
                        validValue = true;
                        break;
                    }
                }
                if (validValue) { continue; }
                totalInvalidValues += ticketValue;
            }
        }

        Console.WriteLine($"Total ticket scanning error rate: {totalInvalidValues}");
    }

    private void doPartTwo()
    {
        //  parse data more finely than in part one
        Dictionary<string, List<(int low, int high)>> rules = new Dictionary<string, List<(int, int)>>();
        List<List<int>> nearbyTickets = new List<List<int>>();
        List<List<int>> validTickets = new List<List<int>>();
        List<int> myTicket = new List<int>();

        //  parse data
        bool readingRules = true;
        for (int i = 0; i < data.Count; i++)
        {
            //  add the valid ranges to the rules list
            if (readingRules)
            {
                if (data[i] == "")
                {
                    // Console.WriteLine("Hit the end of the ranges");
                    readingRules = false;
                    i++;
                    continue;
                }

                string fieldName = data[i].Split(":")[0];
                string[] rangeData = data[i].Split(": ")[1].Split(' ', '-');
                // foreach (string datapoint in splitLine)
                // {
                //     Console.WriteLine(datapoint);

                List<(int, int)> ranges = new List<(int, int)>();

                (int, int) validRangeLow = (int.Parse(rangeData[0]), int.Parse(rangeData[1]));
                (int, int) validRangeHigh = (int.Parse(rangeData[3]), int.Parse(rangeData[4]));
                ranges.Add(validRangeLow);
                ranges.Add(validRangeHigh);
                rules.Add(fieldName, ranges);
            }
            //  add values of other tickets to other list
            else
            {
                List<int> ticketValues = new List<int>();
                foreach (string ticketVal in data[i].Split(','))
                {
                    ticketValues.Add(int.Parse(ticketVal));
                }
                if (i > 0 && data[i - 1] == "your ticket:")
                {
                    myTicket = ticketValues;
                    i += 2;
                }
                else
                {
                    nearbyTickets.Add(ticketValues);
                }
            }
        }

        // //  print everything out
        // Console.WriteLine("Rules");
        // foreach (var rule in rules)
        // {
        //     Console.WriteLine(rule.Key);
        //     foreach (var range in rule.Value)
        //     {
        //         Console.WriteLine($"\tlow: {range.low}, high: {range.high}");
        //     }
        // }
        // Console.WriteLine("My ticket");
        // foreach (int val in myTicket)
        // {
        //     Console.Write($"{val},");
        // }
        // Console.Write("\n");
        // Console.WriteLine("other tickets");
        // foreach (var ticket in allTickets)
        // {

        //     foreach (var val in ticket)
        //     {
        //         Console.Write($"{val},");
        //     }
        //     Console.Write("\n");
        // }

        //  create list of only valid tickets
        for (int i = 0; i < nearbyTickets.Count; i++)
        {
            bool valuesAllFit = true;
            foreach (int val in nearbyTickets[i])
            {
                if (!valueFitsInAtLeastOneRange(val, rules))
                {
                    valuesAllFit = false;
                    break;
                }
            }
            if (valuesAllFit)
            {
                validTickets.Add(nearbyTickets[i]);
            }
        }
        // Console.WriteLine("Valid tickets");
        // foreach (var ticket in validTickets)
        // {
        //     foreach (var val in ticket)
        //     {
        //         Console.Write($"{val},");
        //     }
        //     Console.WriteLine("");
        // }

        //  now, check each column for which fields it might be valid for
        List<(int, List<string>)> validFieldsByColumn = new List<(int, List<string>)>();
        for (int i = 0; i < validTickets[0].Count; i++)
        {
            List<int> column = new List<int>();
            foreach (var ticket in validTickets)
            {
                column.Add(ticket[i]);
            }
            List<string> validFields = returnValidFieldNames(column, rules);
            // Console.WriteLine("***********************");
            // Console.WriteLine($"column {i} must be: ");
            // foreach (string field in validFields)
            // {
            //     Console.WriteLine(field);
            // }
            validFieldsByColumn.Add((i, validFields));
        }

        //  order our list of columns by length of valid fields
        validFieldsByColumn.Sort((x, y) => x.Item2.Count.CompareTo(y.Item2.Count));
        for (int i = 0; i < validFieldsByColumn.Count; i++)
        {
            // Console.WriteLine($"**************Column {column.Item1}");
            // foreach (var field in column.Item2)
            // {
            //     Console.WriteLine(field);
            // }

            //  at this point in the for loop, each column should have only one item, as we delete them progressively from the additional items
            //  if not, give warning
            if (validFieldsByColumn[i].Item2.Count != 1)
            {
                Console.WriteLine("Unexpected number of items left in list of valid fields!");
            }
            string relevantField = validFieldsByColumn[i].Item2[0];
            for (int j = i + 1; j < validFieldsByColumn.Count; j++)
            {
                //  delete the relevant field from other fields
                validFieldsByColumn[j].Item2.Remove(relevantField);
            }
        }
        //  make sure we just have one left apiece
        //  also calculate our final answer
        long product = 1;
        for (int i = 0; i < validFieldsByColumn.Count; i++)
        {
            if (validFieldsByColumn[i].Item2.Count != 1)
            {
                Console.WriteLine("Something went wrong");
            }
            Console.WriteLine($"Column {validFieldsByColumn[i].Item1} must be {validFieldsByColumn[i].Item2[0]}");
            if (validFieldsByColumn[i].Item2[0].StartsWith("departure"))
            {
                Console.WriteLine($"Column is {validFieldsByColumn[i].Item1}, value in my ticket is {myTicket[validFieldsByColumn[i].Item1]}");
                product *= myTicket[validFieldsByColumn[i].Item1];
                Console.WriteLine($"New product: {product}");
            }
        }
        Console.WriteLine($"Answer is {product}");

    }

    private bool valueFitsInAtLeastOneRange(int val, Dictionary<string, List<(int low, int high)>> rules)
    {
        bool valueIsValid = false;
        foreach (var rule in rules)
        {
            if (val >= rule.Value[0].low && val <= rule.Value[0].high)
            {
                valueIsValid = true;
                break;
            }
            if (val >= rule.Value[1].low && val <= rule.Value[1].high)
            {
                valueIsValid = true;
                break;
            }
        }
        return valueIsValid;
    }

    private List<string> returnValidFieldNames(List<int> values, Dictionary<string, List<(int low, int high)>> rules)
    {
        Dictionary<string, bool> ruleNames = new Dictionary<string, bool>();
        foreach (var rule in rules)
        {
            ruleNames.Add(rule.Key, false);
            bool allValuesWork = true;
            foreach (int val in values)
            {
                if ((val >= rule.Value[0].low && val <= rule.Value[0].high) || (val >= rule.Value[1].low && val <= rule.Value[1].high))
                {
                    continue;
                }
                allValuesWork = false;
            }
            ruleNames[rule.Key] = allValuesWork;
        }


        List<string> validFields = new List<string>();
        foreach (var rule in ruleNames)
        {
            if (rule.Value)
            {
                validFields.Add(rule.Key);
            }
        }
        return validFields;
    }

}
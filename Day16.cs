using System;
using System.Collections.Generic;

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
        List<List<int>> allTickets = new List<List<int>>();
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
                Console.WriteLine(data[i]);
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
                    allTickets.Add(ticketValues);
                }
            }
        }

        // //  print everything out
        // Console.WriteLine("Rules");
        foreach (var rule in rules)
        {
            Console.WriteLine(rule.Key);
            foreach (var range in rule.Value)
            {
                Console.WriteLine($"\tlow: {range.low}, high: {range.high}");
            }
        }
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
        for (int i = 0; i < allTickets.Count; i++)
        {

        }


    }

    private bool valueFitsInAtLeastOneRange(int val, Dictionary<string, List<(int low, int high)>> rules)
    {
        return true;
    }

}
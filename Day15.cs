using System;
using System.Collections.Generic;

class Day15
{
    private List<string> data;
    // private Dictionary<int, int> numbers;
    // private List<int> originalList;
    public Day15(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        doCalculationWithList(2020);
        doCalculationWithDictionary(3000000);
    }

    private void doCalculationWithList(int target)
    {
        List<int> numbers = new List<int>();
        foreach (string number in data[0].Split(","))
        {
            numbers.Add(int.Parse(number));
        }
        int numStarters = numbers.Count;
        Console.WriteLine($"Started: {DateTime.Now}");
        for (int i = numStarters - 1; i < target; i++)
        {
            //  consider the last number
            int considerNum = numbers[i];

            //  see how many exist
            int distanceBack = 0;
            bool foundNum = false;
            for (int j = numbers.Count - 2; j > -1; j--)
            {
                if (numbers[j] == considerNum)
                {
                    distanceBack = numbers.Count - 1 - j;
                    numbers.Add(distanceBack);
                    foundNum = true;
                    break;
                }
            }
            if (!foundNum)
            {
                // Console.WriteLine($"Step {i}; adding {0}");
                numbers.Add(0);
            }
        }
        Console.WriteLine($"Finished: {DateTime.Now}");
        Console.WriteLine($"numbers size: {numbers.Count}");
        Console.WriteLine($"Answer is {numbers[target - 1]}");
    }
    public void doCalculationWithDictionary(int target)
    {
        Dictionary<int, int> numbers = new Dictionary<int, int>();
        List<int> originalList = new List<int>();
        int turn = 1;
        foreach (string number in data[0].Split(","))
        {
            //  add all but last number to the dictionary
            if (turn < data[0].Split(",").Length)
            {
                numbers.Add(int.Parse(number), turn);
            }
            //  add all to the list
            originalList.Add(int.Parse(number));
            turn++;
        }
        // //  checks
        // Console.WriteLine($"This many in map: {numbers.Count}");
        // foreach (var pair in numbers)
        // {
        //     Console.WriteLine(pair);
        // }
        // Console.WriteLine($"This many in list: {originalList.Count}");

        //  start by considering the last number in the original list
        //  it's not yet added to the dictionary
        int lastNumSpoken = originalList[originalList.Count - 1];
        turn = originalList.Count + 1;
        Console.WriteLine($"Started: {DateTime.Now}");
        while (true)
        {
            //  if we've said this number before
            if (!numbers.ContainsKey(lastNumSpoken))
            {
                numbers.Add(lastNumSpoken, turn - 1);
                lastNumSpoken = 0;
            }
            //  if it's new
            else
            {
                int lastTimeOccurred = numbers[lastNumSpoken];
                int newValueToConsider = turn - 1 - lastTimeOccurred;
                numbers[lastNumSpoken] = turn - 1;
                lastNumSpoken = newValueToConsider;
            }

            if (turn == target)
            {
                Console.WriteLine($"Turn {turn} said {lastNumSpoken}");
                break;
            }
            turn++;
        }
        Console.WriteLine($"How many items in dict at the end? {numbers.Count}");
        Console.WriteLine($"Finished: {DateTime.Now}");
    }
}
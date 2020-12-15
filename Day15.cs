using System;
using System.Collections.Generic;

class Day15
{
    private List<string> data;
    private List<int> numbers;
    public Day15(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        numbers = new List<int>();
        foreach (string number in data[0].Split(","))
        {
            numbers.Add(int.Parse(number));
        }
        doPartOne();
    }

    public void doPartOne()
    {
        int numStarters = numbers.Count;
        for (int i = numStarters - 1; i < 30000000; i++)
        {
            Console.WriteLine($"starting: i = {i}");
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
                    Console.WriteLine($"Step {i}; adding {distanceBack}");
                    numbers.Add(distanceBack);
                    foundNum = true;
                    break;
                }
            }
            if (!foundNum)
            {
                Console.WriteLine($"Step {i}; adding {0}");
                numbers.Add(0);
            }
        }
        Console.WriteLine($"numbers size: {numbers.Count}");
        Console.WriteLine($"Answer is {numbers[30000000]}");
    }

    private void addNextOne()
    {

    }
}
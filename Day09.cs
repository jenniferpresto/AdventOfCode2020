using System;
using System.Collections.Generic;

class Day09
{
    private List<UInt64> data;

    public Day09(List<UInt64> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        UInt64 answer = doPartOne();
        if (answer > 0)
        {
            doPartTwo(answer);
        }
    }

    private UInt64 doPartOne()
    {
        for (int i = 25; i < data.Count; i++)
        {
            UInt64 testNum = data[i];
            bool matches = false;
            for (int j = i - 25; j < i; j++)
            {
                for (int k = j + 1; k < i; k++)
                {
                    if (data[j] + data[k] == testNum && data[j] != data[k])
                    {
                        matches = true;
                        break;
                    }
                }
                if (matches) break;
            }
            if (!matches)
            {
                Console.WriteLine($"There are no matches for {data[i]}, index: {i}");
                return data[i];
            }
        }
        return 0;
    }

    private void doPartTwo(UInt64 target)
    {
        UInt64 runningTotal = 0;
        for (int i = 0; i < data.Count; i++)
        {
            bool foundAnswer = false;
            runningTotal = data[i];
            int j = i + 1;
            if (j > data.Count - 1)
            {
                break;
            }
            while (runningTotal < target)
            {
                runningTotal += data[j];
                if (runningTotal == target)
                {
                    foundAnswer = true;
                    UInt64 smallest = target;
                    UInt64 largest = 0;
                    //  get smallest and largest values
                    for (int x = i; x < j + 1; x++)
                    {
                        if (smallest > data[x])
                        {
                            smallest = data[x];
                        }
                        if (largest < data[x])
                        {
                            largest = data[x];
                        }
                    }
                    Console.WriteLine($"Found answer from indices: {i} to {j}; smallest is {smallest}; largest is {largest}; total is {smallest + largest}");
                    break;
                }
                j++;
                if (j > data.Count - 1)
                {
                    Console.WriteLine("Reached end of data set without an answer");
                    break;
                }
            }
            if (foundAnswer) break;
        }
    }
}
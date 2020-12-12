using System;
using System.Collections.Generic;

class Day10
{
    private List<int> data;

    public Day10(List<int> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        // partOne();
        partTwo();
    }

    private void partOne()
    {
        data.Sort();
        int ones = 0;
        int threes = 0;
        for (int i = 1; i < data.Count; i++)
        {
            if (data[i] - data[i - 1] == 1)
            {
                ones++;
            }
            else if (data[i] - data[i - 1] == 3)
            {
                threes++;
            }
            else
            {
                Console.WriteLine("Difference is neither one nor three");
            }
        }
        Console.WriteLine($"Ones: {ones}, threes: {threes}; product: {ones * threes}");
    }

    private void partTwo()
    {
        data.Sort();
        data.Insert(0, 0);
        data.Add(data[data.Count - 1] + 3);
        double twoRun = 0; // two possibilities
        double threeRun = 0; // four possibilities
        double fourRun = 0; // six possibilities
        int totalOnes = 0;
        double runningProduct = 1;
        for (int i = 1; i < data.Count; i++)
        {
            int skip = data[i] - data[i - 1];
            if (skip == 1)
            {
                totalOnes++;
            }
            else if (skip == 3)
            {
                if (totalOnes == 2)
                {
                    twoRun++;
                    runningProduct *= 2;
                }
                else if (totalOnes == 3)
                {
                    threeRun++;
                    runningProduct *= 4;
                }
                else if (totalOnes == 4)
                {
                    fourRun++;
                    runningProduct *= 7;
                }
                else
                {
                    Console.WriteLine("space");
                }
                totalOnes = 0;
            }
            else
            {
                Console.WriteLine($"Different skip: {skip}");
            }
        }
        Console.WriteLine($"Running product: {runningProduct}");
    }
}
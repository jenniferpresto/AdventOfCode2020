using System;
using System.Collections.Generic;

class Day13
{
    private List<string> data;

    public Day13(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        doPartOne();
        doPartTwo();
    }

    private void doPartOne()
    {
        int arrivalTime = int.Parse(data[0]);
        int earliestTime = 9999999;
        int busID = -1;
        foreach (string time in data[1].Split(","))
        {
            if (time != "x")
            {
                int busTime = int.Parse(time);
                Console.WriteLine($"bus time: {busTime}");
                int remainder = arrivalTime % busTime;
                Console.WriteLine($"Remainder: {remainder}");
                int firstTimeAfterArrival = arrivalTime + busTime - (arrivalTime % busTime);
                if (firstTimeAfterArrival < earliestTime)
                {
                    earliestTime = firstTimeAfterArrival;
                    busID = busTime;
                }
            }
        }
        int answer = (earliestTime - arrivalTime) * busID;
        Console.WriteLine($"Need to wait until {earliestTime}, for {earliestTime - arrivalTime} for bus {busID}. Answer is {answer}.");
    }

    private void doPartTwo()
    {
        List<(long index, long duration)> busInfoTuples = new List<(long, long)>();
        long count = 0;

        //  create dictionary with indices and bus frequencies
        foreach (string time in data[1].Split(","))
        {
            if (time != "x")
            {
                busInfoTuples.Add((count, long.Parse(time)));
            }
            count++;
        }
        //  sort the values in descending order, keeping with index number in original schedule
        busInfoTuples.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        Console.WriteLine("Tuples");
        foreach (var tuple in busInfoTuples)
        {
            Console.WriteLine(tuple);
        }
        Console.WriteLine("End tuples");
        Console.WriteLine($"Slowest bus is {busInfoTuples[0].duration} at index {busInfoTuples[0].index}");
        int depth = 0;
        long testTime = 1;
        long skip = 1;
        long remainder = (testTime + busInfoTuples[depth].index) % busInfoTuples[depth].duration;
        while (remainder != 0)
        {
            testTime += skip;
            remainder = (testTime + busInfoTuples[depth].index) % busInfoTuples[depth].duration;
            if (remainder == 0)
            {
                Console.WriteLine($"Hit zero at test time {testTime}");
                depth++;
                if (depth == busInfoTuples.Count) break;
                //  test the same number we're on, just to be sure
                remainder = (testTime + busInfoTuples[depth].index) % busInfoTuples[depth].duration;
                if (remainder != 0)
                {
                    skip *= busInfoTuples[depth - 1].duration; // least common multiple; this works only because all prime numbers
                }
            }
            // Console.WriteLine($"testTime: {testTime}, skipping: {skip}");
        }
        Console.WriteLine($"Answer is {testTime}");

    }



}
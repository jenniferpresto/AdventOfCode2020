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
        // doPartOne();
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
        Console.WriteLine($"part two: {data[1]}");
        Dictionary<long, long> busInfo = new Dictionary<long, long>();
        long count = 0;

        //  create dictionary with indices and bus frequencies
        long slowestBus = 0;
        long indexForSlowest = -1;
        foreach (string time in data[1].Split(","))
        {
            if (time == "x")
            {
                busInfo.Add(count, -1L);
            }
            else
            {
                busInfo.Add(count, long.Parse(time));
                if (long.Parse(time) > slowestBus)
                {
                    slowestBus = long.Parse(time);
                    indexForSlowest = count;
                }
            }
            count++;
        }
        Console.WriteLine($"Slowest bus is {slowestBus} at index {indexForSlowest}");
        long possibleNum = 1;

        // foreach (var info in busInfo)
        // {
        //     Console.WriteLine(info);
        //     if (info.Key == 0 || info.Value == -1)
        //     {
        //         continue;
        //     }
        //     possibleNum *= (info.Key + info.Value);
        // }
        // Console.WriteLine(possibleNum);
        long testPoint = slowestBus;
        bool found = false;
        while (!found)
        {
            // Console.WriteLine($"testing time: {testPoint}");
            //  test each bus at that point
            found = true;
            List<long> remainders = new List<long>();
            foreach (var info in busInfo)
            {
                if (info.Value == -1) { continue; } // skip blank ones
                if (info.Key == indexForSlowest) { continue; } // skip the slowest bus
                long diff = indexForSlowest - info.Key;
                long testIndex = testPoint - diff;
                long remainder = (testIndex % info.Value);
                // Console.WriteLine($"Remainder is {remainder}; does it not equal 0? {remainder != 0}");
                // remainders.Add(remainder);
                if (remainder != 0)
                {
                    found = false;
                    break;
                }
                // Console.WriteLine($"Testing value {info.Value} at time {testIndex}, remainder is {remainder}");
            }
            // Console.WriteLine("Remainders");
            // foreach (long remainder in remainders)
            // {
            //     Console.Write($"{remainder}, ");
            // }
            // Console.WriteLine("");
            if (found)
            {
                break; // break before incrementing testPoint
            }
            testPoint += slowestBus;
            // Console.ReadLine();
        }
        Console.WriteLine($"Broke at {testPoint}, key is {indexForSlowest}, so answer is {testPoint - indexForSlowest}");
    }
}
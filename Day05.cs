using System;
using System.Collections.Generic;

class Day05
{
    private List<string> data;
    public Day05(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        int highestSeat = 0;
        List<int> allSeats = new List<int>();
        for (int i = 0; i < data.Count; i++)
        {
            int seat = parsePass(data[i]);
            // Console.WriteLine($"pass {i}: {data[i]}: {seat}");
            if (seat > highestSeat)
            {
                highestSeat = seat;
            }
            allSeats.Add(seat);
        }
        allSeats.Sort();
        for (int i = 0; i < allSeats.Count - 1; i++)
        {
            if (allSeats[i + 1] - allSeats[i] != 1)
            {
                Console.WriteLine($"Highest seat is {highestSeat}. My seat is {allSeats[i] + 1}");
                break;
            }
        }

    }

    private int parsePass(string pass)
    {
        // Console.WriteLine($"Full boarding pass: {pass}");
        string rowStr = pass.Substring(0, 7);
        string colStr = pass.Substring(7);
        int seat = parseBinary(rowStr, 'F', 'B') * 8 + parseBinary(colStr, 'L', 'R');
        return seat;
    }

    private int parseBinary(string unit, char zero, char one)
    {
        string binaryStr = "";
        foreach (char c in unit)
        {
            binaryStr += c == zero ? "0" : "1";
        }
        return Convert.ToInt32(binaryStr, 2);
    }
}
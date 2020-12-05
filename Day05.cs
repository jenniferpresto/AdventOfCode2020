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
        for (int i = 0; i < data.Count; i++)
        {
            int seat = parsePass(data[i]);
            Console.WriteLine($"pass {i}: {data[i]}: {seat}");
            if (seat > highestSeat)
            {
                highestSeat = seat;
            }
        }
        Console.WriteLine($"Highest seat: {highestSeat}");
    }

    private int parsePass(string pass)
    {
        // Console.WriteLine($"Full boarding pass: {pass}");
        string rowStr = pass.Substring(0, 7);
        string colStr = pass.Substring(7);
        int row = parseRow(rowStr);
        int col = parseColumn(colStr);

        int seat = row * 8 + col;
        // Console.WriteLine($"Seat: {seat}");
        return seat;
    }

    private int parseRow(string passRow)
    {
        return parseUnit(passRow, 'F', 'B', 0, 127);
    }

    private int parseColumn(string passCol)
    {
        return parseUnit(passCol, 'L', 'R', 0, 7);
    }

    private int parseBinary(string unit, string zero, string one)
    {
        return 0;
    }

    private int parseUnit(string unit, char lowerChar, char upperChar, int lower, int upper)
    {
        // Console.WriteLine($"{unit} -- lower: {lower}, upper: {upper}");

        //  Final stage
        if (unit.Length == 0)
        {
            if (upper != lower)
            {
                Console.WriteLine($"Problem with parsing! Indices don't match! {lower} {upper}");
            }
            // string type = lowerChar == 'F' ? "row" : "col";
            // Console.WriteLine($"We're done! {type} {lower} {upper}");
            return lower;
        }

        decimal average = (upper + lower) / 2;
        int midway = Convert.ToInt32(Math.Floor(average));
        if (unit[0] == lowerChar)
        {
            return parseUnit(unit.Substring(1), lowerChar, upperChar, lower, midway);
        }
        else
        {
            return parseUnit(unit.Substring(1), lowerChar, upperChar, midway + 1, upper);
        }
    }
}
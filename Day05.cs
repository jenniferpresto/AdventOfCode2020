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
        for (int i = 0; i < 2; i++)
        {
            parsePass(data[i]);
        }
        // foreach (string line in data)
        // {
        //     parsePass(line);
        // }
    }

    private int parsePass(string pass)
    {
        Console.WriteLine($"Full boarding pass: {pass}");
        int seat = 0;
        string rowStr = pass.Substring(0, 6);
        string colStr = pass.Substring(7);
        int row = parseRow(rowStr);
        int col = parseColumn(colStr);
        Console.WriteLine(pass);

        return seat;
    }

    private int parseRow(string passRow)
    {
        parseUnit(passRow, 'F', 'B', 0, 127);
        return 0;
    }

    private int parseColumn(string passCol)
    {
        parseUnit(passCol, 'L', 'R', 0, 8);
        return 0;
    }

    private int parseUnit(string unit, char lowerChar, char upperChar, int lower, int upper)
    {
        decimal average = (upper + lower) / 2;
        int midway = Convert.ToInt32(Math.Floor(average));

        if (unit.Length == 1)
        {
            string type = lowerChar == 'F' ? "row" : "col";
            Console.WriteLine($"We're done! {type} {lower} {upper} {midway}");
            return midway;
        }


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
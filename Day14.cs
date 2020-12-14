using System;
using System.Collections.Generic;

class Day14
{
    private List<string> data;

    public Day14(List<string> dataList)
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
        Dictionary<string, Int64> finalValues = new Dictionary<string, Int64>();
        string currentMask = "000000000000000000000000000000000000";
        foreach (string line in data)
        {
            //  set the mask to the current one
            if (line.StartsWith("mask"))
            {
                currentMask = line.Substring(7);
                continue;
            }

            //  parse location strings
            int endLocationIndex = line.IndexOf(']');
            int locationLength = endLocationIndex - 4;
            string location = line.Substring(4, locationLength);

            //  get values
            Int64 value = Int64.Parse(line.Split(" = ")[1]);

            //  apply mask
            string binaryStr = intToBinaryString(value);
            string newValStr = applyMask(currentMask, binaryStr);
            Int64 newVal = binaryStringToInt(newValStr);
            if (finalValues.ContainsKey(location))
            {
                finalValues[location] = newVal;
            }
            else
            {
                finalValues.Add(location, newVal);
            }
        }

        Int64 finalAnswer = 0;
        foreach (var entry in finalValues)
        {
            finalAnswer += entry.Value;
        }
        Console.WriteLine($"Final sum is {finalAnswer}");
    }

    private void doPartTwo()
    {
        Dictionary<Int64, Int64> finalValues = new Dictionary<Int64, Int64>();
        string currentMask = "000000000000000000000000000000000000";
        foreach (string line in data)
        {
            //  set the mask to the current one
            if (line.StartsWith("mask"))
            {
                currentMask = line.Substring(7);
                continue;
            }

            //  parse location strings
            int endLocationIndex = line.IndexOf(']');
            int locationLength = endLocationIndex - 4;
            string locationStr = line.Substring(4, locationLength);
            Int64 location = Int64.Parse(locationStr);

            //  get values
            Int64 value = Int64.Parse(line.Split(" = ")[1]);

            //  apply mask to location (not value)
            string binaryStr = intToBinaryString(location);
            // Console.WriteLine($"Value: {location}, string: {binaryStr}; length of string: {binaryStr.Length}");
            string maskedStr = applyPartTwoMask(currentMask, binaryStr);
            List<string> newList = new List<string>();
            generateAllPossibleBinaryStrings(newList, maskedStr);
            for (int i = 0; i < newList.Count; i++)
            {
                Int64 valueOfNewMemoryLocation = binaryStringToInt(newList[i]);
                // Console.WriteLine($"newList[i], value: {valueOfNewMemoryLocation}");
                if (finalValues.ContainsKey(valueOfNewMemoryLocation))
                {
                    // Console.WriteLine($"Overwriting memory location {valueOfNewMemoryLocation}");
                    finalValues[valueOfNewMemoryLocation] = value;
                }
                else
                {
                    finalValues.Add(valueOfNewMemoryLocation, value);
                }
            }
        }

        Int64 finalAnswer = 0;
        foreach (var entry in finalValues)
        {
            finalAnswer += entry.Value;
        }
        Console.WriteLine($"Final sum is {finalAnswer}");
    }

    private Int64 binaryStringToInt(string str)
    {
        Int64 result = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '1')
            {
                result += Convert.ToInt64(Math.Pow(2, str.Length - 1 - i));
            }
        }
        return result;
    }

    private string intToBinaryString(Int64 val)
    {
        return Convert.ToString(val, 2);
    }

    private string applyMask(string mask, string binaryStr)
    {
        //  add leading zeroes to string if necessary
        //  all masks should be 36 char long
        if (binaryStr.Length < mask.Length)
        {
            string fullStr = "";
            for (int i = 0; i < mask.Length - binaryStr.Length; i++)
            {
                fullStr += "0";
            }
            fullStr += binaryStr;
            binaryStr = fullStr;
        }

        //  this shouldn't happen, based on puzzle input
        if (binaryStr.Length != mask.Length)
        {
            Console.WriteLine("Error, binary string too long for mask");
        }

        char[] binaryCharArray = binaryStr.ToCharArray();
        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i] == 'X') { continue; }
            binaryCharArray[i] = mask[i];
        }
        string charStr = new string(binaryCharArray);
        binaryStr = charStr;
        return binaryStr;
    }

    private string applyPartTwoMask(string mask, string binaryStr)
    {
        //  add leading zeroes to string if necessary
        //  all masks should be 36 char long
        if (binaryStr.Length < mask.Length)
        {
            string fullStr = "";
            for (int i = 0; i < mask.Length - binaryStr.Length; i++)
            {
                fullStr += "0";
            }
            fullStr += binaryStr;
            binaryStr = fullStr;
        }

        //  this shouldn't happen, based on puzzle input
        if (binaryStr.Length != mask.Length)
        {
            Console.WriteLine("Error, binary string too long for mask");
        }

        //  substitute with new rules for 0s, 1s, and Xs
        char[] binaryCharArray = binaryStr.ToCharArray();
        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i] == '0') { continue; }
            else if (mask[i] == '1')
            {
                binaryCharArray[i] = '1';
            }
            else
            {
                binaryCharArray[i] = 'X';
            }
        }
        string charStr = new string(binaryCharArray);
        binaryStr = charStr;
        return binaryStr;
    }

    private void generateAllPossibleBinaryStrings(List<string> startingList, string binaryStr)
    {
        if (!binaryStr.Contains('X'))
        {
            startingList.Add(binaryStr);
        }
        else
        {
            //  create the two possibilities
            int nextXIndex = binaryStr.IndexOf('X');
            char[] possibility1 = binaryStr.ToCharArray();
            char[] possibility2 = binaryStr.ToCharArray();
            possibility1[nextXIndex] = '1';
            possibility2[nextXIndex] = '0';
            string possibility1Str = new string(possibility1);
            string possibility2Str = new string(possibility2);
            generateAllPossibleBinaryStrings(startingList, possibility1Str);
            generateAllPossibleBinaryStrings(startingList, possibility2Str);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;

class Day23
{

    //  952316487
    // int[] data1 = { 9, 5, 2, 3, 1, 6, 4, 8, 7 }; // my data

    int[] data1 = { 3, 8, 9, 1, 2, 5, 4, 6, 7 }; // test data
    int[] data2 = new int[9];

    int[] currentArray = new int[9];
    int[] targetArray = new int[9];

    const int INT_SIZE = 4;

    string logFile = "./log.txt";
    int currentCupValue = 0;
    public Day23()
    {
    }

    public void calculate()
    {
        currentCupValue = data1[0];
        Console.WriteLine("Hello, welcome to Day 23");
        // doOneHundredMoves();
        doTenMillionMoves();
    }

    private void doOneHundredMoves()
    {
        for (int i = 0; i < 100; i++)
        {
            doAMove();
        }
        rearrangeForTargetCupOnLeft(1);
        printArray("Final answer:", data1);
    }

    private void doTenMillionMoves()
    {
        int NUM_CUPS = 9;
        int NUM_MOVES = 100;
        //  populate new array
        int[] allTheCups = new int[NUM_CUPS];
        int newCupVal = data1.Length + 1; // assuming all sequential numbers are included in original (they are)
        for (int i = 0; i < data1.Length; i++)
        {
            allTheCups[i] = data1[i];
        }
        for (int i = data1.Length; i < allTheCups.Length; i++)
        {
            allTheCups[i] = newCupVal;
            newCupVal++;
        }

        //  substitute new array for data
        data1 = null;
        data1 = allTheCups;
        data2 = null;
        data2 = new int[NUM_CUPS];

        //  data1 and data2 should be the same
        data1.CopyTo(data2, 0);

        //  currentArray and targetArray will flip back and forth as to which ones they're referencing
        currentArray = null;
        targetArray = null;
        currentArray = data1;
        targetArray = data2;

        //  then do the same as before
        Console.WriteLine(DateTime.Now);
        // File.AppendAllText(logFile, DateTime.Now.ToString());
        for (int i = 0; i < NUM_MOVES; i++)
        {
            Console.WriteLine($"Move {i + 1}------");
            // File.AppendAllText(logFile, "Move " + (i + 1).ToString() + "-------\n");
            doAMove();
            // rearrangeForTargetCupOnLeft(1);
            // printArrayHead("rearranged for 1", data);
        }
        Console.WriteLine(DateTime.Now);
        // File.AppendAllText(logFile, DateTime.Now.ToString());
        rearrangeForTargetCupOnLeft(1);
        int limit = Math.Min(data1.Length, 20);
        for (int i = 0; i < limit; i++)
        {
            Console.Write($"{data1[i]}, ");
        }
        Console.WriteLine();
        Console.WriteLine(DateTime.Now);

    }

    private void doAMove()
    {
        //  find current cup
        rearrangeForTargetCupOnLeft(currentCupValue);
        //  make a copy of the data
        currentArray.CopyTo(targetArray, 0);
        int destValue = currentCupValue - 1;
        //  find destination cup
        while (true)
        {
            bool foundDest = true;
            //  try subtracting one (loop around if go below 1)
            destValue = destValue < 1 ? destValue + data1.Length : destValue;
            //  check against three cups just lifted
            for (int i = 1; i < 4; i++)
            {
                if (data1[i] == destValue)
                {
                    destValue--;
                    foundDest = false;
                    break;
                }
            }
            if (foundDest)
            {
                break;

            }
        }
        printArray("data after rearranging (starting with this)", currentArray);
        //  find where the destination cup is
        int destIndex = Array.IndexOf(currentArray, destValue);
        // Console.WriteLine($"Destination value: {destValue}, index: {destIndex}");

        //  put the cups back down up to and including the destination index
        for (int i = 4; i < destIndex + 1; i++)
        {
            // Console.WriteLine($"First, place {data[i]} at tempData spot {i - 3}");
            targetArray[i - 3] = currentArray[i];
        }
        //  place the three cups
        for (int i = 1; i < 4; i++)
        {
            // Console.WriteLine($"Second, place {data[i]} at tempData spot {destIndex - 3 + i}");
            targetArray[destIndex - 3 + i] = currentArray[i];
        }
        // place the remaining cups, which will be the same
        // Buffer.BlockCopy(currentArray, (destIndex + 1) * INT_SIZE, targetArray, (destIndex + 1) * INT_SIZE, (currentArray.Length - destIndex - 1) * INT_SIZE);
        for (int i = destIndex + 1; i < currentArray.Length; i++)
        {
            // Console.WriteLine($"Finall, place {data[i]} at tempData spot {i}");
            targetArray[i] = currentArray[i];
        }
        //  copy our data back to the main array

        //  switch the arrays
        if (currentArray == data1)
        {
            currentArray = data2;
            targetArray = data1;
        }
        else
        {
            currentArray = data1;
            targetArray = data2;
        }



        // data2.CopyTo(data1, 0);
        currentCupValue = currentArray[1];
        printArray("data after move", currentArray);
    }

    private void rearrangeForTargetCupOnLeft(int targetCupVal)
    {
        int targetCupPos = Array.IndexOf(currentArray, targetCupVal);
        int lengthToEnd = currentArray.Length - targetCupPos;
        //  rearrange so current cup is at the left
        Buffer.BlockCopy(currentArray, targetCupPos * INT_SIZE, targetArray, 0, lengthToEnd * INT_SIZE);
        Buffer.BlockCopy(currentArray, 0, targetArray, lengthToEnd * INT_SIZE, (currentArray.Length - lengthToEnd) * INT_SIZE);
        if (currentArray == data1)
        {
            currentArray = data2;
            targetArray = data1;
        }
        else
        {
            currentArray = data1;
            targetArray = data2;
        }
    }

    private void printArray(string name, int[] array)
    {
        // string stringText = "";
        Console.WriteLine(name);
        foreach (var val in array)
        {
            Console.Write($"{val} ");
            // stringText += val.ToString() + " ";
        }
        Console.WriteLine();
        // File.AppendAllText(logFile, name + "\n");
        // File.AppendAllText(logFile, stringText + "\n");
    }

    private void printArrayHead(string name, int[] array)
    {
        int limit = Math.Min(20, array.Length);
        Console.WriteLine(name);
        for (int i = 0; i < limit; i++)
        {
            Console.Write($"{array[i]}, ");
        }
        Console.WriteLine();
    }
}
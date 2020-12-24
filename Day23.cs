using System;
using System.Collections.Generic;
using System.IO;

class Day23
{

    //  952316487
    int[] data1 = { 9, 5, 2, 3, 1, 6, 4, 8, 7 }; // my data

    // int[] data1 = { 3, 8, 9, 1, 2, 5, 4, 6, 7 }; // test data
    int[] data2 = new int[9];

    int[] currentArray = new int[9];
    int[] targetArray = new int[9];

    const int INT_SIZE = 4;

    // string logFile = "./log.txt";
    int currentCupValue = 0;
    public Day23()
    {
    }

    public void calculate()
    {
        currentCupValue = data1[0];
        Console.WriteLine("Hello, welcome to Day 23");
        playTheGame();
    }

    private void playTheGame()
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
            if (i % 2000 == 0)
            {
                Console.WriteLine($"Move {i + 1}------");
            }
            // File.AppendAllText(logFile, "Move " + (i + 1).ToString() + "-------\n");
            doAMove();
        }
        Console.WriteLine(DateTime.Now);
        // File.AppendAllText(logFile, DateTime.Now.ToString());
        rearrangeForTargetCupOnLeft(1);
        int limit = Math.Min(data1.Length, 20);
        for (int i = 0; i < limit; i++)
        {
            Console.Write($"{currentArray[i]}, ");
        }
        Console.WriteLine();
        Console.WriteLine(DateTime.Now);

    }

    private void doAMove()
    {
        //  put current cup in left-most position
        rearrangeForTargetCupOnLeft(currentCupValue);
        //  make a copy of the data
        Buffer.BlockCopy(currentArray, 0, targetArray, 0, currentArray.Length * INT_SIZE);
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
        // printArray("data after rearranging (starting with this)", currentArray);
        //  find where the destination cup is in the array
        int destIndex = Array.IndexOf(currentArray, destValue);

        //  put the cups back down up to and including the destination index
        Buffer.BlockCopy(currentArray, 4 * INT_SIZE, targetArray, INT_SIZE, (destIndex - 3) * INT_SIZE);
        //  place the three cups
        for (int i = 1; i < 4; i++)
        {
            targetArray[destIndex - 3 + i] = currentArray[i];
        }
        // remaining cups are all the same

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

        currentCupValue = currentArray[1];
        // printArray("data after move", currentArray);
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
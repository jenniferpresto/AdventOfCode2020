using System;
using System.Collections.Generic;

class Day17
{
    private List<string> data;

    public Day17(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        doPartOne();
    }

    //*****************************************
    //  Part One
    //*****************************************
    private void doPartOne()
    {
        int numCycles = 6;
        int initialDimension = data[0].Length;
        //  create two arrays of identical dimensions
        int[,,] dataArray = new int[initialDimension + (numCycles * 2), initialDimension + (numCycles * 2), initialDimension + (numCycles * 2)];
        int[,,] calculatedDataArray = new int[initialDimension + (numCycles * 2), initialDimension + (numCycles * 2), initialDimension + (numCycles * 2)];

        //  initialize all values to 0
        initializeArrayToZeroes(dataArray);
        initializeArrayToZeroes(calculatedDataArray);

        //  fill initial calculated values with input data
        // initializeInputData(dataArray);
        initializeInputData(calculatedDataArray);

        //  each time we run the cycle, we need to scootch everything over by one on each dimension
        for (int i = 0; i < numCycles; i++)
        {
            scootchCalculatedArrayOverByOne(calculatedDataArray, dataArray);
            calculateNextArray(dataArray, calculatedDataArray);
        }
        // printArray(calculatedDataArray);

        int answer = 0;
        foreach (int val in calculatedDataArray)
        {
            answer += val;
        }

        Console.WriteLine($"answer is {answer}");

    }

    private void initializeArrayToZeroes(int[,,] dataArray)
    {
        for (int x = 0; x < dataArray.GetLength(0); x++)
        {
            for (int y = 0; y < dataArray.GetLength(1); y++)
            {
                for (int z = 0; z < dataArray.GetLength(2); z++)
                {
                    dataArray[x, y, z] = 0;
                }
            }
        }
    }

    private void initializeInputData(int[,,] dataArray)
    {
        //  initialize based on input
        for (int y = 0; y < data[0].Length; y++)
        {
            for (int x = 0; x < data[0].Length; x++)
            {
                if (data[y][x] == '#')
                {
                    dataArray[x, y, 0] = 1;
                }
                else
                {
                    dataArray[x, y, 0] = 0;
                }
            }
        }
    }


    private void calculateNextArray(int[,,] dataArray, int[,,] calculatedDataArray)
    {
        for (int x = 0; x < dataArray.GetLength(0); x++)
        {
            for (int y = 0; y < dataArray.GetLength(1); y++)
            {
                for (int z = 0; z < dataArray.GetLength(2); z++)
                {
                    if (testSpecificCoordinate(x, y, z, dataArray))
                    {
                        calculatedDataArray[x, y, z] = 1;
                    }
                    else
                    {
                        calculatedDataArray[x, y, z] = 0;
                    }
                }
            }
        }
    }

    private bool testSpecificCoordinate(int x, int y, int z, int[,,] dataArray)
    {
        bool active = false;
        int totalActiveNeighbors = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            if (i < 0 || i > dataArray.GetLength(0) - 1) { continue; }
            for (int j = y - 1; j < y + 2; j++)
            {
                if (j < 0 || j > dataArray.GetLength(1) - 1) { continue; }
                for (int k = z - 1; k < z + 2; k++)
                {
                    if (k < 0 || k > dataArray.GetLength(2) - 1) { continue; }
                    if (dataArray[i, j, k] == 1)
                    {
                        if (x == i && y == j && z == k) { continue; }
                        totalActiveNeighbors++;
                    }
                }
            }
        }
        if (dataArray[x, y, z] == 1)
        {
            // Console.WriteLine($"Testing active coordinate at {x}, {y}, {z}, total active neighbors: {totalActiveNeighbors}");
            if (totalActiveNeighbors == 2 || totalActiveNeighbors == 3)
            {
                active = true;
                // Console.WriteLine($"Keeping active active: {x}, {y}, {z}");
                // Console.ReadLine();
            }
        }
        else
        {
            if (totalActiveNeighbors == 3)
            {
                active = true;
                // Console.WriteLine($"Changing inactive to active: {x}, {y}, {z}");
                // Console.ReadLine();
            }
        }
        // if (z == 2 || z == 3 || z == 4)
        // {
        //     Console.WriteLine($"Testing coordinate {x}, {y}, {z}, currently {dataArray[x, y, z]}; total neighbors: {totalActiveNeighbors}; changing to {active}");
        // }
        return active;
    }

    private void scootchCalculatedArrayOverByOne(int[,,] calculatedArray, int[,,] originalArray)
    {
        //  TODO: can optimize by stopping at cycle number
        int sideOfCube = calculatedArray.GetLength(0);
        for (int z = 0; z < sideOfCube; z++)
        {
            for (int y = 0; y < sideOfCube; y++)
            {
                for (int x = 0; x < sideOfCube; x++)
                {
                    int newVal = 0;
                    if (x == 0 || y == 0 || z == 0)
                    {
                        newVal = 0;
                    }
                    else
                    {
                        newVal = calculatedArray[x - 1, y - 1, z - 1];
                    }
                    originalArray[x, y, z] = newVal;
                }
            }
        }
    }

    private void printArray(int[,,] dataArray)
    {
        decimal middleDec = Math.Floor(Convert.ToDecimal(dataArray.GetLength(0) / 2));
        int middleIndex = Convert.ToInt32(middleDec);
        int intialDataLength = data[0].Length;
        for (int z = 0; z < dataArray.GetLength(2); z++)
        {
            // if (z < 2 || z > 4) continue;
            Console.WriteLine($"z = {z}");
            for (int y = 0; y < dataArray.GetLength(1); y++)
            {
                for (int x = 0; x < dataArray.GetLength(0); x++)
                {
                    Console.Write(dataArray[x, y, z]);
                }
                Console.WriteLine();
            }
        }
    }
}
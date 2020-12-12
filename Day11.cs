using System;
using System.Linq;
using System.Collections.Generic;

class Day11
{
    private List<string> data;
    private int numRows;
    private int numCols;

    public Day11(List<string> dataList)
    {
        data = dataList;
        numRows = dataList.Count;
        numCols = dataList[0].Length;
    }

    public void calculate()
    {
        Console.WriteLine($"{numRows} rows and {numCols} columns");
        doPartOne();
        doPartTwo();
    }

    private void doPartOne()
    {
        //  create 2D char array
        char[,] seatingChart = createSeatingChartFromData();

        while (true)
        {
            char[,] newChart = doSeatSubstitution(seatingChart, 4, testSeatPt1);
            // printSeatingChart(newChart);
            if (seatingChartsAreSame(newChart, seatingChart))
            {
                Console.WriteLine("They're the same!");
                break;
            }
            seatingChart = newChart;
        }
        int numOccupied = 0;
        foreach (var seat in seatingChart)
        {
            if (seat == '#')
            {
                numOccupied++;
            }
        }
        Console.WriteLine($"Answer is {numOccupied}");
    }

    private void doPartTwo()
    {
        char[,] seatingChart = createSeatingChartFromData();
        // printSeatingChart(seatingChart);
        while (true)
        {
            char[,] newChart = doSeatSubstitution(seatingChart, 5, testSeatPt2);
            // printSeatingChart(newChart);
            if (seatingChartsAreSame(newChart, seatingChart))
            {
                Console.WriteLine("Same!");
                break;
            }
            seatingChart = newChart;
            // Console.ReadLine();
        }
        int numOccupied = 0;
        foreach (var seat in seatingChart)
        {
            if (seat == '#')
            {
                numOccupied++;
            }
        }
        Console.WriteLine($"Answer is {numOccupied}");
    }

    private char[,] createSeatingChartFromData()
    {
        char[,] seatingChart = new char[numRows, numCols];
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                seatingChart[i, j] = data[i][j];
            }
        }
        return seatingChart;
    }

    private char[,] doSeatSubstitution(char[,] oldData, int seatTolerance, Func<char[,], int, int, int> testMethod)
    {
        //  make copy of latest version of the chart
        char[,] newData = oldData.Clone() as char[,];
        //  seat substitution
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (oldData[i, j] == '.') { continue; }
                int numAdj = testMethod(oldData, i, j);
                if (numAdj == 0 && oldData[i, j] == 'L')
                {
                    newData[i, j] = '#';
                }
                else if (numAdj > seatTolerance - 1 && oldData[i, j] == '#')
                {
                    newData[i, j] = 'L';
                }
            }
        }
        return newData;
    }

    private int testSeatPt1(char[,] oldChart, int row, int col)
    {
        int numOccupiedAdjacent = 0;
        for (int i = row - 1; i < row + 2; i++)
        {
            for (int j = col - 1; j < col + 2; j++)
            {
                // Console.Write($"[{row}, {col}]");
                if (i < 0 || i > numRows - 1)
                {
                    continue;
                }
                if (j < 0 || j > numCols - 1)
                {
                    continue;
                }
                if (i == row && j == col)
                {
                    continue;
                }
                if (oldChart[i, j] == '#')
                {
                    numOccupiedAdjacent++;
                }
            }
            // Console.Write("\n");
        }
        return numOccupiedAdjacent;
    }

    private int testSeatPt2(char[,] oldChart, int row, int col)
    {
        int numOccupied = 0;
        // test all eight directions
        if (directionIsOccupied(oldChart, row, col, 1, 0)) { numOccupied++; } // north
        if (directionIsOccupied(oldChart, row, col, -1, 0)) { numOccupied++; } // south
        if (directionIsOccupied(oldChart, row, col, 0, 1)) { numOccupied++; } // east
        if (directionIsOccupied(oldChart, row, col, 0, -1)) { numOccupied++; } // west
        if (directionIsOccupied(oldChart, row, col, 1, 1)) { numOccupied++; } // northeast
        if (directionIsOccupied(oldChart, row, col, 1, -1)) { numOccupied++; } // northwest
        if (directionIsOccupied(oldChart, row, col, -1, 1)) { numOccupied++; } // southeast
        if (directionIsOccupied(oldChart, row, col, -1, -1)) { numOccupied++; } // southwest
        return numOccupied;
    }

    private bool directionIsOccupied(char[,] data, int row, int col, int northsouth, int eastwest)
    {
        int testRow = row;
        int testCol = col;
        while (
            testRow != -1 &&
            testRow != numRows &&
            testCol != -1 &&
            testCol != numCols
            )
        {
            testRow += northsouth;
            testCol += eastwest;
            if (testRow < 0 || testRow > numRows - 1 || testCol < 0 || testCol > numCols - 1) return false;
            if (data[testRow, testCol] == '#')
            {
                return true;
            }
            else if (data[testRow, testCol] == 'L')
            {
                return false;
            }
        }
        return false;
    }

    //  Compare seating charts
    private bool seatingChartsAreSame(char[,] chartOne, char[,] chartTwo)
    {
        return chartOne.Rank == chartTwo.Rank &&
        Enumerable.Range(0, chartOne.Rank).All(dimension => chartOne.GetLength(dimension) == chartOne.GetLength(dimension)) &&
        chartOne.Cast<char>().SequenceEqual(chartTwo.Cast<char>());
    }

    //  For debugging
    private void printSeatingChart(char[,] chart)
    {
        Console.WriteLine("*****************");
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                Console.Write($"{chart[i, j]}");
            }
            Console.Write("\n");
        }
        Console.WriteLine("*****************");
    }
}
using System;
using System.Collections.Generic;

class Day12
{
    private List<string> data;

    public Day12(List<string> dataList)
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
        int[] position = new int[] { 0, 0 }; // x = position[0], y = position[1]
        char[] directions = new char[] { 'N', 'E', 'S', 'W' };
        int currentlyFacing = 1;
        foreach (string line in data)
        {
            Console.WriteLine("****** " + line);
            (char instruction, int value) step = parseLine(line);
            if (step.instruction == 'N' || step.instruction == 'E' || step.instruction == 'S' || step.instruction == 'W')
            {
                goDirection(position, step.instruction, step.value);
            }
            else if (step.instruction == 'F')
            {
                goDirection(position, directions[currentlyFacing], step.value);
            }
            else if (step.instruction == 'R')
            {
                int clicksRight = step.value / 90;
                currentlyFacing = (currentlyFacing + clicksRight) % 4;
            }
            else if (step.instruction == 'L')
            {
                int clicksLeft = step.value / 90;
                currentlyFacing -= clicksLeft;
                while (currentlyFacing < 0)
                {
                    currentlyFacing += 4;
                }
            }
            // Console.WriteLine($"{position[0]}, {position[1]}");
        }
        Console.WriteLine($"Answer to part one: {Math.Abs(position[0]) + Math.Abs(position[1])}");
    }

    private void goDirection(int[] pos, char dir, int value)
    {
        if (dir == 'N')
        {
            pos[1] -= value;
        }
        else if (dir == 'E')
        {
            pos[0] += value;
        }
        else if (dir == 'S')
        {
            pos[1] += value;
        }
        else if (dir == 'W')
        {
            pos[0] -= value;
        }
    }

    private void doPartTwo()
    {
        int[] shipPos = new int[] { 0, 0 }; // x = position[0], y = position[1]
        int[] waypointRelPos = new int[] { 10, -1 };
        char[] directions = new char[] { 'N', 'E', 'S', 'W' };
        int currentlyFacing = 1;

        foreach (string line in data)
        {
            (char instruction, int value) step = parseLine(line);
            if (step.instruction == 'N' || step.instruction == 'E' || step.instruction == 'S' || step.instruction == 'W')
            {
                goDirection(waypointRelPos, step.instruction, step.value);
            }
            else if (step.instruction == 'R')
            {
                int clicksRight = step.value / 90;
                moveWaypointNumClicksRight(waypointRelPos, clicksRight);
            }
            else if (step.instruction == 'L')
            {
                int clicksRight = -step.value / 90;
                while (clicksRight < 0)
                {
                    clicksRight += 4;
                }
                moveWaypointNumClicksRight(waypointRelPos, clicksRight);
            }
            else if (step.instruction == 'F')
            {
                int forX = waypointRelPos[0] * step.value;
                int forY = waypointRelPos[1] * step.value;
                shipPos[0] += forX;
                shipPos[1] += forY;
            }
            // Console.WriteLine($"Ship: {shipPos[0]}, {shipPos[1]}, waypoint: {waypointRelPos[0]}, {waypointRelPos[1]}");
        }
        Console.WriteLine($"Answer to part two: {Math.Abs(shipPos[0]) + Math.Abs(shipPos[1])}");
    }

    private void moveWaypointNumClicksRight(int[] waypointPos, int numClicks)
    {
        if (numClicks % 4 == 0)
        {
            return;
        }
        else if (numClicks % 4 == 1)
        {
            int newX = waypointPos[1] * -1;
            int newY = waypointPos[0];
            waypointPos[0] = newX;
            waypointPos[1] = newY;
        }
        else if (numClicks % 4 == 2)
        {
            int newX = waypointPos[0] * -1;
            int newY = waypointPos[1] * -1;
            waypointPos[0] = newX;
            waypointPos[1] = newY;
        }
        else if (numClicks % 4 == 3)
        {
            int newX = waypointPos[1];
            int newY = waypointPos[0] * -1;
            waypointPos[0] = newX;
            waypointPos[1] = newY;
        }
    }

    private (char instruction, int value) parseLine(string line)
    {
        char inst = line[0];
        string valStr = line.Substring(1);
        int val = int.Parse(valStr);
        return (inst, val);
    }

}
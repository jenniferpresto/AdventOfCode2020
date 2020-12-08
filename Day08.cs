using System;
using System.Collections.Generic;

class Day08
{
    private List<string> data;

    public Day08(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        Console.WriteLine($"Instructions: {data.Count}");
        HashSet<int> executedInstructions = doPartOne(data, true);
        doPartTwo(executedInstructions);
    }

    private HashSet<int> doPartOne(List<string> dataToTest, bool writeInfiniteLoopResults)
    {
        HashSet<int> executedInstructions = new HashSet<int>();
        int total = 0;
        int currentIndex = 0;
        while (!executedInstructions.Contains(currentIndex))
        {
            if (currentIndex == dataToTest.Count - 1)
            {
                Console.WriteLine($"End of program reached at index {currentIndex}: {total}");
                break;
            }
            executedInstructions.Add(currentIndex);
            (int totalChange, int indexChange) = executeInstruction(dataToTest[currentIndex]);
            currentIndex += indexChange;
            total += totalChange;
        }
        if (writeInfiniteLoopResults)
        {
            Console.WriteLine($"While loop completed. Total: {total}");
            Console.WriteLine($"Number of attempted instructions: {executedInstructions.Count}");
        }
        return executedInstructions;
    }
    private (int newTotal, int newIndex) executeInstruction(string instruction)
    {
        string[] parsed = instruction.Split(" ");
        int value = parsed[1][0] == '+' ? int.Parse(parsed[1].Substring(1)) : int.Parse(parsed[1].Substring(1)) * -1;
        switch (parsed[0])
        {
            case "acc":
                return (value, 1);
            case "jmp":
                return (0, value);
            case "nop":
                return (0, 1);
            default:
                Console.WriteLine($"Error with instruction: {instruction}");
                break;
        }
        return (0, 0);
    }

    private void doPartTwo(HashSet<int> problemInstructions)
    {
        foreach (int index in problemInstructions)
        {
            if (data[index].Split(" ")[0] == "acc") { continue; }
            List<string> testInstructions = new List<string>(data);
            if (testInstructions[index].Substring(0, 3) == "nop")
            {
                testInstructions[index] = testInstructions[index].Replace("nop", "jmp");
            }
            else
            {
                testInstructions[index] = testInstructions[index].Replace("jmp", "nop");
            }
            doPartOne(testInstructions, false);
        }
    }
}
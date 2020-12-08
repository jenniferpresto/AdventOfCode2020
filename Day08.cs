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
        HashSet<int> executedInstructions = doPartOne(data);
        doPartTwo(executedInstructions);
    }

    private HashSet<int> doPartOne(List<string> dataToTest)
    {
        HashSet<int> executedInstructions = new HashSet<int>();
        int total = 0;
        int currentIndex = 0;
        while (!executedInstructions.Contains(currentIndex))
        {
            executedInstructions.Add(currentIndex);
            (int newTotal, int newIndex) = executeInstruction(dataToTest, total, currentIndex);
            currentIndex = newIndex;
            total = newTotal;
        }
        Console.WriteLine($"Infinite loop found: {total}");
        Console.WriteLine($"Number of attempted instructions: {executedInstructions.Count}");
        return executedInstructions;
    }
    private (int newTotal, int newIndex) executeInstruction(List<string> dataToTest, int total, int index)
    {
        if (index >= dataToTest.Count - 1)
        {
            Console.WriteLine("************************************");
            Console.WriteLine($"End of program reached: {total}, index: {index}");
            return (0, 0);
        }
        string[] instruction = dataToTest[index].Split(" ");
        int value = instruction[1][0] == '+' ? int.Parse(instruction[1].Substring(1)) : int.Parse(instruction[1].Substring(1)) * -1;
        switch (instruction[0])
        {
            case "acc":
                return (total + value, index + 1);
            case "jmp":
                return (total, index + value);
            case "nop":
                return (total, index + 1);
            default:
                Console.WriteLine($"Error with instruction, line {index}, prob value: {instruction[0]}");
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
            Console.WriteLine($"Changing index {index}");
            if (testInstructions[index].Substring(0, 3) == "nop")
            {
                testInstructions[index] = testInstructions[index].Replace("nop", "jmp");
                Console.WriteLine($"Changing test instr repl with jmp {index}: {testInstructions[index]}");
            }
            else
            {
                testInstructions[index] = testInstructions[index].Replace("jmp", "nop");
                Console.WriteLine($"Changing test instr repl with nop {index}: {testInstructions[index]}");
            }
            doPartOne(testInstructions);
        }
    }
}
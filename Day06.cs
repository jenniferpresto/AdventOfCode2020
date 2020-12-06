using System;
using System.Collections.Generic;

class Day06
{
    private List<string> data;

    public Day06(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        calculatePartOne();
        calculatePartTwo();
    }

    private void calculatePartOne()
    {
        int totalCount = 0;
        HashSet<char> group = new HashSet<char>();
        foreach (string line in data)
        {
            if (line.Length == 0)
            {
                totalCount += group.Count;
                group.Clear();
            }
            else
            {
                foreach (char c in line)
                {
                    group.Add(c);
                }
            }
        }
        //  grab the last one
        if (group.Count > 0)
        {
            totalCount += group.Count;
        }

        Console.WriteLine($"Final answer: {totalCount}");
    }

    private void calculatePartTwo()
    {
        int totalAnswers = 0;
        List<string> group = new List<string>();
        foreach (string line in data)
        {
            if (line.Length == 0)
            {
                totalAnswers += getTotalForGroup(group);
                group.Clear();
            }
            else
            {
                group.Add(line);
            }
        }
        //  grab the last one
        if (group.Count > 0)
        {
            totalAnswers += getTotalForGroup(group);
        }
        Console.WriteLine($"Total answers: {totalAnswers}");
    }

    private int getTotalForGroup(List<string> group)
    {
        if (group.Count == 0) return 0;
        int count = 0;
        foreach (char answer in group[0])
        {
            bool allAnswer = true;
            for (int i = 1; i < group.Count; i++)
            {
                if (group[i].Contains(answer))
                {
                    continue;
                }
                else
                {
                    allAnswer = false;
                    break;
                }
            }
            if (allAnswer)
            {
                count++;
            }
        }
        // Console.WriteLine($"Total for group: {count}");
        return count;
    }
}
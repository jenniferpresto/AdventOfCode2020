using System;
using System.Collections.Generic;

namespace AdventCode
{
    class Day01
    {
        private List<int> data;

        public Day01(List<int> dataList)
        {
            data = dataList;
        }

        public void calculate()
        {
            Console.WriteLine($"This many data points: {data.Count}");
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    if (data[i] + data[j] == 2020)
                    {
                        Console.WriteLine($"Found it!: {data[i]} + {data[j]} at points {i} and {j}");
                        Console.WriteLine($"Product is: {data[i] * data[j]}");
                    }
                    for (int k = j + 1; k < data.Count; k++)
                    {
                        if (data[i] + data[j] + data[k] == 2020)
                        {
                            Console.WriteLine($"Found it!: {data[i]} + {data[j]} + {data[k]} at points {i}, {j}, and {k}");
                            Console.WriteLine($"Product is: {data[i] * data[j] * data[k]}");
                        }
                    }
                }
            }
        }
    }

    class Day02
    {
        private List<string> data;
        public Day02(List<string> dataList)
        {
            data = dataList;
        }

        public void calculate()
        {
            Console.WriteLine($"{data.Count} data points");
            int numValidPasswords = 0;
            int csvLine = 1; // track where it is for spot-checking CSV file

            foreach (string password in data)
            {
                //  parse out the substrings
                int hyphenPos = password.IndexOf("-");
                int spacePos = password.IndexOf(" ");
                string minSubstring = password.Substring(0, hyphenPos);
                int minNum = int.Parse(minSubstring);
                string maxSubstring = password.Substring(hyphenPos + 1, spacePos - hyphenPos);
                int maxNum = int.Parse(maxSubstring);
                string keySubstring = password.Substring(spacePos + 1, 1);
                string passwordSubstring = password.Substring(spacePos + 4);

                int numMatches = 0;
                //  first password policy
                //  iterate through password substring and check for matches
                // foreach (char c in passwordSubstring) {
                //     if (c == char.Parse(keySubstring)) {
                //         numMatches++;
                //     }
                // }

                // if (numMatches >= minNum && numMatches <= maxNum) {
                //     Console.WriteLine($"Valid password on line {csvLine}");
                //     numValidPasswords++;
                // }

                //  second password policy
                if (passwordSubstring.Length < maxNum)
                {
                    Console.WriteLine("Error: password too short");
                    continue;
                }

                if (maxNum < 1)
                {
                    Console.WriteLine("Starting position less than one");
                    continue;
                }

                if (passwordSubstring[minNum - 1] == char.Parse(keySubstring))
                {
                    numMatches++;
                }
                if (passwordSubstring[maxNum - 1] == char.Parse(keySubstring))
                {
                    numMatches++;
                }

                if (numMatches == 1)
                {
                    // Console.WriteLine($"Valid password on line {csvLine}");
                    numValidPasswords++;
                }

                csvLine++;
            }
            Console.WriteLine($"Total valid passwords: {numValidPasswords}");
        }
    }

    class Day03
    {
        private List<string> data;

        public Day03(List<string> dataList)
        {
            data = dataList;
        }

        public void calculate()
        {
            Console.WriteLine($"{data.Count} data points");
            uint slope1 = goDownSlope(1, 1);
            uint slope2 = goDownSlope(3, 1);
            uint slope3 = goDownSlope(5, 1);
            uint slope4 = goDownSlope(7, 1);
            uint slope5 = goDownSlope(1, 2);

            Console.WriteLine($"{slope1}, {slope2}, {slope3}, {slope4}, {slope5}");

            uint product = slope1 * slope2 * slope3 * slope4 * slope5;
            Console.WriteLine($"Final product: {product}");
        }

        private uint goDownSlope(int right, int down)
        {
            int csvLine = 0;
            int charIndex = right;
            int lineIndex = down;
            uint numTrees = 0;
            while (lineIndex < data.Count)
            {
                string pattern = data[lineIndex];
                if (pattern[charIndex] == '#')
                {
                    numTrees++;
                }

                //  increment indices
                lineIndex += down;
                charIndex += right;

                if (charIndex > pattern.Length - 1)
                {
                    int priorIndex = charIndex;
                    charIndex = priorIndex - pattern.Length;
                }
                csvLine++;
            }
            // Console.WriteLine($"Total trees: {numTrees}");
            return numTrees;
        }
    }
}


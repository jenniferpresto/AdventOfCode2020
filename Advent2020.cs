using System;
using System.IO;
using System.Collections.Generic;

namespace AdventCode
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> data1 = ReadIntData("./data/adventData_01.csv");
            var day1 = new Day01(data1);
            day1.calculate();

            List<string> data2 = ReadStringData("./data/adventData_02.csv");
            var day2 = new Day02(data2);
            day2.calculate();

            List<string> data3 = ReadStringData("./data/adventData_03.csv");
            var day3 = new Day03(data3);
            day3.calculate();

            Console.Write("\nDone");
        }

        static List<int> ReadIntData(string pathname)
        {
            var reader = new StreamReader(pathname);
            using (reader)
            {
                List<int> numbers = new List<int>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    int number = int.Parse(line);
                    numbers.Add(number);
                }
                return numbers;
            }
        }

        static List<string> ReadStringData(string pathname)
        {
            var reader = new StreamReader(pathname);
            List<string> lines = new List<string>();
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    lines.Add(line);
                }
            }
            return lines;
        }
    }
}

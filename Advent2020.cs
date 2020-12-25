using System;
using System.IO;
using System.Collections.Generic;

namespace AdventCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // List<int> data1 = ReadIntData("./data/adventData_01.csv");
            // var day1 = new Day01(data1);
            // day1.calculate();

            // List<string> data2 = ReadStringData("./data/adventData_02.csv");
            // var day2 = new Day02(data2);
            // day2.calculate();

            // List<string> data3 = ReadStringData("./data/adventData_03.csv");
            // var day3 = new Day03(data3);
            // day3.calculate();

            // List<string> data4 = ReadStringData("./data/adventData_04.txt");
            // var day4 = new Day04(data4);
            // day4.calculate();

            // List<string> data5 = ReadStringData("./data/adventData_05.txt");
            // var day5 = new Day05(data5);
            // day5.calculate();

            // List<string> data6 = ReadStringData("./data/adventData_06.txt");
            // var day6 = new Day06(data6);
            // day6.calculate();

            // List<string> data7 = ReadStringData("./data/adventData_07.txt");
            // var day7 = new Day07(data7);
            // day7.calculate();

            // List<string> data8 = ReadStringData("./data/adventData_08.txt");
            // var day8 = new Day08(data8);
            // day8.calculate();

            // List<UInt64> data9 = ReadIntData("./data/adventData_09.txt");
            // var day9 = new Day09(data9);
            // day9.calculate();

            // List<string> data10 = ReadStringData("./data/adventData_10.txt");
            // var day10 = new Day10(data10);
            // day10.calculate();

            // List<string> data11 = ReadStringData("./data/adventData_11.txt");
            // var day11 = new Day11(data11);
            // day11.calculate();

            // List<string> data12 = ReadStringData("./data/adventData_12.txt");
            // var day12 = new Day12(data12);
            // day12.calculate();

            // List<string> data13 = ReadStringData("./data/adventData  _13.txt");
            // var day13 = new Day13(data13);
            // day13.calculate();

            // List<string> data14 = ReadStringData("./data/adventData_14.txt");
            // var day14 = new Day14(data14);
            // day14.calculate();

            // List<string> data15 = ReadStringData("./data/adventData_15_test.txt");
            // var day15 = new Day15(data15);
            // day15.calculate();

            // List<string> data16 = ReadStringData("./data/adventData_16.txt");
            // var day16 = new Day16(data16);
            // day16.calculate();

            // List<string> data17 = ReadStringData("./data/adventData_17.txt");
            // var day17 = new Day17(data17);
            // day17.calculate();

            // List<string> data18 = ReadStringData("./data/adventData_18.txt");
            // var day18 = new Day18(data18);
            // day18.calculate();

            // List<string> data19 = ReadStringData("./data/adventData_19_test.txt");
            // var day19 = new Day19(data19);
            // day19.calculate();

            // List<string> data20 = ReadStringData("./data/adventData_20.txt");
            // var day20 = new Day20(data20);
            // day20.calculate();

            // List<string> data21 = ReadStringData("./data/adventData_21.txt");
            // var day21 = new Day21(data21);
            // day21.calculate();

            // List<string> data22 = ReadStringData("./data/adventData_22.txt");
            // var day22 = new Day22(data22);
            // day22.calculate();

            // var day23 = new Day23();
            // day923.calculate();

            // List<string> data24 = ReadStringData("./data/adventData_24.txt");
            // var day24 = new Day24(data24);
            // day24.calculate();

            var day25 = new Day25();
            day25.calculate();


            Console.WriteLine("\nDone");

        }

        static List<UInt64> ReadIntData(string pathname)
        {
            var reader = new StreamReader(pathname);
            using (reader)
            {
                List<UInt64> numbers = new List<UInt64>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    UInt64 number = UInt64.Parse(line);
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

using System;
using System.Collections.Generic;

class Day04
{
    private List<string> data;
    public Day04(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        var passportList = parseData();

    }

    private List<Dictionary<string, string>> parseData()
    {
        var allPassports = new List<Dictionary<string, string>>();
        int numLines = data.Count;
        int numPassports = 0;
        Console.WriteLine($"This many lines: {numLines}");

        //  create one string for each passport
        List<string> allPassportStrings = new List<string>();
        string fullPassportString = "";
        bool isFirst = true;
        for (int i = 0; i < data.Count; i++)
        {
            //  end of passport
            if (data[i].Length == 0 || i == data.Count - 1)
            {
                allPassportStrings.Add(fullPassportString);
                if (i == data.Count - 1)
                {
                    fullPassportString += " " + data[i];
                }
                Console.WriteLine($"{i}: {fullPassportString}");
                numPassports++;
                fullPassportString = "";
                isFirst = true;
            }
            //  concatnate passport into one string
            else
            {
                if (isFirst)
                {
                    fullPassportString += data[i];
                    isFirst = false;
                }
                else
                {
                    fullPassportString += " " + data[i];
                }
            }
        }

        //  parse each full string to create dictionary objects
        foreach (string passportString in allPassportStrings)
        {
            var passportObj = parsePassportString(passportString);
            allPassports.Add(passportObj);
        }
        Console.WriteLine($"This many passports: {allPassports.Count}");
        return allPassports;
    }

    private Dictionary<string, string> parsePassportString(string passportString)
    {
        var passportObj = new Dictionary<string, string>();
        bool onKey = true;
        string key = "";
        string value = "";
        for (int i = 0; i < passportString.Length; i++)
        {
            if (onKey)
            {
                if (passportString[i] == ':')
                {
                    onKey = false;
                }
                else
                {
                    key += passportString[i];
                }
            }
            else
            {
                if (passportString[i] == ' ' || i > passportString.Length)
                {
                    onKey = true;
                    passportObj.Add(key, value);
                    key = "";
                    value = "";
                }
                else
                {
                    value += passportString[i];
                }
            }
        }
        foreach (KeyValuePair<string, string> kvp in passportObj)
        {
            Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
        return passportObj;
    }
}
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
        foreach (string line in data)
        {
            if (line.Length == 0)
            {
                allPassportStrings.Add(fullPassportString);
                numPassports++;
                fullPassportString = "";
            }
            else
            {
                fullPassportString += line;
            }
        }
        //  make sure there are no lingering lines
        //  (happens if last line not empty)
        if (fullPassportString.Length > 0)
        {
            allPassportStrings.Add(fullPassportString);
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
        foreach (char c in passportString)
        {
            if (onKey)
            {
                if (c != ':')
                {
                    key += c;
                }
                else
                {
                    onKey = false;
                }
            }
            else
            {
                if (c != ' ')
                {
                    value += c;
                }
                else
                {
                    onKey = true;
                    passportObj.Add(key, value);
                    key = "";
                    value = "";
                }
            }
        }
        if (key.Length > 0 || value.Length > 0)
        {
            passportObj.Add(key, value);
        }
        return passportObj;
    }
}
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
        int numValid = 0;
        foreach (Dictionary<string, string> passport in passportList)
        {
            numValid += isPassportValid(passport) ? 1 : 0;
            if (isPassportValid(passport))
            {
                Console.WriteLine("VALID");
                if (passport.Count == 7)
                {
                    Console.WriteLine("Elf passport!!");
                }
            }
            else
            {
                Console.WriteLine("INVALID");
            }
            printPassport(passport);
        }
        Console.WriteLine($"Valid passports: {numValid}");

    }

    private List<Dictionary<string, string>> parseData()
    {
        var allPassports = new List<Dictionary<string, string>>();
        int numLines = data.Count;
        int numPassports = 0;
        Console.WriteLine($"This many lines: {numLines}");

        //  add a blank line at the end of the data
        if (data[data.Count - 1].Length > 0)
        {
            data.Add("");
        }

        //  create one string for each passport
        List<string> allPassportStrings = new List<string>();
        string fullPassportString = "";
        foreach (string line in data)
        {
            if (line.Length > 0)
            {
                fullPassportString += line + " ";
            }
            else
            {
                allPassportStrings.Add(fullPassportString);
                fullPassportString = "";
                numPassports++;
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
        foreach (char c in passportString)
        {
            if (onKey)
            {
                if (c == ':')
                {
                    onKey = false;
                }
                else
                {
                    key += c;
                }
            }
            else
            {
                if (c == ' ')
                {
                    passportObj.Add(key, value);
                    key = "";
                    value = "";
                    onKey = true;
                }
                else
                {
                    value += c;
                }
            }
        }
        return passportObj;
    }

    private bool isPassportValid(Dictionary<string, string> passportObj)
    {
        if (passportObj.Count == 8)
        {
            return true;
        }

        if (passportObj.Count == 7 && !passportObj.ContainsKey("cid"))
        {
            return true;
        }
        return false;
    }

    //  for debugging
    private void printPassport(Dictionary<string, string> passportObj)
    {
        Console.WriteLine("***********Passport");
        foreach (KeyValuePair<string, string> kvp in passportObj)
        {
            Console.WriteLine($"\tKey: {kvp.Key}, Value: {kvp.Value}");
        }
    }
}
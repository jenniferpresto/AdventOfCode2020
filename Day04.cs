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

    private bool isPassportValid(Dictionary<string, string> passport)
    {
        //  validity for part one
        bool partOneValid = (passport.Count == 8) || (passport.Count == 7 && !passport.ContainsKey("cid"));
        if (!partOneValid) return false;

        //  validity for part two
        if (int.Parse(passport["byr"]) < 1920 || int.Parse(passport["byr"]) > 2002) return false; // birth year
        if (int.Parse(passport["iyr"]) < 2010 || int.Parse(passport["iyr"]) > 2020) return false; // issue year
        if (int.Parse(passport["eyr"]) < 2020 || int.Parse(passport["eyr"]) > 2030) return false; // issue year
        if (!passport["hgt"].EndsWith("in") && !passport["hgt"].EndsWith("cm")) return false; // height format
        string height = passport["hgt"].Substring(0, passport["hgt"].Length - 2); // height value
        if (passport["hgt"].EndsWith("cm"))
        {
            if (int.Parse(height) < 150 || int.Parse(height) > 193) return false;
        }
        else
        {
            if (int.Parse(height) < 59 || int.Parse(height) > 76) return false;
        }
        if (!passport["hcl"].StartsWith("#")) return false; // haircolor hex start
        if (!System.Text.RegularExpressions.Regex.IsMatch(passport["hcl"].Substring(1), @"\A\b[0-9a-fA-F]+\b\Z")) return false; // hex value
        if (passport["ecl"] != "amb" &&
            passport["ecl"] != "blu" &&
            passport["ecl"] != "brn" &&
            passport["ecl"] != "gry" &&
            passport["ecl"] != "grn" &&
            passport["ecl"] != "hzl" &&
            passport["ecl"] != "oth"
        ) return false; // eye color
        if (!System.Text.RegularExpressions.Regex.IsMatch(passport["pid"], @"^\d{9}$")) return false;
        // Console.WriteLine(passport["ecl"]);

        return true;
    }

    //  for debugging
    private void printPassport(Dictionary<string, string> passportObj)
    {
        Console.WriteLine("***********Passport**********");
        foreach (KeyValuePair<string, string> kvp in passportObj)
        {
            Console.WriteLine($"\t{kvp.Key}: {kvp.Value}");
        }
    }
}
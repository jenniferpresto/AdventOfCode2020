using System;
using System.Collections.Generic;
class Day18
{
    private List<string> data;

    public Day18(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        long total = 0;
        foreach (string line in data)
        {
            total += solveExpressionLeftToRight(line);
        }
        Console.WriteLine($"Answer to part one is {total}");

        total = 0;
        foreach (string line in data)
        {
            total += solveExpressionAdditionFirst(line);
        }
        Console.WriteLine($"Answer to part two is {total}");

    }

    private long solveExpressionLeftToRight(string expression)
    {
        string[] splitExpression = expression.Split(" ");
        //  see if we still have parentheses
        bool hasParens = false;
        foreach (string unit in splitExpression)
        {
            if (unit.Contains("("))
            {
                hasParens = true;
                break;
            }
        }
        if (hasParens)
        {
            string innerMost = getInnermostParentheticalUnit(expression);
            long innerVal = doLeftToRightOperations(innerMost);
            string condensedExpression = expression.Replace("(" + innerMost + ")", innerVal.ToString());
            return solveExpressionLeftToRight(condensedExpression);
        }
        else
        {
            return doLeftToRightOperations(expression);
        }
    }

    private long solveExpressionAdditionFirst(string expression)
    {
        string[] splitExpression = expression.Split(" ");
        //  see if we still have parentheses
        bool hasParens = false;
        foreach (string unit in splitExpression)
        {
            if (unit.Contains("("))
            {
                hasParens = true;
                break;
            }
        }
        if (hasParens)
        {
            string innerMost = getInnermostParentheticalUnit(expression);
            long innerVal = solveExpressionAdditionFirst(innerMost);
            string condensedExpression = expression.Replace("(" + innerMost + ")", innerVal.ToString());
            return solveExpressionAdditionFirst(condensedExpression);
        }
        else
        {
            return doAdditionFirstOperations(expression);
        }
    }

    private string getInnermostParentheticalUnit(string expression)
    {
        string parentheticalExpression = "";
        int indexLeft = 0;
        int indexRight = 0;
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == '(')
            {
                indexLeft = i;
            }
            else if (expression[i] == ')')
            {
                indexRight = i;
                break;
            }
        }
        parentheticalExpression = expression.Substring(indexLeft + 1, indexRight - indexLeft - 1);
        return parentheticalExpression;
    }

    private long doLeftToRightOperations(string expression)
    {
        long ans = 0;
        string currOp = "add"; // will be "add" or "mult"
        string[] splitExpression = expression.Split(" ");
        for (int i = 0; i < splitExpression.Length; i++)
        {
            if (splitExpression[i] == "*")
            {
                currOp = "mult";
            }
            else if (splitExpression[i] == "+")
            {
                currOp = "add";
            }
            else
            {
                long val = long.Parse(splitExpression[i]);
                if (currOp == "mult")
                {
                    ans *= val;
                }
                else
                {
                    ans += val;
                }
            }
        }
        return ans;
    }

    private long doAdditionFirstOperations(string expression)
    {
        if (expression.Contains("+"))
        {
            string[] splitExpression = expression.Split(" ");
            int indexOfOperator = Array.IndexOf(splitExpression, "+");
            long newVal = long.Parse(splitExpression[indexOfOperator - 1]) + long.Parse(splitExpression[indexOfOperator + 1]);
            string condensedExpression = expression.Replace(splitExpression[indexOfOperator - 1] + " + " + splitExpression[indexOfOperator + 1], newVal.ToString());
            return doAdditionFirstOperations(condensedExpression);
        }
        else
        {
            return doLeftToRightOperations(expression);
        }
    }
}
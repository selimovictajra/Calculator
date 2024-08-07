﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCalc;

public class CalculatorImpl
{
    private string Expression;
    private string Result;
    private const double MAX_RESULT = 4.0;

    public string ResultP
    {
        get { return Result; }
    }

    public CalculatorImpl()
    {
        Expression = "";
        Result = "0";
    }



    public void ParseExpression(string input)
    {
        // If operators and operands are divided by space
        if (!IsValidExpression(input))
        {
            Expression = "";
        }
        else
        {
            Expression = input;
        }
    }

    private bool IsValidExpression(string input)
    {
        string[] validOperators = { "+", "*", "/", "%" };

        int count1 = 0;
        int count2 = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '(') count1++;
            if (input[i] == ')') count2++;
        }

        if (count1 != count2) return false;

        if (input[0] == '-' && 3 < input.Length && input[1] == ' ')
        {
            for (int i = 2; i < input.Length; i++)
            {
                char c = input[i];
                if (c > '9' && c != 'E' && c != '(' && c != ')' && c != '+' && c != '-' && c != '*'
                    && c != '/' && c != '%' && c != ' ' && c != '.' && c != ',')
                {
                    return false;
                }
                if (input[i] == '-')
                {
                    // Operator inside expression
                    if (i > 2 && i < input.Length - 1)
                    {
                        if (input[i - 1] != ' ' || input[i + 1] != ' ')
                        {
                            return false;
                        }
                    }
                }
            }
        }


        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c > '9' && c != 'E' && c != '(' && c != ')' && c != '+' && c != '-' && c != '*'
                && c != '/' && c != '%' && c != ' ' && c != '.' && c != ',')
            {
                return false;
            }
            foreach (string op in validOperators)
            {
                if (input[i].ToString() == op)
                {
                    // Operator inside expression
                    if (i > 0 && i < input.Length - 1)
                    {
                        if (input[i - 1] != ' ' || input[i + 1] != ' ')
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }



    public void EvaluateExpression()
    {
        string[] splitParts = Expression.Split(' ');
        List<string> parts = splitParts.ToList();

        int pos1 = 0; int pos2 = parts.Count - 1;
        for (int l = 0; l < parts.Count; l++)
        {
            if (parts[l] == "(" && l + 1 < parts.Count)
            {
                pos1 = l + 1;
            }
            if (parts[l] == ")")
            {
                pos2 = l;
            }
        }

        // First operators * / %
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[0] == "-" && i == 0 && parts.Count > 3)
            {
                double num = Convert.ToDouble(parts[1]);
                num *= (-1);
                parts[1] = num.ToString();
                parts.RemoveAt(0);
            }
            if ((parts[i] == "*" || parts[i] == "/" || parts[i] == "%") && i - 1 >= 0 && i + 1 < parts.Count)
            {
                // First number
                double first = 0;
                int EPos = parts[i - 1].IndexOf("E");
                if (EPos >= 0)
                {
                    // First part
                    string first1 = parts[i - 1].Substring(0, EPos);
                    int DPos = first1.IndexOf('.');
                    if (DPos >= 0)
                    {
                        first1 = first1.Substring(0, DPos) + ',' + first1.Substring(DPos + 1);
                    }

                    // Second part
                    string first2 = parts[i - 1].Substring(EPos + 1);
                    DPos = first2.IndexOf('.');
                    if (DPos >= 0)
                    {
                        first2 = first2.Substring(0, DPos) + ',' + first2.Substring(DPos + 1);
                    }
                    
                    first = Convert.ToDouble(first1) * Math.Pow(10, Convert.ToDouble(first2));
                }
                else
                {
                    int DPos = parts[i - 1].IndexOf('.');
                    if (DPos >= 0)
                    {
                        parts[i - 1] = parts[i - 1].Substring(0, DPos) + ',' + parts[i - 1].Substring(DPos + 1);
                    }
                    first = Convert.ToDouble(parts[i - 1]);
                }

                // Second number
                double second = 0;
                EPos = parts[i + 1].IndexOf("E");
                if (EPos >= 0)
                {
                    // First part
                    string second1 = parts[i + 1].Substring(0, EPos);
                    int CPos = second1.IndexOf('.');
                    if (CPos >= 0)
                    {
                        second1 = second1.Substring(0, CPos) + ',' + second1.Substring(CPos + 1);
                    }

                    // Second part
                    string second2 = parts[i + 1].Substring(EPos + 1);
                    CPos = second2.IndexOf('.');
                    if (CPos >= 0)
                    {
                        second2 = second2.Substring(0, CPos) + ',' + second2.Substring(CPos + 1);
                    }

                    second = Convert.ToDouble(second1) * Math.Pow(10, Convert.ToDouble(second2));
                }
                else
                {
                    int CPos = parts[i + 1].IndexOf('.');
                    if (CPos >= 0)
                    {
                        parts[i + 1] = parts[i + 1].Substring(0, CPos) + ',' + parts[i + 1].Substring(CPos + 1);
                    }
                    second = Convert.ToDouble(parts[i + 1]);
                }

                double res = 0;
                if (parts[i] == "*") { res = first * second; }
                else if (parts[i] == "/") { res = first / second; }
                else if (parts[i] == "%") { res = first % second; }
                
                List<string> newParts = new List<string>();
                for (int j = 0; j < i - 1; j++)
                {
                    newParts.Add(parts[j]);
                }
                newParts.Add(res.ToString());
                //int position = newParts.Count - 1;
                for (int j = i + 2; j < parts.Count; j++)
                {
                    newParts.Add(parts[j]);
                }

                parts.Clear();
                for (int j = 0; j < newParts.Count; j++)
                {
                    parts.Add(newParts[j]);
                }
                i = -1;
            }
        }

        // Second operators + -
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[0] == "-" && i == 0 && parts.Count > 3)
            {
                double num = Convert.ToDouble(parts[1]);
                num *= (-1);
                parts[1] = num.ToString();
                parts.RemoveAt(0);
            }
            if ((parts[i] == "+" || parts[i] == "-") && i - 1 >= 0 && i + 1 < parts.Count)
            {
                // First number
                double first = 0;
                int EPos = parts[i - 1].IndexOf("E");
                if (EPos >= 0)
                {
                    // First part
                    string first1 = parts[i - 1].Substring(0, EPos);
                    int CPos = first1.IndexOf('.');
                    if (CPos >= 0)
                    {
                        first1 = first1.Substring(0, CPos) + ',' + first1.Substring(CPos + 1);
                    }

                    // Second part
                    string first2 = parts[i - 1].Substring(EPos + 1);
                    CPos = first2.IndexOf('.');
                    if (CPos >= 0)
                    {
                        first2 = first2.Substring(0, CPos) + ',' + first2.Substring(CPos + 1);
                    }

                    first = Convert.ToDouble(first1) * Math.Pow(10, Convert.ToDouble(first2));
                }
                else
                {
                    int CPos = parts[i - 1].IndexOf('.');
                    if (CPos >= 0)
                    {
                        parts[i - 1] = parts[i - 1].Substring(0, CPos) + ',' + parts[i - 1].Substring(CPos + 1);
                    }
                    first = Convert.ToDouble(parts[i - 1]);
                }

                // Second number
                double second = 0;
                EPos = parts[i + 1].IndexOf("E");
                if (EPos >= 0)
                {
                    // First part
                    string second1 = parts[i + 1].Substring(0, EPos);
                    int CPos = second1.IndexOf('.');
                    if (CPos >= 0)
                    {
                        second1 = second1.Substring(0, CPos) + ',' + second1.Substring(CPos + 1);
                    }

                    // Second part
                    string second2 = parts[i + 1].Substring(EPos + 1);
                    CPos = second2.IndexOf('.');
                    if (CPos >= 0)
                    {
                        second2 = second2.Substring(0, CPos) + ',' + second2.Substring(CPos + 1);
                    }

                    second = Convert.ToDouble(second1) * Math.Pow(10, Convert.ToDouble(second2));
                }
                else
                {
                    int CPos = parts[i + 1].IndexOf('.');
                    if (CPos >= 0)
                    {
                        parts[i + 1] = parts[i + 1].Substring(0, CPos) + ',' + parts[i + 1].Substring(CPos + 1);
                    }
                    second = Convert.ToDouble(parts[i + 1]);
                }

                double res = 0;
                if (parts[i] == "+") { res = first + second; }
                else if (parts[i] == "-") { res = first - second; }

                List<string> newParts = new List<string>();
                for (int j = 0; j < i - 1; j++)
                {
                    newParts.Add(parts[j]);
                }
                newParts.Add(res.ToString());
                //int position = newParts.Count - 1;
                for (int j = i + 2; j < parts.Count; j++)
                {
                    newParts.Add(parts[j]);
                }

                parts.Clear();
                for (int j = 0; j < newParts.Count; j++)
                {
                    parts.Add(newParts[j]);
                }
                i = -1;
            }
        }

        //Result = Convert.ToDouble(parts[0]);


        if (parts.Count == 1)
        {
            Result = parts[0];
            if (!CheckMaxResult())
            {
                PrintResult();
            }
        }
    }



    public bool CheckMaxResult()
    {
        if (Convert.ToDouble(Result) > MAX_RESULT)
        {
            Console.WriteLine("A Suffusion of Yellow");
            return true;
        }
        return false;
    }

    public void PerformOperation()
    {
        switch (Expression)
        {
            case "Red Button":
                PrintYijingCharacter();
                break;
            case "":
                Console.WriteLine("ERROR: Invalid expression!");
                break;
            default:
                EvaluateExpression();
                break;
        }
    }

    public void PrintResult()
    {
        Console.WriteLine($"Result: {Result}");
    }

    public void PrintYijingCharacter ()
    {
        Random rand = new Random();
        int hexagramNumber = rand.Next(0, 64);
        char hexagram = (char)(0x4DC0 + hexagramNumber);
        Console.WriteLine($"Random Yijing hexagram Unicode character: {hexagram}");
    }
    
}


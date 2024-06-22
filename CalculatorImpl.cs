using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCalc;

public class CalculatorImpl
{
    private string Expression;
    private double Result;
    private const double MAX_RESULT = 4.0;

    public double ResultP
    {
        get { return Result; }
    }

    public CalculatorImpl() 
    {
        Expression = "";
        Result = 0;
    }

    

    public void ParseExpression(string input)
    {
        // If operators and operands are devided by space
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
        string[] validOperators = { "+", "-", "*", "/", "%", "(", ")" };

        for (int i = 0; i < input.Length; i++)
        {
            foreach (string op in validOperators)
            {
                if (input[i].ToString() == op)
                {
                    // If '(' is on first place
                    if (i == 0 && input.Length > 1 && input[i + 1] != ' ')
                    {
                        return false;
                    }
                    // If operator is on last place
                    else if (i == input.Length - 1 && input.Length > 1 && input[i - 1] != ' ')
                    {
                        return false;
                    }
                    // Operator inside expression
                    else if (i > 0 && i < input.Length - 1)
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
        try
        {
            DataTable table = new DataTable();
            Result = Convert.ToDouble(table.Compute(Expression, ""));
            if (!CheckMaxResult())
            {
                PrintResult();
            }
        }
        catch
        {
            Console.WriteLine("Error: Unable to evaluate expression");
        }
    }



    public bool CheckMaxResult()
    {
        if (Result > MAX_RESULT)
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


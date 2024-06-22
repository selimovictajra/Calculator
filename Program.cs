using System;
class Program
{
    static void Main()
    {
        CalculatorImpl calc = new CalculatorImpl();

        while (true)
        {
            Console.WriteLine("\nEnter a mathematical expression: ");
            string expression = Console.ReadLine();

            calc.ParseExpression(expression);
            
            calc.PerformOperation();

            
        }
    }
}

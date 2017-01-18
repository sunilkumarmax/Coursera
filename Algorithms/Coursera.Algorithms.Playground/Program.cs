namespace Coursera.Algorithms.Playground
{
    using ClassLibrary;
    using System;
    using System.Diagnostics;

    public class Program
    {
        private enum Operation { Multiply, Add, Subtract };

        private static void Course1Week1()
        {
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                Console.WriteLine("Choose an Option - [Multiply (1), Add (2), Subtract (3)]:");
                string str = Console.ReadLine();
                if (str.Length == 0)
                {
                    return;
                }
                int opt = 0;
                int.TryParse(str, out opt);
                if (opt < 1 || opt > 3)
                {
                    continue;
                }
                Operation option = (Operation)(opt - 1);
                try
                {
                    Console.WriteLine("Enter first number to " + option + ":");
                    str = Console.ReadLine();
                    if (str.Length == 0)
                    {
                        return;
                    }
                    NumberString x = new NumberString(str);
                    Console.WriteLine("Enter second number to " + option + ":");
                    str = Console.ReadLine();
                    NumberString y = new NumberString(str);
                    Console.Write("Result of the " + option + ": ");
                    sw.Restart();
                    switch (option)
                    {
                        case Operation.Add:
                            Console.WriteLine(x.Add(y).PrettyPrint());
                            break;
                        case Operation.Subtract:
                            Console.WriteLine(x.Subtract(y).PrettyPrint());
                            break;
                        default:
                            Console.WriteLine(x.Multiply(y).PrettyPrint());
                            break;
                    }
                    sw.Stop();
                    Console.WriteLine("Elapsed: "+ sw.ElapsedMilliseconds+" milliseonds");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
            }
        }

        public static void Main(string[] args)
        {
            Course1Week1();
        }
    }
}

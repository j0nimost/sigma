using System;
using System.Collections.Generic;

namespace sigma
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = null;
            Console.Write("Sigma > ");
            while (true)
            {
                
                string text = Console.ReadLine();
                
                lexer = new Lexer(text);
                try
                {
                    List<Token> tokens = lexer.Generate_Tokens();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
                Console.WriteLine(lexer.ToString()); // only printing

                Console.Write("\nSigma > ");
            }
        }
    }
}

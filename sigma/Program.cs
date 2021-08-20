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
                
                
                try
                {
                    lexer = new Lexer(text);
                    List<Token> tokens = lexer.Generate_Tokens();
                    Parser parser = new Parser(tokens);
                    Console.Write(parser.expression().Eval());
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
                //Console.WriteLine(lexer.ToString()); // only printing

                Console.Write("\nSigma > ");
            }
        }
    }
}

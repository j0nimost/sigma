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
            int counter = 0;
            while (true)
            {
                if(counter > 0)
                {
                    Console.Write("\nSigma > ");
                }
                string text = Console.ReadLine();
                counter++;
                // check if text is a variable
                AST assignment = null;
                if(Parser.LocalAssignment.TryGetValue(text, out assignment))
                {
                    Console.Write(assignment.Eval());
                    continue;
                }
                
                try
                {
                    lexer = new Lexer(text);
                    List<Token> tokens = lexer.Generate_Tokens();
                    //Console.WriteLine(lexer.ToString());
                    Parser parser = new Parser(tokens);
                    AST ast = parser.expression();
                    //Console.WriteLine(ast.ToString());
                    Console.Write(ast.Eval());
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
                //Console.WriteLine(lexer.ToString()); // only printing

                //Console.Write("\nSigma > ");
            }
        }
    }
}

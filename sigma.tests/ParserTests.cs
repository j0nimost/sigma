using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace sigma.tests
{
    public class ParserTests
    {
        Lexer lexer = null;
        [Fact]
        public void TestAddition()
        {
            lexer = new Lexer("5 + 6");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(11, res);
        }

        [Fact]
        public void TestSubtraction()
        {
            lexer = new Lexer("87-7");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(80, res);
        }

        [Fact]
        public void TestAdditionandSubtraction()
        {
            lexer = new Lexer("5 + 6 - 5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(6, res);
        }
    }
}

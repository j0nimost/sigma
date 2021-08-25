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

        [Fact]
        public void TestMultiplication()
        {
            lexer = new Lexer("14 * 7");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(98, res);
        }

        [Fact]
        public void TestMultiplicationWithPlusandMinus()
        {
            lexer = new Lexer("12 - 14 * 7 + 9 * 6 + 7 * 5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(3, res);
        }

        [Fact]
        public void TestDivisionByZero()
        {
            
            lexer = new Lexer("1/0");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            Assert.Throws<DivideByZeroException>(() => result.Eval());
        }

        [Fact]
        public void TestDivision()
        {
            lexer = new Lexer("15/ 5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(3, res);
        }

        [Fact]
        public void TestCombinedOperations()
        {
            lexer = new Lexer("45/9-5+187*2-345+65/5-4*5+3-100/4");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(0, res);
        }

        [Fact]
        public void TestParenthesis()
        {
            lexer = new Lexer("(5-2)+7-25/5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(5, res);
        }

        [Fact]
        public void TestNestedParenthesis()
        {
            lexer = new Lexer("((6*4)-4)");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(20, res);
        }

        [Fact]
        public void TestExceptionIncompleteParenthesis()
        {
            // NullReferenceException
            lexer = new Lexer("((6*4-4)");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);


            Assert.Throws<NullReferenceException>(() => parser.expression());
        }
    }
}

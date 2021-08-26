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


            Assert.Throws<FormatException>(() => parser.expression());
        }

        [Fact]
        public void TestVariableDeclarations()
        {
            lexer = new Lexer("p=(((8*7)/4)-6)");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(8, res);

            // Test Variable Existence
            AST variableAST = null;
            Assert.True(Parser.LocalAssignment.TryGetValue("p", out variableAST));
            Assert.NotNull(variableAST);
            decimal variableEval = variableAST.Node;
            Assert.Equal(8, variableEval);

        }

        [Fact]
        public void TestExceptionForDuplicateVariableDeclarations()
        {
            lexer = new Lexer("d=(((8*7)/4)-6)");
            List<Token> tokens = null;
            tokens =lexer.Generate_Tokens();
            Parser parser = null;
            parser =new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(8, res);

            // Test Duplication
            lexer = new Lexer("d=-6)");
            tokens = lexer.Generate_Tokens();
            parser = new Parser(tokens);

            Assert.Throws<Exception>(() => parser.expression());

        }

        [Fact]
        public void TestExceptionForInvalidVariableDeclaration()
        {
            lexer = new Lexer("x=");
            List<Token> tokens = null;
            tokens = lexer.Generate_Tokens();
            Parser parser = null;
            parser = new Parser(tokens);

            Assert.Throws<InvalidOperationException>(() => parser.expression());

        }

        [Fact]
        public void TestAnomalousMathematicalExpressions()
        {
            lexer = new Lexer("10-5*(25/5)+15");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(0, res);
        }

        [Fact]
        public void TestOperationsOnVariables()
        {
            // First Variable
            lexer = new Lexer("u=9-4");
            List<Token> tokens = null;
            tokens = lexer.Generate_Tokens();
            Parser parser = null;
            parser = new Parser(tokens);

            AST result = parser.expression();

            Assert.NotNull(result);
            decimal res = result.Eval();
            Assert.Equal(5, res);

            // Second Variable
            lexer = new Lexer("v=u*2");
            tokens = lexer.Generate_Tokens();
            parser = new Parser(tokens);

            result = parser.expression();

            Assert.NotNull(result);
            decimal res_ = result.Eval();
            Assert.Equal(10, res_);
        }
    }
}

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

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(11, res);
        }

        [Fact]
        public void TestSubtraction()
        {
            lexer = new Lexer("87-7");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(80, res);
        }

        [Fact]
        public void TestAdditionandSubtraction()
        {
            lexer = new Lexer("5 + 6 - 5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(6, res);
        }

        [Fact]
        public void TestMultiplication()
        {
            lexer = new Lexer("14 * 7");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(98, res);
        }

        [Fact]
        public void TestMultiplicationWithPlusandMinus()
        {
            lexer = new Lexer("12 - 14 * 7 + 9 * 6 + 7 * 5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(3, res);
        }

        [Fact]
        public void TestDivisionByZero()
        {
            
            lexer = new Lexer("1/0");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            Assert.Throws<DivideByZeroException>(() => (long)result.Eval());
        }

        [Fact]
        public void TestDivision()
        {
            lexer = new Lexer("15/ 5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(3, res);
        }

        [Fact]
        public void TestCombinedOperations()
        {
            lexer = new Lexer("45/9-5+187*2-345+65/5-4*5+3-100/4");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(0, res);
        }

        [Fact]
        public void TestParenthesis()
        {
            lexer = new Lexer("(5-2)+7-25/5");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(5, res);
        }

        [Fact]
        public void TestNestedParenthesis()
        {
            lexer = new Lexer("((6*4)-4)");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
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

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(8, res);

            // Test Variable Existence
            object variableAST = null;
            Assert.True(Parser.LocalAssignment.TryGetValue("p", out variableAST));
            Assert.NotNull(variableAST);
            long variableEval = (long)variableAST;
            Assert.Equal(8, variableEval);

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

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
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

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(5, res);

            // Second Variable
            lexer = new Lexer("v=u*2");
            tokens = lexer.Generate_Tokens();
            parser = new Parser(tokens);

            result = parser.expression();

            Assert.NotNull(result);
            long res_ = (long)result.Eval();
            Assert.Equal(10, res_);
        }

        [Fact]
        public void TestVariableReassignment()
        {
            // First Variable
            lexer = new Lexer("a=9-4");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();

            Assert.NotNull(result);
            long res = (long)result.Eval();
            Assert.Equal(5, res);

            // Second Variable
            lexer = new Lexer("a=a*2");
            tokens = lexer.Generate_Tokens();
            parser = new Parser(tokens);

            result = parser.expression();

            Assert.NotNull(result);
            long res_ = (long)result.Eval();
            Assert.Equal(10, res_);
        }

        [Fact]
        public void TestREPLStringDeclarations()
        {
            lexer = new Lexer("\"Donda\"");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Equal("Donda", result.Eval());
        }

        [Fact]
        public void TestStringVariableDeclarations()
        {
            lexer = new Lexer("ye   = \"Donda\"");
            List<Token> tokens = lexer.Generate_Tokens();
            Assert.NotEmpty(tokens);
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotNull(result);
            Assert.Equal("Donda", result.Eval());

            // Validate Variable value is maintained
            lexer = new Lexer("ye");
            tokens = lexer.Generate_Tokens();
            Assert.NotEmpty(tokens);
            parser = new Parser(tokens);

            result = parser.expression();
            Assert.Equal("Donda", result.Eval());
        }

        [Fact]
        public void TestStringFormatExceptions()
        {
            lexer = new Lexer("drake   = \"CLB");
            Assert.Throws<InvalidOperationException>(() => lexer.Generate_Tokens()); 
        }

        [Fact]
        public void TestBitwiseOpAND()
        {
            lexer = new Lexer(" 7 & 2");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Equal(2, (long)result.Eval());
        }

        [Fact]
        public void TestBitwiseOpOR()
        {
            lexer = new Lexer(" 13 | 2");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Equal(15, (long)result.Eval());
        }

        [Fact]
        public void TestBitwiseOpXOR()
        {
            lexer = new Lexer(" 15 ^ 2");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Equal(13, (long)result.Eval());
        }

        [Fact]
        public void TestBitwiseOpLShift()
        {
            lexer = new Lexer(" 2 << 2");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Equal(8, (long)result.Eval());
        }

        [Fact]
        public void TestBitwiseOpRShift()
        {
            lexer = new Lexer(" 9223372036854775807 >> 1");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Equal(4611686018427387903, (long)result.Eval());
        }

        [Fact]
        public void TestIntegerOverflowForBitOps()
        {
            lexer = new Lexer(" 9223372036854775807 << 9223372036854775807");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Throws<OverflowException>(() => result.Eval());
        }

        [Fact]
        public void TestInvalidCastForBitOps()
        {
            lexer = new Lexer(" 9223372036854775807 << \"CLB Sucks\"");
            List<Token> tokens = lexer.Generate_Tokens();
            Parser parser = new Parser(tokens);

            IASTNode result = parser.expression();
            Assert.NotEmpty(tokens);
            Assert.NotNull(result);
            Assert.Throws<InvalidCastException>(() => result.Eval());
        }
    }
}

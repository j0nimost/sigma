using System;
using Xunit;
using sigma;
using System.Collections.Generic;

namespace sigma.tests
{
    public class LexerTests
    {
        Lexer lexer = null;
        [Fact]
        public void TestBasicNumberStructure()
        {
            string l_exp = "NUMBER:5 MULTIPLY:* NUMBER:6 ";
            lexer = new Lexer("5 * 6");
            List<Token> tokens = lexer.Generate_Tokens();
            string l_str = lexer.ToString();

            Assert.NotNull(tokens);
            Assert.Equal(l_exp, l_str);
        }

        [Fact]
        public void TestExceptionThrown()
        {
            lexer = new Lexer("$");
            Assert.Throws<NotSupportedException>(() => lexer.Generate_Tokens());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    // BODMAS
    public enum TokenType
    {
        NUMBER=0,
        STRING,
        DIVIDE,
        MULTIPLY,
        PLUS,
        MINUS,
        RPAREN,
        LPAREN,
        IDENTIFIER,
        EQ,

        /*BITWISE OPERATORS*/
        AND, // &
        OR, // |
        XOR, // ^
        LSHIFT, // <<
        RSHIFT, // >>

        LBRACKET, // [
        RBRACKET, // ]
        COMMA, // ,
        EOF
    }
    public class Token
    {
        public TokenType TokenType { get; set; }
        public object TokenValue { get; set; } // Changed from Dynamic Since it Fails for Long

        public override string ToString()
        {
            return "Token: " + TokenType + " " + TokenValue; 
        }
    }
}

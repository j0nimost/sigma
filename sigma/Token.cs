﻿using System;
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
        EOF
    }
    public class Token
    {
        public TokenType TokenType { get; set; }
        public dynamic TokenValue { get; set; }

        public override string ToString()
        {
            return "Token: " + TokenType + " " + TokenValue; 
        }
    }
}

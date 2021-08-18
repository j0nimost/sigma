﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public class Lexer
    {
        private List<char> WHITESPACE = new List<char> {' ', '\n', '\t' };
        private List<char> NUMBERS = new List<char> {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private List<Token> tokens = new List<Token>();

        private string input;
        private int next = -1;
        private char curr_char;
        public Lexer(string input)
        {
            this.input = input;
            Advance();
        }

        public void Advance()
        {
            try
            {
                next++;

                curr_char = input[next];
                
            }
            catch (IndexOutOfRangeException)
            {
                // DO NADA
                curr_char = '\0';
            }
            
        }

        public List<Token> Generate_Tokens()
        {
            
            while(curr_char != '\0')
            {
                // check if space
                if (WHITESPACE.Contains(curr_char))
                {
                    Advance();
                }
                else if(curr_char == '.' || NUMBERS.Contains(curr_char))
                {
                    // Parse
                    Token numToken = Generate_Number();
                    if (numToken != null)
                    {
                        tokens.Add(numToken);
                    }
                    else
                    {
                        throw new Exception("Not a Number");
                    }
                }
                else if(curr_char == '*')
                {
                    Token multipleToken = new Token
                    {
                        TokenType = TokenType.MULTIPLY,
                        TokenValue = '*'
                    };
                    tokens.Add(multipleToken);
                    Advance(); // Get Next CHAR
                }
                else if (curr_char == '/')
                {
                    Token divToken = new Token
                    {
                        TokenType = TokenType.DIVIDE,
                        TokenValue = '/'
                    };
                    tokens.Add(divToken);
                    Advance(); // Get Next CHAR
                }
                else if (curr_char == '-')
                {
                    Token subToken = new Token
                    {
                        TokenType = TokenType.MINUS,
                        TokenValue = '-'
                    };
                    tokens.Add(subToken);
                    Advance(); // Get Next CHAR
                }
                else if (curr_char == '+')
                {
                    Token addToken = new Token
                    {
                        TokenType = TokenType.PLUS,
                        TokenValue = '+'
                    };
                    tokens.Add(addToken);
                    Advance(); // Get Next CHAR
                }
                else
                {
                    throw new Exception($"Character: {curr_char} is unknown");
                }
                
            }
            return tokens;
        }

        public Token Generate_Number()
        {
            // ITER ALL THROUGH
            int count_decimal_points = 0;
            string number = "";//curr_char.ToString();
            while(curr_char != '\0' &&(curr_char == '.' || NUMBERS.Contains(curr_char)))
            {
                if(curr_char=='.')
                {
                    count_decimal_points++;
                }

                if(count_decimal_points > 1)
                {
                    return null;
                }

                // append
                number += curr_char;
                Advance();
            }
            
            // check position of decimal point
            if(number[0] == '.')
            {
                number = '0' + number;
            }
            else if(number[number.Length-1] == '0')
            {
                number += '0';
            }

            // Check it's not just a decimal point
            if(number.Length == 1 && number[0] == '.')
            {
                return null;
            }

            // use Decimal since it has 128 bit
            // TODO: change to BigInteger
            return new Token
            {
                TokenType = TokenType.NUMBER,
                TokenValue = Convert.ToDecimal(number)
            };

        }

        public override string ToString()
        {
            string tokenStr = "";
            foreach (var token in tokens)
            {
                tokenStr += (token.TokenType +":" + token.TokenValue + " ");
            }
            return tokenStr;
        }

    }
}

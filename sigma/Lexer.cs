using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public class Lexer
    {
        private List<char> WHITESPACE = new List<char> {' ', '\n', '\t' };
        private List<char> NUMBERS = new List<char> {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private List<char> LETTERS = new List<char> {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                                                     'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
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
            next++;

            if (next < input.Length)
            {
                curr_char = input[next];
            }
            else
            {
                curr_char = '\0';
            }

        }

        public List<Token> Generate_Tokens()
        {
            
            while(true)
            {
                // check if space
                if (WHITESPACE.Contains(curr_char))
                {
                    Advance();
                }
                else if(curr_char == '\"') // Start of a string
                {
                    Token strToken = Generate_String();
                    tokens.Add(strToken);
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
                        throw new NotSupportedException("Not a Number");
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
                else if (curr_char == '(')
                {
                    Token lparenToken = new Token
                    {
                        TokenType = TokenType.LPAREN,
                        TokenValue = null
                    };
                    tokens.Add(lparenToken);
                    Advance(); // Get Next CHAR
                }
                else if (curr_char == ')')
                {
                    Token rparenToken = new Token
                    {
                        TokenType = TokenType.RPAREN,
                        TokenValue = null
                    };
                    tokens.Add(rparenToken);
                    Advance(); // Get Next CHAR
                }
                else if (LETTERS.Contains(curr_char))
                {
                    // Get the Character 
                    Token identifierToken = Generate_Variable();
                    if (!String.IsNullOrEmpty((string)identifierToken.TokenValue))
                    {
                        tokens.Add(identifierToken);
                    }
                }
                else if(curr_char == '=')
                {
                    Token token = new Token
                    {
                        TokenType = TokenType.EQ,
                        TokenValue = null
                    };

                    tokens.Add(token);
                    Advance();
                }
                else if(curr_char == '&')
                {
                    // AND bitwise operator
                    // TODO: Add Logical AND
                    Token token = new Token
                    {
                        TokenType = TokenType.AND,
                        TokenValue = null
                    };
                    tokens.Add(token);
                    Advance();
                }
                else if(curr_char == '\0')
                {
                    // 
                    Token eof = new Token
                    {
                        TokenType = TokenType.EOF,
                        TokenValue = null
                    };

                    tokens.Add(eof);
                    break;
                }
                else
                {
                    throw new NotSupportedException($"Character: {curr_char} is unknown");
                }
                
            }

            

            Generate_VariableTokenValues(tokens); // IF ANY
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
            else if(number[number.Length-1] == '.')
            {
                number += '0';
            }

            // Check it's not just a decimal point
            if(number.Length == 1 && number[0] == '.')
            {
                return null;
            }

            // Changed to long to support bitwise operations
            return new Token
            {
                TokenType = TokenType.NUMBER,
                TokenValue = Convert.ToInt64(number)
            };

        }

        public Token Generate_Variable()
        {
            StringBuilder sb = new StringBuilder();

            while (curr_char != '\0' && !WHITESPACE.Contains(curr_char) && LETTERS.Contains(curr_char))
            {
                sb.Append(curr_char);

                Advance();
            }

            return new Token
            {
                TokenType = TokenType.IDENTIFIER,
                TokenValue = sb.ToString()
            };
        }

        public Token Generate_String()
        {
            StringBuilder sb = new StringBuilder(); // A memory efficient way of building string
            Advance(); 
            while (((int)curr_char >= 1 && (int)curr_char <=255) && curr_char != '\"') // Skip the NULL byte and Ensure we are still within range of the Quotations Marks
            {
                sb.Append(curr_char);
                Advance();
            }

            if (curr_char != '\"') // Validate there is a closing Quotation Mark
            {
                throw new InvalidOperationException("Invalid string format");
            }

            return new Token
            {
                TokenType = TokenType.STRING,
                TokenValue = sb.ToString()
            };
        }

        public List<Token> Generate_VariableTokenValues(List<Token> tokens)
        {

            List<TokenType> tokenTypes = new List<TokenType>();

            for (int j = 0; j < tokens.Count; j++)
            {
                tokenTypes.Add(tokens[j].TokenType); // Flatten the List
            }

            // Loop through tokens
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].TokenType == TokenType.EQ && i != 1)
                {
                    throw new InvalidOperationException("The Assignment '=' Is invalidly placed");
                }

                // Check if all Variables are declared
                if (tokens[i].TokenType == TokenType.IDENTIFIER) // only work with Identifiers
                {
                    object assignment = null;
                    if (Parser.LocalAssignment.TryGetValue((string)tokens[i].TokenValue, out assignment))
                    {
                        if(tokenTypes.Contains(TokenType.EQ))
                        {
                            if(i > 1)
                            {
                                tokens[i] = new Token
                                {
                                    TokenType = TokenType.NUMBER,
                                    TokenValue = assignment
                                };
                            }
                        }
                        else
                        {
                            long val = 0;
                            if(Int64.TryParse(assignment.ToString(), out val))
                            {
                                tokens[i] = new Token
                                {
                                    TokenType = TokenType.NUMBER,
                                    TokenValue = val
                                };
                            }
                            else {
                                tokens[i] = new Token
                                {
                                    TokenType = TokenType.STRING,
                                    TokenValue = assignment
                                };
                            }
                        }
                    }
                }

            }
            // Pass to the Parser
            return tokens;
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

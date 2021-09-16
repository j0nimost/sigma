using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public class Parser
    {
        // Resolve the Lexer to Nodes
        private List<TokenType> ExpressionOps = new List<TokenType> { TokenType.PLUS, TokenType.MINUS };
        private List<TokenType> TermOps = new List<TokenType> { TokenType.DIVIDE, TokenType.MULTIPLY, TokenType.AND, TokenType.OR, TokenType.XOR, TokenType.LSHIFT, TokenType.RSHIFT};
        public static Dictionary<string, object> LocalAssignment = new Dictionary<string, object>();
        private List<Token> tokens;
        private int next = -1;
        private Token curr_token = null;
        private IASTNode resultTree;//new AST();
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            Advance();
        }

        public void Advance()
        {
            next++;
            if (next < tokens.Count)
            {
                curr_token = tokens[next];
            }
        }
        
        public IASTNode Expression()
        {
            if (curr_token.TokenType == TokenType.IDENTIFIER)
            {
                string variable = String.Empty;
                if (curr_token.TokenValue is string)
                {
                    variable = (string)curr_token.TokenValue;

                }
                else
                {
                    return new ASTIdentifier(null, curr_token.TokenValue);
                }

                resultTree = ParseIdentifiers(variable);
                // Store In Dictionary
                try
                {
                    object identifierValue = resultTree.Eval();

                    if (!IsPredefinedIdentifier(variable)) // Ensure we don't add Predefined Variables
                    {
                        bool isAdded = LocalAssignment.TryAdd(variable, identifierValue);

                        if (!isAdded)
                        {
                            LocalAssignment[variable] = identifierValue; // Update
                        }
                    }

                }
                catch (Exception)
                {
                    throw new Exception("Unexpected Error When Adding Variable");
                }
                // return
            }
            else
            {
                if(resultTree == null)
                {
                    resultTree = Term();
                    while (curr_token.TokenType != TokenType.EOF && resultTree != null && ExpressionOps.Contains(curr_token.TokenType))
                    {
                        if (curr_token.TokenType == TokenType.PLUS)
                        {
                            Advance();
                            IASTNode right = Term();
                            resultTree = new ASTPlus(resultTree, right);

                        }
                        else if (curr_token.TokenType == TokenType.MINUS)
                        {
                            Advance();
                            IASTNode right = Term();
                            resultTree = new ASTMinus(resultTree, right);
                        }
                    }
                }
                else
                {
                    // During Recursion avoid overwriting value of resulttree
                    IASTNode tempResult = Term();
                    while (curr_token.TokenType != TokenType.EOF && tempResult != null && ExpressionOps.Contains(curr_token.TokenType))
                    {
                        if (curr_token.TokenType == TokenType.PLUS)
                        {
                            Advance();
                            IASTNode right = Term();
                            resultTree = new ASTPlus(tempResult, right);

                        }
                        else if (curr_token.TokenType == TokenType.MINUS)
                        {
                            Advance();
                            IASTNode right = Term();
                            resultTree = new ASTMinus(tempResult, right);
                        }
                    }

                    return tempResult;
                }
                
            }
            

            return resultTree;
        }

        public IASTNode Term()
        {
            IASTNode result = Factor();
            //
            while (curr_token.TokenType != TokenType.EOF && TermOps.Contains(curr_token.TokenType))
            {
                if (curr_token.TokenType == TokenType.MULTIPLY)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTMultiply(result, right);

                }
                else if(curr_token.TokenType == TokenType.DIVIDE)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTDivide(result, right);
                }
                else if(curr_token.TokenType == TokenType.AND)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTBitwiseAND(result, right);
                }
                else if (curr_token.TokenType == TokenType.OR)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTBitwiseOR(result, right);
                }
                else if (curr_token.TokenType == TokenType.XOR)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTBitwiseXOR(result, right);
                }
                else if (curr_token.TokenType == TokenType.LSHIFT)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTBitwiseLShift(result, right);
                }
                else if (curr_token.TokenType == TokenType.RSHIFT)
                {
                    Advance();
                    IASTNode right = Factor();
                    result = new ASTBitwiseRShift(result, right);
                }
            }
            return result;
        }
        public IASTNode Factor()
        {
            IASTNode result = null;
            if (curr_token.TokenType == TokenType.LPAREN)
            {
                Advance();
                result = Expression();// recursion to get next input
                
                if (curr_token.TokenType == TokenType.EOF)
                {
                    throw new FormatException("Invalid Syntax missing )");
                }

                if(curr_token.TokenType != TokenType.RPAREN)
                {
                    throw new FormatException("Invalid Syntax missing )");
                }
            }

            if (curr_token.TokenType == TokenType.NUMBER)
            {
                result = new ASTNumber((long)curr_token.TokenValue);                
            }
            else if (curr_token.TokenType == TokenType.STRING)
            {
                result = new ASTString((string)curr_token.TokenValue);
            }
            Advance();
            return result;

        }

        public IASTNode ParseIdentifiers(string identifier)
        {
            // Check identifier if its a predefined Identifier i.e
            // Bool, Char, Byte
            // Else Make a variable declaration
            IASTNode identifierNode = null;
            bool bolValue;
            if (Boolean.TryParse(identifier, out bolValue) && identifierNode == null)
            {
                identifierNode = new ASTIdentifier("Boolean", bolValue);
                ValidatePredefinedVariables(bolValue);
            }
            else if (identifierNode == null)
            {
                Advance();
                if (curr_token.TokenType == TokenType.EOF)
                {
                    throw new InvalidOperationException("Missing Assignment '=' After Identifier");
                }

                //Advance
                Advance();
                // Check for Expression
                if (curr_token.TokenType == TokenType.EOF)
                {
                    throw new InvalidOperationException("Missing Assignment Type After '=");
                }
                //Recurse on Expression to get the expression
                IASTNode expression = Expression();

                //Pass back to the identifier
                identifierNode = new ASTIdentifier(identifier, expression.Eval());

                Advance();
            }
            else
            {
                // Too many arguments are being passed
                throw new OverflowException($"Too Many Assignment for the variable: {identifier}");
            }
            
            return identifierNode; // return it
        }

        public void ValidatePredefinedVariables(object obj)
        {
            // Validate there is no attempt to assignvalue to the
            Advance();

            if (curr_token.TokenType == TokenType.EQ)
            {
                throw new InvalidOperationException($"You cannot assign value to a :{obj.GetType()}");
            }
        }

        public bool IsPredefinedIdentifier(string identifierName)
        {
            bool IsPredefined = false;//Boolean.TryParse(identifierName, out _);
            if(Boolean.TryParse(identifierName, out _))
            {
                IsPredefined = true;
            }
            return IsPredefined;
        }
    }
}

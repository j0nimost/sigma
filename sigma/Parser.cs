using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public class Parser
    {
        // Resolve the Lexer to Nodes
        private List<TokenType> AddMinus = new List<TokenType> { TokenType.PLUS, TokenType.MINUS };
        private List<TokenType> DivMultiply = new List<TokenType> { TokenType.DIVIDE, TokenType.MULTIPLY};
        public static Dictionary<string, AST> LocalAssignment = new Dictionary<string, AST>();
        private List<Token> tokens;
        private int next = -1;
        private Token curr_token = null;
        private AST resultTree = new AST();
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            Advance();
        }

        public void Advance()
        {
            try
            {
                next++;
                curr_token = tokens[next];
            }
            catch (ArgumentOutOfRangeException)
            {
                curr_token = null;
            }
        }
        
        public AST expression()
        {
            if (curr_token.TokenType == TokenType.IDENTIFIER)
            {
                string variable = curr_token.TokenValue;
                // validate EQ
                Advance();
                if (curr_token == null)
                {
                    throw new InvalidOperationException("Missing Assignment '=' After Identifier");
                }

                //Advance
                Advance();
                // Check for Expression
                if (curr_token == null)
                {
                    throw new InvalidOperationException("Missing Assignment Type After '=");
                }
                //Recurse on Expression to get the expression
                resultTree = expression();
                // Store In Dictionary
                try
                {
                    bool isAdded =LocalAssignment.TryAdd(variable, new AST() { Node = resultTree.Eval()});

                    if (!isAdded)
                    {
                        LocalAssignment[variable] = new AST() { Node = resultTree.Eval() }; // Update
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
                if(resultTree.Node == null)
                {
                    resultTree = term();
                    while (curr_token != null && resultTree.Node != null && AddMinus.Contains(curr_token.TokenType))
                    {
                        if (curr_token.TokenType == TokenType.PLUS)
                        {
                            Advance();
                            AST right = term();
                            resultTree.Node = new ASTPlus(resultTree.Node, right.Node);

                        }
                        else if (curr_token.TokenType == TokenType.MINUS)
                        {
                            Advance();
                            AST right = term();
                            resultTree.Node = new ASTMinus(resultTree.Node, right.Node);
                        }
                    }
                }
                else
                {
                    // During Recursion avoid overwriting value of resulttree
                    AST tempResult = term();
                    while (curr_token != null && tempResult.Node != null && AddMinus.Contains(curr_token.TokenType))
                    {
                        if (curr_token.TokenType == TokenType.PLUS)
                        {
                            Advance();
                            AST right = term();
                            resultTree.Node = new ASTPlus(tempResult.Node, right.Node);

                        }
                        else if (curr_token.TokenType == TokenType.MINUS)
                        {
                            Advance();
                            AST right = term();
                            resultTree.Node = new ASTMinus(tempResult.Node, right.Node);
                        }
                    }

                    return tempResult;
                }
                
            }
            

            return resultTree;
        }

        public AST term()
        {
            AST result = new AST();
            result = factor();
            //
            while (curr_token != null && DivMultiply.Contains(curr_token.TokenType))
            {
                if (curr_token.TokenType == TokenType.MULTIPLY)
                {
                    Advance();
                    AST right = factor();
                    result.Node = new ASTMultiply(result.Node, right.Node);

                }
                else if(curr_token.TokenType == TokenType.DIVIDE)
                {
                    Advance();
                    AST right = factor();
                    result.Node = new ASTDivide(result.Node, right.Node);
                }
                
            }
            return result;
        }
        public AST factor()
        {
            AST result = new AST();
            if (curr_token.TokenType == TokenType.LPAREN)
            {
                Advance();
                result.Node = expression();// recursion to get next input
                
                if (curr_token == null)
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
                result.Node = new ASTNumber(curr_token.TokenValue);                
            }
            Advance();
            return result;

        }
    }
}

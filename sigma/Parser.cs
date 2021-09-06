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
        
        public IASTNode expression()
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
                    bool isAdded =LocalAssignment.TryAdd(variable, resultTree.Eval());

                    if (!isAdded)
                    {
                        LocalAssignment[variable] = resultTree.Eval(); // Update
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
                    resultTree = term();
                    while (curr_token != null && resultTree != null && AddMinus.Contains(curr_token.TokenType))
                    {
                        if (curr_token.TokenType == TokenType.PLUS)
                        {
                            Advance();
                            IASTNode right = term();
                            resultTree = new ASTPlus(resultTree, right);

                        }
                        else if (curr_token.TokenType == TokenType.MINUS)
                        {
                            Advance();
                            IASTNode right = term();
                            resultTree = new ASTMinus(resultTree, right);
                        }
                    }
                }
                else
                {
                    // During Recursion avoid overwriting value of resulttree
                    IASTNode tempResult = term();
                    while (curr_token != null && tempResult != null && AddMinus.Contains(curr_token.TokenType))
                    {
                        if (curr_token.TokenType == TokenType.PLUS)
                        {
                            Advance();
                            IASTNode right = term();
                            resultTree = new ASTPlus(tempResult, right);

                        }
                        else if (curr_token.TokenType == TokenType.MINUS)
                        {
                            Advance();
                            IASTNode right = term();
                            resultTree = new ASTMinus(tempResult, right);
                        }
                    }

                    return tempResult;
                }
                
            }
            

            return resultTree;
        }

        public IASTNode term()
        {
            IASTNode result = factor();
            //
            while (curr_token != null && DivMultiply.Contains(curr_token.TokenType))
            {
                if (curr_token.TokenType == TokenType.MULTIPLY)
                {
                    Advance();
                    IASTNode right = factor();
                    result = new ASTMultiply(result, right);

                }
                else if(curr_token.TokenType == TokenType.DIVIDE)
                {
                    Advance();
                    IASTNode right = factor();
                    result = new ASTDivide(result, right);
                }
                
            }
            return result;
        }
        public IASTNode factor()
        {
            IASTNode result = null;
            if (curr_token.TokenType == TokenType.LPAREN)
            {
                Advance();
                result = expression();// recursion to get next input
                
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
                result = new ASTNumber(curr_token.TokenValue);                
            }
            else if (curr_token.TokenType == TokenType.STRING)
            {
                result = new ASTString(curr_token.TokenValue);
            }
            Advance();
            return result;

        }
    }
}

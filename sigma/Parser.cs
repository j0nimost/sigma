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
                string variable = (string)curr_token.TokenValue;
                // validate EQ
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
                resultTree = Expression();
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

    }
}

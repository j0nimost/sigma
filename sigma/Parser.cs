using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public class Parser
    {
        // Resolve the Lexer to Nodes
        private List<TokenType> AddMinus = new List<TokenType> { TokenType.PLUS, TokenType.MINUS };
        private List<TokenType> DivMultiply = new List<TokenType> { TokenType.DIVIDE, TokenType.MULTIPLY };
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

        public AST factor()
        {
            return null;
            
        }

        public AST expression()
        {
            resultTree.Node = term();
            Advance();
            while(curr_token != null && resultTree.Node != null)
            {
                if(curr_token.TokenType == TokenType.PLUS)
                {
                    Advance();
                    ASTNumber right = term();
                    resultTree.Node = new ASTPlus(resultTree.Node, right);
                    

                }
                else if(curr_token.TokenType == TokenType.MINUS)
                {
                    Advance();
                    ASTNumber right = term();
                    resultTree.Node = new ASTMinus(resultTree.Node, right);
                }

                Advance();
            }

            return resultTree;
        }

        public ASTNumber term()
        {
            if(curr_token.TokenType ==  TokenType.NUMBER)
            {
                ASTNumber number = new ASTNumber(curr_token.TokenValue);
                return number;
            }
            return null;
        }

    }
}

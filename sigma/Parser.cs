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
        private ASTNode resultTree = new ASTNode();
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

        public ASTNode factor()
        {
            return null;
            
        }

        public ASTNode expression()
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
                    Console.Write(resultTree.Node.ToString() + "\n");
                    Console.Write(resultTree.Node.Eval());
                    
                }

                Advance();
            }
            return null;
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

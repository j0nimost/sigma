﻿using System;
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

        
        public AST expression()
        {
            resultTree = term();
            while(curr_token != null && resultTree.Node != null && AddMinus.Contains(curr_token.TokenType))
            {
                if(curr_token.TokenType == TokenType.PLUS)
                {
                    Advance();
                    AST right = term();
                    resultTree.Node = new ASTPlus(resultTree.Node, right);
                    

                }
                else if(curr_token.TokenType == TokenType.MINUS)
                {
                    Advance();
                    AST right = term();
                    resultTree.Node = new ASTMinus(resultTree.Node, right);
                }

            }

            return resultTree;
        }

        public AST term()
        {
            AST result = new AST();
            result.Node = factor();
            //
            while (curr_token != null && DivMultiply.Contains(curr_token.TokenType))
            {
                if (curr_token.TokenType == TokenType.MULTIPLY)
                {
                    Advance();
                    ASTNumber right = factor();
                    result.Node = new ASTMultiply(result.Node, right);

                }
            }
            return result;
        }
        public ASTNumber factor()
        {
            ASTNumber number = null;
            if(curr_token.TokenType == TokenType.NUMBER)
            {
                number = new ASTNumber(curr_token.TokenValue);
            }

            Advance();
            return number;

        }
    }
}

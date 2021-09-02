using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public abstract class IASTNode
    {
        public abstract object Eval();
    }

    public class ASTNumber: IASTNode
    {
        public readonly decimal leafValue;
        public ASTNumber(decimal leafVal)
        {
            this.leafValue = leafVal;
        }

        public override object Eval()
        {
            return leafValue;
        }

        public override string ToString()
        {
            return  this.leafValue.ToString();
        }
    }


    public class ASTPlus: IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTPlus(IASTNode leftNode, IASTNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public override string ToString()
        {
            return "(" +LeftNode.ToString() + " + " + RightNode.ToString() + ")";
        }

        public override object Eval()
        {
            return (decimal)LeftNode.Eval() + (decimal)RightNode.Eval();
        }

    }

    public class ASTMinus: IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTMinus(IASTNode leftNode, IASTNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public override string ToString()
        {
            return "(" + LeftNode.ToString() + "- " + RightNode.ToString() + ")";
        }

        public override object Eval()
        {
            return (decimal)LeftNode.Eval() - (decimal)RightNode.Eval();
        }
    }

    public class ASTMultiply : IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTMultiply(IASTNode leftNode, IASTNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public override string ToString()
        {
            return "(" + LeftNode.ToString() + " * " + RightNode.ToString() + ")";
        }

        public override object Eval()
        {
            return (decimal)LeftNode.Eval() * (decimal)RightNode.Eval();
        }
    }

    public class ASTDivide : IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTDivide(IASTNode leftNode, IASTNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public override string ToString()
        {
            return "(" + LeftNode.ToString() + " / " + RightNode.ToString() + ")";
        }

        public override object Eval()
        {
            return (decimal)LeftNode.Eval() / (decimal)RightNode.Eval();
        }
    }
}

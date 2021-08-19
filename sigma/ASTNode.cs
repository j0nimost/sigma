using System;
using System.Collections.Generic;
using System.Text;

namespace sigma
{
    public abstract class IASTNode
    {
        public abstract decimal Eval();
    }
    public class ASTNode: IASTNode
    {
        public dynamic Node { get; set; } // store any type of node

        public override decimal Eval()
        {
            return Node.Eval();
        }
    }

    public class ASTNumber
    {
        public decimal LeafValue;
        public ASTNumber(decimal leafVal)
        {
            LeafValue = leafVal;
        }

        public override string ToString()
        {
            return "("+ LeafValue.ToString() + ")";
        }
    }


    public class ASTPlus: IASTNode
    {
        public ASTNumber LeftNode;
        public ASTNumber RigthNode;

        public ASTPlus(ASTNumber leftNode, ASTNumber rightNode)
        {
            LeftNode = leftNode;
            RigthNode = rightNode;
        }

        public override string ToString()
        {
            return LeftNode.ToString() + " + " + RigthNode.ToString();
        }

        public override decimal Eval()
        {
            return LeftNode.LeafValue + RigthNode.LeafValue;
        }

    }
}

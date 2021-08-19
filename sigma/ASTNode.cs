﻿using System;
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

    public class ASTNumber: IASTNode
    {
        public decimal leafValue;
        public ASTNumber(decimal leafVal)
        {
            this.leafValue = leafVal;
        }

        public override decimal Eval()
        {
            return leafValue;
        }

        public override string ToString()
        {
            return "(" + this.leafValue.ToString() + ")";
        }
    }


    public class ASTPlus: IASTNode
    {
        public dynamic LeftNode;
        public dynamic RigthNode;

        public ASTPlus(dynamic leftNode, dynamic rightNode)
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
            return LeftNode.Eval() + RigthNode.Eval();
        }

    }
}

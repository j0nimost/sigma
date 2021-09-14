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
        public readonly long leafValue;
        public ASTNumber(long leafVal)
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

    public class ASTString : IASTNode
    {
        public readonly string stringNode;
        public ASTString(string stringNode)
        {
            this.stringNode = stringNode;
        }
        public override object Eval()
        {
            return this.stringNode;
        }

        public override string ToString()
        {
            return this.stringNode;
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
            return (long)LeftNode.Eval() + (long)RightNode.Eval();
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
            return (long)LeftNode.Eval() - (long)RightNode.Eval();
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
            return (long)LeftNode.Eval() * (long)RightNode.Eval();
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
            return (long)LeftNode.Eval() / (long)RightNode.Eval();
        }
    }

    public class ASTBitwiseAND : IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTBitwiseAND(IASTNode leftNode, IASTNode rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
        }

        public override string ToString()
        {
            return "(" + LeftNode.ToString() + " & " + RightNode.ToString() + ")";
        }
        public override object Eval()
        {
            return (long)this.LeftNode.Eval() & (long)this.RightNode.Eval();
        }
    }

    public class ASTBitwiseOR : IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTBitwiseOR(IASTNode leftnode, IASTNode rightnode)
        {
            this.LeftNode = leftnode;
            this.RightNode = rightnode;
        }

        public override string ToString()
        {
            return "(" + LeftNode.ToString() + " | " + RightNode.ToString() + ")";
        }
        public override object Eval()
        {
            return (long)this.LeftNode.Eval() | (long)this.RightNode.Eval();
        }
    }

    public class ASTBitwiseXOR : IASTNode
    {
        public readonly IASTNode LeftNode;
        public readonly IASTNode RightNode;

        public ASTBitwiseXOR(IASTNode leftNode, IASTNode rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
        }

        public override string ToString()
        {
            return "(" + LeftNode.ToString() + " ^ " + RightNode.ToString() + ")";
        }
        public override object Eval()
        {
            return (long)this.LeftNode.Eval() ^ (long)this.RightNode.Eval();
        }
    }
}

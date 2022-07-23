using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace ByLearningDSA.DataStructure
{
    public class BinaryTreeTraversalTest
    {
        public BinaryTree standardTree;
        public List<IComparable> outputDataRec;
        public List<IComparable> outputDataNoRec;
        public BinaryTreeTraversalTest()
        {
            BinaryTree num1 = new BinaryTree() { Obj = 1 };
            BinaryTree num2 = new BinaryTree() { Obj = 2 };
            BinaryTree num3 = new BinaryTree() { Obj = 3 };
            BinaryTree num4 = new BinaryTree() { Obj = 4 };
            BinaryTree num5 = new BinaryTree() { Obj = 5 };
            BinaryTree num6 = new BinaryTree() { Obj = 6 };
            BinaryTree num7 = new BinaryTree() { Obj = 7 };
            BinaryTree num8 = new BinaryTree() { Obj = 8 };
            BinaryTree num9 = new BinaryTree() { Obj = 9 };
            standardTree = num1;
            standardTree.LeftChild = num2;
            standardTree.RightChild = num3;
            num2.LeftChild = num4;
            num2.RightChild = num5;
            num3.LeftChild = num6;
            num3.RightChild = num7;
        }
        /// <summary>
        /// 二叉树的前序遍历
        /// </summary>
        [Fact]
        public void PreOrderTraversalTest()
        {
            outputDataRec = new List<IComparable>();
            PreOrderRecursively(standardTree);
            outputDataRec.Count.ShouldBe(7);
            outputDataNoRec = new List<IComparable>();
            PreOrderWithStack(standardTree);
            outputDataNoRec.Count.ShouldBe(7);
            outputDataNoRec.ShouldBe(outputDataRec);
        }
        /// <summary>
        /// 递归方法
        /// </summary>
        /// <param name="root"></param>
        private void PreOrderRecursively(BinaryTree root)
        {
            if (root == null)
                return;
            outputDataRec.Add(root.Obj);
            PreOrderRecursively(root.LeftChild);
            PreOrderRecursively(root.RightChild);
        }
        /// <summary>
        /// 使用栈
        /// </summary>
        /// <param name="root"></param>
        private void PreOrderWithStack(BinaryTree root)
        {
            Stack<BinaryTree> stack = new Stack<BinaryTree>();
            BinaryTree tmp = root;
            while (tmp != null || stack.Count > 0)
            {
                while (tmp != null)
                {
                    outputDataNoRec.Add(tmp.Obj);
                    stack.Push(tmp);
                    tmp = tmp.LeftChild;
                }
                if (stack.Count > 0)
                {
                    tmp = stack.Pop();
                    tmp = tmp.RightChild;
                }
            }
        }
        /// <summary>
        /// 二叉树的中序遍历
        /// </summary>
        [Fact]
        public void InOrderTraersalTest()
        {
            outputDataRec = new List<IComparable>();
            InOderRecursively(standardTree);
            outputDataRec.Count.ShouldBe(7);
            outputDataNoRec = new List<IComparable>();
            InOrderWithStack(standardTree);
            outputDataNoRec.Count.ShouldBe(7);
            outputDataNoRec.ShouldBe(outputDataRec);
        }
        private void InOderRecursively(BinaryTree root)
        {
            if (root == null)
                return;
            InOderRecursively(root.LeftChild);
            outputDataRec.Add(root.Obj);
            InOderRecursively(root.RightChild);
        }
        private void InOrderWithStack(BinaryTree root)
        {
            Stack<BinaryTree> stack = new Stack<BinaryTree>();
            BinaryTree tmp = root;
            while (tmp != null || stack.Count > 0)
            {
                while (tmp != null)
                {
                    stack.Push(tmp);
                    tmp = tmp.LeftChild;
                }
                if (stack.Count > 0)
                {
                    tmp = stack.Pop();
                    outputDataNoRec.Add(tmp.Obj);
                    tmp = tmp.RightChild;
                }
            }
        }
        /// <summary>
        /// 二叉树的后序遍历
        /// </summary>
        [Fact]
        public void PostOrderTraversalTest()
        {
            outputDataRec = new List<IComparable>();
            PostOrderRecursively(standardTree);
            outputDataRec.Count.ShouldBe(7);
            outputDataNoRec = new List<IComparable>();
            PostOrderWithStack(standardTree);
            outputDataNoRec.Count.ShouldBe(7);
            outputDataNoRec.ShouldBe(outputDataRec);
        }
        private void PostOrderRecursively(BinaryTree root)
        {
            if (root == null)
                return;
            PostOrderRecursively(root.LeftChild);
            PostOrderRecursively(root.RightChild);
            outputDataRec.Add(root.Obj);
        }
        private void PostOrderWithStack(BinaryTree root)
        {
            Stack<BinaryTreeWithFlag> stack = new Stack<BinaryTreeWithFlag>();
            BinaryTree bTreeNode = root;
            BinaryTreeWithFlag bTreeWithFlag;
            while (bTreeNode != null || stack.Count > 0)
            {
                while (bTreeNode != null)
                {
                    bTreeWithFlag = new BinaryTreeWithFlag();
                    bTreeWithFlag.Node = bTreeNode;
                    bTreeWithFlag.IsFirst = true;
                    stack.Push(bTreeWithFlag);
                    bTreeNode = bTreeNode.LeftChild;
                }
                if (stack.Count > 0)
                {
                    bTreeWithFlag = stack.Peek();
                    stack.Pop();
                    if (bTreeWithFlag.IsFirst)
                    {
                        bTreeWithFlag.IsFirst = false;
                        stack.Push(bTreeWithFlag);
                        bTreeNode = bTreeWithFlag.Node.RightChild;
                    }
                    else
                    {
                        outputDataNoRec.Add(bTreeWithFlag.Node.Obj);
                        bTreeNode = null;
                    }
                }
            }
        }

        internal class BinaryTreeWithFlag
        {
            public BinaryTree Node;
            public bool IsFirst;
        }
    }
}

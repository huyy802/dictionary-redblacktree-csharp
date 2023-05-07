using System;
using System.Collections;
using System.Collections.Generic;

namespace RedBlackTreeImplementation
{
    public enum NodeColor
    {
        Red,
        Black
    }

    public class RedBlackTree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
    {
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Traverse(_root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> Traverse(Node node)
        {
            if (node != null)
            {
                foreach (KeyValuePair<TKey, TValue> pair in Traverse(node.Left))
                {
                    yield return pair;
                }

                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);

                foreach (KeyValuePair<TKey, TValue> pair in Traverse(node.Right))
                {
                    yield return pair;
                }
            }
        }
        public KeyValuePair<TKey, TValue> GetRandomNode()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The tree is empty.");
            }

            int randomIndex = new Random().Next(Count);
            Node node = GetNodeAtPosition(_root, randomIndex);
            return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }

        private Node GetNodeAtPosition(Node node, int position)
        {
            int leftCount = GetCount(node.Left);
            if (position < leftCount)
            {
                return GetNodeAtPosition(node.Left, position);
            }
            else if (position > leftCount)
            {
                return GetNodeAtPosition(node.Right, position - leftCount - 1);
            }
            else
            {
                return node;
            }
        }

        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public NodeColor Color { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int Count { get; set; }

            public Node(TKey key, TValue value, NodeColor color)
            {
                Key = key;
                Value = value;
                Color = color;
                Count = 1;
            }
        }

        private Node _root;

        public int Count => _root?.Count ?? 0;

        public bool IsEmpty => _root == null;

        public bool ContainsKey(TKey key)
        {
            return GetNode(key) != null;
        }

        public TValue this[TKey key]
        {
            get
            {
                Node node = GetNode(key);
                if (node == null)
                {
                    throw new KeyNotFoundException();
                }
                return node.Value;
            }
            set
            {
                Insert(key, value);
            }
        }

        private Node GetNode(TKey key)
        {
            Node current = _root;
            while (current != null)
            {
                int cmp = key.CompareTo(current.Key);
                if (cmp == 0)
                {
                    return current;
                }
                else if (cmp < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
            return null;
        }

        public void Insert(TKey key, TValue value)
        {
            _root = Insert(_root, key, value);
            _root.Color = NodeColor.Black;
        }

        private Node Insert(Node node, TKey key, TValue value)
        {
            if (node == null)
            {
                return new Node(key, value, NodeColor.Red);
            }

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
            {
                node.Left = Insert(node.Left, key, value);
            }
            else if (cmp > 0)
            {
                node.Right = Insert(node.Right, key, value);
            }
            else
            {
                node.Value = value;
            }

            if (IsRed(node.Right) && !IsRed(node.Left))
            {
                node = RotateLeft(node);
            }
            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = RotateRight(node);
            }
            if (IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColors(node);
            }

            node.Count = 1 + GetCount(node.Left) + GetCount(node.Right);
            return node;
        }

        public void Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
                return;
            }

            if (!IsRed(_root.Left) && !IsRed(_root.Right))
            {
                _root.Color = NodeColor.Red;
            }

            _root = Remove(_root, key);
            if (!IsEmpty)
            {
                _root.Color = NodeColor.Black;
            }
        }

        private Node Remove(Node node, TKey key)
        {
            int cmp = key.CompareTo(node.Key);
            if (cmp < 0)
            {
                if (!IsRed(node.Left) && !IsRed(node.Left.Left))
                {
                    node = MoveRedLeft(node);
                }
                node.Left = Remove(node.Left, key);
            }
            else
            {
                if (IsRed(node.Left))
                {
                    node = RotateRight(node);
                }
                if (cmp == 0 && node.Right == null)
                {
                    return null;
                }
                if (!IsRed(node.Right) && !IsRed(node.Right.Left))
                {
                    node = MoveRedRight(node);
                }
                if (cmp == 0)
                {
                    Node minNode = GetMinNode(node.Right);
                    node.Key = minNode.Key;
                    node.Value = minNode.Value;
                    node.Right = DeleteMin(node.Right);
                }
                else
                {
                    node.Right = Remove(node.Right, key);
                }
            }

            node.Count = 1 + GetCount(node.Left) + GetCount(node.Right);
            return Balance(node);
        }

        private Node MoveRedLeft(Node node)
        {
            FlipColors(node);
            if (IsRed(node.Right.Left))
            {
                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);
                FlipColors(node);
            }
            return node;
        }

        private Node MoveRedRight(Node node)
        {
            FlipColors(node);
            if (!IsRed(node.Left.Left))
            {
                node = RotateRight(node);
                FlipColors(node);
            }
            return node;
        }

        private Node DeleteMin(Node node)
        {
            if (node.Left == null)
            {
                return null;
            }
            if (!IsRed(node.Left) && !IsRed(node.Left.Left))
            {
                node = MoveRedLeft(node);
            }
            node.Left = DeleteMin(node.Left);
            return Balance(node);
        }

        private Node GetMinNode(Node node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }

        private Node Balance(Node node)
        {
            if (IsRed(node.Right))
            {
                node = RotateLeft(node);
            }
            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = RotateRight(node);
            }
            if (IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColors(node);
            }

            node.Count = 1 + GetCount(node.Left) + GetCount(node.Right);
            return node;
        }

        private Node RotateLeft(Node node)
        {
            Node temp = node.Right;
            node.Right = temp.Left;
            temp.Left = node;
            temp.Color = node.Color;
            node.Color = NodeColor.Red;
            node.Count = 1 + GetCount(node.Left) + GetCount(node.Right);
            return temp;
        }

        private Node RotateRight(Node node)
        {
            Node temp = node.Left;
            node.Left = temp.Right;
            temp.Right = node;
            temp.Color = node.Color;
            node.Color = NodeColor.Red;
            node.Count = 1 + GetCount(node.Left) + GetCount(node.Right);
            return temp;
        }

        private void FlipColors(Node node)
        {
            node.Color = InvertColor(node.Color);
            node.Left.Color = InvertColor(node.Left.Color);
            node.Right.Color = InvertColor(node.Right.Color);
        }

        private NodeColor InvertColor(NodeColor color)
        {
            return color == NodeColor.Red ? NodeColor.Black : NodeColor.Red;
        }

        private bool IsRed(Node node)
        {
            return node != null && node.Color == NodeColor.Red;
        }


        private int GetCount(Node node)
        {
            return node?.Count ?? 0;
        }
    }
}
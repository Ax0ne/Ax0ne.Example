/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/11/6 10:59:07
 *  FileName:StackRealize.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/

namespace Example.ConsoleApp
{
    public interface IStack<T>
    {
        /// <summary>
        ///     往栈里新增一个元素
        /// </summary>
        /// <param name="s">The s.</param>
        void Push(T s);

        /// <summary>
        ///     删除并返回最新添加的一个元素
        /// </summary>
        /// <returns>T.</returns>
        T Pop();

        /// <summary>
        ///     栈是否有元素
        /// </summary>
        /// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
        bool IsEmpty();

        /// <summary>
        ///     栈里有多少个元素
        /// </summary>
        /// <returns>System.Int32.</returns>
        int Size();
    }

    /// <summary>
    ///     栈的实现
    ///     <para>后进先出 Last in first out</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackRealize<T> : IStack<T>
    {
        private Node _first;
        private int _number;

        /// <summary>
        ///     Pushes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        public void Push(T s)
        {
            // 推入元素就是当前最新的元素,并把他的Next指向旧的First
            Node old = _first; // 存放原最新元素
            _first = new Node(); // 实例化最新元素
            _first.Item = s;
            _first.Next = old; // 当前最新元素指向 原最新元素
            _number++; // 计数器加一
        }

        public T Pop()
        {
            // 删除并返回最新的元素 并使最新元素的Next成为最新元素
            T item = _first.Item;
            _first = _first.Next; // 最新的元素就是当前最新的下一个元素
            _number--;
            return item;
        }

        public bool IsEmpty()
        {
            return _first == null;
        }

        public int Size()
        {
            return _number;
        }

        public class Node
        {
            public T Item { get; set; }

            public Node Next { get; set; }
        }
    }
}
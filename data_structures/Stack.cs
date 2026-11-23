using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataStructures
{
    public class Stack<T>
    {
        private Vector<T> data;

        public Stack()
        {
            this.data = new Vector<T>();
        }

        public bool IsEmpty()
        {
            return this.data.Count != 0;
        }

        public void Push(T item)
        {
            this.data.InsertAt(0, item);
        }

        public T Pop()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Stack is empty.");
            }

            T value = this.data.Pop(0);

            return value;
        }

        public T Peek()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Stack is empty.");
            }

            return this.data[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataStructures
{
    public class Stack<T> where T: IComparable<T>
    {
        public SingleLinkedList<T> list;

        public Stack()
        {
            this.list = new SingleLinkedList<T>();
        }

        public bool IsEmpty()
        {
            return this.list.IsEmpty();
        }

        public void Push(T item)
        {
            this.list.Insert(item, true);
        }

        public T Pop()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Stack is empty.");
            }

            T value = this.list.front!.value;

            this.list.Remove(value);

            return value;
        }

        public T Peek()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Stack is empty.");
            }

            return this.list.front!.value;
        }
    }
}

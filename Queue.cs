using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Queue<T> where T: IComparable<T>
    {
        public SingleLinkedList<T> list;

        public Queue()
        {
            this.list = new SingleLinkedList<T>();
        }

        public bool IsEmpty()
        {
            return this.list.IsEmpty();
        }

        public void Enqueue(T item)
        {
            this.list.Insert(item, false);
        }

        public T Dequeue()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Queue is empty.");
            }

            T value = this.list.front!.value;

            this.list.Remove(value);

            return value;
        }

        public T Front()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Queue is empty.");
            }

            return this.list.front!.value;
        }
    }
}

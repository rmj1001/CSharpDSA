using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Queue<T>
    {
        public Vector<T> data;

        public Queue()
        {
            this.data = new Vector<T>();
        }

        public bool IsEmpty()
        {
            return this.data.Count != 0;
        }

        public void Enqueue(T item)
        {
            this.data.Append(item);
        }

        public T Dequeue()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Queue is empty.");
            }

            return this.data.Pop();
        }

        public T Front()
        {
            if (this.IsEmpty())
            {
                throw new Exception("Queue is empty.");
            }

            return this.data[-1];
        }
    }
}

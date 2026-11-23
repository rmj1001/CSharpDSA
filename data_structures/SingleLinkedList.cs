using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    /// <summary>
    /// A dynamic-sized linear list of connected nodes with values
    /// of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleLinkedList<T> where T: IComparable<T>
    {
        public SingleLinkedNode<T>? front;
        public SingleLinkedNode<T>? back;
        public uint _length;

        /// <summary>
        /// Construct a SingleLinkedList without starting data
        /// </summary>
        public SingleLinkedList()
        {
            this.front = null;
            this.back = null;
            this._length = 0;
        }

        /// <summary>
        /// Construct a SingleLinkedList with a starting array of data
        /// </summary>
        /// <param name="arr">Starting data</param>
        /// <param name="front">Append items to front or back</param>
        public SingleLinkedList(T[] arr, bool front = false)
        {
            foreach(T item in arr)
            {
                this.Insert(item, front);
            }
        }

        /// <summary>
        /// Get the length of the Linked List
        /// </summary>
        public uint Length { get { return this._length; } }

        /// <summary>
        /// Check if the Linked List contains any data
        /// </summary>
        /// <returns>true if data exists, false otherwise</returns>
        public bool IsEmpty()
        {
            return this.front == null;
        }

        /// <summary>
        /// Insert data into the Linked List, either at the front
        /// or the back of the list.
        /// </summary>
        /// <param name="value">The value to insert</param>
        /// <param name="front">Insert at front or back</param>
        public void Insert(T value, bool front = false)
        {
            if (this.IsEmpty())
            {
                this.front = new SingleLinkedNode<T>(value);
                this.back = this.front;
                this._length++;
                return;
            }

            if (front)
            {
                var temp = this.front;
                this.front = new SingleLinkedNode<T>(value, temp);
                this._length++;
            }

            else
            {
                this.back!.next = new SingleLinkedNode<T>(value);
                this.back = this.back.next;
                this._length++;
            }
        }

        /// <summary>
        /// Remove data from the linked list
        /// </summary>
        /// <param name="value">The value to remove</param>
        public void Remove(T value)
        {
            if (this.front == null)
            {
                return;
            }

            if (this.front == this.back)
            {
                if (!Equals(this.front.value, value)) return;

                this.front = null;
                this.back = null;
                this._length--;
                return;
            }

            if (Equals(this.front.value, value))
            {
                this.front = this.front.next;
                this._length--;
                return;
            }

            var previous = this.front;
            var current = previous.next;

            while (current != null)
            {
                if (!Equals(current.value, value))
                {
                    previous = current;
                    current = current.next;
                    continue;
                }

                if (current == this.back)
                {
                    this.back = previous;
                }

                previous.next = current.next;
                this._length--;
                return;
            }
        }

        /// <summary>
        /// Convert the linked list to an array
        /// </summary>
        /// <returns>Array of elements of type T</returns>
        public T[] ToArray()
        {
            if (this.IsEmpty())
            {
                return [];
            }

            T[] arr = new T[this.Length];

            var current = this.front;
            int idx = 0;

            while (current != null)
            {
                arr[idx] = current.value;
                current = current.next;
                idx++;
            }

            return arr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class SingleLinkedNode<T> where T: IComparable<T>
    {
        // This is the data the node holds
        public T value;

        // This is the next node, if it exists. The ? means it could be null (nothing)
        public SingleLinkedNode<T>? next;

        // This creates a new queue node. Because I set next = null, it assumes the next node
        // will be null unless you pass one to that parameter.
        //
        // Ex.
        // queueNode(1, queueNode(10)) -> this sets the current node to 1 and adds a next node
        // queueNode(1) -> this sets the current node to 1 and does not add a next node
        public SingleLinkedNode(T value, SingleLinkedNode<T>? next = null)
        {
            this.value = value;
            this.next = next;
        }
    }
}

using DataStructures.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Set<T> : Vector<T>, IEquatable<Set<T>>
    {
        public Set() : base() { }

        public Set(IEnumerable<T> items): base()
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "items cannot be null.");
            }

            foreach(T item in items)
            {
                this.Append(item);
            }
        }

        public bool Equals(Set<T>? other)
        {
            if (other == null) return false;
            if (this.Count != other.Count) return false;
            if (ReferenceEquals(this, other)) return true;

            foreach(T item in this)
            {
                if (!other.Contains(item)) return false;
            }

            return true;
        }

        public override bool Equals(object? other)
        {
            return other is Set<T> set && this.Equals(set);
        }

        public override int GetHashCode()
        {
            int hash = 0;

            foreach(T item in this)
            {
                if (item == null)
                {
                    hash ^= 0;
                    continue;
                }

                hash ^= EqualityComparer<T>.Default.GetHashCode(item);
            }

            return hash;
        }

        private void BaseAppend(T item)
        {
            base.Append(item);
        }

        public override Set<T> Append(T item, bool inPlace = true)
        {
            Set<T> set = inPlace ? this : new Set<T>(this);

            if (!set.Contains(item))
            {
                set.BaseAppend(item);
            }

            return set;
        }

        public override Set<T> InsertAt(int index, T item)
        {
            if (!this.Contains(item)) base.InsertAt(index, item);

            return this;
        }

        public override Set<T> SetAt(int index, T item)
        {
            if (Comparer<T>.EQ(this[index], item) || !this.Contains(item))
            {
                base.SetAt(index, item);
            }

            return this;
        }
    }
}

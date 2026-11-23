using DataStructures.Utilities;
using System.Collections;

namespace DataStructures
{
    public class Vector<T> : IEnumerable<T>, IEquatable<Vector<T>>
    {
        /// <summary>
        /// Internal capacity variable that gets updated when the vector grows/shrinks
        /// </summary>
        private int _capacity;

        /// <summary>
        /// Internal count variable that gets updated when items are added/removed
        /// </summary>
        private int _count;

        /// <summary>
        /// Fixed-size array containing Vector's elements
        /// </summary>
        private T[] _data;

        /// <summary>
        /// Multiply/divide Vector capacity by this when growing/shrinking
        /// </summary>
        private const int ResizeBy = 2;

        /// <summary>
        /// This is the total possible number of slots the current vector
        /// can hold data in.
        /// </summary>
        public int Capacity { get { return _capacity; } }

        /// <summary>
        /// This gets the number of items stored in the vector, not the
        /// full capacity of the vector.
        /// </summary>
        public int Count { get { return this._count; } }

        /// <summary>
        /// This is the number of elements that should be in the vector
        /// in order to trigger automatic shrinking.
        /// </summary>
        private int DecreaseAt { get { return this._capacity / 4; } }

        /// <summary>
        /// This gets the index of the last present item in the vector.
        /// </summary>
        public int LastIndex { get { return this._count - 1; } }

        /// <summary>
        /// Create a blank vector with an optional starting capacity defined.
        /// </summary>
        /// <param name="capacity">Optional starting capacity</param>
        /// <exception cref="ArgumentOutOfRangeException">The capacity has to be greater than 1.</exception>
        public Vector(int capacity = 10)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("Vector capacity must be at least 1.");
            }

            this._capacity = capacity;
            this._count = 0;
            this._data = new T[capacity];
        }

        /// <summary>
        /// Create a vector from an optional starting fixed-sized array
        /// </summary>
        /// <param name="start">The starting array</param>
        /// <exception cref="ArgumentNullException">The starting array cannot be null.</exception>
        public Vector(T[] start)
        {
            if (start == null)
            {
                throw new ArgumentNullException("Starting array cannot be null.");
            }

            if (start.Length > 0)
            {
                this._capacity = start.Length * ResizeBy;
            }
            else
            {
                this._capacity = 10;
            }

            this._data = new T[this._capacity];

            foreach(var item in start)
            {
                this.Append(item);
            }
        }

        /// <summary>
        /// This allows callers to get/set data in the vector using
        /// name[index] syntax.
        /// </summary>
        /// <param name="index">The index of the item in the vector</param>
        /// <returns>T or Self</returns>
        public T this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        /// <summary>
        /// Check to see if all items in the vector return true
        /// when passed to the predicate function fn
        /// </summary>
        /// <param name="fn">Predicate function</param>
        /// <returns>true if all items return true in predicate, else false</returns>
        public bool All(Func<T, bool> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Function cannot be null.");
            }

            foreach (T item in this)
            {
                if (!fn(item)) return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if any items in the vector return true
        /// when passed to the predicate function fn
        /// </summary>
        /// <param name="fn">Predicate function</param>
        /// <returns>true if at least one item returns true in predicate, else false</returns>
        public bool Any(Func<T, bool> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Function cannot be null.");
            }

            foreach (T item in this)
            {
                if (fn(item)) return true;
            }

            return false;
        }

        /// <summary>
        /// Add an item to the end of the vector. Calls TryGrow at the
        /// beginning in case the vector needs to be resized.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Self</returns>
        public virtual Vector<T> Append(T item, bool inPlace = true)
        {
            Vector<T> vec = this;

            if (!inPlace)
            {
                vec = this.Duplicate();
            }

            vec.TryGrow();

            vec._data[vec.Count] = item;

            vec._count++;

            return vec;
        }

        /// <summary>
        /// Empty the vector and resize it to the default capacity (10 items).
        /// </summary>
        /// <returns>Self</returns>
        public Vector<T> Clear()
        {
            this._capacity = 10;
            this._data = new T[this._capacity];
            this._count = 0;

            return this;
        }

        /// <summary>
        /// Check if the vector contains an item.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>true if item exists, otherwise false</returns>
        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        /// <summary>
        /// Helper function that copies elements from an input array
        /// to an output array, optionally setting the number of
        /// items to copy.
        /// </summary>
        /// <param name="input">The input array (items to copy)</param>
        /// <param name="output">The output array (destination for copied items)</param>
        /// <param name="count">The number of items to copy</param>
        /// <returns>Copied array of items</returns>
        /// <exception cref="ArgumentNullException">input cannot be null.</exception>
        /// <exception cref="IndexOutOfRangeException">Count must be between 0 and the length of the input array</exception>
        private static T[] CopyArray(T[] input, T[]? output = null, int count = -1)
        {
            if (input == null)
            {
                throw new ArgumentNullException("Input array must not be null.");
            }

            if (count == -1)
            {
                count = input.Length;
            }

            if (count < 0 || count > input.Length)
            {
                throw new IndexOutOfRangeException($"Count must be between 0 and {input.Length}");
            }

            if (output == null)
            {
                output = new T[count];
            }

            if (output.Length < count)
            {
                throw new IndexOutOfRangeException($"Output array needs to be at least of size {count}.");
            }

            for (int i = 0; i < count; i++)
            {
                output[i] = input[i];
            }

            return output;
        }

        /// <summary>
        /// Create a copy of this vector. Used primarily for non-destructive sorting, but
        /// can be used in other cases.
        /// </summary>
        /// <returns>Duplicated vector</returns>
        public virtual Vector<T> Duplicate()
        {
            return this.Slice();
        }

        /// <summary>
        /// Check to see if two vectors are equal to each other
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>true if both vectors equal each other, false otherwise</returns>
        public bool Equals(Vector<T>? other)
        {
            if (other == null) return false;
            if (this.Count != other.Count) return false;
            if (ReferenceEquals(this, other)) return true;

            for (int i = 0; i <= this.LastIndex; i++)
            {
                if (Utilities.Comparer<T>.NE(this[i], other[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if this vector is equal to another object of unknown type
        /// </summary>
        /// <param name="obj">Object of unknown type</param>
        /// <returns>true if both objects are equal, false otherwise</returns>
        public override bool Equals(object? obj)
        {
            return obj is Vector<T> other && Equals((Vector<T>)obj);
        }

        /// <summary>
        /// Append items from another Enumerable object to the end of
        /// the vector.
        /// </summary>
        /// <param name="items">The items to append to this vector</param>
        /// <param name="inPlace">Whether or not to append to existing vector, or create a new duplicated vector and append items to that.</param>
        /// <returns>Self, or new vector if inPlace is false</returns>
        /// <exception cref="ArgumentNullException">items cannot be null</exception>
        public Vector<T> Extend(IEnumerable<T> items, bool inPlace = true)
        {
            if (items == null)
            {
                throw new ArgumentNullException($"{nameof(items)} cannot be null.");
            }

            Vector<T> newVector = this;

            if (!inPlace)
            {
                newVector = this.Duplicate();
            }

            foreach (T item in items)
            {
                newVector.Append(item);
            }

            return newVector;
        }

        /// <summary>
        /// Create a new Vector that only adds item from the current one
        /// if they are passed to fn and return true
        /// </summary>
        /// <param name="fn">The predicate function</param>
        /// <returns>New vector</returns>
        public Vector<T> Filter(Func<T, bool> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Function cannot be null.");
            }

            Vector<T> newVector = new Vector<T>();

            foreach (T item in this)
            {
                if (fn(item))
                {
                    newVector.Append(item);
                }
            }

            return newVector;
        }
        
        /// <summary>
        /// Find the first index of an item which returns true when
        /// passed as a parameter to fn
        /// </summary>
        /// <param name="fn">Function that takes T item and returns bool</param>
        /// <returns>Index, or -1 if not found</returns>
        public int FindIf(Func<T, bool> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "predicate function cannot be null");
            }

            for (int i = 0; i <= this.LastIndex; i++)
            {
                if (fn(this[i])) return i;
            }

            return -1;
        }

        /// <summary>
        /// Creates a new vector with the current contents, and takes a function 
        /// that passes T as a parameter and returns a Vector, extendig the new
        /// vector with the result of fn.
        /// </summary>
        /// <typeparam name="U">The type of the new vector's elements</typeparam>
        /// <param name="fn">The function taking T and resulting in U Vector</param>
        /// <returns>U Vector</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Vector<U> FlatMap<U>(Func<T, Vector<U>> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Flatmap function cannot be null.");
            }

            Vector<U> newVector = new Vector<U>();

            foreach(T item in this)
            {
                Vector<U> inner = fn(item);

                if (inner != null)
                {
                    newVector.Extend(inner);
                }
            }

            return newVector;
        }

        /// <summary>
        /// Run a function on each item in the vector that does
        /// some side effect(s).
        /// </summary>
        /// <param name="fn">The function taking one item T as a parameter</param>
        public Vector<T> ForEach(Action<T> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Function cannot be null.");
            }

            foreach (T item in this)
            {
                fn(item);
            }

            return this;
        }

        /// <summary>
        /// Get the item located at the provided index.
        /// </summary>
        /// <param name="index">Index of item</param>
        /// <returns>Item</returns>
        public T GetAt(int index)
        {
            index = this.NormalizeIndex(index);
            return this._data[index];
        }

        /// <summary>
        /// Convert this vector to an Enumerable object.
        /// </summary>
        /// <returns>Iterator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this._count; i++)
            {
                yield return this._data[i];
            }
        }

        /// <summary>
        /// Create a hash code integer from all elements in the Vector
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            this.ForEach((T item) => hash.Add(item));

            return hash.ToHashCode();
        }

        /// <summary>
        /// Get the first index of an item in the vector, or -1
        /// if it does not exist.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>-1 if it does not exist, otherwise positive index</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < this._count; i++)
            {
                if (Utilities.Comparer<T>.EQ(this[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Insert an item at the provided index, shifting all
        /// items after it one space to the right.
        /// </summary>
        /// <param name="index">The index where the item will be inserted</param>
        /// <param name="item">The item to insert into the vector</param>
        /// <returns>Self</returns>
        public virtual Vector<T> InsertAt(int index, T item)
        {
            index = this.NormalizeIndex(index, true);
            this.TryGrow();
            T new_value = item;

            for (int i = index; i <= this._count; i++)
            {
                T temp = this._data[i];
                this._data[i] = new_value;
                new_value = temp;
            }

            this._count++;
            return this;
        }

        /// <summary>
        /// Create a new Vector that changes each element in the
        /// current one using a function.
        /// </summary>
        /// <param name="fn">The transformation function</param>
        /// <returns>New vector</returns>
        public Vector<U> Map<U>(Func<T, U> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Function cannot be null.");
            }

            Vector<U> newVector;

            if (this.Count == 0)
            {
                newVector = new Vector<U>();
            }
            else
            {
                newVector = new Vector<U>(this.Count);
            }
            
            foreach(T item in this)
            {
                newVector.Append(fn(item));
            }

            return newVector;
        }

        /// <summary>
        /// Check to see if no items in the vector return true
        /// when passed to the predicate function fn
        /// </summary>
        /// <param name="fn">Predicate function</param>
        /// <returns>true if no items return true in predicate, else false</returns>
        public bool None(Func<T, bool> fn)
        {
            return !this.Any(fn);
        }

        /// <summary>
        /// Helper function that converts negative indexes into
        /// positive ones, and also checks to make sure that indexes
        /// are valid for functions that use them.
        /// </summary>
        /// <param name="index">The index to validate</param>
        /// <param name="allowEnd">Allow an index to equal the length of the vector (used for 'Vector.InsertAt()')</param>
        /// <returns>The validated/converted index</returns>
        /// <exception cref="IndexOutOfRangeException">Converted index needs to be between 0 and the last index of the vector</exception>
        private int NormalizeIndex(int index, bool allowEnd = false)
        {
            if (index < 0)
            {
                index += this.Count;
            }

            if (allowEnd && index == this.Count)
            {
                return index;
            }

            if (index > this.LastIndex)
            {
                throw new IndexOutOfRangeException($"Index {index} is greater than last index {this.LastIndex}.");
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException($"Negative index {index} is less than 0 when normalized.");
            }

            return index;
        }

        /// <summary>
        /// Create a new vector from a single item
        /// </summary>
        /// <param name="item">The item to create a vector from</param>
        /// <returns>New vector</returns>
        public static Vector<T> Of(T item)
        {
            return new Vector<T>(1).Append(item);
        }

        /// <summary>
        /// Remove an item from the vector and return its value. Pops the last item by default.
        /// </summary>
        /// <param name="index">Optional index to pop from the vector.</param>
        /// <returns>The item popped</returns>
        public T Pop(int index = -1)
        {
            index = this.NormalizeIndex(index);
            T value = this._data[index];

            for (int i = index; i < this._count; i++)
            {
                if (i == this.LastIndex)
                {
                    this._data[i] = default(T)!;
                }
                else
                {
                    this._data[i] = this._data[i + 1];
                }
            }

            this._count--;
            this.TryShrink();
            return value;
        }

        /// <summary>
        /// Reduce all elements in the vector to a single value
        /// using a function.
        /// 
        /// Ex.
        ///     int Sum(int n1, int n2)
        ///     {
        ///         return n1 + n2;
        ///     }
        ///     
        ///     int total = 0;
        ///     Vector&lt;int&gt; numbers = new Vector&lt;int&gt;([0, 1, 2, 3, 4, 5]);
        ///     total = numbers.Reduce(0, (int acc, int item) => Sum(acc, item));
        /// </summary>
        /// <typeparam name="U">The type of the accumulated value</typeparam>
        /// <param name="initial">Initial value for the accumulator</param>
        /// <param name="fn">The function that reduces the vector to a single value. Takes an accumulator value and the current item in the vector, returning a new accumulated value.</param>
        /// <returns>A value U</returns>
        public U Reduce<U>(U initial, Func<U, T, U> fn)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Function cannot be null.");
            }

            U runningTotal = initial;

            foreach(T item in this)
            {
                runningTotal = fn(runningTotal, item);
            }

            return runningTotal;
        }

        /// <summary>
        /// Remove the first instance of an item from the vector
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>Self</returns>
        public Vector<T> Remove(T item)
        {
            int index = this.IndexOf(item);

            if (index == -1)
            {
                return this;
            }

            this.Pop(index);
            return this;
        }

        /// <summary>
        /// Shrink the vector to an arbitrary size between 1 and the current length.
        /// </summary>
        /// <param name="newSize">The new size of the vector</param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentOutOfRangeException">newSize needs to be between 1 and current vector length</exception>
        public Vector<T> Resize(int newSize)
        {
            if (newSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(newSize), "Size of new vector must be at least 1.");
            }

            if (newSize > this._count)
            {
                throw new ArgumentOutOfRangeException(nameof(newSize), "Size of new vector must not exceed current vector's length.");
            }


            T[] newArray = new T[newSize];

            Vector<T>.CopyArray(this._data, newArray, newSize);

            this._data = newArray;
            this._capacity = newSize;
            this._count = newSize;

            return this;
        }

        /// <summary>
        /// Reverse the elements of the vector in-place
        /// </summary>
        /// <returns>Self</returns>
        public Vector<T> Reverse(bool inPlace = true)
        {
            void swap(Vector<T> vec, int first, int second)
            {
                T temp = vec[second];
                vec[second] = vec[first];
                vec[first] = temp;
            }

            Vector<T> reversedVector = this;

            if (!inPlace)
            {
                reversedVector = this.Duplicate();
            }

            for (int first = 0; first < reversedVector.Count / 2; first++)
            {
                int second = reversedVector.LastIndex - first;
                swap(reversedVector, first, second);
            }

            return reversedVector;
        }

        /// <summary>
        /// LINQ selector function delegating functionality
        /// to Vector.Map()
        /// </summary>
        /// <typeparam name="U">New type resulting from selector function</typeparam>
        /// <param name="selector">Called on every element in Vector, creating a new element</param>
        /// <returns>New Vector</returns>
        public Vector<U> Select<U>(Func<T, U> selector)
        {
            return this.Map(selector);
        }
        
        /// <summary>
        /// Used for chaining multiple select from operations with C# LINQ.
        /// </summary>
        public Vector<V> SelectMany<U, V>(Func<T, Vector<U>> collectionSelector, Func<T, U, V> resultSelector)
        {
            if (collectionSelector == null)
            {
                throw new ArgumentNullException(nameof(collectionSelector), "Collection selector cannot be null.");
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector), "Result selector cannot be null.");
            }

            Vector<V> newVector = new Vector<V>();

            foreach(T outerItem in this)
            {
                Vector<U> innerVector = collectionSelector(outerItem);

                if (innerVector == null) continue;

                foreach(U innerItem in innerVector)
                {
                    newVector.Append(resultSelector(outerItem, innerItem));
                }
            }

            return newVector;
        }

        /// <summary>
        /// Set a value at the provided index, overwriting any data that was there.
        /// </summary>
        /// <param name="index">The index of the item</param>
        /// <param name="item">The item to add to the vector</param>
        /// <returns>Self</returns>
        public virtual Vector<T> SetAt(int index, T item)
        {
            index = this.NormalizeIndex(index);
            this._data[index] = item;

            return this;
        }

        /// <summary>
        /// This shrinks the vector to its length. Use this only if the vector's size
        /// will be fixed, or if memory optimization is needed.
        /// </summary>
        /// <returns>Self</returns>
        public Vector<T> ShrinkToSize()
        {
            if (this._count == 0)
            {
                this._capacity = 10;
            }
            else
            {
                this._capacity = this._count;
            }

            T[] new_vector = new T[this._capacity];

            Vector<T>.CopyArray(this._data, new_vector, this._count);
            this._data = new_vector;

            return this;
        }

        /// <summary>
        /// Returns a slice with all elements in the vector, although skipping
        /// over each [count] elements
        /// </summary>
        /// <param name="count">Number of elements to skip</param>
        /// <returns>New Vector</returns>
        public Vector<T> Skip(int count)
        {
            if (count <= 0) return this.Duplicate();
            if (count >= this.Count) return new Vector<T>();
            return this.Slice(start: count);
        }

        /// <summary>
        /// Create a new Dynamic vector from the current one with defined bounds and
        /// skipping if desired.
        /// </summary>
        /// <param name="start">The first element of the vector to copy</param>
        /// <param name="end">The last element of the vector to copy</param>
        /// <param name="step">How many elements to skip when copying</param>
        /// <returns>A new Vector</returns>
        /// <exception cref="Exception">The vector has to step by at least 1.</exception>
        public Vector<T> Slice(int start = 0, int end = -1, int step = 1)
        {
            if (step < 1)
            {
                throw new Exception("Step must be 1 or more.");
            }

            if (this._count == 0)
            {
                return new Vector<T>();
            }

            start = this.NormalizeIndex(start);
            end = this.NormalizeIndex(end);

            Vector<T> new_vector = new Vector<T>();

            for (int current = start; current <= end; current += step)
            {
                new_vector.Append(this[current]);
            }

            return new_vector;
        }

        /// <summary>
        /// Sort the vector using a function that compares two elements and returns a boolean. Only swaps
        /// elements if the function returns true.
        /// </summary>
        /// <param name="fn">The sorting function</param>
        /// <param name="reverse">Whether to reverse the sorted vector</param>
        /// <param name="inPlace">Whether to sort this vector or create a copy to sort and return</param>
        /// <returns>Self, or new Vector if inPlace is false</returns>
        public Vector<T> SortBy(Func<T, T, bool> fn, bool reverse = false, bool inPlace = true)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "sorting function cannot be null.");
            }

            Vector<T> sorted = this;

            if (!inPlace)
            {
                sorted = this.Duplicate();
            }

            if (sorted.Count <= 1)
            {
                return sorted;
            }

            int mid = sorted.Count / 2;
            Vector<T> left = sorted.Slice(start: 0, end: mid - 1).SortBy(fn, inPlace: true);
            Vector<T> right = sorted.Slice(start: mid).SortBy(fn, inPlace: true);

            int i = 0;
            int j = 0;
            int k = 0;

            while (i < left.Count && j < right.Count)
            {
                if (!fn(left[i], right[j]))
                {
                    sorted[k++] = left[i++];
                }
                else
                {
                    sorted[k++] = right[j++];
                }
            }

            while (i < left.Count)
            {
                sorted[k++] = left[i++];
            }

            while (j < right.Count)
            {
                sorted[k++] = right[j++];
            }

            if (reverse) sorted.Reverse();

            return sorted;
        }

        /// <summary>
        /// Returns a slice that takes the specified number of elements
        /// from the initial vector, starting from the beginning
        /// </summary>
        /// <param name="count"></param>
        /// <returns>New vector</returns>
        public Vector<T> Take(int count)
        {
            if (count <= 0) return new Vector<T>();
            if (count >= this.Count) return this.Duplicate();
            return this.Slice(end: count - 1);
        }

        /// <summary>
        /// Convert the dynamic vector to a fixed-size array
        /// </summary>
        /// <returns>Fixed-size array</returns>
        public T[] ToArray()
        {
            T[] new_array = new T[this._count];
            Vector<T>.CopyArray(this._data, new_array, this._count);

            return new_array;
        }

        /// <summary>
        /// Create a new vector where each item is unique.
        /// </summary>
        /// <returns>New vector</returns>
        public Set<T> ToSet()
        {
            return new Set<T>(this);
        }

        /// <summary>
        /// Create a string from the vector, resulting in "[item, item, ...]"
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return $"[{string.Join(", ", this)}]";
        }

        /// <summary>
        /// Try to get an item at the provided index.
        /// </summary>
        /// <param name="index">The index to retrive from</param>
        /// <param name="value">Value variable to update if element found</param>
        /// <returns>true if retrieval is successful, false otherwise.</returns>
        public bool TryGetAt(int index, out T? value)
        {
            try
            {
                value = this[index];
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// Checks if the vector needs to be resized. If so then
        /// it will double the current capacity of the vector.
        /// </summary>
        /// <param name="force">Skip resizing requirements and force the vector to grow</param>
        private void TryGrow(bool force = false)
        {
            if (force || this._count == this._capacity)
            {
                int new_capacity = this._capacity * ResizeBy;
                T[] new_arr = new T[new_capacity];

                Vector<T>.CopyArray(this._data, new_arr, this._count);
                this._capacity = new_capacity;
                this._data = new_arr;
                return;
            }
        }

        /// <summary>
        /// Checks if the vector needs to be resized. If so then it will halve the
        /// current capacity of the vector.
        /// </summary>
        /// <param name="force">Skip resizing requirements and force the vector to shrink</param>
        private void TryShrink(bool force = false)
        {
            if (force || this._count <= this.DecreaseAt)
            {
                int new_capacity = this._capacity / ResizeBy;

                if (new_capacity < 1)
                {
                    new_capacity = 1;
                }

                if (this._capacity == new_capacity)
                {
                    return;
                }

                T[] new_arr = new T[new_capacity];

                Vector<T>.CopyArray(this._data, new_arr, this._count);
                this._capacity = new_capacity;
                this._data = new_arr;
                return;
            }
        }

        /// <summary>
        /// LINQ function that delegates filtering to
        /// Vector.Filter()
        /// </summary>
        /// <param name="predicate">Function that takes an element and returns a boolean</param>
        /// <returns>Filtered vector</returns>
        public Vector<T> Where(Func<T, bool> predicate)
        {
            return this.Filter(predicate);
        }

        /// <summary>
        /// Zip together two vectors into one vector, so that each element in
        /// the two vectors are still located at their original indexes inside
        /// some other data structure.
        /// </summary>
        /// <typeparam name="U">Other vector's element type</typeparam>
        /// <typeparam name="V">New vector's element type</typeparam>
        /// <param name="other">Other vector</param>
        /// <param name="fn">Zipping function producing a data structure containing each element</param>
        /// <returns>New vector</returns>
        /// <exception cref="ArgumentNullException">other and fn cannot be null</exception>
        /// <exception cref="Exception">The two vectors need to have the same number of elements</exception>
        public Vector<V> Zip<U, V>(Vector<U> other, Func<T, U, V> fn)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other), "Other vector cannot be null.");
            }

            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "fn cannot be null.");
            }

            if (this.Count != other.Count)
            {
                throw new Exception($"{nameof(other)} does not have the same number of elements as the first vector.");
            }

            Vector<V> newVector = new Vector<V>();
            
            for (int i = 0; i <= this.LastIndex; i++)
            {
                newVector.Append(fn(this[i], other[i]));
            }

            return newVector;
        }
          
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class VectorComparableExtensions
    {
        /// <summary>
        /// Sort the vector in-place using the Merge sort algorithm. Uses Vector.Merge() as a helper function.
        /// </summary>
        /// <param name="reverseVec">Whether or not to reverse the elements after sorting</param>
        /// <returns>Self</returns>
        public static Vector<T> Sort<T>(this Vector<T> vector, bool reverseVec = false, bool inPlace = true) where T : IComparable<T>
        {
            if (vector == null)
            {
                throw new ArgumentNullException(nameof(vector), "Vector cannot be null.");
            }

            if (!reverseVec)
            {
                return vector.SortBy((T first, T second) => Utilities.Comparer<T>.GT(first, second), reverse: false, inPlace);
            }

            return vector.SortBy((T first, T second) => Utilities.Comparer<T>.LT(first, second), reverse: false, inPlace);
        }
    }
}

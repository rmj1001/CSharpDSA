namespace DataStructures
{
    public class BSNode
    {
        public int value;
        public BSNode? left;
        public BSNode? right;

        public BSNode(int value, BSNode? left = null, BSNode? right = null)
        {
            this.value = value;
            this.left = left;
            this.right = right;
        }
    }

    public class BSTree
    {
        public BSNode? root;

        public BSTree(BSNode? root = null)
        {
            this.root = root;
        }

        // Check if root does not exist
        public bool IsEmpty()
        {
            if (this.root == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Insert a node based on if it is greater or less than a node
        // in the tree. If it is equal to a node, don't create a new node.
        public BSNode? Insert(int value)
        {
            BSNode? result = this.InsertRecursive(this.root, value);

            if (this.root == null)
            {
                this.root = result;
            }

            return result;
        }

        public BSNode? InsertRecursive(BSNode? node, int value)
        {
            if (node == null)
            {
                return new BSNode(value);
            }

            if (value == node.value)
            {
                return node;
            }

            else if (value < node.value)
            {
                node.left = this.InsertRecursive(node.left, value);
            }

            else if (value > node.value)
            {
                node.right = this.InsertRecursive(node.right, value);
            }

            return node;
        }

        // Determine if a value is present in the tree
        public bool HasValue(int value)
        {
            return this.HasValueRecursive(this.root, value);
        }

        public bool HasValueRecursive(BSNode? node, int value)
        {
            if (node == null)
            {
                return false;
            }

            if (node.value == value)
            {
                return true;
            }

            if (value < node.value)
            {
                return this.HasValueRecursive(node.left, value);
            }

            if (value > node.value)
            {
                return this.HasValueRecursive(node.right, value);
            }

            return false;
        }

        // Get a node from the tree if it contains the value
        public BSNode? GetNode(int value)
        {
            return this.GetNodeRecursive(this.root, value);
        }

        public BSNode? GetNodeRecursive(BSNode? node, int value)
        {
            if (node == null)
            {
                return null;
            }

            if (node.value == value)
            {
                return node;
            }

            if (value < node.value)
            {
                return this.GetNodeRecursive(node.left, value);
            }

            if (value > node.value)
            {
                return this.GetNodeRecursive(node.right, value);
            }

            return null;
        }

        // Remove a node from the tree with the value while
        // preserving its descendant nodes (nodes on left/right
        // from the deleted node)
        public void Delete(int value)
        {
            this.root = this.DeleteRecursive(this.root, value);
        }

        public BSNode? DeleteRecursive(BSNode? node, int value)
        {
            if (node == null)
            {
                return null;
            }

            if (value < node.value)
            {
                node.left = this.DeleteRecursive(node.left, value);
            }

            else if (value > node.value)
            {
                node.right = this.DeleteRecursive(node.right, value);
            }

            else
            {
                if (node.left == null)
                {
                    BSNode? temp = node.right;
                    node = null;
                    return temp;
                }

                else if (node.right == null)
                {
                    BSNode? temp = node.left;
                    node = null;
                    return temp;
                }

                int? minimum = this.MinimumValue(node.right);

                if (minimum == null)
                {
                    return node;
                }
                else
                {
                    node.value = (int)minimum;
                    node.right = this.DeleteRecursive(node.right, node.value);
                }
            }

            return node;
        }

        // Get the left-most value from a node
        // in the tree
        public int? MinimumValue(BSNode? node = null)
        {
            if (node == null)
            {
                if (this.root == null)
                {
                    return null;
                }
                else
                {
                    node = this.root;
                }
            }

            while (node.left != null)
            {
                node = node.left;
            }

            return node.value;
        }

        // Create a list of values from the tree
        // Root value, left values, right values
        public List<int> PreOrder()
        {
            List<int> items = new List<int>();
            
            this.PreOrderRecursive(this.root, items);

            return items;
        }

        public void PreOrderRecursive(BSNode? node, List<int> list)
        {
            if (node == null)
            {
                return;
            }
            else
            {
                list.Add(node.value);
                this.PreOrderRecursive(node.left, list);
                this.PreOrderRecursive(node.right, list);
            }
        }

        // Create a list of values from the tree
        // left values, Root value, right values
        public List<int> InOrder()
        {
            List<int> items = new List<int>();

            this.InOrderRecursive(this.root, items);

            return items;
        }

        public void InOrderRecursive(BSNode? node, List<int> list)
        {
            if (node == null)
            {
                return;
            }
            else
            {
                this.InOrderRecursive(node.left, list);
                list.Add(node.value);
                this.InOrderRecursive(node.right, list);
            }
        }

        // Create a list of values from the tree
        // left values, right values, Root value
        public List<int> PostOrder()
        {
            List<int> items = new List<int>();

            this.PostOrderRecursive(this.root, items);

            return items;
        }

        public void PostOrderRecursive(BSNode? node, List<int> list)
        {
            if (node == null)
            {
                return;
            }
            else
            {
                this.PostOrderRecursive(node.left, list);
                this.PostOrderRecursive(node.right, list);
                list.Add(node.value);
            }
        }
    }
}

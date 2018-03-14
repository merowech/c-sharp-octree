// ----------------------------------
// Namespace:   CSharpOctree
// Class:       Octree
// Author:      Udo Schlegel
// Company:     DBVIS
// ----------------------------------

using System.Collections.Generic;

namespace CSharpOctree {

    public class Octree
    {
        private OctreeNode _root;

        public Octree(double x_min, double x_max, double y_min, double y_max, double z_min, double z_max)
        {
            this._root = new OctreeNode(x_min, x_max, y_min, y_max, z_min, z_max, 25);
        }

        public Octree(double x_min, double x_max, double y_min, double y_max, double z_min, double z_max, int capacity)
        {
            this._root = new OctreeNode(x_min, x_max, y_min, y_max, z_min, z_max, capacity);
        }

        public bool Add(double x, double y, double z, object obj)
        {
            return this._root.AddLeaf(x, y, z, obj);
        }

        public int getDepth()
        {
            return this._root.getDepth();
        }

        public int getNumberOfOuterNodes()
        {
            return this._root.getNumberOfOuterNodes();
        }

        public List<OctreeLeaf> getRepresentation()
        {
            return this._root.getRepresentation();
        }

        public string printTree()
        {
            string ret = "Root:\n";
            ret += this._root.toString();
            return ret;
        }
    }
}

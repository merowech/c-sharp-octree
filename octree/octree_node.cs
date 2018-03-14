using System;
using System.Collections.Generic;

namespace CSharpOctree {

    public class OctreeNode
    {
        private OctreeBounds _bounds;
        private OctreeNode[] _childs;
        /*
         * Cube:
         *   4 5
         * 2 3 6
         * 0 1
         * Not visible 7
         */
        private List<OctreeLeaf> _leafs;
        private int _capacity;
        private bool _innerNode;

        public OctreeNode(double x_min, double x_max, double y_min, double y_max, double z_min, double z_max, int capacity)
        {
            this._bounds = new OctreeBounds(x_min, x_max, y_min, y_max, z_min, z_max);

            this._leafs = new List<OctreeLeaf>();

            this._innerNode = false;

            this._capacity = capacity;
        }

        private bool _divide()
        {
            double _size_x = (this._bounds.getXMax() - this._bounds.getXMin()) / 2;
            double _size_y = (this._bounds.getYMax() - this._bounds.getYMin()) / 2;
            double _size_z = (this._bounds.getZMax() - this._bounds.getZMin()) / 2;

            this._childs = new OctreeNode[8];

            this._childs[0] = new OctreeNode(
            this._bounds.getXMin(), this._bounds.getXMax() - _size_x,
            this._bounds.getYMin(), this._bounds.getYMax() - _size_y,
            this._bounds.getZMin(), this._bounds.getZMax() - _size_z,
            this._capacity);

            this._childs[1] = new OctreeNode(
            this._bounds.getXMin() + _size_x, this._bounds.getXMax(),
            this._bounds.getYMin(), this._bounds.getYMax() - _size_y,
            this._bounds.getZMin(), this._bounds.getZMax() - _size_z,
            this._capacity);

            this._childs[2] = new OctreeNode(
            this._bounds.getXMin(), this._bounds.getXMax() - _size_x,
            this._bounds.getYMin() + _size_y, this._bounds.getYMax(),
            this._bounds.getZMin(), this._bounds.getZMax() - _size_z,
            this._capacity);

            this._childs[3] = new OctreeNode(
            this._bounds.getXMin() + _size_x, this._bounds.getXMax(),
            this._bounds.getYMin() + _size_y, this._bounds.getYMax(),
            this._bounds.getZMin(), this._bounds.getZMax() - _size_z,
            this._capacity);

            this._childs[4] = new OctreeNode(
            this._bounds.getXMin(), this._bounds.getXMax() - _size_x,
            this._bounds.getYMin() + _size_y, this._bounds.getYMax(),
            this._bounds.getZMin() + _size_z, this._bounds.getZMax(),
            this._capacity);

            this._childs[5] = new OctreeNode(
            this._bounds.getXMin() + _size_x, this._bounds.getXMax(),
            this._bounds.getYMin() + _size_y, this._bounds.getYMax(),
            this._bounds.getZMin() + _size_z, this._bounds.getZMax(),
            this._capacity);

            this._childs[6] = new OctreeNode(
            this._bounds.getXMin() + _size_x, this._bounds.getXMax(),
            this._bounds.getYMin(), this._bounds.getYMax() - _size_y,
            this._bounds.getZMin() + _size_z, this._bounds.getZMax(),
            this._capacity);

            this._childs[7] = new OctreeNode(
            this._bounds.getXMin(), this._bounds.getXMax() - _size_x,
            this._bounds.getYMin(), this._bounds.getYMax() - _size_y,
            this._bounds.getZMin() + _size_z, this._bounds.getZMax(),
            this._capacity);

            for (int i = 0; i < this._leafs.Count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (this._childs[j].AddLeaf(this._leafs[i]))
                    {
                        break;
                    }
                }
            }

            this._innerNode = true;
            this._leafs = null;

            return true;
        }

        public bool AddLeaf(double x, double y, double z, object obj)
        {
            if (!this._bounds.InBound(x, y, z))
            {
                return false;
            }

            if (!this._innerNode && this._leafs.Count + 1 > this._capacity)
            {
                this._divide();
            }

            if (this._innerNode)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this._childs[i].AddLeaf(x, y, z, obj))
                    {
                        break;
                    }
                }
            }
            else
            {
                this._leafs.Add(new OctreeLeaf(x, y, z, obj));
            }

            return true;
        }

        public bool AddLeaf(OctreeLeaf leaf)
        {
            double x = leaf.getX();
            double y = leaf.getY();
            double z = leaf.getZ();

            if (!this._bounds.InBound(x, y, z))
            {
                return false;
            }

            if (this._leafs.Count + 1 > this._capacity)
            {
                this._divide();
            }

            if (this._innerNode)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this._childs[i].AddLeaf(leaf))
                    {
                        break;
                    }
                }
            }
            else
            {
                this._leafs.Add(leaf);
            }

            return true;
        }

        public int getDepth()
        {
            if (this._innerNode)
            {
                int max = 0;

                for (int i = 0; i < 8; i++)
                {
                    int depth = this._childs[i].getDepth() + 1;
                    if (depth > max)
                    {
                        max = depth;
                    }
                }

                return max;
            }
            else
            {
                return 1;
            }
        }

        public int getNumberOfOuterNodes()
        {
            if (this._innerNode)
            {
                int sum = 0;

                for (int i = 0; i < 8; i++)
                {
                    sum += this._childs[i].getNumberOfOuterNodes();

                }

                return sum;
            }
            else
            {
                return 1;
            }
        }

        public List<OctreeLeaf> getRepresentation()
        {
            if (this._innerNode)
            {
                List<OctreeLeaf> ret = new List<OctreeLeaf>();

                for (int i = 0; i < 8; i++)
                {
                    ret.AddRange(this._childs[i].getRepresentation());
                }

                return ret;
            }
            else
            {
                OctreeLeaf representation = getMedoid();

                List<OctreeLeaf> ret = new List<OctreeLeaf>();
                ret.Add(representation);
                return ret;
            }
        }

        private OctreeLeaf getCentroid()
        {
            double min = double.MaxValue;
            OctreeLeaf representation = null;

            for (int i = 0; i < this._leafs.Count; i++)
            {
                double dist = this._bounds.getDistanceToCenter(this._leafs[i]);
                if (dist < min)
                {
                    min = dist;
                    representation = this._leafs[i];
                }
            }

            return representation;
        }

        private OctreeLeaf getMedoid()
        {
            double x = 0, y = 0, z = 0;

            for (int i = 0; i < this._leafs.Count; i++)
            {
                x += this._leafs[i].getX();
                y += this._leafs[i].getY();
                z += this._leafs[i].getZ();
            }

            x = x/this._leafs.Count;
            y = y/this._leafs.Count;
            z = z/this._leafs.Count;

            double min = double.MaxValue;
            OctreeLeaf representation = null;

            for (int i = 0; i < this._leafs.Count; i++)
            {
                double sum = 0.0d;

                sum += Math.Pow(this._leafs[i].getX() - x, 2);
                sum += Math.Pow(this._leafs[i].getY() - y, 2);
                sum += Math.Pow(this._leafs[i].getZ() - z, 2);

                double dist = Math.Sqrt(sum);
                if (dist < min)
                {
                    min = dist;
                    representation = this._leafs[i];
                }
            }

            return representation;
        }

        public string toString()
        {
            string ret = "";

            if (this._innerNode)
            {
                ret = "Children:\n";
                for (int i = 0; i < 8; i++)
                {
                    ret += this._childs[i].toString() + "\n\n\n";
                }
            }
            else
            {
                for (int i = 0; i < this._leafs.Count; i++)
                {
                    ret += "\t" + this._leafs[i].toString() + "\n";
                }
                ret += "\n";
                if (this.getRepresentation() != null && this.getRepresentation()[0] != null)
                {
                    ret += "Rep: " + this.getRepresentation()[0].toString() + "\n";
                }
            }

            return ret;
        }
    }
}

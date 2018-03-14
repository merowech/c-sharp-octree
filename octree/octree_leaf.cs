
namespace CSharpOctree {

    public class OctreeLeaf
    {
        private object _obj;
        private double _x, _y, _z;

        public OctreeLeaf(double x, double y, double z, object obj)
        {
            this._x = x;
            this._y = y;
            this._z = z;

            this._obj = obj;
        }

        public string toString()
        {
            return "[" + this._x + ", " + this._y + ", " + this._z + "]";
        }

        public double getX()
        {
            return this._x;
        }

        public double getY()
        {
            return this._y;
        }

        public double getZ()
        {
            return this._z;
        }

        public object getObj()
        {
            return this._obj;
        }
    }
}

using System;

namespace CSharpOctree {

    public class OctreeBounds
    {

        private double _x_min, _x_max;
        private double _y_min, _y_max;
        private double _z_min, _z_max;

        public OctreeBounds(double x_min, double x_max, double y_min, double y_max, double z_min, double z_max)
        {
            this._x_min = x_min;
            this._x_max = x_max;
            this._y_min = y_min;
            this._y_max = y_max;
            this._z_min = z_min;
            this._z_max = z_max;
        }

        public bool InBound(double x, double y, double z)
        {
            if (this._x_min < x
                && this._x_max > x
                && this._y_min < y
                && this._y_max > y
                && this._z_min < z
                && this._z_max > z)
            {
                return true;
            }

            return false;
        }

        public double getDistanceToCenter(OctreeLeaf oL)
        {
            double sum = 0.0d;

            sum += Math.Pow(oL.getX() - this.getXCenter(), 2);
            sum += Math.Pow(oL.getY() - this.getYCenter(), 2);
            sum += Math.Pow(oL.getZ() - this.getZCenter(), 2);

            return Math.Sqrt(sum);
        }

        public double getXCenter()
        {
            return this._x_min + (this._x_max - this._x_min) / 2;
        }

        public double getYCenter()
        {
            return this._y_min + (this._y_max - this._y_min) / 2;
        }

        public double getZCenter()
        {
            return this._z_min + (this._z_max - this._z_min) / 2;
        }

        public double getXMin()
        {
            return this._x_min;
        }

        public double getYMin()
        {
            return this._y_min;
        }

        public double getZMin()
        {
            return this._z_min;
        }

        public double getXMax()
        {
            return this._x_max;
        }

        public double getYMax()
        {
            return this._y_max;
        }

        public double getZMax()
        {
            return this._z_max;
        }
    }
}

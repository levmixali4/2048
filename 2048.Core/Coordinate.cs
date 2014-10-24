using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048.Core
{
    public struct Coordinate : IComparable, IComparable<Coordinate>, IEquatable<Coordinate>
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override string ToString()
        {
            return string.Format("X={0}; Y={1}", X, Y);
        }

        public override bool Equals(object obj)
        {
            if(obj is Coordinate)
                return this.Equals((Coordinate)obj);
            return false;
        }

        public bool Equals(Coordinate other)
        {            
            return this.CompareTo(other) == 0;
        }

        public int CompareTo(object obj)
        {
            if (obj is Coordinate)
                return this.CompareTo((Coordinate)obj);
            return -1;
        }

        public int CompareTo(Coordinate other)
        {
            int xComparision = this.X - other.X;
            int yComparision = this.Y - other.Y;
            if (xComparision == 0 && yComparision == 0)
                return 0;
            if (xComparision + yComparision > 0)
                return 1;
            if (xComparision + yComparision < 0)
                return -1;
            return -1;
        }
    }

    //public class CoordinateEqualityComparer : IEqualityComparer<Coordinate>
    //{
    //    public bool Equals(Coordinate x, Coordinate y)
    //    {
    //        return x.X == y.X && x.Y == y.Y;
    //    }

    //    public int GetHashCode(Coordinate obj)
    //    {
    //        return obj.GetHashCode();
    //    }
    //}
}

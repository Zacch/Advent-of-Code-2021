// ReSharper disable MemberCanBePrivate.Global
namespace Advent_of_Code_2021.Utilities;

public class Point3D :IEquatable<Point3D>, IComparable<Point3D>
{
    public readonly int X;
    public readonly int Y;
    public readonly int Z;

    public Point3D() {}
    public Point3D(int x, int y, int z) { X = x; Y = y; Z = z; }
    public Point3D(IReadOnlyList<int> coordinates) { X = coordinates[0]; Y = coordinates[1]; Z = coordinates[2]; }

    public bool IsOrigin => X == 0 && Y == 0 && Z == 0;
    
    public Point3D ToOrientation(int orientation) => 
        orientation switch
        {
            0 => new Point3D(X, Y, Z),
            1 => new Point3D(X, Z, -Y),
            2 => new Point3D(X, -Y, -Z),
            3 => new Point3D(X, -Z, Y),
            4 => new Point3D(-X, Y, -Z),
            5 => new Point3D(-X, Z, Y),
            6 => new Point3D(-X, -Y, Z),
            7 => new Point3D(-X, -Z, -Y),

            8 => new Point3D(Z, X, Y),
            9 => new Point3D(Z, Y, -X),
            10 => new Point3D(Z, -X, -Y),
            11 => new Point3D(Z, -Y, X),
            12 => new Point3D(-Z, X, -Y),
            13 => new Point3D(-Z, Y, X),
            14 => new Point3D(-Z, -X, Y),
            15 => new Point3D(-Z, -Y, -X),

            16 => new Point3D(Y, Z, X),
            17 => new Point3D(Y, X, -Z),
            18 => new Point3D(Y, -Z, -X),
            19 => new Point3D(Y, -X, Z),
            20 => new Point3D(-Y, Z, -X),
            21 => new Point3D(-Y, X, Z),
            22 => new Point3D(-Y, -Z, X),
            23 => new Point3D(-Y, -X, -Z),

            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };

    public Point3D FromOrientation(int orientation) =>
        orientation switch
        {
            0 => new Point3D(X, Y, Z),
            1 => new Point3D(X, -Z, Y),
            2 => new Point3D(X, -Y, -Z),
            3 => new Point3D(X, Z, -Y),
            4 => new Point3D(-X, Y, -Z),
            5 => new Point3D(-X, Z, Y),
            6 => new Point3D(-X, -Y, Z),
            7 => new Point3D(-X, -Z, -Y),

            8 => new Point3D(Y, Z, X),
            9 => new Point3D(-Z, Y, X),
            10 => new Point3D(-Y, -Z, X),
            11 => new Point3D(Z, -Y, X),
            12 => new Point3D(Y, -Z, -X),
            13 => new Point3D(Z, Y, -X),
            14 => new Point3D(-Y, Z, -X),
            15 => new Point3D(-Z, -Y, -X),

            16 => new Point3D(Z, X, Y),
            17 => new Point3D(Y, X, -Z),
            18 => new Point3D(-Z, X, -Y),
            19 => new Point3D(-Y, X, Z),
            20 => new Point3D(-Z, -X, Y),
            21 => new Point3D(Y, -X, Z),
            22 => new Point3D(Z, -X, -Y),
            23 => new Point3D(-Y, -X, -Z),

            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };
    
    public static Point3D operator +(Point3D a, Point3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Point3D operator -(Point3D a, Point3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    
    public Point3D DistanceTo(Point3D other) => new(Math.Abs(X - other.X), Math.Abs(Y - other.Y), Math.Abs(Z - other.Z));
    public long ManhattanDistanceTo(Point3D other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
    
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Point3D) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public bool Equals(Point3D? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override string ToString() { return $"({X}, {Y}, {Z})"; }

    public int CompareTo(Point3D? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var xComparison = X.CompareTo(other.X);
        if (xComparison != 0) return xComparison;
        var yComparison = Y.CompareTo(other.Y);
        if (yComparison != 0) return yComparison;
        return Z.CompareTo(other.Z);
    }
}
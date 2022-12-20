using System.Diagnostics.CodeAnalysis;

record Point
{
    public long X { get; set; }
    public long Y { get; set; }

    public long Z { get; set; }

    public Point(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point(IEnumerable<long> vector)
    {
        if (vector.Count() != 3)
            throw new ArgumentOutOfRangeException();

        this.X = vector.First();
        this.Y = vector.Skip(1).First();
        this.Z = vector.Skip(2).First();
    }
}

class Solution
{
    static void Main(string[] args)
    {
        var input0 = GetInput($"{Environment.CurrentDirectory}\\Input0.txt");
        var input1 = GetInput($"{Environment.CurrentDirectory}\\Input1.txt");

        Console.WriteLine($"Part 1, input 0: '{solutionPart1(input0)}'");
        Console.WriteLine($"Part 1, input 1: '{solutionPart1(input1)}'");
        Console.WriteLine($"Part 2, input 0: '{solutionPart2(input0)}'");
        Console.WriteLine($"Part 2, input 1: '{solutionPart2(input1)}'");
    }

    static string solutionPart1(string[] input)
    {
        var points = input.Select(l => l.Split(',').Select(long.Parse))
             .Select(v => new Point(v))
             .ToList();

        var minX = points.Select(p => p.X).Min();
        var maxX = points.Select(p => p.X).Max();
        var minY = points.Select(p => p.Y).Min();
        var maxY = points.Select(p => p.Y).Max();
        var minZ = points.Select(p => p.Z).Min();
        var maxZ = points.Select(p => p.Z).Max();

        var faces = getFaces(points);

        return faces.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var points = input.Select(l => l.Split(',').Select(long.Parse))
                          .Select(v => new Point(v))
                          .ToList();

        var checkedEmptyPoints = new List<Point>();

        var minX = points.Select(p => p.X).Min();
        var maxX = points.Select(p => p.X).Max();
        var minY = points.Select(p => p.Y).Min();
        var maxY = points.Select(p => p.Y).Max();
        var minZ = points.Select(p => p.Z).Min();
        var maxZ = points.Select(p => p.Z).Max();

        var pointsNegative = new List<Point>();

        getNegativeForm(points, pointsNegative, minX - 1, minY - 1, minZ - 1, maxX + 1, maxY + 1, maxZ + 1, minX - 1, minY - 1, minZ - 1);

        var faces = getFaces(pointsNegative);

        var lenghtX = (maxX + 1) - (minX - 1) + 1;
        var lenghtY = (maxY + 1) - (minY - 1) + 1;
        var lenghtZ = (maxZ + 1) - (minZ - 1) + 1;

        var sideA = lenghtX * lenghtY;
        var sideB = lenghtX * lenghtZ;
        var sideC = lenghtY * lenghtZ;

        return (faces - 2*sideA - 2*sideB - 2*sideC).ToString();
    }

    static List<Point> pointsChecked = new List<Point>();

    static int getFaces(List<Point> points)
    {
        var faceCount = points.Count * 6;

        for (int i = 0; i < points.Count() - 1; i++)
        {
            for (int j = i + 1; j < points.Count(); j++)
            {
                if ((points[i].X + 1 == points[j].X || points[i].X - 1 == points[j].X) && points[i].Y == points[j].Y && points[i].Z == points[j].Z)
                    faceCount -= 2;

                if ((points[i].Y + 1 == points[j].Y || points[i].Y - 1 == points[j].Y) && points[i].X == points[j].X && points[i].Z == points[j].Z)
                    faceCount -= 2;

                if ((points[i].Z + 1 == points[j].Z || points[i].Z - 1 == points[j].Z) && points[i].X == points[j].X && points[i].Y == points[j].Y)
                    faceCount -= 2;
            }
        }

        return faceCount;
    }

    static void getNegativeForm(List<Point> positivePoints, List<Point> negativePoints, long x, long y, long z, long maxX, long maxY, long maxZ, long minX, long minY, long minZ)
    {
        if (positivePoints.Any(p => p.X == x && p.Y == y && p.Z == z) == true ||
            negativePoints.Any(p => p.X == x && p.Y == y && p.Z == z) == true)
            return;

        if (x < minX || x > maxX || y < minY || y > maxY || z < minZ || z > maxZ)
            return;

        negativePoints.Add(new Point(x, y, z));

        getNegativeForm(positivePoints, negativePoints, x + 1, y, z, maxX, maxY, maxZ, minX, minY, minZ);
        getNegativeForm(positivePoints, negativePoints, x - 1, y, z, maxX, maxY, maxZ, minX, minY, minZ);
        getNegativeForm(positivePoints, negativePoints, x, y + 1, z, maxX, maxY, maxZ, minX, minY, minZ);
        getNegativeForm(positivePoints, negativePoints, x, y - 1, z, maxX, maxY, maxZ, minX, minY, minZ);
        getNegativeForm(positivePoints, negativePoints, x, y, z + 1, maxX, maxY, maxZ, minX, minY, minZ);
        getNegativeForm(positivePoints, negativePoints, x, y, z - 1, maxX, maxY, maxZ, minX, minY, minZ);
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
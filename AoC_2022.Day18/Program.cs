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
        return faceCount.ToString();
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

        var faceCount = 0;

        var isInsight = false;

        var pointsToMark = new List<Point>();

        pointsChecked = new List<Point>();

        for (var z = minZ; z <= maxZ; z++)
        {
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z))
                        continue;

                    if (checkedEmptyPoints.Any(p => p.X == x && p.Y == y && p.Z == z) == false)
                    {
                        if (x==2 && y==2 && z==5)
                        {

                        }

                        if (CheckPoints(points, x, y, z, maxX, maxY, maxZ, minX, minY, minZ) == true)
                        {
                            MarkPoints(points, x, y, z, maxX, maxY, maxZ, minX, minY, minZ);
                        }
                        else
                        {
                            checkedEmptyPoints.Add(new Point(x, y, z));
                        }
                    }
                }
            }
        }

        for (var z = minZ; z <= maxZ; z++)
        {
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true && points.Any(p => p.X == x + 1 && p.Y == y && p.Z == z) == false)
                        faceCount++;

                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true && points.Any(p => p.X == x - 1 && p.Y == y && p.Z == z) == false)
                        faceCount++;

                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true && points.Any(p => p.X == x && p.Y == y + 1 && p.Z == z) == false)
                        faceCount++;

                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true && points.Any(p => p.X == x && p.Y == y - 1 && p.Z == z) == false)
                        faceCount++;

                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true && points.Any(p => p.X == x && p.Y == y && p.Z == z + 1) == false)
                        faceCount++;

                    if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true && points.Any(p => p.X == x && p.Y == y && p.Z == z - 1) == false)
                        faceCount++;
                }
            }
        }

        // 4136 too high
        return faceCount.ToString();
    }

    static List<Point> pointsChecked = new List<Point>();

    static bool CheckPoints(List<Point> points, long x, long y, long z, long maxX, long maxY, long maxZ, long minX, long minY, long minZ)
    {   
        if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true)
            return true;

        if (pointsChecked.Any(p => p.X == x && p.Y == y && p.Z == z) == true)
            return false;

        pointsChecked.Add(new Point(x, y, z));



        if (x < minX || x > maxX || y < minY || y > maxY || z < minZ || z > maxZ)
            return false;

        var result = true;

        result &= CheckPoints(points, x + 1, y, z, maxX, maxY, maxZ, minX, minY, minZ);
        result &= CheckPoints(points, x - 1, y, z, maxX, maxY, maxZ, minX, minY, minZ);
        result &= CheckPoints(points, x, y + 1, z, maxX, maxY, maxZ, minX, minY, minZ);
        result &= CheckPoints(points, x, y - 1, z, maxX, maxY, maxZ, minX, minY, minZ);
        result &= CheckPoints(points, x, y, z + 1, maxX, maxY, maxZ, minX, minY, minZ);
        result &= CheckPoints(points, x, y, z - 1, maxX, maxY, maxZ, minX, minY, minZ);

        return result;
    }

    static void MarkPoints(List<Point> points, long x, long y, long z, long maxX, long maxY, long maxZ, long minX, long minY, long minZ)
    {
        if (points.Any(p => p.X == x && p.Y == y && p.Z == z) == true)
            return;

        if (x < minX || x > maxX || y < minY || y > maxY || z < minZ || z > maxZ)
            return;

        points.Add(new Point(x, y, z));

        MarkPoints(points, x + 1, y, z, maxX, maxY, maxZ, minX, minY, minZ);
        MarkPoints(points, x - 1, y, z, maxX, maxY, maxZ, minX, minY, minZ);
        MarkPoints(points, x, y + 1, z, maxX, maxY, maxZ, minX, minY, minZ);
        MarkPoints(points, x, y - 1, z, maxX, maxY, maxZ, minX, minY, minZ);
        MarkPoints(points, x, y, z + 1, maxX, maxY, maxZ, minX, minY, minZ);
        MarkPoints(points, x, y, z - 1, maxX, maxY, maxZ, minX, minY, minZ);
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
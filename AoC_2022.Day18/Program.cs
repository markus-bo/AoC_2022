class Point
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
        return "";
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
class Point
{
    public long X { get; set; }
    public long Y { get; set; }

    public Point(long x, long y)
    {
        X = x;
        Y = y;
    }

    public long GetDist(Point other)
    {
        return GetDist(other.X, other.Y);   
    }

    public long GetDist(long x, long y)
    {
        return Math.Abs(this.X - x) + Math.Abs(this.Y - y);
    }
}

class Solution
{
    static void Main(string[] args)
    {
        var input0 = GetInput($"{Environment.CurrentDirectory}\\Input0.txt");
        var input1 = GetInput($"{Environment.CurrentDirectory}\\Input1.txt");

        Console.WriteLine($"Part 1, input 0: '{solutionPart1(input0, 10)}'");
        Console.WriteLine($"Part 1, input 1: '{solutionPart1(input1, 2_000_000)}'");
        Console.WriteLine($"Part 2, input 0: '{solutionPart2(input0, 20)}'");
        Console.WriteLine($"Part 2, input 1: '{solutionPart2(input1, 4_000_000)}'");
    }

    static string solutionPart1(string[] input, long checkForLine)
    {
        var sensors = new List<(Point pos, long dist)>();
        var beacons = new List<Point>();

        foreach(var line in input)
        {
            var split = line.Split();

            var sX = int.Parse(new string(split[2].Split('=')[1][..^1]));
            var sY = int.Parse(new string(split[3].Split('=')[1][..^1]));
            var bX = int.Parse(new string(split[8].Split('=')[1][..^1]));
            var bY = int.Parse(split[9].Split('=')[1]);

            var sensor = new Point(sX, sY);

            sensors.Add((sensor, sensor.GetDist(bX, bY)));
            beacons.Add(new Point(bX, bY));
        }

        var min = sensors.Select(s => s.pos.X - (s.dist - Math.Abs(checkForLine - s.pos.Y)))
                         .Min() - 10;

        var max = sensors.Select(s => s.pos.X + (s.dist - Math.Abs(checkForLine - s.pos.Y)))
                         .Max() + 10;

        var blockedCount = 0;

        for (var x = min; x <= max; x++)
        {
            if (beacons.Any(b => b.X == x && b.Y == checkForLine))
                continue;
           
            foreach(var sensor in sensors)
            {
                if (sensor.pos.GetDist(x, checkForLine) <= sensor.dist)
                {
                    blockedCount++;
                    break;
                }
            }
        }

        return blockedCount.ToString();
    }

    static string solutionPart2(string[] input, int upperBound)
    {
        var sensors = new List<(Point pos, long dist)>();
        var beacons = new List<Point>();

        foreach (var line in input)
        {
            var split = line.Split();

            var sX = int.Parse(new string(split[2].Split('=')[1][..^1]));
            var sY = int.Parse(new string(split[3].Split('=')[1][..^1]));
            var bX = int.Parse(new string(split[8].Split('=')[1][..^1]));
            var bY = int.Parse(split[9].Split('=')[1]);

            var sensor = new Point(sX, sY);

            sensors.Add((sensor, sensor.GetDist(bX, bY)));
            beacons.Add(new Point(bX, bY));
        }


        for (var y = 0L; y <= upperBound; y++)
        {
            for (var x = 0L; x <= upperBound; x++)
            {
                var found = true;

                foreach (var sensor in sensors)
                {
                    var dist = sensor.dist - sensor.pos.GetDist(x, y);
                    if (dist >= 0)
                    {
                        x += dist;
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    var tuningFreq = x * 4_000_000 + y;

                    return tuningFreq.ToString();
                }
            }
        }

        throw new NotImplementedException();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
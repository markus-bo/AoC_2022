using System.Transactions;

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

    static (List<List<(int y, int x)>> pointList, int minY, int maxY, int minX, int maxX) ParseInput(string[] input)
    {
        var pointList = new List<List<(int y, int x)>>();

        var minY = 0;
        var maxY = 0;
        var minX = int.MaxValue;
        var maxX = 0;

        foreach (var line in input)
        {
            pointList.Add(new List<(int y, int x)>());

            foreach (var point in line.Split(" -> "))
            {
                var d = point.Split(",")
                             .Select(int.Parse)
                             .ToList();

                pointList.Last().Add((d[1], d[0]));

                maxY = Math.Max(maxY, d[1]);
                minX = Math.Min(minX, d[0]);
                maxX = Math.Max(maxX, d[0]);
            }
        }

        return (pointList, minY, maxY, minX, maxX);
    }

    static string solutionPart1(string[] input)
    {
        var points = ParseInput(input);

        var width = points.maxX - points.minX + 1;
        var height = points.maxY - points.minY + 1;
        var offset =  -points.minX + 1;

        var map = new int[height][];
        
        for (int y = 0; y < height; y++)
            map[y] = new int[width + 2];

        foreach(var line in points.pointList)
        {
            var prevPoint = line.First();

            foreach(var curPoint in line.Skip(1))
            {
                if (prevPoint.y != curPoint.y && prevPoint.x == curPoint.x)
                {
                    for (int y = Math.Min(prevPoint.y, curPoint.y); y <= Math.Max(prevPoint.y, curPoint.y); y++)
                        map[y][prevPoint.x + offset] = 1;
                }
                else if (prevPoint.x != curPoint.x && prevPoint.y == curPoint.y)
                {
                    for (int x = Math.Min(prevPoint.x, curPoint.x); x <= Math.Max(prevPoint.x, curPoint.x); x++)
                        map[prevPoint.y][x + offset] = 1;
                }
                else
                    throw new NotImplementedException();

                prevPoint = curPoint;
            }
        }

        var sandCount = -1;
        var ySand = -1;
        var xSand = 500 + offset;

        while (ySand <= points.maxY)
        {
            xSand = 500 + offset;
            ySand = -1;

            while (++ySand <= points.maxY)
            {
                if (map[ySand][xSand] == 0)
                    continue;
                else if (map[ySand][xSand - 1] == 0)
                {
                    xSand--;
                    continue;
                }
                else if (map[ySand][xSand + 1] == 0)
                {
                    xSand++;
                    continue;
                }
                else
                {
                    map[ySand - 1][xSand] = 1;
                    break;
                }
            }

            sandCount++;
        }

        return sandCount.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var points = ParseInput(input);
        
        var height = points.maxY - points.minY + 3;
        var width = points.maxX - points.minX + 1 + 2 * height;
        var offset = -points.minX + 1 + height;

        var map = new int[height][];

        for (int y = 0; y < height; y++)
            map[y] = new int[width + 2];

        foreach (var line in points.pointList)
        {
            var prevPoint = line.First();

            foreach (var curPoint in line.Skip(1))
            {
                if (prevPoint.y != curPoint.y && prevPoint.x == curPoint.x)
                {
                    for (int y = Math.Min(prevPoint.y, curPoint.y); y <= Math.Max(prevPoint.y, curPoint.y); y++)
                        map[y][prevPoint.x + offset] = 1;
                }
                else if (prevPoint.x != curPoint.x && prevPoint.y == curPoint.y)
                {
                    for (int x = Math.Min(prevPoint.x, curPoint.x); x <= Math.Max(prevPoint.x, curPoint.x); x++)
                        map[prevPoint.y][x + offset] = 1;
                }
                else
                    throw new NotImplementedException();

                prevPoint = curPoint;
            }
        }

        for (int x = 0; x < width + 2; x++)
            map[^1][x] = 1;

        var sandCount = 0;
        var ySand = -1;
        var xSand = 500 + offset;

        while (map[0][500 + offset] != 1)
        {
            xSand = 500 + offset;
            ySand = -1;

            while (map[0][500 + offset] != 1)
            {
                ySand++;

                if (map[ySand][xSand] == 0)
                    continue;
                else if (map[ySand][xSand - 1] == 0)
                {
                    xSand--;
                    continue;
                }
                else if (map[ySand][xSand + 1] == 0)
                {
                    xSand++;
                    continue;
                }
                else
                {
                    map[ySand - 1][xSand] = 1;
                    break;
                }
            }

            sandCount++;
        }

        return sandCount.ToString();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
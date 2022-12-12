using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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

    record Point (int y, int x);

    static string solutionPart1(string[] input)
    {
        var height = input.Length;
        var width = input.First().Length;

        var map = new int[height][];
        var start = new Point(0, 0);
        var end = new Point(0, 0);

        var distanceMap = new int[height][];

        for (int y = 0; y < height; y++)
        {
            map[y] = new int[width];
            
            distanceMap[y] = new int[width];
            Array.Fill(distanceMap[y], int.MaxValue);

            for (int x = 0; x < width; x++)
            {
                if (input[y][x] == 'S')
                {
                    map[y][x] = 'a';
                    start = new Point(y, x);
                }
                else if (input[y][x] == 'E')
                {
                    map[y][x] = 'z';
                    end = new Point(y, x);
                }
                else
                    map[y][x] = input[y][x];
            }
        }

        var result = dfs(map, distanceMap, height, width, start, start, end, 0);

        return result.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var height = input.Length;
        var width = input.First().Length;

        var map = new int[height][];
        var end = new Point(0, 0);

        var distanceMap = new int[height][];

        for (int y = 0; y < height; y++)
        {
            map[y] = new int[width];

            distanceMap[y] = new int[width];
            Array.Fill(distanceMap[y], int.MaxValue);

            for (int x = 0; x < width; x++)
            {
                if (input[y][x] == 'S')
                    map[y][x] = 'a';
                else if (input[y][x] == 'E')
                {
                    map[y][x] = 'z';
                    end = new Point(y, x);
                }
                else
                    map[y][x] = input[y][x];
            }
        }

        var minDistance = inverseDfs(map, distanceMap, height, width, end, end, 'a', 0);

        return minDistance.ToString();
    }

    static int dfs(int[][] map, int[][] dist, int height, int width, Point previous, Point current, Point target, int value)
    {
        if (current.y < 0 || current.y >= height || current.x < 0 || current.x >= width)
            return int.MaxValue;

        if (dist[current.y][current.x] <= value)
            return int.MaxValue;

        if (map[current.y][current.x] - map[previous.y][previous.x] > 1)
            return int.MaxValue;

        if (current == target)
            return value;

        dist[current.y][current.x] = value;

        var minimumDistance = int.MaxValue;

        minimumDistance = Math.Min(minimumDistance, dfs(map, dist, height, width, current, new Point(current.y - 1, current.x), target, value + 1));
        minimumDistance = Math.Min(minimumDistance, dfs(map, dist, height, width, current, new Point(current.y + 1, current.x), target, value + 1));
        minimumDistance = Math.Min(minimumDistance, dfs(map, dist, height, width, current, new Point(current.y, current.x - 1), target, value + 1));
        minimumDistance = Math.Min(minimumDistance, dfs(map, dist, height, width, current, new Point(current.y, current.x + 1), target, value + 1));

        return minimumDistance;
    }

    static int inverseDfs(int[][] map, int[][] dist, int height, int width, Point previous, Point current, char target, int value)
    {
        if (current.y < 0 || current.y >= height || current.x < 0 || current.x >= width)
            return int.MaxValue;

        if (dist[current.y][current.x] <= value)
            return int.MaxValue;

        if (map[previous.y][previous.x] - map[current.y][current.x] > 1)
            return int.MaxValue;

        if (map[current.y][current.x] == target)
            return value;

        dist[current.y][current.x] = value;

        var minimumDistance = int.MaxValue;

        minimumDistance = Math.Min(minimumDistance, inverseDfs(map, dist, height, width, current, new Point(current.y - 1, current.x), target, value + 1));
        minimumDistance = Math.Min(minimumDistance, inverseDfs(map, dist, height, width, current, new Point(current.y + 1, current.x), target, value + 1));
        minimumDistance = Math.Min(minimumDistance, inverseDfs(map, dist, height, width, current, new Point(current.y, current.x - 1), target, value + 1));
        minimumDistance = Math.Min(minimumDistance, inverseDfs(map, dist, height, width, current, new Point(current.y, current.x + 1), target, value + 1));

        return minimumDistance;
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
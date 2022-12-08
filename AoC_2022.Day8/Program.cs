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
        var size = input.Length;

        var treeMap = input.Select(x => x.Select(y => y - '0').ToArray()).ToArray();

        var visibleTreesMap = new bool[size][];

        for (int y = 0; y < size; y++)
            visibleTreesMap[y] = new bool[size];

        var visibleTreeCount = 0;

        for(int i = 1; i < size - 1; i++)
        {
            var highestTreeFromLeft = treeMap[i][0];
            var highestTreeFromRigth = treeMap[i][^1];
            var highestTreeFromAbove = treeMap[0][i];
            var highestTreeFromBelow = treeMap[^1][i];

            for (int j = 1; j< size - 1; j++)
            {
                if (highestTreeFromLeft < treeMap[i][j])
                {
                    visibleTreeCount += visibleTreesMap[i][j] ? 0 : 1;
                    visibleTreesMap[i][j] = true;
                }
                
                if (highestTreeFromRigth < treeMap[i][^(j + 1)])
                {
                    visibleTreeCount += visibleTreesMap[i][^(j + 1)] ? 0 : 1;
                    visibleTreesMap[i][^(j + 1)] = true;
                }

                if (highestTreeFromAbove < treeMap[j][i])
                {
                    visibleTreeCount += visibleTreesMap[j][i] ? 0 : 1;
                    visibleTreesMap[j][i] = true;
                }

                if (highestTreeFromBelow < treeMap[^(j + 1)][i])
                {
                    visibleTreeCount += visibleTreesMap[^(j + 1)][i] ? 0 : 1;
                    visibleTreesMap[^(j + 1)][i] = true;
                }

                highestTreeFromLeft = Math.Max(highestTreeFromLeft, treeMap[i][j]);
                highestTreeFromRigth = Math.Max(highestTreeFromRigth, treeMap[i][^(j + 1)]);
                highestTreeFromAbove = Math.Max(highestTreeFromAbove, treeMap[j][i]);
                highestTreeFromBelow = Math.Max(highestTreeFromBelow, treeMap[^(j + 1)][i]);
            }
        }

        return (visibleTreeCount + (size + size - 2) * 2).ToString();
    }

    static string solutionPart2(string[] input)
    {
        var size = input.Length;

        var treeMap = input.Select(x => x.Select(y => y - '0').ToArray()).ToArray();

        var maxValue = 0;

        for (int y = 1; y < size - 1; y++)
        {
            for (int x = 1; x < size - 1; x++)
            {
                var treeHeight = treeMap[y][x];

                var rightCount = 1;
                var leftCount = 1;
                var belowCount = 1;
                var aboveCount = 1;

                for (int xr = x + 1; xr < size - 1; xr++)
                {
                    if (treeHeight <= treeMap[y][xr])
                        break;

                    rightCount++;
                }

                for (int xl = x - 1; xl > 0; xl--)
                {
                    if (treeHeight <= treeMap[y][xl])
                        break;

                    leftCount++;
                }

                for (int yb = y + 1; yb < size - 1; yb++)
                {
                    if (treeHeight <= treeMap[yb][x])
                        break;

                    belowCount++;
                }

                for (int ya = y - 1; ya > 0; ya--)
                {
                    if (treeHeight <= treeMap[ya][x])
                        break;

                    aboveCount++;
                }

                var value = rightCount * leftCount * belowCount * aboveCount;

                maxValue = Math.Max(maxValue, value);
            }
        }

        return maxValue.ToString();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
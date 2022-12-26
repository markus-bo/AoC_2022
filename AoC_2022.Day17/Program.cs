using System.Diagnostics;

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
        var jetpattern = input.First().ToCharArray();

        var rockPatterns = new List<string[]>()
        {
            new string[] { "####" },
            new string[] { ".#.", "###", ".#." },
            new string[] { "..#", "..#", "###" },
            new string[] { "#", "#", "#", "#" },
            new string[] { "##", "##" }
        };

        var map = new char[10000][];

        map[0] = "#########".ToCharArray();
        for (int i = 1; i < 10000; i++)
            map[i] = "#.......#".ToCharArray();

        

        var topRow = 0;
        var jetIndex = -1;
        var rockIndex = -1;

        for (int i = 0; i < 2022; i++)
        {
            if (++rockIndex >= rockPatterns.Count)
                rockIndex = 0;

            var rockFalling = true;
            var rockYOffset = topRow + 3 + rockPatterns[rockIndex].Length;
            var rockXOffset = 3;

            while (rockFalling)
            {
                if (++jetIndex >= jetpattern.Length)
                    jetIndex = 0;

                var jetXOffset = jetpattern[jetIndex] == '>' ? 1 : -1;
                var canMoveX = true;

                for (int y = 0; y < rockPatterns[rockIndex].Length; y++)
                {
                    for (int x = 0; x < rockPatterns[rockIndex][0].Length; x++)
                    {
                        if (map[rockYOffset - y][rockXOffset + x + jetXOffset] == '#' && rockPatterns[rockIndex][y][x] == '#')
                        {
                            canMoveX = false;
                            break;
                        }
                    }

                    if (!canMoveX)
                        break;
                }

                if (canMoveX)
                    rockXOffset += jetXOffset;


                var canMoveY = true;

                for (int y = 0; y < rockPatterns[rockIndex].Length; y++)
                {
                    for (int x = 0; x < rockPatterns[rockIndex][0].Length; x++)
                    {
                        if (map[rockYOffset - y - 1][rockXOffset + x] == '#' && rockPatterns[rockIndex][y][x] == '#')
                        {
                            canMoveY = false;
                            break;
                        }
                    }

                    if (!canMoveY)
                        break;
                }

                if (canMoveY)
                    rockYOffset--;
                else
                {
                    break;
                }
            }

            for (int y = 0; y < rockPatterns[rockIndex].Length; y++)
            {
                for (int x = 0; x < rockPatterns[rockIndex][0].Length; x++)
                {
                    if (rockPatterns[rockIndex][y][x] == '#')
                    {
                        map[rockYOffset - y][rockXOffset + x] = '#';
                    }
                }
            }

            topRow = Math.Max(topRow, rockYOffset);

            /*Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Rock {i}:");
            Console.WriteLine();
            for (int j = topRow; j >= 0; j--)
            {
                Console.WriteLine(String.Join("", map[j]));
            }*/

            //Console.ReadKey();
        }





        return topRow.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var jetpattern = input.First().ToCharArray();

        var rockPatterns = new List<string[]>()
        {
            new string[] { "####" },
            new string[] { ".#.", "###", ".#." },
            new string[] { "..#", "..#", "###" },
            new string[] { "#", "#", "#", "#" },
            new string[] { "##", "##" }
        };

        var map = new char[100000][];

        map[0] = "#########".ToCharArray();
        for (int i = 1; i < 100000; i++)
            map[i] = "#.......#".ToCharArray();



        var topRow = 0;
        var jetIndex = -1;
        var rockIndex = -1;

        var hashList = new Dictionary<string, (long topRow, long index)>();
        var heightList = new List<int>();

        var remainder = 1_000_000_000_000L;

        var topRowAfterMultiply = long.MinValue;
        var topRowBuffer = 0;

        for (var i = 0L; i < 1_000_000_000_000L; i++)
        {
            if (i == remainder)
            {
                var diff = topRow - topRowBuffer;

                var result = topRowAfterMultiply + diff;

                return result.ToString();
            }

            if (i >= 500)
            {
                //var hash = string.Join("", map[(topRow - 100)..topRow].Select(s => new string(s))).GetHashCode();

                var hash = string.Join("", map[(topRow - 500)..(topRow + 1)].Select(s => new string(s)));


                if (topRowAfterMultiply == long.MinValue)
                {
                    if (hashList.ContainsKey(hash))
                    {
                        var step = topRow - hashList[hash].topRow;

                        Trace.WriteLine($"step {step}, index old {hashList[hash].index}, index current {i}");

                        var patternCount = (long)Math.Floor((1_000_000_000_000L - 500.0) / (i - hashList[hash].index));
                        topRowAfterMultiply = patternCount * step + hashList[hash].topRow;
                        var remainingRocks = 1_000_000_000_000L - (patternCount * (i - hashList[hash].index) + 500);
                        remainder = remainingRocks + i;
                        topRowBuffer = topRow;
                        hashList[hash] = (topRow, i);
                    }
                    else
                    {
                        hashList.Add(hash, (topRow, i));
                    }
                }
            }

            if (++rockIndex >= rockPatterns.Count)
                rockIndex = 0;
         
            var rockFalling = true;
            var rockYOffset = topRow + 3 + rockPatterns[rockIndex].Length;
            var rockXOffset = 3;

            while (rockFalling)
            {
                if (++jetIndex >= jetpattern.Length)
                    jetIndex = 0;

                var jetXOffset = jetpattern[jetIndex] == '>' ? 1 : -1;
                var canMoveX = true;

                for (int y = 0; y < rockPatterns[rockIndex].Length; y++)
                {
                    for (int x = 0; x < rockPatterns[rockIndex][0].Length; x++)
                    {
                        if (map[rockYOffset - y][rockXOffset + x + jetXOffset] == '#' && rockPatterns[rockIndex][y][x] == '#')
                        {
                            canMoveX = false;
                            break;
                        }
                    }

                    if (!canMoveX)
                        break;
                }

                if (canMoveX)
                    rockXOffset += jetXOffset;


                var canMoveY = true;

                for (int y = 0; y < rockPatterns[rockIndex].Length; y++)
                {
                    for (int x = 0; x < rockPatterns[rockIndex][0].Length; x++)
                    {
                        if (map[rockYOffset - y - 1][rockXOffset + x] == '#' && rockPatterns[rockIndex][y][x] == '#')
                        {
                            canMoveY = false;
                            break;
                        }
                    }

                    if (!canMoveY)
                        break;
                }

                if (canMoveY)
                    rockYOffset--;
                else
                {
                    break;
                }
            }

            for (int y = 0; y < rockPatterns[rockIndex].Length; y++)
            {
                for (int x = 0; x < rockPatterns[rockIndex][0].Length; x++)
                {
                    if (rockPatterns[rockIndex][y][x] == '#')
                    {
                        map[rockYOffset - y][rockXOffset + x] = '#';
                    }
                }
            }

            topRow = Math.Max(topRow, rockYOffset);

            heightList.Add(topRow);
        }

        return topRow.ToString();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
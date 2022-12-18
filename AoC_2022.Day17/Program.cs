class Solution
{
    static void Main(string[] args)
    {
        var input0 = GetInput($"{Environment.CurrentDirectory}\\Input0.txt");
        var input1 = GetInput($"{Environment.CurrentDirectory}\\Input1.txt");

        Console.WriteLine($"Part 1, input 0: '{solutionPart1(input0)}'");
        Console.WriteLine($"Part 1, input 1: '{solutionPart1(input1)}'");
        //Console.WriteLine($"Part 2, input 0: '{solutionPart2(input0)}'");
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

        var map = new List<char[]>();

        map.Add("#########".ToCharArray());

        for (int i = 1; i < 10000; i++)
            map.Add("#.......#".ToCharArray());



        var topRow = 0;
        var topRowMultiplier = 0;
        var jetIndex = -1;
        var rockIndex = -1;

        for (var i = 0L; i < 1_000_000_000_000; i++)
        {
            if (i<5000)
            {
                Console.Error.WriteLine(i.ToString().PadLeft(7) + ";" + (topRowMultiplier * 5000 + topRow).ToString());
            }

            if (topRow >= 9000)
            {
                map.RemoveRange(0, 5000);

                for (int j = 0; j < 5000; j++)
                    map.Add("#.......#".ToCharArray());

                topRow -= 5000;
                topRowMultiplier++;
            }

            if (++rockIndex >= rockPatterns.Count)
                rockIndex = 0;

            //Console.Error.WriteLine(rockIndex);

            var rockFalling = true;
            var rockYOffset = topRow + 3 + rockPatterns[rockIndex].Length;
            var rockXOffset = 3;

            while (rockFalling)
            {
                if (++jetIndex >= jetpattern.Length)
                {
                    jetIndex = 0;

                 
                }

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

            if (map[Math.Max(0, rockYOffset - 3)].All(c => c == '#'))
            {

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





        return (topRowMultiplier * 5000 + topRow).ToString();

    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
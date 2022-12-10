using System.Text;

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
        var cycle = 0;

        var registerX = 1;

        var signalStrength = 0;

        foreach(var line in input)
        {
            if ((++cycle - 20) % 40 == 0)
                signalStrength += cycle * registerX;

            if (line.StartsWith("noop"))
                continue;

            var splitLine = line.Split();
            var command = splitLine[0];
            var increment = int.Parse(splitLine[1]);

            if ((++cycle - 20) % 40 == 0)
                signalStrength += cycle * registerX;

            registerX += increment;
        }

        return signalStrength.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var cycle = 0;

        var spriteStart = 1;

        var crtScreen = new StringBuilder();

        Func<int, int, string> setPixel = (cycle, spriteStart) =>
        {
            if (cycle % 40 >= spriteStart && cycle % 40 <= spriteStart + 2)
                return "#";
            else
                return ".";
        };

        foreach (var line in input)
        {
            crtScreen.Append(setPixel(++cycle, spriteStart));

            if (line.StartsWith("noop"))
                continue;

            var splitLine = line.Split();
            var command = splitLine[0];
            var increment = int.Parse(splitLine[1]);

            crtScreen.Append(setPixel(++cycle, spriteStart));

            spriteStart += increment;
        }

        return "\n" + string.Join('\n', crtScreen
                                        .ToString()
                                        .Chunk(40)
                                        .Select(x => new string(x)));
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
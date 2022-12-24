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
        var map = input.TakeWhile(line => line != "").ToList();
        var maxLengthX = map.Max(x => x.Length);

        map = map.Select(x => x.PadRight(maxLengthX, ' ')).ToList();

        var instructions = input.SkipWhile(line => line != "").Skip(1).First().Replace("R", " R ").Replace("L", " L ").Trim().Split().Where(x => x != "");

        var curX = map[0].IndexOf('.');
        var curY = 0;

        var pose = 0; // 0 is right, 1 is down, 2 is left, 3 is up
        var length = 0;

        foreach(var instr in instructions)
        {
            if (int.TryParse(instr, out int temp_length))
            {
                length = temp_length;
            }
            else if (instr == "L" || instr == "R")
            {
                pose += instr == "L" ? -1 : 1;

                if (pose < 0)
                    pose += 4;

                if (pose > 3)
                    pose -= 4;
            }

            while(length > 0)
            {
                var checkX = curX + (pose == 0 ? 1 : pose == 2 ? -1 : 0);
                var checkY = curY + (pose == 1 ? 1 : pose == 3 ? -1 : 0);

                if (checkY < 0)
                {
                    var y = map.Count;
                    var f = ' ';
                    while(f != '#' && f != '.')
                        f = map[--y][checkX];
                    checkY = y;
                }
                else if (checkY >= map.Count)
                {
                    var y = -1;
                    var f = ' ';
                    while(f != '#' && f != '.')
                        f = map[++y][checkX];
                    checkY = y;
                }
                else if (pose == 1 || pose == 3)
                {
                    if (map[checkY][checkX] == ' ' && pose == 1)
                    {
                        var y = -1;
                        var f = ' ';
                        while(f != '#' && f != '.')
                            f = map[++y][checkX];
                        checkY = y;
                    }
                    else if (map[checkY][checkX] == ' ' && pose == 3)
                    {
                        var y = map.Count;
                        var f = ' ';
                        while(f != '#' && f != '.')
                            f = map[--y][checkX];
                        checkY = y;
                    }
                }


                if (checkX < 0)
                    checkX = Math.Max(map[checkY].LastIndexOf('.'), map[checkY].LastIndexOf('#'));
                else if (checkX >= map[checkY].Length)
                    checkX = Math.Min(Math.Max(0, map[checkY].IndexOf('.')), Math.Max(0,map[checkY].IndexOf('#')));
                else if (pose == 0 || pose == 2)
                {
                    if (map[checkY][checkX] == ' ' && pose == 0)
                        checkX = Math.Min(Math.Max(0, map[checkY].IndexOf('.')), Math.Max(0,map[checkY].IndexOf('#')));
                    else if (map[checkY][checkX] == ' ' && pose == 2)
                        checkX = Math.Max(map[checkY].LastIndexOf('.'), map[checkY].LastIndexOf('#'));
                }

                if (map[checkY][checkX] == '#')
                    break;

                curX = checkX;
                curY = checkY;

                length--;
            }

            length = 0;
        }

        var result = (curY + 1) * 1000 + (curX + 1) * 4 + pose;
        
        return result.ToString();
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
public record Point (int Y, int X);

public class Elfe
{
    public Point current;

    public Point proposed;

    public bool Skip;
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
        var elfes = new List<Elfe>();

        for(int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input.First().Length; x++)
            {
                if (input[y][x] == '#')
                    elfes.Add(new Elfe() { current = new Point(y, x)});
            }
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < elfes.Count; j++)
            {
                var x = elfes[j].current.X;
                var y = elfes[j].current.Y;

                var freeN = elfes.Any(p => p.current.X == x && p.current.Y == y - 1) == false;
                var freeNE = elfes.Any(p => p.current.X == x + 1 && p.current.Y == y - 1) == false;
                var freeE = elfes.Any(p => p.current.X == x + 1 && p.current.Y == y) == false;
                var freeSE = elfes.Any(p => p.current.X == x + 1 && p.current.Y == y + 1) == false;
                var freeS = elfes.Any(p => p.current.X == x && p.current.Y == y + 1) == false;
                var freeSW = elfes.Any(p => p.current.X == x - 1 && p.current.Y == y + 1) == false;
                var freeW = elfes.Any(p => p.current.X == x - 1 && p.current.Y == y) == false;
                var freeNW = elfes.Any(p => p.current.X == x - 1 && p.current.Y == y - 1) == false;

                elfes[j].Skip = true;
                elfes[j].proposed = new Point(int.MinValue, int.MinValue);

                if (freeN && freeNE && freeE && freeSE && freeS && freeSW && freeW && freeNW)
                    continue;

                var northFreeForProposal = freeNW & freeN & freeNE;
                var eastFreeForProposal = freeNE & freeE & freeSE;
                var southFreeForProposal = freeSE & freeS & freeSW;
                var westFreeForProposal = freeSW & freeW & freeNW;

                var proposalPoint = new Point(int.MinValue, int.MinValue);

                if (i % 4 == 0)
                {
                    if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                    else if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                    else if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                    else if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                }
                else if ((i - 1) % 4 == 0)
                {
                    if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                    else if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                    else if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                    else if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                }
                else if ((i - 2) % 4 == 0)
                {
                    if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                    else if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                    else if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                    else if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                }
                else if ((i - 3) % 4 == 0)
                {
                    if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                    else if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                    else if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                    else if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                }

                elfes[j].proposed = proposalPoint;
                elfes[j].Skip = false;
            }

            for (int j = 0; j < elfes.Count; j++)
            {
                if (elfes[j].Skip == true)
                    continue;

                if (elfes.Count(p => p.proposed == elfes[j].proposed) == 1)
                {
                    elfes[j].current = elfes[j].proposed;
                }
            }
        }

        var minY = elfes.Select(p => p.current.Y).Min();
        var maxY = elfes.Select(p => p.current.Y).Max();
        var minX = elfes.Select(p => p.current.X).Min();
        var maxX = elfes.Select(p => p.current.X).Max();

        var width = maxX - minX + 1;
        var height = maxY - minY + 1;

        var area = width * height;

        var result = area - elfes.Count;

        return result.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var elfes = new List<Elfe>();

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input.First().Length; x++)
            {
                if (input[y][x] == '#')
                    elfes.Add(new Elfe() { current = new Point(y, x) });
            }
        }

        var round = 0;
        var someElfMoving = true;

        var mapSize = 2000;
        var mapOffset = mapSize / 2;
        var map = new bool[mapSize, mapSize];

        for (int j = 0; j < elfes.Count; j++)
            map[elfes[j].current.Y + mapOffset, elfes[j].current.X + mapOffset] = true;
        
        while (someElfMoving)
        {
            var proposalMap = new int[mapSize, mapSize];

            for (int j = 0; j < elfes.Count; j++)
            {
                var x = elfes[j].current.X;
                var y = elfes[j].current.Y;

                var freeN = !map[y - 1 + mapOffset, x + mapOffset];
                var freeNE = !map[y - 1 + mapOffset, x + 1 + mapOffset];
                var freeE = !map[y + mapOffset, x + 1 + mapOffset];
                var freeSE = !map[y + 1 + mapOffset, x + 1 + mapOffset];
                var freeS = !map[y + 1 + mapOffset, x + mapOffset];
                var freeSW = !map[y + 1 + mapOffset, x - 1 + mapOffset];
                var freeW = !map[y + mapOffset, x - 1 + mapOffset];
                var freeNW = !map[y - 1 + mapOffset, x - 1 + mapOffset];

                elfes[j].Skip = true;
                elfes[j].proposed = new Point(int.MinValue, int.MinValue);

                if (freeN && freeNE && freeE && freeSE && freeS && freeSW && freeW && freeNW)
                    continue;

                var northFreeForProposal = freeNW & freeN & freeNE;
                var eastFreeForProposal = freeNE & freeE & freeSE;
                var southFreeForProposal = freeSE & freeS & freeSW;
                var westFreeForProposal = freeSW & freeW & freeNW;

                var proposalPoint = new Point(int.MinValue, int.MinValue);

                if (round % 4 == 0)
                {
                    if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                    else if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                    else if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                    else if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                }
                else if ((round - 1) % 4 == 0)
                {
                    if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                    else if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                    else if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                    else if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                }
                else if ((round - 2) % 4 == 0)
                {
                    if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                    else if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                    else if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                    else if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                }
                else if ((round - 3) % 4 == 0)
                {
                    if (eastFreeForProposal)
                        proposalPoint = new Point(y, x + 1);
                    else if (northFreeForProposal)
                        proposalPoint = new Point(y - 1, x);
                    else if (southFreeForProposal)
                        proposalPoint = new Point(y + 1, x);
                    else if (westFreeForProposal)
                        proposalPoint = new Point(y, x - 1);
                }

                if (proposalPoint.X != int.MinValue)
                {
                    elfes[j].proposed = proposalPoint;

                    elfes[j].Skip = false;

                    proposalMap[proposalPoint.Y + mapOffset, proposalPoint.X + mapOffset]++;
                }
            }

            someElfMoving = false;

            map = new bool[mapSize, mapSize];

            for (int j = 0; j < elfes.Count; j++)
            {
                if (elfes[j].Skip == false)
                {
                    if (proposalMap[elfes[j].proposed.Y + mapOffset, elfes[j].proposed.X + mapOffset] == 1)
                    {
                        elfes[j].current = elfes[j].proposed;
                        someElfMoving = true;
                    }
                }

                map[elfes[j].current.Y + mapOffset, elfes[j].current.X + mapOffset] = true;
            }

            round++;
        }

        return round.ToString();
    }

    static string[] GetInput(string inputPath) =>
    new StreamReader(inputPath)
        .ReadToEnd()
        .Split('\n')
        .Select(x => x.Trim('\r'))
        .ToArray();
}
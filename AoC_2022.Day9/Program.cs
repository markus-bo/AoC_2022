
class Solution
{
    static void Main(string[] args)
    {
        var input0a = GetInput($"{Environment.CurrentDirectory}\\Input0a.txt");
        var input0b = GetInput($"{Environment.CurrentDirectory}\\Input0b.txt");
        var input1 = GetInput($"{Environment.CurrentDirectory}\\Input1.txt");

        Console.WriteLine($"Part 1, input 0a: '{solutionPart1(input0a)}'");
        Console.WriteLine($"Part 1, input 1: '{solutionPart1(input1)}'");
        Console.WriteLine($"Part 2, input 0a: '{solutionPart2(input0a)}'");
        Console.WriteLine($"Part 2, input 0b: '{solutionPart2(input0b)}'");
        Console.WriteLine($"Part 2, input 1: '{solutionPart2(input1)}'");
    }

    static string solutionPart1(string[] input) => solution(input, 2);

    static string solutionPart2(string[] input) => solution(input, 10);  

    static string solution(string[] input, int numberOfKNots)
    {
        var directionLookup = new Dictionary<string, int>()
        { { "R", 1 }, { "L", -1 }, { "D", 1 }, { "U", -1 } };

        // Parse input
        var movements = input.Select(x => x.Split())
                             .Select(x => (dir: x[0], steps: int.Parse(x[1])));

        // Get required map size and starting point to minimize array size
        var mapParameters = GetMapParameters(movements, directionLookup);

        // Define array which marks visits of last knot
        var mapVisitedKnot = new bool[mapParameters.height][];

        for (int y = 0; y < mapParameters.height; y++)
            mapVisitedKnot[y] = new bool[mapParameters.width];

        //mark starting point in map as visited by default
        mapVisitedKnot[mapParameters.startY][mapParameters.startX] = true;

        // Define head and knots
        var knotPositions = new (int y, int x)[numberOfKNots];

        // Initialize all knots with starting position
        for (int i = 0; i < numberOfKNots; i++)
            knotPositions[i] = (mapParameters.startY, mapParameters.startX);

        foreach (var movement in movements)
        {
            for (int i = 0; i < movement.steps; i++)
            {
                if (movement.dir == "L" || movement.dir == "R")
                    knotPositions[0] = (knotPositions[0].y, knotPositions[0].x + directionLookup[movement.dir]);

                if (movement.dir == "U" || movement.dir == "D")
                    knotPositions[0] = (knotPositions[0].y + directionLookup[movement.dir], knotPositions[0].x);

                for (int j = 1; j < numberOfKNots; j++)
                    knotPositions[j] = GetTailPosition((knotPositions[j - 1].y, knotPositions[j - 1].x), knotPositions[j]);

                mapVisitedKnot[knotPositions[numberOfKNots - 1].y][knotPositions[numberOfKNots - 1].x] = true;
            }
        }

        var visitedFieldsCountOfLastKnot = mapVisitedKnot.Sum(x => x.Count(y => y));

        return visitedFieldsCountOfLastKnot.ToString();
    }

    private static (int height, int width, int startY, int startX) GetMapParameters(IEnumerable<(string dir, int steps)> movements, Dictionary<string, int> directionLookup)
    {
        int minX = 0, maxX = 0, minY = 0, maxY = 0;
        int posX = 0, posY = 0;

        foreach (var move in movements)
        {
            if (move.dir == "R" || move.dir == "L")
            {
                posX += move.steps * directionLookup[move.dir];
                maxX = Math.Max(maxX, posX);
                minX = Math.Min(minX, posX);
            }

            if (move.dir == "D" || move.dir == "U")
            {
                posY += move.steps * directionLookup[move.dir];
                maxY = Math.Max(maxY, posY);
                minY = Math.Min(minY, posY);
            }
        }

        return (height: maxY - minY + 1,
                width: maxX - minX + 1,
                startY: Math.Abs(minY),
                startX: Math.Abs(minX));
    }

    static (int y, int x) GetTailPosition((int y, int x) head, (int y, int x) tail)
    {
        var diffX = head.x - tail.x;
        var diffY = head.y - tail.y;

        // don't move if it is adjacent
        if (Math.Abs(diffX) <= 1 && Math.Abs(diffY) <= 1) 
            return tail;

        var tailY = tail.y;
        var tailX = tail.x;

        if (diffY < 0)
            tailY--;

        if (diffY > 0)
            tailY++;

        if (diffX < 0)
            tailX--;

        if (diffX > 0)
            tailX++;

        return (tailY, tailX);
    }


    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
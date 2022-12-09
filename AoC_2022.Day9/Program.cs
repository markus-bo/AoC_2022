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
        var directionLookup = new Dictionary<string, int>()
        { { "R", 1 }, { "L", -1 }, { "D", 1 }, { "U", -1 } };

        var movements = input.Select(x => x.Split())
                             .Select(x => (direction: x[0], steps: int.Parse(x[1])));

        var minX = 0;
        var maxX = 0;
        var minY = 0;
        var maxY = 0;

        var posX = 0;
        var posY = 0;

        foreach (var movement in movements)
        {
            if (movement.direction == "R" || movement.direction == "L")
            {
                posX += movement.steps * directionLookup[movement.direction];
                maxX = Math.Max(maxX, posX);
                minX = Math.Min(minX, posX);
            }

            if (movement.direction == "D" || movement.direction == "U")
            {
                posY += movement.steps * directionLookup[movement.direction];
                maxY = Math.Max(maxY, posY);
                minY = Math.Min(minY, posY);
            }
        }

        Console.Error.WriteLine($"Expected limits ymin:{minY}, ymax:{maxY} xmin:{minX}, xmax:{maxX}");

        var sizeX = maxX - minX + 1;
        var sizeY = maxY - minY + 1;

        Console.Error.WriteLine($"Size y:{sizeY}, x:{sizeX}");

        var startX = Math.Abs(minX);
        var startY = Math.Abs(minY);

        Console.Error.WriteLine($"Startingpoint y:{startY}, x:{startX}");

        var mapVisitedTail = new bool[sizeY][];

        for (int y = 0; y < sizeY; y++)
            mapVisitedTail[y] = new bool[sizeX];

        //mark starting point
        mapVisitedTail[startY][startX] = true;

        var headPositionX = startX;
        var headPositionY = startY;
        var tailPosition = (y: startY, x: startX);

        foreach (var movement in movements)
        {
            Console.Error.WriteLine($"Movement {movement.direction} {movement.steps}");
            for (int i = 0; i < movement.steps; i++)
            {
                if (movement.direction == "L" || movement.direction == "R")
                {
                    headPositionX += directionLookup[movement.direction];
                }

                if (movement.direction == "U" || movement.direction == "D")
                {
                    headPositionY += directionLookup[movement.direction];
                }

                tailPosition = GetTailPosition((headPositionY, headPositionX), tailPosition);

                mapVisitedTail[tailPosition.y][tailPosition.x] = true;

                Console.Error.WriteLine($"  Head Position x:{headPositionX} y:{headPositionY}; Tail Position x:{tailPosition.x} y:{tailPosition.y}");
            }
        }

        var visitedFieldsCount = mapVisitedTail.Sum(x => x.Count(y => y));
        
        return visitedFieldsCount.ToString();
    }

    static (int y, int x) GetTailPosition((int y, int x) head, (int y, int x) tail)
    {
        var diffX = head.x - tail.x;
        var diffY = head.y - tail.y;

        var diffXAbs = Math.Abs(head.x - tail.x);
        var diffYAbs = Math.Abs(head.y - tail.y);

        if (diffXAbs <= 1 && diffYAbs <= 1) // don't move it is adjacent
        {
            return tail;
        }

        if (diffXAbs == 0 && diffYAbs > 1) // same coloumn
        {
            if (head.y < tail.y)
                return (tail.y - 1, tail.x);
            else
                return (tail.y + 1, tail.x);
        }

        if (diffXAbs > 1 && diffYAbs == 0) // same row
        {
            if (head.x < tail.x)
                return (tail.y, tail.x - 1);
            else
                return (tail.y, tail.x + 1);
        }

        if (diffXAbs > 1 || diffYAbs > 1) // diagonally away
        {
            if (head.y < tail.y && head.x < tail.x)
                return (tail.y - 1, tail.x - 1);
            else if (head.y < tail.y && head.x > tail.x)
                return (tail.y - 1, tail.x + 1);
            else if (head.y > tail.y && head.x < tail.x)
                return (tail.y + 1, tail.x - 1);
            else if (head.y > tail.y && head.x > tail.x)
                return (tail.y + 1, tail.x + 1);
        }

        throw new NotImplementedException();
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
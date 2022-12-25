using System.Collections;
using System.Text;

record Blizzard(int xy, int dir);

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
        var startX = input.First().IndexOf('.');
        var startY = 0;
        var endX = input.Last().IndexOf('.');
        var endY = input.Length - 1;

        var height = input.Length;
        var width = input.First().Length;

        SetInitialHoricontalBlizzardPositions(input);
        SetInitialVerticalBlizzardPositions(input);
        FindBlizzardPositionsHoricontal(1000, width);
        FindBlizzardPositionsVertical(1000, height);

        var result = getMinimumStepsBfs(0, width, height, startY, startX, endY, endX);

        return result.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var startX = input.First().IndexOf('.');
        var startY = 0;
        var endX = input.Last().IndexOf('.');
        var endY = input.Length - 1;

        var height = input.Length;
        var width = input.First().Length;

        SetInitialHoricontalBlizzardPositions(input);
        SetInitialVerticalBlizzardPositions(input);
        FindBlizzardPositionsHoricontal(1000, width);
        FindBlizzardPositionsVertical(1000, height);

        var resultStep = getMinimumStepsBfs(0, width, height, startY, startX, endY, endX);
        resultStep = getMinimumStepsBfs(resultStep, width, height, endY, endX, startY, startX);
        resultStep = getMinimumStepsBfs(resultStep, width, height, startY, startX, endY, endX);

        return resultStep.ToString();
    }

    static void DebugOutput(int step, int width, int height)
    {
        Console.WriteLine($"\n\nStep: {step}\n");
        Console.WriteLine(new String('#', width));

        for (int y = 1; y < height - 1; y++)
        {
            var output = new StringBuilder("#");

            for (int x = 1; x < width - 1; x++)
            {
                if (blizzardsVert[step][x].Any(b => b.xy == y))
                    if (blizzardsVert[step][x].First(b => b.xy == y).dir == 1)
                        output.Append("v");
                    else
                        output.Append("^");
                else if (blizzardsHori[step][y].Any(b => b.xy == x))
                    if (blizzardsHori[step][y].First(b => b.xy == x).dir == 1)
                        output.Append(">");
                    else
                        output.Append("<");
                else
                    output.Append(".");
            }

            output.Append("#");
            Console.WriteLine(output.ToString());
        }

        Console.WriteLine(new String('#', width));
    }

    static Dictionary<int, Dictionary<int, List<Blizzard>>> blizzardsHori;
    static Dictionary<int, Dictionary<int, List<Blizzard>>> blizzardsVert;

    static void SetInitialHoricontalBlizzardPositions(string[] input)
    {
        blizzardsHori = new Dictionary<int, Dictionary<int, List<Blizzard>>>();
        blizzardsHori.Add(0, new Dictionary<int, List<Blizzard>>());

        for (int y = 1; y < input.Length - 1; y++)
        {
            blizzardsHori[0].Add(y, new List<Blizzard>());

            for (int x = 1; x < input.First().Length - 1; x++)
            {
                if (input[y][x] == '>')
                    blizzardsHori[0][y].Add(new Blizzard(x, 1));
                if (input[y][x] == '<')
                    blizzardsHori[0][y].Add(new Blizzard(x, -1));
            }
        }
    }

    static void SetInitialVerticalBlizzardPositions(string[] input)
    {
        blizzardsVert = new Dictionary<int, Dictionary<int, List<Blizzard>>>();
        blizzardsVert.Add(0, new Dictionary<int, List<Blizzard>>());

        for (int x = 1; x < input.First().Length - 1; x++)
        {
            blizzardsVert[0].Add(x, new List<Blizzard>());

            for (int y = 1; y < input.Length - 1; y++)
            {
                if (input[y][x] == 'v')
                    blizzardsVert[0][x].Add(new Blizzard(y, 1));
                if (input[y][x] == '^')
                    blizzardsVert[0][x].Add(new Blizzard(y, -1));
            }
        }
    }

    static void FindBlizzardPositionsHoricontal(int steps, int width)
    {
        for (int step = 1; step < steps; step++)
        {
            blizzardsHori.Add(step, new Dictionary<int, List<Blizzard>>());

            foreach(var blizzardsInRow in blizzardsHori[step - 1])
            {
                var row = blizzardsInRow.Key;

                blizzardsHori[step].Add(row, new List<Blizzard>());

                foreach (var blizzard in blizzardsInRow.Value)
                {
                    var newX = blizzard.xy + blizzard.dir;

                    if (newX <= 0)
                        newX = width - 2;
                    else if (newX >= width - 1)
                        newX = 1;

                    blizzardsHori[step][row].Add(new Blizzard(newX, blizzard.dir));
                }
            }
        }
    }

    static void FindBlizzardPositionsVertical(int steps, int heigth)
    {
        for (int step = 1; step < steps; step++)
        {
            blizzardsVert.Add(step, new Dictionary<int, List<Blizzard>>());

            foreach (var blizzardsInCol in blizzardsVert[step - 1])
            {
                var col = blizzardsInCol.Key;

                blizzardsVert[step].Add(col, new List<Blizzard>());

                foreach (var blizzard in blizzardsInCol.Value)
                {
                    var newY = blizzard.xy + blizzard.dir;

                    if (newY <= 0)
                        newY = heigth - 2;
                    else if (newY >= heigth - 1)
                        newY = 1;

                    blizzardsVert[step][col].Add(new Blizzard(newY, blizzard.dir));
                }
            }
        }
    }

    static Dictionary<int, List<(int y, int x, int wait)>> exploredPoints = new Dictionary<int, List<(int y, int x, int wait)>>();

    public class State
    {
        public string Name { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public int Wait { get; init; }
        public int Step { get; init; }

        public override string ToString() => $"{Step} {Name} y:{Y} x:{X}";
    }   

    static Dictionary<int, List<State>> exploredStates = new Dictionary<int, List<State>>();

    static int getMinimumStepsBfs(int startStep, int width, int height, int startY, int startX, int endY, int endX)
    {
        var queue = new Queue<State>();

        queue.Enqueue(new State() { Name = "Init", Y = startY, X = startX, Wait = 0, Step = startStep });

        var minimumSteps = int.MaxValue;

        exploredStates.Clear();

        for (int i = 0; i < 100000; i++)
            exploredStates.Add(i, new List<State>());

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            if (state.Y == endY && state.X == endX)
            {
                minimumSteps = Math.Min(minimumSteps, state.Step);
                continue;
            }

            if (state.Step >= minimumSteps)
                continue;

            if (state.Step > startStep)
            {
                if (state.X == startX && state.Y == startY)
                {

                }
                else
                {
                    if (state.X <= 0 || state.X >= width - 1 || state.Y <= 0 || state.Y >= height - 1)
                        continue;

                    if (blizzardsHori[state.Step][state.Y].Any(b => b.xy == state.X))
                        continue;

                    if (blizzardsVert[state.Step][state.X].Any(b => b.xy == state.Y))
                        continue;
                }
            }

            if (exploredStates[state.Step].Any(s => s.Y == state.Y && s.X == state.X && s.Wait == state.Wait))
                continue;
      
            exploredStates[state.Step].Add(state);

            queue.Enqueue(new State() { Name = "Down", Y = state.Y + 1, X = state.X, Step = state.Step + 1, Wait = 0 });
            queue.Enqueue(new State() { Name = "Up", Y = state.Y - 1, X = state.X, Step = state.Step + 1, Wait = 0 });
            queue.Enqueue(new State() { Name = "Right", Y = state.Y, X = state.X + 1, Step = state.Step + 1, Wait = 0 });
            queue.Enqueue(new State() { Name = "Left", Y = state.Y, X = state.X - 1, Step = state.Step + 1, Wait = 0 });
            queue.Enqueue(new State() { Name = "Wait", Y = state.Y, X = state.X, Step = state.Step + 1, Wait = state.Wait + 1 });  
        }

        return minimumSteps;
    }

    static string[] GetInput(string inputPath) =>
    new StreamReader(inputPath)
        .ReadToEnd()
        .Split('\n')
        .Select(x => x.Trim('\r'))
        .ToArray();
}
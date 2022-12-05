using System;
using System.Linq;

class Solution
{
    static readonly int NUMBER_STACKS = 9;
    static readonly int MAX_STACK_HEIGHT = 8;

    static void Main(string[] args)
    {
        var inputPath = $"{Environment.CurrentDirectory}\\Input1.txt";

        var input = new List<string>();

        using (var file = new StreamReader(inputPath))
        {
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();

                input.Add(line);
            }
        }

        Console.WriteLine(solutionPart1(input));
        Console.WriteLine(solutionPart2(input));
    }

    static string solutionPart1(List<string> input)
    {
        // Define stacks - could also be done with Stack<char> but List<char> gives more flexibility
        var stacks = new List<char>[NUMBER_STACKS];

        for (int i = 0; i < NUMBER_STACKS; i++)
            stacks[i] = new List<char>();

        // Read input but destack/stack before as input comes in reverse order
        foreach (var stackLine in input.Take(MAX_STACK_HEIGHT).Reverse())
        {
            for (int j = 0; j < stackLine.Length; j++)
            {
                if (char.IsLetter(stackLine[j]))
                {
                    var index = (j - 1) / 4;
                    stacks[index].Add(stackLine[j]);
                }
            }
        }

        // Perform manipulation
        foreach (var operation in input.Skip(MAX_STACK_HEIGHT + 2))
        {
            var split = operation.Split();
            var count = int.Parse(split[1]);
            var from = int.Parse(split[3]) - 1;
            var to = int.Parse(split[5]) - 1;

            for (int i = 0; i < count; i++)
            {
                var onCrane = stacks[from][^1];
                stacks[from].RemoveAt(stacks[from].Count - 1);
                stacks[to].Add(onCrane);
            }
        }

        return string.Join("", stacks.Select(x => x.LastOrDefault(' ')));
    }

    static string solutionPart2(List<string> input)
    {
        // Define stacks - could also be done with Stack<char> but List<char> gives more flexibility
        var stacks = new List<char>[NUMBER_STACKS];

        for (int i = 0; i < NUMBER_STACKS; i++)
            stacks[i] = new List<char>();

        // Read input but destack/stack before as input comes in reverse order
        foreach (var stackLine in input.Take(MAX_STACK_HEIGHT).Reverse())
        {
            for (int j = 0; j < stackLine.Length; j++)
            {
                if (char.IsLetter(stackLine[j]))
                {
                    var index = (j - 1) / 4;
                    stacks[index].Add(stackLine[j]);
                }
            }
        }

        // Perform manipulation
        foreach (var operation in input.Skip(MAX_STACK_HEIGHT + 2))
        {
            var split = operation.Split();
            var count = int.Parse(split[1]);
            var from = int.Parse(split[3]) - 1;
            var to = int.Parse(split[5]) - 1;

            var onCrane = stacks[from].GetRange(stacks[from].Count - count, count);
            stacks[from].RemoveRange(stacks[from].Count - count, count);
            stacks[to].AddRange(onCrane);
        }

        return string.Join("", stacks.Select(x => x.LastOrDefault(' ')));
    }
}
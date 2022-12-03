using System;
using System.Linq;

class Solution
{
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

    static int solutionPart1(List<string> input)
    {
        var sumPriority = 0;

        foreach (var backpack in input)
        {
            var c1 = backpack[..(backpack.Length / 2)];
            var c2 = backpack[(backpack.Length / 2)..];

            var duplicate = c1.First(x => c2.Contains(x));

            sumPriority += char.ToLower(duplicate) - 'a' + 1;
            sumPriority += char.IsUpper(duplicate) ? 26 : 0;
        }
            
        return sumPriority;
    }

    static int solutionPart2(List<string> input)
    {
        var sumPriority = 0;
        
        for (int i = 0; i <= input.Count - 3; i += 3)
        {
            var duplicate = input[i].First(x => input[i+1].Contains(x) && input[i+2].Contains(x));

            sumPriority += char.ToLower(duplicate) - 'a' + 1;
            sumPriority += char.IsUpper(duplicate) ? 26 : 0;
        }

        return sumPriority;
    }
}

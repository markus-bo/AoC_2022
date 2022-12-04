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
        var completeOverlaps = 0;

        foreach (var assignement in input)
        {
            var left = assignement.Split(',')[0];
            var right = assignement.Split(',')[1];

            var minLeft = int.Parse(left.Split('-')[0]);
            var maxLeft = int.Parse(left.Split('-')[1]);
            var minRight = int.Parse(right.Split('-')[0]);
            var maxRight = int.Parse(right.Split('-')[1]);

            if ( (minLeft >= minRight && maxLeft <= maxRight) ||
                 (minRight >= minLeft && maxRight <= maxLeft))
                completeOverlaps++;
        }

        return completeOverlaps;
    }

    static int solutionPart2(List<string> input)
    {
        var partialOverlaps = 0;

        foreach (var assignement in input)
        {
            var left = assignement.Split(',')[0];
            var right = assignement.Split(',')[1];

            var minLeft = int.Parse(left.Split('-')[0]);
            var maxLeft = int.Parse(left.Split('-')[1]);
            var minRight = int.Parse(right.Split('-')[0]);
            var maxRight = int.Parse(right.Split('-')[1]);

            if ( (minLeft >= minRight && minLeft <= maxRight) ||
                 (maxLeft >= minRight && maxLeft <= maxRight) ||
                 (minRight >= minLeft && minRight <= maxLeft) ||
                 (maxRight >= minLeft && maxRight <= maxLeft) )
                partialOverlaps++;
        }

        return partialOverlaps;
    }
}

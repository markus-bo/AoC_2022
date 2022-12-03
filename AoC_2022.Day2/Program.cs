

using System;
/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    public static string ROCK_A = "A";
    public static string PAPER_A = "B";
    public static string SCISSORS_A = "C";

    public static string ROCK_B = "X";
    public static string PAPER_B = "Y";
    public static string SCISSORS_B = "Z";

    public static string LOOSE = "X";
    public static string DRAW = "Y";
    public static string WIN = "Z";


    static void Main(string[] args)
    {
        var inputPath = $"{Environment.CurrentDirectory}\\Input1.txt";

        var input = new List<(string, string)>();

        using (var file = new StreamReader(inputPath))
        {
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();

                var left = line!.Split()[0];
                var right = line!.Split()[1];

                input.Add((left, right));
            }
        }

        Console.WriteLine(solutionPart1(input));
        Console.WriteLine(solutionPart2(input));
    }

    static int solutionPart1(List<(string, string)> input)
    {
        var myPoints = 0;

        foreach (var round in input)
        {
            var opponentPlay = round.Item1;
            var myPlay = round.Item2;

            // win cases
            if ((myPlay == ROCK_B && opponentPlay == SCISSORS_A) ||
                 (myPlay == PAPER_B && opponentPlay == ROCK_A) ||
                 (myPlay == SCISSORS_B && opponentPlay == PAPER_A))
            {
                myPoints += 6;
            }

            // draw cases
            if ((myPlay == ROCK_B && opponentPlay == ROCK_A) ||
                 (myPlay == PAPER_B && opponentPlay == PAPER_A) ||
                 (myPlay == SCISSORS_B && opponentPlay == SCISSORS_A))
            {
                myPoints += 3;
            }

            if (myPlay == ROCK_B)
                myPoints += 1;

            if (myPlay == PAPER_B)
                myPoints += 2;

            if (myPlay == SCISSORS_B)
                myPoints += 3;
        }

        return myPoints;
    }

    static int solutionPart2(List<(string, string)> input)
    {
        var myPoints = 0;

        foreach (var round in input)
        {
            var opponentPlay = round.Item1;
            var roundEnd = round.Item2;

            var myPlay = "";

            // win cases
            if ((roundEnd == LOOSE && opponentPlay == SCISSORS_A))
                myPlay = PAPER_B;

            if ((roundEnd == LOOSE && opponentPlay == ROCK_A))
                myPlay = SCISSORS_B;

            if ((roundEnd == LOOSE && opponentPlay == PAPER_A))
                myPlay = ROCK_B;

            if ((roundEnd == DRAW && opponentPlay == SCISSORS_A))
                myPlay = SCISSORS_B;

            if ((roundEnd == DRAW && opponentPlay == ROCK_A))
                myPlay = ROCK_B;

            if ((roundEnd == DRAW && opponentPlay == PAPER_A))
                myPlay = PAPER_B;

            if ((roundEnd == WIN && opponentPlay == SCISSORS_A))
                myPlay = ROCK_B;

            if ((roundEnd == WIN && opponentPlay == ROCK_A))
                myPlay = PAPER_B;

            if ((roundEnd == WIN && opponentPlay == PAPER_A))
                myPlay = SCISSORS_B;


            // draw cases
            if (roundEnd == WIN)
                myPoints += 6;

            if (roundEnd == DRAW)
                myPoints += 3;

            if (myPlay == ROCK_B)
                myPoints += 1;

            if (myPlay == PAPER_B)
                myPoints += 2;

            if (myPlay == SCISSORS_B)
                myPoints += 3;

        }

        return myPoints;
    }
}

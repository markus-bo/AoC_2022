using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

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
        var rand = new Random();

        var encryption = input.Select(int.Parse)
                            .Select(x => (value: x, hash: (x * rand.Next()).GetHashCode()))
                            .ToList();

        var mix = new ObservableCollection<(int value, int hash)>((((int value, int hash)[])encryption.ToArray().Clone()).ToList());

        var fitIndex = (int i, int l) => (i % l + l) % l;

        foreach (var number in encryption)
        {
            var currentIndex = mix.IndexOf(number);
            var newIndex = fitIndex(currentIndex + number.value, mix.Count - 1);

            mix.Move(currentIndex, newIndex);
        }

        var zeroIndex = mix.Select(x => x.value).ToList().IndexOf(0);

        var getIndexFromZero = (int n) => mix.ElementAt((int)fitIndex(zeroIndex + n, mix.Count)).value;

        var sumGroveCoordinates = getIndexFromZero(1000) + getIndexFromZero(2000) + getIndexFromZero(3000);

        return sumGroveCoordinates.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var rand = new Random();

        var encryption = input.Select(long.Parse)
                            .Select(x => x * 811589153)
                            .Select(x => (value: x, hash: (long)(x * rand.Next()).GetHashCode()))
                            .ToList();

        var mix = new ObservableCollection<(long value, long hash)>((((long value, long hash)[])encryption.ToArray().Clone()).ToList());

        var fitIndex = (long i, int l) => (i % l + l) % l;

        for (int i = 0; i < 10; i++)
        {
            foreach (var number in encryption)
            {
                var currentIndex = mix.IndexOf(number);
                var newIndex = fitIndex(currentIndex + number.value, mix.Count - 1);

                mix.Move(currentIndex, (int)newIndex);
            }
        }

        var zeroIndex = mix.Select(x => x.value).ToList().IndexOf(0);

        var getIndexFromZero = (int n) => mix.ElementAt((int)fitIndex(zeroIndex + n, mix.Count)).value;

        var sumGroveCoordinates = getIndexFromZero(1000) + getIndexFromZero(2000) + getIndexFromZero(3000);

        return sumGroveCoordinates.ToString();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
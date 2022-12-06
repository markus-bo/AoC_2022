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

    static string solutionPart1(string[] input) => solution(input, 4);

    static string solutionPart2(string[] input) => solution(input, 14);

    static string solution(string[] input, int seqLength)
    {
        var data = input.First();

        return Enumerable.Range(seqLength, data.Length)
            .First(x => data[(x - seqLength)..x].Distinct().Count() == seqLength)
            .ToString();
    }

    static string[] GetInput(string inputPath) => 
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n');
}
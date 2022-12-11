class Monkey
{
    public int Id { get; set; }

    public List<int> Items { get; set; }

    private Func<int, int> Operation;

    private Func<int, bool> Test;

    public Monkey(int id)
    {
        Id = id;
    }

    public void ParseOperation(string operation)
    {

    }

    public void ParseTest(string test)
    {

    }
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
        var monkeys = new Dictionary<int, Monkey>();

        var monkeyId = -1;

        foreach(var line in input.Select(x => x.TrimStart())
        {
            if (line.StartsWith("Monkey"))
            {
                monkeyId = int.Parse(line.Substring("Monkey ".Length).TrimEnd(':'));

                monkeys.Add(monkeyId, new Monkey(monkeyId));
            }

            if (line.StartsWith("Starting items"))
            {
                monkeys[monkeyId].Items = line.Substring("Starting items: ".Length)
                    .Split(", ")
                    .Select(int.Parse)
                    .ToList();
            }

            if (line.StartsWith("Operation"))
            {
                monkeys[monkeyId].ParseOperation(line.Substring("Operation: ".Length))
            }


        }
        return "";
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
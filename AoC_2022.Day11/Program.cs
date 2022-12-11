using System;

class Monkey
{
    public int Id { get; set; }

    public long InspectionCounter { get; private set; }

    public int Divisor { get; private set; } = 0;

    public int WorryReduction { get; private set; } = 3;

    public List<long> Items { get; set; }

    private Func<long, long> calculateWorryLevel;

    private Func<long, bool> testWorryLevel;

    private int throwToMonkeyIdOnTrue;

    private int throwToMonkeyIdOnFalse;

    public Monkey(int id, int worryReduction)
    {
        Id = id;

        this.WorryReduction = worryReduction;
    }

    public IEnumerable<(long item, int toMonkeyId)> ProcessItems()
    {
        var itemShuffle = new List<(long item, int toMonkeyId)>();

        for (int i = 0; i < Items.Count; i++)
        {
            this.InspectionCounter++;

            var worryLevel = this.calculateWorryLevel(Items[i]);

            worryLevel = (long)Math.Floor(worryLevel / (double)WorryReduction);

            var testResult = this.testWorryLevel(worryLevel);

            if (testResult)
                itemShuffle.Add((worryLevel, throwToMonkeyIdOnTrue));
            else
                itemShuffle.Add((worryLevel, throwToMonkeyIdOnFalse));
        }

        this.Items.Clear();

        return itemShuffle;
    }

    public void ParseOperation(string operation)
    {
        var term = operation.Substring("new = ".Length).Split();

        var left = term[0];
        var op = term[1];
        var right = term[2];

        switch (op)
        {
            case "+":
                this.calculateWorryLevel = (old) =>
                {
                    return (left == "old" ? old : long.Parse(left)) + (right == "old" ? old : long.Parse(right));
                };
                break;

            case "*":
                this.calculateWorryLevel = (old) =>
                {
                    return (left == "old" ? old : long.Parse(left)) * (right == "old" ? old : long.Parse(right));
                };
                break;

            default:
                throw new NotImplementedException();
        }
    }

    public void ParseTest(string test)
    {
        var divider = int.Parse(test.Substring("divisible by ".Length));

        this.testWorryLevel = (worryLevel) => worryLevel % divider == 0;

        this.Divisor = divider;
    }

    public void ParseOnTrue(string action)
    {
        this.throwToMonkeyIdOnTrue = int.Parse(action.Substring("throw to monkey ".Length));
    }

    public void ParseOnFalse(string action)
    {
        this.throwToMonkeyIdOnFalse = int.Parse(action.Substring("throw to monkey ".Length));
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

        foreach(var line in input.Select(x => x.TrimStart()))
        {
            if (line.StartsWith("Monkey"))
            {
                monkeyId = int.Parse(line.Substring("Monkey ".Length).TrimEnd(':'));

                monkeys.Add(monkeyId, new Monkey(monkeyId, 3));
            }

            if (line.StartsWith("Starting items"))
            {
                monkeys[monkeyId].Items = line.Substring("Starting items: ".Length)
                    .Split(", ")
                    .Select(long.Parse)
                    .ToList();
            }

            if (line.StartsWith("Operation"))
            {
                monkeys[monkeyId].ParseOperation(line.Substring("Operation: ".Length));
            }

            if (line.StartsWith("Test"))
            {
                monkeys[monkeyId].ParseTest(line.Substring("Test: ".Length));
            }

            if (line.StartsWith("If true"))
            {
                monkeys[monkeyId].ParseOnTrue(line.Substring("If true: ".Length));
            }

            if (line.StartsWith("If false"))
            {
                monkeys[monkeyId].ParseOnFalse(line.Substring("If false: ".Length));
            }
        }

        for (int round = 1; round <= 20; round++)
        {
            foreach (var monkey in monkeys.Select(x => x.Value))
            {
                var processedItems = monkey.ProcessItems();

                foreach (var processedItem in processedItems)
                    monkeys[processedItem.toMonkeyId].Items.Add(processedItem.item);
            }
        }


        var result = monkeys.Select(x => x.Value.InspectionCounter)
               .OrderByDescending(x => x)
               .Take(2)
               .ToList();
               
        
        return (result[0] * result[1]).ToString();
    }

    static string solutionPart2(string[] input)
    {
        var monkeys = new Dictionary<int, Monkey>();

        var monkeyId = -1;

        foreach (var line in input.Select(x => x.TrimStart()))
        {
            if (line.StartsWith("Monkey"))
            {
                monkeyId = int.Parse(line.Substring("Monkey ".Length).TrimEnd(':'));

                monkeys.Add(monkeyId, new Monkey(monkeyId, 1));
            }

            if (line.StartsWith("Starting items"))
            {
                monkeys[monkeyId].Items = line.Substring("Starting items: ".Length)
                    .Split(", ")
                    .Select(long.Parse)
                    .ToList();
            }

            if (line.StartsWith("Operation"))
            {
                monkeys[monkeyId].ParseOperation(line.Substring("Operation: ".Length));
            }

            if (line.StartsWith("Test"))
            {
                monkeys[monkeyId].ParseTest(line.Substring("Test: ".Length));
            }

            if (line.StartsWith("If true"))
            {
                monkeys[monkeyId].ParseOnTrue(line.Substring("If true: ".Length));
            }

            if (line.StartsWith("If false"))
            {
                monkeys[monkeyId].ParseOnFalse(line.Substring("If false: ".Length));
            }
        }

        var mod = 1L;

        foreach (var div in monkeys.Select(x => x.Value.Divisor).Distinct())
        {
            mod *= div;
        }

        for (int round = 1; round <= 10000; round++)
        {
            foreach (var monkey in monkeys.Select(x => x.Value))
            {
                var processedItems = monkey.ProcessItems();

                foreach (var processedItem in processedItems)
                    monkeys[processedItem.toMonkeyId].Items.Add(processedItem.item%mod);
            }
        }


        var result = monkeys.Select(x => x.Value.InspectionCounter)
               .OrderByDescending(x => x)
               .Take(2)
               .ToList();
               

        return (result[0]* result[1]).ToString();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
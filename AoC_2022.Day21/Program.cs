using System.Runtime.CompilerServices;

public enum ArgumentTypeEnum
{
    VALUE,
    REFERENCE
}

interface IArgument
{
    public double GetValue(Dictionary<string, Operation> operations);

    public void ResetMemory();
}

class ValueArgument : IArgument
{
    public double Value { get; set; }

    public ValueArgument()
    {
        this.Value = 0;
    }

    public ValueArgument(double value)
    { 
        this.Value = value; 
    }

    public double GetValue(Dictionary<string, Operation> operations)
    {
        return Value;
    }

    public void ResetMemory()
    {
    }
}

class ReferenceArgument : IArgument
{
    public string Reference { get; private set; }

    private bool ValueCalculated = false;
    private double calculatedValue = 0.0;

    public ReferenceArgument(string reference)
    {
        Reference = reference;
    }

    public double GetValue(Dictionary<string, Operation> operations)
    {
        if (ValueCalculated == true)
            return calculatedValue;

        calculatedValue = operations[Reference].GetValue();
        ValueCalculated = true;
        return calculatedValue;
    }

    public void ResetMemory() => this.ValueCalculated = false;
}

class Operation
{
    public static Dictionary<string, Operation> Operations = new Dictionary<string, Operation>();

    public string Op { get; private set; }

    public IArgument Arg1 { get; private set; }

    public IArgument Arg2 { get; private set; }

    public Operation(string op, IArgument arg1, IArgument arg2)
    {
        Op = op;
        Arg1 = arg1;
        Arg2 = arg2;
    }

    public double GetValue()
    {
        return Op switch
        {
            "val" => Arg1.GetValue(Operations),
            "+" => Arg1.GetValue(Operations) + Arg2.GetValue(Operations),
            "-" => Arg1.GetValue(Operations) - Arg2.GetValue(Operations),
            "*" => Arg1.GetValue(Operations) * Arg2.GetValue(Operations),
            "/" => Arg1.GetValue(Operations) / Arg2.GetValue(Operations),
            _ => throw new NotImplementedException()
        };
    }

    public static void ResetMemory()
    {
        foreach(var op in Operations)
        {
            op.Value.Arg1.ResetMemory();
            op.Value.Arg2.ResetMemory();
        }
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
        Operation.Operations.Clear();

        foreach(var line in input)
        {
            var monkey = line.Split(": ")[0];
            var operationTerm = line.Split(": ")[1].Split(); 

            if (operationTerm.Length == 1)
            {
                var arg1 = double.Parse(operationTerm[0]);

                Operation.Operations.Add(
                    monkey,
                    new Operation("val", new ValueArgument(arg1), new ValueArgument()));
            }
            else
            {
                var arg1 = operationTerm[0];
                var arg2 = operationTerm[2];
                var op = operationTerm[1];

                Operation.Operations.Add(
                    monkey,
                    new Operation(op, new ReferenceArgument(arg1), new ReferenceArgument(arg2)));
            }
        }

        var result = Operation.Operations["root"].GetValue();

        return result.ToString();
    }

    static string solutionPart2(string[] input)
    {
        Operation.Operations.Clear();
        
        foreach (var line in input)
        {
            var monkey = line.Split(": ")[0];
            var operationTerm = line.Split(": ")[1].Split();

            if (operationTerm.Length == 1)
            {
                var arg1 = double.Parse(operationTerm[0]);

                Operation.Operations.Add(
                    monkey,
                    new Operation("val", new ValueArgument(arg1), new ValueArgument()));
            }
            else
            {
                var arg1 = operationTerm[0];
                var arg2 = operationTerm[2];
                var op = operationTerm[1];

                Operation.Operations.Add(
                    monkey,
                    new Operation(op, new ReferenceArgument(arg1), new ReferenceArgument(arg2)));
            }
        }

        var initialHumanValue = (long)(Operation.Operations["humn"].Arg1 as ValueArgument)!.Value;

        var rootLeft = Operation.Operations["root"].Arg1;
        var rootRight = Operation.Operations["root"].Arg2;

        for (int i = 0; i <= 1; i++)
        {
            Operation.ResetMemory();

            (Operation.Operations["humn"].Arg1 as ValueArgument)!.Value = initialHumanValue;

            var leftRootResult = rootLeft.GetValue(Operation.Operations);
            var rightRootResult = rootRight.GetValue(Operation.Operations);

            var maxHuman = long.MaxValue;
            var minHuman = long.MinValue;
            var count = 0;

            var searchDirection = i == 0 ? 1 : -1;

            while (leftRootResult != rightRootResult && ++count < 100)
            {
                if (leftRootResult.CompareTo(rightRootResult) == searchDirection)
                    minHuman = Math.Max(minHuman, (long)(Operation.Operations["humn"].Arg1 as ValueArgument)!.Value);
                else
                    maxHuman = Math.Min(maxHuman, (long)(Operation.Operations["humn"].Arg1 as ValueArgument)!.Value);

                (Operation.Operations["humn"].Arg1 as ValueArgument)!.Value = minHuman + (maxHuman - minHuman) / 2;

                Operation.ResetMemory();

                leftRootResult = rootLeft.GetValue(Operation.Operations);
                rightRootResult = rootRight.GetValue(Operation.Operations);
            }

            if (leftRootResult == rightRootResult)
                return Operation.Operations["humn"].Arg1.GetValue(Operation.Operations).ToString();
        }

        throw new InvalidOperationException();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
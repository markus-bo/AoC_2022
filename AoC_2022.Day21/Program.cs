using System.Runtime.CompilerServices;

public enum ArgumentTypeEnum
{
    VALUE,
    REFERENCE
}

interface IArgument
{
    public long GetValue(Dictionary<string, Operation> operations);
}

class ValueArgument : IArgument
{
    public long Value { get; private set; }

    public ValueArgument(long value)
    { 
        this.Value = value; 
    }

    public long GetValue(Dictionary<string, Operation> operations)
    {
        return Value;
    }
}

class ReferenceArgument : IArgument
{
    public string Reference { get; private set; }

    private bool ValueCalculated = false;
    private long calculatedValue = 0L;

    public ReferenceArgument(string reference)
    {
        Reference = reference;
    }

    public long GetValue(Dictionary<string, Operation> operations)
    {
        if (ValueCalculated == true)
            return calculatedValue;

        calculatedValue = operations[Reference].GetValue();
        ValueCalculated = true;
        return calculatedValue;
    }
}

class Operation
{
    public static Dictionary<string, Operation> Operations = new Dictionary<string, Operation>();

    public string Op { get; private set; }

    public IArgument Arg1 { get; private set; }

    public IArgument Arg2 { get; private set; }

    public Operation(string op, IArgument arg1, IArgument arg2 = null)
    {
        Op = op;
        Arg1 = arg1;
        Arg2 = arg2;
    }

    public long GetValue()
    {
        return Op switch
        {
            "=" => Arg1.GetValue(Operations),
            "+" => Arg1.GetValue(Operations) + Arg2.GetValue(Operations),
            "-" => Arg1.GetValue(Operations) - Arg2.GetValue(Operations),
            "*" => Arg1.GetValue(Operations) * Arg2.GetValue(Operations),
            "/" => Arg1.GetValue(Operations) / Arg2.GetValue(Operations),
            _ => throw new NotImplementedException()
        };
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
                var arg1 = long.Parse(operationTerm[0]);

                Operation.Operations.Add(
                    monkey,
                    new Operation("=", new ValueArgument(arg1)));
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
        return "";
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}


/*
 * class Cell
{
    public enum ValueTypeEnum
    {
            VALUE,
            REFERENCE
    }

    public ValueTypeEnum ValueType {get; set;}

    public int Value {get; set;}

    private bool ValueCalculated = false;
    private int calculatedValue = 0;

    public int GetValue(List<Operation> operations)
    { 
        if (ValueType == ValueTypeEnum.VALUE)
        {
            return Value;
        }
        else if (ValueType == ValueTypeEnum.REFERENCE)
        {
            if (ValueCalculated == false)
            {

                calculatedValue = operations[Value].GetValue();
                ValueCalculated = true;
                return calculatedValue;
            }
            else
            {
                return calculatedValue;
            }
        }

        return 0;
    }
}

class Operation
{
    public static List<Operation> Operations = new List<Operation>();

    public enum OperatorType
    {
            VALUE,
            ADD,
            SUB,
            MULT
    }

    public OperatorType Operator {get; set;}

    public Cell Argument1 {get; set;}

    public Cell Argument2{get; set;}

    

    public int GetValue()
    {
        switch(Operator)
        {
            case OperatorType.VALUE:
                return Argument1.GetValue(Operations);
                break;

            case OperatorType.ADD:
                return Argument1.GetValue(Operations) + Argument2.GetValue(Operations);
                break;

            case OperatorType.SUB:
                return Argument1.GetValue(Operations) - Argument2.GetValue(Operations);
                break;

            case OperatorType.MULT:
                return Argument1.GetValue(Operations) * Argument2.GetValue(Operations);
                break;
        }

        return 0;
    }
}


class Solution
{
    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            string operation = inputs[0];
            string arg1 = inputs[1];
            string arg2 = inputs[2];

            Operation.OperatorType type;
            if (operation == "VALUE")
                type = Operation.OperatorType.VALUE;
            else if (operation == "ADD")
                type = Operation.OperatorType.ADD;
            else if (operation == "SUB")
                type = Operation.OperatorType.SUB;
            else
                type = Operation.OperatorType.MULT;


            Cell argument1 = new Cell()
            {
                ValueType = arg1.StartsWith("$") ? Cell.ValueTypeEnum.REFERENCE : Cell.ValueTypeEnum.VALUE,
                Value = int.Parse(arg1.TrimStart('$'))
            };

            Cell argument2; 
            try
            {
                argument2 = new Cell()
                {
                    ValueType = arg2.StartsWith("$") ? Cell.ValueTypeEnum.REFERENCE : Cell.ValueTypeEnum.VALUE,
                    Value = int.Parse(arg2.TrimStart('$'))
                };
            }
            catch {
                argument2 = new Cell();
            }

            Operation.Operations.Add(new Operation()
            {
                Operator = type,
                Argument1 = argument1,
                Argument2 = argument2
            });
        }


        for (int i = 0; i < N; i++)
        {
            
            // Write an answer using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(Operation.Operations[i].GetValue());
        }
    }
}
 * 
 * 
 * 
 * 
 */
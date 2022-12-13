using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        var pairs = input.Where(x => x != "")
                         .Select(x => x.Replace(",", " , ")
                                       .Replace("]", " ] ")
                                       .Replace("[", " [ ")
                                       .Split()
                                       .Where(y => y != "" && y != ",")
                                       .ToArray())
                         .Select(x => ParseInput(x))
                         .Select((line, index) => new { line, index })
                         .GroupBy(g => g.index / 2, c => c.line);

        var sumOfInOrderPairs = pairs.Where(x => compareOrder(x.First(), x.Last()) == Order.RIGHT)
                               .Select(x => x.Key + 1)
                               .Sum();

        return sumOfInOrderPairs.ToString();
    }

    static string solutionPart2(string[] input)
    {

        var arrays = input.Append("[[2]]")
                         .Append("[[6]]")
                         .Where(x => x != "")
                         .Select(x => x.Replace(",", " , ")
                                       .Replace("]", " ] ")
                                       .Replace("[", " [ ")
                                       .Split()
                                       .Where(y => y != "" && y != ",")
                                       .ToArray())
                         .Select(x => ParseInput(x))
                         .ToList();

        arrays.Sort((a, b) => (int)compareOrder(a, b));

        var result = arrays.Select((x, i) => (value: x as List<object>, index: i + 1))
                            .Where(x => x.value.Count == 1)
                            .Where(x => x.value[0] is List<object>)
                            .Where(x => (x.value[0] as List<object>).Count == 1)
                            .Where(x => (x.value[0] as List<object>)[0] is int)
                            .Where(x => (int)(x.value[0] as List<object>)[0] == 2 || (int)(x.value[0] as List<object>)[0] == 6)
                            .Select(x => x.index)
                            .Aggregate(1, (a, b) => a * b);

        return result.ToString();
    }


    static int indexParser = 0;

    static object ParseInput(string[] input)
    {
        indexParser = 0;

        return ParseInputRecursion(input);
    }

    static object ParseInputRecursion(string[] input)
    {
        object parsedArray = null;

        if (input[indexParser] == "[")
        {
            parsedArray = new List<object>();

            while (input[++indexParser] != "]")
                (parsedArray as List<object>).Add(ParseInputRecursion(input));
        }
        else if (input[indexParser].All(x => char.IsDigit(x)))
            parsedArray = int.Parse(input[indexParser]);

        return parsedArray;
    }

    enum Order
    {
        RIGHT = -1,
        UNCLEAR = 0,
        WRONG = 1
    }

    static Order compareOrder(object left, object right)
    {
        if (left is int && right is int)
        {
            if ((int)left < (int)right)
                return Order.RIGHT;
            else if ((int)left == (int)right)
                return Order.UNCLEAR;
            else
                return Order.WRONG;
        }
        else if (left is List<object> && right is List<object>)
        {
            var leftList = left as List<object>;
            var rightList = right as List<object>;

            var i = 0;

            while (i < leftList.Count && i < rightList.Count)
            {
                var inorder = compareOrder(leftList[i], rightList[i]);

                if (inorder == Order.RIGHT)
                    return Order.RIGHT;
                else if (inorder == Order.WRONG)
                    return Order.WRONG;

                i++;
            }

            if (i == leftList.Count && i < rightList.Count)
                return Order.RIGHT;
            else if (i == rightList.Count && i < leftList.Count)
                return Order.WRONG;
            else
                return Order.UNCLEAR;
        }
        else if (left is int && right is List<object>)
            return compareOrder(new List<object>() { left }, right);
        else
            return compareOrder(left, new List<object>() { right });
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
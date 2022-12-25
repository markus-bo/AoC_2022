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
        var sum_10 = 0L;

        foreach (var line in input)
        {
            var n_10 = ConvertFromSnafu(line);
            sum_10 += n_10;   
        }

        Console.Error.WriteLine($"decimal sum: {sum_10}");

        var result = ConvertToSnafu(sum_10);

        return result;
    }

    public class State
    {
        public int[] Snafu { get; init; }

        public int lastindex;

        private long n_10;
        public long N_10 => n_10;

        public long LowBorder { get; private set; }
        public long HighBorder { get; private set; }

        public State(int[] snafu, int lastindex)
        {
            Snafu = snafu;

            this.n_10 = ConvertFromSnafu(snafu);

            this.lastindex = lastindex;

            if (lastindex == Snafu.Length - 1)
            {
                LowBorder = n_10;
                HighBorder = n_10;
            }
            else
            {
                var baseValue = (long)Math.Pow(5, Snafu.Length - (lastindex + 1)) / 2;

                LowBorder = n_10 - baseValue;
                HighBorder = n_10 + baseValue;
            }
        }

        public override string ToString()
        {
            var lookup = new Dictionary<int, char>() { { -2, '=' }, { -1, '-' }, { 0, '0' }, { 1, '1' }, { 2, '2' } };

            return string.Join("", Snafu.Select(x => lookup[x]).ToArray());
        }
    }

    static HashSet<string> exploredStates = new HashSet<string>();

    static string ConvertToSnafu(long n_10)
    {
        var queue = new Queue<State>();




                

        queue.Enqueue(new State(snafu: new int[] { 2, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0},
                                lastindex: 0));

        queue.Enqueue(new State(snafu: new int[] { 1, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0},
                                lastindex: 0));

        queue.Enqueue(new State(snafu: new int[] { 0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0,
                                                   0, 0, 0, 0, 0},
                                lastindex: 0));



        exploredStates.Clear();

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            if (state.N_10 == n_10)
                return state.ToString();

            if (n_10 < state.LowBorder || n_10 > state.HighBorder)
                continue;

            if (state.lastindex == state.Snafu.Length - 1)
                continue;

            if (exploredStates.Contains(state.ToString() + state.lastindex.ToString()))
                continue;

            exploredStates.Add(state.ToString() + state.lastindex.ToString());

            var s2 = ((int[])state.Snafu.Clone());
            var s1 = ((int[])state.Snafu.Clone());
            var s0 = ((int[])state.Snafu.Clone());
            var s_1 = ((int[])state.Snafu.Clone());
            var s_2 = ((int[])state.Snafu.Clone());

            var nextIndex = state.lastindex + 1;

            s2[nextIndex] = 2;
            s1[nextIndex] = 1;
            s0[nextIndex] = 0;
            s_1[nextIndex] = -1;
            s_2[nextIndex] = -2;

            queue.Enqueue(new State(s2, nextIndex));
            queue.Enqueue(new State(s1, nextIndex));
            queue.Enqueue(new State(s0, nextIndex));
            queue.Enqueue(new State(s_1, nextIndex));
            queue.Enqueue(new State(s_2, nextIndex));
        }

        return "?";
    }

    public static long ConvertFromSnafu(int[] n_sanufu)
    {
        var b = 1L;
        var n_10 = 0L;

        foreach (var d in n_sanufu.Reverse())
        {
            n_10 += d * b;
            b *= 5;
        }

        return n_10;
    }

    public static long ConvertFromSnafu(string n_sanufu)
    {
        var lookup = new Dictionary<char, int>() { { '=', -2 }, { '-', -1 }, { '0', 0 }, { '1', 1 }, { '2', 2 } };

        return ConvertFromSnafu(n_sanufu.Select(x => lookup[x])
                                        .ToArray());
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
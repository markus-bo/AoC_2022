using System.Drawing;
using System.Linq;

class Node
{
    public static Dictionary<string, Node> Nodes = new Dictionary<string, Node>();

    public string Name { get; private set; }

    public int Flowrate { get; set; }

    public List<string> AdjacentNodeNames { get; set; }

    public Node(string name)
    {
        this.Name = name;
        this.AdjacentNodeNames = new List<string> ();
        
        Nodes.Add(name, this);
    }

    public override string ToString()
    {
        return $"Valve {this.Name} has flow rate={this.Flowrate}; tunnels lead to valves {string.Join(", ", this.AdjacentNodeNames)}";
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
        Node.Nodes.Clear();

        foreach(var line in input)
        {
            var self = new string(line[6..8]);
            var flowrate = int.Parse(line.Split(';')[0].Split('=')[1]);
            var edges = line.Split(" valve")[1].Trim(" s".ToArray()).Split(", ");

            var node = new Node(self) { Flowrate = flowrate };
            node.AdjacentNodeNames.AddRange(edges);
        }

        var maxFlowRatePossible = Node.Nodes.Values.Sum(x => x.Flowrate);

        var maxPressure = dfs("", Node.Nodes["AA"], "", 0, 29, maxFlowRatePossible, 0, int.MinValue);

        return maxPressure.ToString();
    }

    static string solutionPart2(string[] input)
    {
        Node.Nodes.Clear();

        foreach (var line in input)
        {
            var self = new string(line[6..8]);
            var flowrate = int.Parse(line.Split(';')[0].Split('=')[1]);
            var edges = line.Split(" valve")[1].Trim(" s".ToArray()).Split(", ");

            var node = new Node(self) { Flowrate = flowrate };
            node.AdjacentNodeNames.AddRange(edges);
        }

        var maxFlowRatePossible = Node.Nodes.Values.Sum(x => x.Flowrate);

        var maxPressure = dfs2("", Node.Nodes["AA"], "", Node.Nodes["AA"], "", 0, 25, maxFlowRatePossible, 0, int.MinValue);

        return maxPressure.ToString();
    }

    static int dfs(string previousValve, Node currentValve, string openValves, int flowrate, int minutesRemaing, int maxFlowRatePossible, int pressure, int alpha)
    {
        pressure += flowrate;

        var possiblePressure = pressure + maxFlowRatePossible * minutesRemaing;

        if (possiblePressure <= alpha)
            return pressure;

        if (minutesRemaing == 0)
            return pressure;

        if (flowrate == maxFlowRatePossible)
            return pressure + flowrate * minutesRemaing;
       
        var maxPressure = int.MinValue;

        if (currentValve.Flowrate > 0 && openValves.Contains(currentValve.Name) == false)
        {
            maxPressure = Math.Max(maxPressure, dfs(currentValve.Name, currentValve, openValves + currentValve.Name, flowrate + currentValve.Flowrate, minutesRemaing - 1, maxFlowRatePossible, pressure, Math.Max(alpha,maxPressure)));
        }

        foreach (var connectedValves in currentValve.AdjacentNodeNames)
        {
            if (connectedValves == previousValve)
                continue;

            maxPressure = Math.Max(maxPressure, dfs(currentValve.Name, Node.Nodes[connectedValves], openValves, flowrate, minutesRemaing - 1, maxFlowRatePossible, pressure, Math.Max(alpha, maxPressure)));
        }

        return maxPressure;
    }

    static int dfs2(string previousValveSelf, Node currentValveSelf, string previousValveElephant, Node currentValveElephant,  string openValves, int flowrate, int minutesRemaing, int maxFlowRatePossible, int pressure, int alpha)
    {
        pressure += flowrate;

        var possiblePressure = pressure + maxFlowRatePossible * minutesRemaing;

        if (possiblePressure <= alpha)
            return pressure;

        if (minutesRemaing == 0)
            return pressure;

        if (flowrate == maxFlowRatePossible)
            return pressure + flowrate * (minutesRemaing);

        var maxPressure = int.MinValue;

        var canOpenValveSelf = currentValveSelf.Flowrate > 0 && openValves.Contains(currentValveSelf.Name) == false;
        var canOpenValveElephant = currentValveElephant.Flowrate > 0 && openValves.Contains(currentValveElephant.Name) == false;

        if (canOpenValveSelf && canOpenValveElephant && currentValveSelf.Name != currentValveElephant.Name)
        {
            maxPressure = Math.Max(maxPressure, dfs2(currentValveSelf.Name, currentValveSelf, currentValveElephant.Name, currentValveElephant, openValves + currentValveSelf.Name + currentValveElephant.Name, flowrate + currentValveSelf.Flowrate + currentValveElephant.Flowrate, minutesRemaing - 1, maxFlowRatePossible, pressure, Math.Max(alpha, maxPressure)));
        }
        else if (canOpenValveSelf && !canOpenValveElephant)
        {
            foreach (var connectedValvesElephant in currentValveElephant.AdjacentNodeNames)
            {
                if (connectedValvesElephant == previousValveElephant)
                    continue;

                maxPressure = Math.Max(maxPressure, dfs2(currentValveSelf.Name, currentValveSelf, currentValveElephant.Name, Node.Nodes[connectedValvesElephant], openValves + currentValveSelf.Name, flowrate + currentValveSelf.Flowrate, minutesRemaing - 1, maxFlowRatePossible, pressure, Math.Max(alpha, maxPressure)));
            }
        }
        else if (!canOpenValveSelf && canOpenValveElephant)
        {
            foreach (var connectedValvesSelf in currentValveSelf.AdjacentNodeNames)
            {
                if (connectedValvesSelf == previousValveSelf)
                    continue;

                maxPressure = Math.Max(maxPressure, dfs2(currentValveSelf.Name, Node.Nodes[connectedValvesSelf], currentValveElephant.Name, currentValveElephant, openValves + currentValveElephant.Name, flowrate + currentValveElephant.Flowrate, minutesRemaing - 1, maxFlowRatePossible, pressure, Math.Max(alpha, maxPressure)));              
            }
        }
        else
        {
            foreach (var connectedValvesSelf in currentValveSelf.AdjacentNodeNames)
            {
                if (connectedValvesSelf == previousValveSelf)
                    continue;

                foreach (var connectedValvesElephant in currentValveElephant.AdjacentNodeNames)
                {
                    if (connectedValvesElephant == previousValveElephant)
                        continue;

                    maxPressure = Math.Max(maxPressure, dfs2(currentValveSelf.Name, Node.Nodes[connectedValvesSelf], currentValveElephant.Name, Node.Nodes[connectedValvesElephant], openValves, flowrate, minutesRemaing - 1, maxFlowRatePossible, pressure, Math.Max(alpha, maxPressure)));
                }
            }
        }

        return maxPressure;
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
class Solution
{
    static void Main(string[] args)
    {
        var input0 = GetInput($"{Environment.CurrentDirectory}\\Input0.txt");
        var input1 = GetInput($"{Environment.CurrentDirectory}\\Input1.txt");

        //Console.WriteLine($"Part 1, input 0: '{solutionPart1(input0)}'");
        //Console.WriteLine($"Part 1, input 1: '{solutionPart1(input1)}'");
        Console.WriteLine($"Part 2, input 0: '{solutionPart2(input0)}'");
        Console.WriteLine($"Part 2, input 1: '{solutionPart2(input1)}'");
    }

    static string solutionPart1(string[] input)
    {
        var sumQualityLevels = 0;

        foreach(var line in input)
        {
            var split = line.Split(':');

            var blueprintId = int.Parse(split[0].Split(' ')[1]);

            var geodesCount = calculateBlueprintGeodes(split[1], 24);

            var qualityLevel = blueprintId * geodesCount;

            sumQualityLevels += qualityLevel;

            Console.Error.WriteLine($"id: {blueprintId}, q-level: {qualityLevel}");

        }

        return sumQualityLevels.ToString();
    }

    static int calculateBlueprintGeodes(string input, int minutes)
    {
        // Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 12 clay. Each geode robot costs 3 ore and 8 obsidian.
        var inputSplit = input.Trim().Split('.');

        var oreRobotCostOre = int.Parse(inputSplit[0].Trim().Split()[4]);
        var clayRobotCostOre = int.Parse(inputSplit[1].Trim().Split()[4]);
        var obsidianRobotCostOre = int.Parse(inputSplit[2].Trim().Split()[4]);
        var obsidianRobotCostClay = int.Parse(inputSplit[2].Trim().Split()[7]);
        var geodeRobotCostOre = int.Parse(inputSplit[3].Trim().Split()[4]);
        var geodeRobotCostObsidian = int.Parse(inputSplit[3].Trim().Split()[7]);

        var maxGeodes = dfs(minutes - 1,
                            oreRobotCostOre,
                            clayRobotCostOre,
                            obsidianRobotCostOre,
                            obsidianRobotCostClay,
                            geodeRobotCostOre,
                            geodeRobotCostObsidian,
                            minedOre: 1,
                            minedClay: 0,
                            minedObsidian: 0,
                            minedGeode: 0,
                            oreRobotCount: 1,
                            clayRobotCount: 0,
                            obsidianRobotCount: 0,
                            geodeRobotCount: 0,
                            geodePruning: 0);

        return maxGeodes;
    }

    static int dfs(int minRemaining, 
                   int oreRobotCostOre,
                   int clayRobotCostOre,
                   int obsidianRobotCostOre,
                   int obsidianRobotCostClay,
                   int geodeRobotCostOre,
                   int geodeRobotCostObsidian,
                   int minedOre,
                   int minedClay,
                   int minedObsidian,
                   int minedGeode,
                   int oreRobotCount,
                   int clayRobotCount,
                   int obsidianRobotCount,
                   int geodeRobotCount,
                   int geodePruning)
    {
        if (minRemaining <= 0)
            return minedGeode;

        if (geodeRobotCount == 0 && minRemaining <= 1)
            return 0;

        var possibleGeodeRobotCount = geodeRobotCount;
        var possibleGeodes = minedGeode;

        for (int m = minRemaining; m > 0; m--)
        {
            possibleGeodes += possibleGeodeRobotCount;
            possibleGeodeRobotCount++;
        }

        if (geodePruning >= possibleGeodes)
            return minedGeode;

        var maxGeodes = int.MinValue;

        if (geodeRobotCostOre <= minedOre && geodeRobotCostObsidian <= minedObsidian && minRemaining >= 1)
        {
            maxGeodes = Math.Max(maxGeodes, dfs(minRemaining - 1,
                oreRobotCostOre, clayRobotCostOre, obsidianRobotCostOre, obsidianRobotCostClay, geodeRobotCostOre, geodeRobotCostObsidian,
                minedOre + oreRobotCount - geodeRobotCostOre,
                minedClay + clayRobotCount,
                minedObsidian + obsidianRobotCount - geodeRobotCostObsidian,
                minedGeode + geodeRobotCount,
                oreRobotCount,
                clayRobotCount,
                obsidianRobotCount,
                geodeRobotCount + 1,
                Math.Max(geodePruning, maxGeodes)));
        }
        else
        {
            if (oreRobotCostOre <= minedOre && minRemaining >= 2)
            {
                maxGeodes = Math.Max(maxGeodes, dfs(minRemaining - 1,
                    oreRobotCostOre, clayRobotCostOre, obsidianRobotCostOre, obsidianRobotCostClay, geodeRobotCostOre, geodeRobotCostObsidian,
                    minedOre + oreRobotCount - oreRobotCostOre,
                    minedClay + clayRobotCount,
                    minedObsidian + obsidianRobotCount,
                    minedGeode + geodeRobotCount,
                    oreRobotCount + 1,
                    clayRobotCount,
                    obsidianRobotCount,
                    geodeRobotCount,
                    Math.Max(geodePruning, maxGeodes)));
            }

            if (clayRobotCostOre <= minedOre && minRemaining >= 3)
            {
                maxGeodes = Math.Max(maxGeodes, dfs(minRemaining - 1,
                    oreRobotCostOre, clayRobotCostOre, obsidianRobotCostOre, obsidianRobotCostClay, geodeRobotCostOre, geodeRobotCostObsidian,
                    minedOre + oreRobotCount - clayRobotCostOre,
                    minedClay + clayRobotCount,
                    minedObsidian + obsidianRobotCount,
                    minedGeode + geodeRobotCount,
                    oreRobotCount,
                    clayRobotCount + 1,
                    obsidianRobotCount,
                    geodeRobotCount,
                    Math.Max(geodePruning, maxGeodes)));
            }

            if (obsidianRobotCostOre <= minedOre && obsidianRobotCostClay <= minedClay && minRemaining >= 2)
            {
                maxGeodes = Math.Max(maxGeodes, dfs(minRemaining - 1,
                    oreRobotCostOre, clayRobotCostOre, obsidianRobotCostOre, obsidianRobotCostClay, geodeRobotCostOre, geodeRobotCostObsidian,
                    minedOre + oreRobotCount - obsidianRobotCostOre,
                    minedClay + clayRobotCount - obsidianRobotCostClay,
                    minedObsidian + obsidianRobotCount,
                    minedGeode + geodeRobotCount,
                    oreRobotCount,
                    clayRobotCount,
                    obsidianRobotCount + 1,
                    geodeRobotCount,
                    Math.Max(geodePruning, maxGeodes)));
            }

            if (geodeRobotCostOre <= minedOre && geodeRobotCostObsidian <= minedObsidian && minRemaining >= 1)
            {
                maxGeodes = Math.Max(maxGeodes, dfs(minRemaining - 1,
                    oreRobotCostOre, clayRobotCostOre, obsidianRobotCostOre, obsidianRobotCostClay, geodeRobotCostOre, geodeRobotCostObsidian,
                    minedOre + oreRobotCount - geodeRobotCostOre,
                    minedClay + clayRobotCount,
                    minedObsidian + obsidianRobotCount - geodeRobotCostObsidian,
                    minedGeode + geodeRobotCount,
                    oreRobotCount,
                    clayRobotCount,
                    obsidianRobotCount,
                    geodeRobotCount + 1,
                    Math.Max(geodePruning, maxGeodes)));
            }

            maxGeodes = Math.Max(maxGeodes, dfs(minRemaining - 1,
                    oreRobotCostOre, clayRobotCostOre, obsidianRobotCostOre, obsidianRobotCostClay, geodeRobotCostOre, geodeRobotCostObsidian,
                    minedOre + oreRobotCount,
                    minedClay + clayRobotCount,
                    minedObsidian + obsidianRobotCount,
                    minedGeode + geodeRobotCount,
                    oreRobotCount,
                    clayRobotCount,
                    obsidianRobotCount,
                    geodeRobotCount,
                    Math.Max(geodePruning, maxGeodes)));
        }

        return maxGeodes;
    }

    static string solutionPart2(string[] input)
    {
        var totalGeodeProduct = 1;

        foreach (var line in input.Take(3))
        {
            var split = line.Split(':');

            var blueprintId = int.Parse(split[0].Split(' ')[1]);

            var geodesCount = calculateBlueprintGeodes(split[1], 32);

            totalGeodeProduct *= geodesCount;

            Console.Error.WriteLine($"id: {blueprintId}, geodes Count: {geodesCount}");

        }

        return totalGeodeProduct.ToString();
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n')
            .Select(x => x.Trim('\r'))
            .ToArray();
}
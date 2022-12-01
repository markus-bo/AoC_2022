Console.WriteLine("Advent Of Code Day 1");

var inputPath = $"{Environment.CurrentDirectory}\\Input1.txt";
var elveInventory = new Dictionary<int, List<int>>();

var index = 0;

using (var file = new StreamReader(inputPath))
{
    while (!file.EndOfStream)
    {
        var line = file.ReadLine();

        if (line == "")
        {
            index++;
            continue;
        }

        if (elveInventory.ContainsKey(index))
        {
            elveInventory[index].Add(int.Parse(line!));
        }
        else
        {
            elveInventory.Add(index, new List<int> { int.Parse(line!) });
        }
    }
}

var sortedInventory = elveInventory
    .Select(x => (index: x.Key, allocInventory: x.Value.Sum()))
    .OrderByDescending(x => x.allocInventory);
    

Console.WriteLine($"Solution for part 1: {sortedInventory.First().allocInventory}");
Console.WriteLine($"Solution for part 2: {sortedInventory.Take(3).Sum(x => x.allocInventory)}");

Console.ReadKey();
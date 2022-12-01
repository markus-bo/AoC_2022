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

var maxInventoryElve = elveInventory
    .Select(x => (index: x.Key, allocInventory: x.Value.Sum()))
    .OrderByDescending(x => x.allocInventory)
    .First();

Console.WriteLine($"{maxInventoryElve.index} {maxInventoryElve.allocInventory}");

Console.ReadKey();
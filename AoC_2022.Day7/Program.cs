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

    record File(string Name, int Size);

    class Directory
    {
        public string Name { get; private set; }

        public Directory? ParentFolder { get; private set; }

        private IList<Directory> SubFolders;

        private IList<File> Files;

        public Directory(string name, Directory? parent)
        {
            this.Name = name;
            this.ParentFolder = parent;
            this.SubFolders = new List<Directory>();
            this.Files = new List<File>();
        }

        public Directory GetSubFolder(string name) =>
            this.SubFolders.First(x => x.Name == name);

        public IEnumerable<Directory> GetAllSubFolders() =>
            this.SubFolders
                .SelectMany(sub =>
                    sub.GetAllSubFolders()
                    .Append(sub))
                .ToList();

        public void CreateSubFolder(string name) =>
            this.SubFolders.Add(new Directory(name, this));

        public void CreateFile(string name, int size) =>
            this.Files.Add(new File(name, size));

        public int GetTotalSize() =>
             this.Files.Sum(x => x.Size) + this.SubFolders.Sum(x => x.GetTotalSize());
    }

    static string solutionPart1(string[] input)
    {
        var root = getDirectoryTreeRoot(input);

        var totalSizeAboveThreshold = root.GetAllSubFolders()
                                            .Select(x => x.GetTotalSize())
                                            .Where(x => x <= 100000)
                                            .Sum();

        return totalSizeAboveThreshold.ToString();
    }

    static string solutionPart2(string[] input)
    {
        var root = getDirectoryTreeRoot(input);

        var freeDiscSpace = 70_000_000 - root.GetTotalSize();
        var neededSpace = 30_000_000 - freeDiscSpace;

        var directoryToDelete = root.GetAllSubFolders()
            .Select(x => x.GetTotalSize())
            .Where(x => x >= neededSpace)
            .OrderBy(x => x)
            .First();

        return directoryToDelete.ToString();
    }

    static Directory getDirectoryTreeRoot(string[] input)
    {
        var root = new Directory(
            name: "/",
            parent: null);

        Directory? current = root;

        foreach (var line in input.Skip(1))
        {
            var splitLine = line.Split();

            if (line.StartsWith("$ cd"))
            {
                var dest = splitLine[2];

                if (dest == "..")
                    current = current!.ParentFolder;
                else
                    current = current!.GetSubFolder(dest);
            }
            else if (line.StartsWith("$ ls"))
            {
                // do nothing
            }
            else
            {
                if (splitLine[0] == "dir")
                    current!.CreateSubFolder(
                        name: splitLine[1]);
                else
                    current!.CreateFile(
                        name: splitLine[1],
                        size: int.Parse(splitLine[0]));
            }
        }

        return root;
    }

    static string[] GetInput(string inputPath) =>
        new StreamReader(inputPath)
            .ReadToEnd()
            .Split('\n');
}
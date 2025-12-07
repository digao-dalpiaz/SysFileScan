using System.Globalization;

namespace SysFileScan
{
    internal class Comparer
    {

        public void Execute(string[] args)
        {
            Console.WriteLine("Reports Comparer");

            if (args.Length != 3)
                throw new Exception("Invalid arguments! Please use /compare file1.txt file2.txt");

            string file1 = args[1];
            string file2 = args[2];

            Console.WriteLine("File 1: " + file1);
            Console.WriteLine("File 2: " + file2);

            var data1 = GetTable(file1);
            var data2 = GetTable(file2);

            Console.WriteLine("Comparing...");

            //run data1
            foreach (var reg in data1)
            {
                if (data2.TryGetValue(reg.Key, out var result))
                {
                    //file found in data2
                    if (result.Timestamp != reg.Value.Timestamp)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Modified: ");
                        Console.ResetColor();
                        Console.WriteLine(reg.Key);
                    }
                }
                else
                {
                    //file not found in data2
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Deleted: ");
                    Console.ResetColor();
                    Console.WriteLine(reg.Key);
                }
            }

            //run data2
            foreach (var reg in data2)
            {
                if (!data1.TryGetValue(reg.Key, out var result))
                {
                    //file not found in data1
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("New: ");
                    Console.ResetColor();
                    Console.WriteLine(reg.Key);
                }
            }
        }

        class FileInfo
        {
            public DateTime Timestamp;
        }
        private Dictionary<string, FileInfo> GetTable(string file)
        {
            Console.WriteLine("Reading file " + file);

            var data = new Dictionary<string, FileInfo>();

            if (!File.Exists(file)) throw new Exception("File not found: " + file);
            var lines = File.ReadAllLines(file);

            foreach (var item in lines)
            {
                var parts = item.Split('|');

                data.Add(parts[1], new FileInfo
                {
                    Timestamp = DateTime.ParseExact(parts[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                });
            }

            return data;
        }

    }
}

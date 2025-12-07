using System.Diagnostics;

namespace SysFileScan
{
    internal class Scanner
    {

        public void Execute()
        {
            Console.WriteLine("File Scanner");

            string ignoreFile = Path.Combine(AppContext.BaseDirectory, "ignore.txt");
            var ignore = File.Exists(ignoreFile) ? File.ReadAllLines(ignoreFile) : [];

            //string baseFolder = Directory.GetCurrentDirectory();
            string baseFolder = "/";
            Console.WriteLine("Base folder: " + baseFolder);

            string outputFile = Path.Combine(AppContext.BaseDirectory, "report_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");
            Console.WriteLine("Report file: " + outputFile);

            int count = 0;

            var sw = Stopwatch.StartNew();

            RunFolder(baseFolder);

            void RunFolder(string folder)
            {
                if (ignore.Contains(folder)) return;

                string[] files;
                try
                {
                    files = Directory.GetFiles(folder);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine($"Can't access folder: {folder} - {ex.Message}");
                    return;
                }
                foreach (var file in files)
                {
                    var ts = File.GetLastWriteTime(file);
                    File.AppendAllText(outputFile, ts.ToString("yyyy-MM-dd HH:mm:ss") + "|" + file + "\n");

                    count++;

                    if (sw.Elapsed > TimeSpan.FromSeconds(15))
                    {
                        Console.WriteLine("Count until now: " + count);
                        sw.Restart();
                    }
                }

                var subfolders = Directory.GetDirectories(folder);
                foreach (var subfolder in subfolders)
                {
                    RunFolder(subfolder);
                }
            }

            Console.WriteLine("Files count: " + count);
            Console.WriteLine("Report is done!");
        }

    }
}

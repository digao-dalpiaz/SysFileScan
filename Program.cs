Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("===============================================");
Console.WriteLine("================ DIGAO DALPIAZ ================");
Console.WriteLine("========== System Files Compare Tool ==========");
Console.WriteLine("===============================================");
Console.ResetColor();

var cmd = args.Length >= 1 ? args[0] : null;

try
{
    switch (cmd)
    {
        case "/compare":
            new SysFileScan.Comparer().Execute(args);
            break;

        case "/scan":
            new SysFileScan.Scanner().Execute();
            break;

        case "/help":
            Console.WriteLine("Valid Arguments:");
            Console.WriteLine("/scan");
            Console.WriteLine("/compare file1.txt file2.txt");
            break;

        default:
            throw new Exception("Invalid command!");
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: " + ex.Message);
    Console.ResetColor();
    Environment.ExitCode = 9;
}
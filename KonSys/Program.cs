using System;
using System.IO;

namespace KonSys
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryWatcher directoryWatcher = new DirectoryWatcher();
            Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()));
            directoryWatcher.Writer(Convert.ToString(Directory.GetParent(Directory.GetCurrentDirectory())));

        }
    }
}

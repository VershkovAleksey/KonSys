using System;
using System.IO;

namespace KonSys
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryWatcher directoryWatcher = new DirectoryWatcher();
            
            directoryWatcher.Writer(@"G:\Важое");

        }
    }
}

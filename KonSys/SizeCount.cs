using System;
using System.IO;

namespace KonSys
{
    public class SizeCount
    {
        public void Method()
        {
            var defaultOut = Console.Out;

            var outfilename = Path.Combine(Environment.CurrentDirectory, "name.html");
            Console.WriteLine(outfilename);
            var outname = new StreamWriter(outfilename);
            Console.SetOut(outname);

            Console.WriteLine("aiiii");
            outname.Close();
            Console.SetOut(defaultOut);

            Console.Write("hai");
            string nam1 = Console.ReadLine();
            Console.WriteLine(nam1);
            Console.ReadLine();
        }
    }
}

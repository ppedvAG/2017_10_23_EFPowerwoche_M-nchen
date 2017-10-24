using System;

namespace HalloDelegates
{
    internal delegate string MyDelegate(int zahl, double wert);

    internal class Program
    {
        static void Main(string[] args)
        {
            MyDelegate del = new MyDelegate(MeineMethode);

            var result = del.Invoke(5, 8.9);
            Console.WriteLine(result);
            Console.ReadLine();
        }

        private static string MeineMethode(int i, double d)
        {
            return (i + d).ToString();
        }
    }
}

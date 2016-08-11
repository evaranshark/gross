using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrossArithmetik;

namespace Test_Gross_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string arg1 = "1g3_7g1";
            string arg2 = "3g0";
            double arg3 = 5;
            Gross g1 = new Gross();
            Gross g2 = new Gross();
            Gross gn = new Gross();
            g1 = arg1.ParseG();
            g2 = arg2.ParseG();
            //Gross g3 = g1 - g1;
            try
            {
                Console.WriteLine("g1 is {0}", g1.Show());
                Console.WriteLine("g2 is {0}", g2.Show());
                Console.WriteLine("arg3 is {0}", arg3);
                Console.WriteLine("gn is {0}", gn.Show());
                Console.WriteLine("g1 + g2 is {0}", (g1 + g2).Show());
                Console.WriteLine("g1 - g2 is {0}", (g1 - g2).Show());
                Console.WriteLine("g2 - g1 is {0}", (g2 - g1).Show());
                Console.WriteLine("g1 - g1 is {0}", (g1 - g1).Show());
                Console.WriteLine("g1 + arg3 is {0}", (g1 + arg3).Show());
                Console.WriteLine("g2 + arg3 is {0}", (g2 + arg3).Show());
                Console.WriteLine("g1 + gn is {0}", (g1 + gn).Show());
                Console.WriteLine("gn + g1 is {0}", (gn + g1).Show());
                Console.WriteLine("gn + gn is {0}", (gn + gn).Show());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured. Message is\n {0}", e.Message);
            }
            Console.ReadKey();
        }
    }
}

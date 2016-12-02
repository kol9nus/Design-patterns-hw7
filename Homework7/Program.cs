using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework7
{
    class Program
    {
        static void Main(string[] args)
        {
            CopyContext copyContext = new CopyContext();

            try
            {
                copyContext.Pay(10);
                copyContext.ChooseDevice(ChooseDeviceState.DEVICE_WIFI);
                copyContext.ChooseDoc("Hello.doc");
                copyContext.PrintDoc();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}

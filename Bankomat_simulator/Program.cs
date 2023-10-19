using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat_simulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InterfacciaUtente.MenuIntestazione("selezione banche");
            InterfacciaUtente.SceltaBanche();
            Console.ReadLine();
        }
    }
}

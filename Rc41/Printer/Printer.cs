using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        int printPosition;
        string printBuffer;
        Cpu cpu;
        Form1 window;
        public char printerMode { get; set; }
        public bool power { get; set; }
        public int arg { get; set; }

        public Printer(Cpu c, Form1 window)
        {
            cpu = c;
            this.window = window;
            printPosition = 0;
            printBuffer = "";
            power = true;
            printerMode = 'M';
        }

    }
}

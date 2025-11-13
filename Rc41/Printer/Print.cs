using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public void Print(string line, char justify)
        {
            char c;
            string printLine;
            printLine = "";
            for (int i = 0; i < line.Length; i++)
            {
                c = line[i];
                //                if (c == 0x7a) c = '"';
                //                if (c == 0x7f) c = '|';
                //                if (c == 0x00) c = '_';
                //                if (c < 32 || c > 126) c = '#';
                printLine += c;
            }
            ui.Print(printLine, justify);
            // Note: PrintToFile functionality needs to be moved to UI layer
            // if (ui.PrintToFile())
            // {
            //     using (StreamWriter file = new StreamWriter("printer.txt", true))
            //     {
            //         file.WriteLine(printLine);
            //     }
            // }
        }
    }
}

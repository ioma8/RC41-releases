using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        void PrintToBuffer(byte b)
        {
            if (BufferLength(printBuffer) >= 168)
            {
                Print(printBuffer, ' ');
                printBuffer = "";
                printPosition = 0;
            }
            printBuffer += ((char)b).ToString();
            printPosition++;
        }
    }
}

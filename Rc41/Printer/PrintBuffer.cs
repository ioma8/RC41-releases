using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public void PrintBuffer(char justify)
        {
            string buffer;
            //            if (printBuffer.Length != 0)
            //            {
            buffer = "";
            buffer += printBuffer;
            Print(buffer, justify);
            printBuffer = "";
            printPosition = 0;
            //            }
        }
    }
}

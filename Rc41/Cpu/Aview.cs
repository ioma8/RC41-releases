using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Aview()
        {
            int n;
            int m;
            string buffer;
            n = 0;
            m = REG_P + 2;
            buffer = "";
            while (m >= REG_M)
            {
                if (ram[m] == 0 && n != 0) buffer += ((char)0x00).ToString();
                else if (ram[m] != 0) buffer += ((char)ram[m]).ToString();
                if (ram[m] != 0) n = -1;
                m--;
            }
            Message(buffer);
            if (printer.power)
            {
                if (!running || FlagSet(21)) printer.Print(buffer, 'L');
            }

        }
    }
}

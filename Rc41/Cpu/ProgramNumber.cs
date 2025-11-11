using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        string ProgramNumber(ref int adr)
        {
            int b;
            int p;
            char mode;
            string mant = "";
            string expn = "";
            char sign;
            char esign;
            string buffer;
            mode = 'M';
            p = 0;
            sign = ' ';
            esign = ' ';
            while (ram[adr] >= 0x10 && ram[adr] <= 0x1c) adr++;
            adr--;
            b = ram[adr];
            while (b >= 0x10 && b <= 0x1c)
            {
                if (b <= 0x19)
                {
                    if (mode == 'M') mant += (char)('0' + b - 0x10);
                    else expn += (char)('0' + b - 0x10);
                }
                if (b == 0x1a) mant += '.';
                if (b == 0x1b)
                {
                    if (mode == 'M')
                    {
                        p = 0;
                        mode = 'E';
                    }
                }
                if (b == 0x1c)
                {
                    if (mode == 'M') sign = (sign == ' ') ? '-' : ' ';
                    else esign = (esign == ' ') ? '-' : ' ';
                }
                adr--;
                b = ram[adr];
            }
            buffer = "";
            if (sign == '-') buffer += "-";
            buffer += mant;
            if (mode == 'E')
            {
                buffer += " E";
                if (esign == '-') buffer += "-";
                buffer += expn;
            }
            return buffer;
        }
    }
}

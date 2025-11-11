using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        int ExecNumber(int address)
        {
            int e;
            int ex;
            int i;
            int p;
            byte dp;
            char mode;
            Number n;
            dp = 0;
            p = 0;
            e = -1;
            ex = 0;
            n = new Number();
            n.sign = 0;
            n.esign = 0;
            mode = 'Z';
            for (i = 0; i < 10; i++) n.mantissa[i] = 0;
            n.exponent[0] = 0;
            n.exponent[1] = 0;
            while (ram[address] >= 0x10 && ram[address] <= 0x1c)
            {
                if (ram[address] >= 0x10 && ram[address] <= 0x19)
                {
                    if (mode == 'Z' && ram[address] > 0x10) mode = 'M';
                    if (mode == 'M')
                    {
                        if (p < 10) n.mantissa[p++] = (byte)(ram[address] - 0x10);
                        if (dp == 0) e++;
                    }
                    if (mode == 'E')
                    {
                        ex = (ex * 10) + (ram[address] - 0x10);
                    }
                    if (mode == 'Z' && ram[address] == 0x10 && dp != 0) e--;
                }
                if (ram[address] == 0x1a && mode != 'E')
                {
                    if (dp == 0) dp = 0xff;
                }
                if (ram[address] == 0x1b)
                {
                    if (mode != 'E')
                    {
                        mode = 'E';
                        ex = 0;
                        if (n.mantissa[0] == 0) n.mantissa[0] = 1;
                    }
                }
                if (ram[address] == 0x1c)
                {
                    if (mode == 'M') n.sign = (byte)((n.sign == 0) ? 9 : 0);
                    else n.esign = (byte)((n.esign == 0) ? 9 : 0);
                }
                address--;
            }
            if (n.esign != 0) e -= ex; else e += ex;
            if (e > 99 || e < -99)
            {
                Message("RANGE ERR");
                Error();
                return address;
            }
            n.esign = (byte)((e < 0) ? 9 : 0);
            if (e < 0) e = -e;
            n.exponent[0] = (byte)(e / 10);
            n.exponent[1] = (byte)(e % 10);
            StoreNumber(n, R_X);
            ram[PENDING] = (byte)'E';
            return address;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        int GtoXeqInd(int address)
        {
            int addr;
            int last;
            int ret;
            byte lbl;
            bool flag;
            string label;
            Number n;
            lbl = (byte)(ram[address - 1] & 0x7f);
            n = Rcl(lbl);
            lbl = (byte)ToInteger(n);
            if (n.sign == 0x01)
            {
                label = Format(n);
                addr = FindGlobal(label);
                if (addr == 0)
                {
                    Message("NONEXISTENT");
                    Error();
                    return 0;
                }
                ret = addr;
                addr = ToPtr(addr);
                ram[REG_B + 1] = (byte)(addr >> 8);
                ram[REG_B + 0] = (byte)(addr & 0xff);
                ram[REG_E + 0] = 0xff;
                ram[REG_E + 1] |= 0x0f;
                return ret;
            }
            address--;
            addr = (ram[REG_B + 1] << 8) | ram[REG_B + 0];
            addr = FromPtr(addr) - 1;
            last = addr;
            flag = true;
            while (flag)
            {
                if (lbl < 15 && (ram[addr] == lbl + 1)) flag = false;
                else if (lbl > 14 && (ram[addr] == 0xcf) && ((ram[addr - 1] & 0x7f) == lbl)) flag = false;
                else if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd &&
                         ram[addr - 2] < 0xf0)
                {
                    addr = FindStart(addr);
                }
                else
                {
                    addr -= isize(addr);
                }
                if (flag != false && addr == last)
                {
                    Message("NONEXISTENT");
                    Error();
                    return 0;
                }
            }
            addr++;
            if (lbl < 15) ret = addr - 2;
            else ret = addr - 1;
            addr = ToPtr(addr);
            ram[REG_B + 1] = (byte)(addr >> 8);
            ram[REG_B + 0] = (byte)(addr & 0xff);
            ram[REG_E + 0] = 0xff;
            ram[REG_E + 1] |= 0x0f;
            return ret;
        }
    }
}

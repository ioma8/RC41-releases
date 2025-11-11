using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int FindGlobal(string label)
        {
            int addr;
            string lbl1;
            string lbl2;
            int i;
            bool flag;
            int dst;
            lbl1 = "";
            for (i = 0; i < label.Length; i++)
            {
                if (label[i] != '"')
                {
                    lbl1 += label.ElementAt(i).ToString();
                }
            }
            addr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            addr = FromPtr(addr) + 2;
            flag = true;
            while (flag)
            {
                if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd && ram[addr - 2] >= 0xf0)
                {
                    lbl2 = "";
                    for (i = 1; i < (ram[addr - 2] & 0xf); i++)
                        lbl2 += ((char)(ram[addr - 3 - i])).ToString();
                    if (lbl1.Equals(lbl2))
                    {
                        return addr + 1;
                    }
                }
                dst = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                dst = ((dst & 0x1ff) * 7) + ((dst >> 9) & 0x7);
                if (dst == 0) flag = false;
                else addr += dst - 1;
            }
            Message("NONEXISTENT");
            Error();
            return 0;
        }
    }
}

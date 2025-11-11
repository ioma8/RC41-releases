using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void UnAsn(byte keycode, int flag)
        {
            int addr;
            int dst;
            SetKaFlag(keycode, false);
            if ((flag & 1) != 0)
            {
                addr = 0x0c0 * 7;
                while (ram[addr + 6] == 0xf0)
                {
                    if (ram[addr + 0] == keycode)
                    {
                        ram[addr + 0] = 0x00;
                        ram[addr + 1] = 0x00;
                        ram[addr + 2] = 0x00;
                    }
                    if (ram[addr + 3] == keycode)
                    {
                        ram[addr + 3] = 0x00;
                        ram[addr + 4] = 0x00;
                        ram[addr + 5] = 0x00;
                    }
                    addr += 7;
                }
            }
            if ((flag & 2) != 0)
            {
                addr = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
                addr = FromPtr(addr) + 2;
                dst = -1;
                while (dst != 0)
                {
                    if (ram[addr] >= 0xc0 && ram[addr] <= 0xcd &&
                        ram[addr - 2] >= 0xf0 && ram[addr - 3] == keycode)
                    {
                        ram[addr - 3] = 0x00;
                    }
                    dst = ((ram[addr] & 0x0f) << 8) | ram[addr - 1];
                    dst = ((dst & 0x1ff) * 7) + ((dst >> 9) & 0x7);
                    if (dst != 0) addr += dst - 1;
                }
            }
        }
    }
}

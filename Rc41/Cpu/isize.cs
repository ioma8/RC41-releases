using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int isize(int address)
        {
            int size;
            byte cmd;
            byte b;
            cmd = ram[address];
            if (cmd >= 0x10 && cmd <= 0x1c)
            {
                size = 1;
                address--;
                while (ram[address] >= 0x10 && ram[address] <= 0x1c)
                {
                    address--;
                    size++;
                }
                return size;
            }
            if (cmd == 0x1d || cmd == 0x1e)
            {
                b = ram[address - 1];
                return 2 + (b & 0xf);
            }
            if (cmd < 0x90) return 1;
            if (cmd >= 0x90 && cmd <= 0x9f) return 2;
            if (cmd >= 0xa0 && cmd <= 0xa7) return 2;
            if (cmd >= 0xa8 && cmd <= 0xae) return 2;
            if (cmd >= 0xb0 && cmd <= 0xbf) return 2;
            if (cmd >= 0xc0 && cmd <= 0xcd)
            {
                b = ram[address - 2];
                if (b < 0xf0) return 3;
                return 3 + (b & 0xf);
            }
            if (cmd >= 0xce && cmd <= 0xcf) return 2;
            if (cmd >= 0xd0 && cmd <= 0xdf) return 3;
            if (cmd >= 0xe0 && cmd <= 0xef) return 3;
            if (cmd >= 0xf0 && cmd <= 0xff) return 1 + (cmd & 0xf);
            return 1;
        }
    }
}

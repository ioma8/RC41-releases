using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Clp(string name)
        {
            int i;
            int address;
            int size;
            address = FindGlobal(name);
            if (address == 0) return;
            address = FindStart(address);
            while (ram[address] < 0xc0 || ram[address] > 0xcd || ram[address - 2] >= 0xf0)
            {
                if (ram[address] >= 0xc0 && ram[address] <= 0xcd && ram[address - 2] >= 0xf0 && ram[address - 3] != 0x00)
                {
                    SetKaFlag(ram[address - 3], false);
                }
                size = isize(address);
                for (i = 0; i < size; i++)
                    ram[address--] = 0x00;
            }
            if ((ram[address - 2] & 0xf0) != 0x20)
            {
                for (i = 0; i < 3; i++)
                    ram[address--] = 0x00;
            }

            ReLink();
            Pack();
        }
    }
}

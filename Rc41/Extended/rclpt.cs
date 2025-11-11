using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void rclpt()
        {
            int addr;
            int rec;
            int chr;
            Number x;
            string number;
            if (currentFile <= 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            addr = currentFile - 1;
            if (ram[addr * 7 + 6] == 'D')
            {
                rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
                x = new Number(rec);
                cpu.StoreNumber(x, Cpu.R_X);
            }
            else if (ram[(currentFile - 1) * 7 + 6] == 'A')
            {
                rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
                chr = (ram[addr * 7 + 1] << 8) | ram[addr * 7 + 0];
                number = $"{rec}.{chr:D3}";
                x = cpu.AtoN(number);
                cpu.StoreNumber(x, Cpu.R_X);
            }
            else
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }

        }
    }
}

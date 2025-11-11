using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;


namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void emdir()
        {
            string entry;
            int p;
            int i;
            int s;
            p = 599 * 7;
            if (ram[p + 6] == 0xff)
            {
                cpu.Message("DIR EMPTY");
                cpu.StoreNumber(new Number(597), Cpu.R_X);
                return;
            }
            while (ram[p+6] != 0xff)
            {
                entry = "";
                for (i = 0; i < 7; i++)
                    entry += (char)ram[p + 6 - i];
                entry += " ";
                p -= 7;
                entry += (char)ram[p + 6];
                s = ram[p + 5] * 256 + ram[p + 4];
                entry += $"{s:d3}";
                window.Print(entry, 'L');
                p -= 7;
                p -= s * 7;
            }
            p /= 7;
            p -= 2;
            entry = $"{p}";
            window.Print(entry, 'L');
            cpu.StoreNumber(new Number(p), Cpu.R_X);
        }
    }
}

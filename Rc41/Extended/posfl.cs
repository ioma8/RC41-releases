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
        public void posfl()
        {
            int addr;
            int currentPtr;
            int chr;
            int rec;
            Number x;
            int start;
            string record;
            string alpha = cpu.GetAlpha();
            if (currentFile <= 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if (ram[(currentFile-1) * 7 + 6] != 'A')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            addr = currentFile - 1;
            currentPtr = CurrentRecord();
            chr = (ram[addr * 7 + 1] << 8) | ram[addr * 7 + 0];
            rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            while (ram[currentPtr] != 0xff)
            {
                record = "";
                addr = currentPtr - (chr + 1);
                start = chr;
                while (chr < ram[currentPtr])
                {
                    record += (char)ram[addr--];
                    chr++;
                }
                chr = 0;
                if (record.IndexOf(alpha) >= 0)
                {
                    addr = currentFile - 1;
                    chr = start + record.IndexOf(alpha);
                    ram[addr * 7 + 3] = (byte)((rec >> 8) & 0xff);
                    ram[addr * 7 + 2] = (byte)(rec & 0xff);
                    ram[addr * 7 + 1] = (byte)((chr >> 8) & 0xff);
                    ram[addr * 7 + 0] = (byte)(chr & 0xff);
                    cpu.StoreNumber(cpu.AtoN($"{rec}.{chr:D3}"), Cpu.R_X);
                    return;
                }
                currentPtr -= (ram[currentPtr] + 1);
                rec++;
            }
            cpu.StoreNumber(new Number(-1), Cpu.R_X);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void getas()
        {
            int i;
            int p;
            int s;
            int addr;
            int size;
            List<byte> data;
            string exname;
            string flname;
            string alpha = cpu.GetAlpha();
            if (alpha.IndexOf(',') >= 0)
            {
                exname = alpha.Substring(0, alpha.IndexOf(","));
                flname = alpha.Substring(alpha.IndexOf(",") + 1);
            }
            else
            {
                flname = alpha;
                exname = alpha;
            }
            if (exname.Length > 7) exname = exname.Substring(0, 7);
            while (exname.Length < 7) exname += " ";
            if (exname.Equals("       "))
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            addr = FindFile(exname);
            if (addr < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            addr--;
            if (ram[addr * 7 + 6] != 'A')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            data = cpu.tapeDrive.LoadData(flname);
            if (data != null)
            {
                size = (ram[addr * 7 + 5] << 8) | ram[addr * 7 + 4];
                ram[addr * 7 + 3] = 0x00;
                ram[addr * 7 + 2] = 0x00;
                ram[addr * 7 + 1] = 0x00;
                ram[addr * 7 + 0] = 0x00;
                size *= 7;
                addr = (addr * 7) - 1;
                p = 0;
                while (p < data.Count)
                {
                    s = data[p];
                    if (p + s > size)
                    {
                        cpu.Message("END OF FL");
                        cpu.Error();
                        ram[addr] = 0xff;
                        return;
                    }
                    if (s + p > data.Count)
                    {
                        ram[addr] = 0xff;
                        return;
                    }
                    for (i=0; i<=s; i++)
                    {
                        ram[addr--] = data[p++];
                    }
                    if (data[p] == 0xff)
                    {
                        ram[addr] = 0xff;
                        p = data.Count;
                    }
                }
            }
        }
    }
}

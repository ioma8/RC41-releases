using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void saveas()
        {
            int addr;
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
            addr--;
            data = new List<byte>();
            addr = addr * 7 + 6;
            while (ram[addr] != 0xff) data.Add(ram[addr--]);
            data.Add(0xff);
            cpu.tapeDrive.SaveData(flname, data);
        }
    }
}

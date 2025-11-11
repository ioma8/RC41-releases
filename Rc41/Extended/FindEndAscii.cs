using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public int FindEndAscii()
        {
            int addr;
            int recs;
            if (currentFile <= 0) return -1;
            addr = currentFile-1;
            if (ram[addr * 7 + 6] != 'A') return -1;
            recs = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            addr--;
            addr = addr * 7 + 6;
            while (recs > 0)
            {
                addr -= (ram[addr] + 1);
                recs--;
            }
            return addr;
        }
    }
}

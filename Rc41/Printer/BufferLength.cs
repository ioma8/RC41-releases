using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public int BufferLength(string buffer)
        {
            int res;
            res = 0;
            foreach (char c in buffer)
            {
                if (c >= 0x80) res += 1;
                else res += 7;
            }
            return res;
        }
    }
}

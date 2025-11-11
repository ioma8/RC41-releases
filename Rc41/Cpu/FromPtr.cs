using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public int FromPtr(int ptr)
        {
            return ((ptr & 0xfff) * 7) + ((ptr >> 12) & 0xf);
        }
    }
}

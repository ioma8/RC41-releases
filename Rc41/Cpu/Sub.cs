using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public Number Sub(Number a, Number b)
        {
            b.sign = (byte)((b.sign == 0) ? 9 : 0);
            return Add(a, b);
        }
    }
}

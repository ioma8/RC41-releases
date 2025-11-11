using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number HmsMinus(Number a, Number b)
        {
            a.sign = (byte)((a.sign != 0) ? 0 : 9);
            return HmsPlus(a, b);
        }
    }
}

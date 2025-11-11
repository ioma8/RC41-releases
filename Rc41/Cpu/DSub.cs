using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        DNumber D_Sub(DNumber a, DNumber b)
        {
            b.sign = (byte)((b.sign == 0) ? 9 : 0);
            return D_Add(a, b);
        }
    }
}

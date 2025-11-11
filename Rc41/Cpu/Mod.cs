using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Mod(Number x, Number y)
        {
            Number a;
            a = Div(x, y);
            a = Integer(a);
            a = Mul(y, a);
            a = Sub(x, a);
            a.sign = (byte)((x.sign != y.sign) ? 9 : 0);
            return a;
        }
    }
}

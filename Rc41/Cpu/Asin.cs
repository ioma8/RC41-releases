using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Asin(Number a)
        {
            Number t;
            Number x;
            t = Mul(a, a);
            t = Sub(S_ONE, t);
            t = Sqrt(t);
            x = Div(a, t);
            t = Atan(x);
            return t;
        }
    }
}

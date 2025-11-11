using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number YtoX(Number y, Number x)
        {
            x = Ln(x);
            x = Mul(x, y);
            x = Ex(x);
            return x;
        }
    }
}

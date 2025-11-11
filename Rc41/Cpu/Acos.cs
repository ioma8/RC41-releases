using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Acos(Number a)
        {
            a = Asin(a);
            return Sub(S_PI2, a);
        }
    }
}

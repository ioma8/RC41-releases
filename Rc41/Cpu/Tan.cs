using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Tan(Number a)
        {
            Number x;
            Number s;
            Number c;
            s = Sin(a);
            c = Cos(a);
            x = Div(s, c);
            return x;
        }
    }
}

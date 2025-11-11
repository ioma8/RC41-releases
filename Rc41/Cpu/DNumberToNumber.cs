using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number DNumberToNumber(DNumber a)
        {
            int i;
            Number r;
            r = new Number();
            r.sign = a.sign;
            r.esign = a.esign;
            for (i = 0; i < 10; i++) r.mantissa[i] = a.mantissa[i];
            r.exponent[0] = a.exponent[1];
            r.exponent[1] = a.exponent[2];
            return r;
        }
    }
}

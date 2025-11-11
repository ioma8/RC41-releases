using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        DNumber NumberToDNumber(Number a)
        {
            int i;
            DNumber r;
            r = new DNumber();
            r.sign = a.sign;
            r.esign = a.esign;
            for (i = 0; i < 10; i++) r.mantissa[i] = a.mantissa[i];
            for (i = 10; i < 20; i++) r.mantissa[i] = 0;
            r.exponent[0] = 0;
            r.exponent[1] = a.exponent[0];
            r.exponent[2] = a.exponent[1];
            return r;
        }
    }
}

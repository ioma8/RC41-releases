using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        bool IsInteger(Number a)
        {
            int e;
            e = (a.exponent[0] * 10) + a.exponent[1];
            if (a.esign != 0) return false;
            e++;
            while (e < 10)
            {
                if (a.mantissa[e] != 0) return false;
                e++;
            }
            return true;
        }
    }
}

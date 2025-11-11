using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        bool IsZero(Number a)
        {
            int i;
            for (i = 0; i < 10; i++)
                if (a.mantissa[i] != 0) return false;
            return true;
        }
    }
}

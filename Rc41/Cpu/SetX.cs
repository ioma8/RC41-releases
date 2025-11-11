using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void SetX(Number n, int l, int s)
        {
            Number a;
            if (lift && s != 0)
            {
                a = RecallNumber(R_Z);
                StoreNumber(a, R_T);
                a = RecallNumber(R_Y);
                StoreNumber(a, R_Z);
                a = RecallNumber(R_X);
                StoreNumber(a, R_Y);
            }
            if (l != 0)
            {
                a = RecallNumber(R_X);
                StoreNumber(a, R_L);
            }
            StoreNumber(n, R_X);
            lift = false;
        }
    }
}

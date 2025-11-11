using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Sci(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 0x0a)
            {
                Message("DATA ERRROR");
                Error();
                return;
            }
            ClearFlag(40);
            ClearFlag(41);
            if ((n & 8) != 0) SetFlag(36); else ClearFlag(36);
            if ((n & 4) != 0) SetFlag(37); else ClearFlag(37);
            if ((n & 2) != 0) SetFlag(38); else ClearFlag(38);
            if ((n & 1) != 0) SetFlag(39); else ClearFlag(39);
        }
    }
}

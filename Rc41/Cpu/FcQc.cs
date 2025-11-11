using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        bool FcQc(byte n)
        {
            Number a;
            bool flag;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return false;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
            }
            if (n >= 30)
            {
                Message("NONEXISTENT");
                Error();
                return false;
            }
            flag = FlagSet(n);
            ClearFlag(n);
            return flag;
        }
    }
}

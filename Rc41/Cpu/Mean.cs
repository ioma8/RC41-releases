using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Mean()
        {
            int b;
            int ofs;
            Number x;
            Number y;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            x = RecallNumber(R_X);
            StoreNumber(x, R_L);
            y = RecallNumber(b + 0);
            x = RecallNumber(b + 5);
            if (IsZero(x))
            {
                Message("DATA ERROR");
                Error();
                return;
            }
            x = Div(y, x);
            StoreNumber(x, R_X);
            y = RecallNumber(b + 2);
            x = RecallNumber(b + 5);
            x = Div(y, x);
            StoreNumber(x, R_Y);
        }
    }
}

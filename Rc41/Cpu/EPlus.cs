using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void EPlus()
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
            a = RecallNumber(b);
            x = RecallNumber(R_X);
            c = Add(a, x);
            StoreNumber(c, b);
            a = RecallNumber(b + 1);
            c = Mul(x, x);
            a = Add(a, c);
            StoreNumber(a, b + 1);
            a = RecallNumber(b + 2);
            y = RecallNumber(R_Y);
            c = Add(a, y);
            StoreNumber(c, b + 2);
            a = RecallNumber(b + 3);
            c = Mul(y, y);
            a = Add(a, c);
            StoreNumber(a, b + 3);
            a = RecallNumber(b + 4);
            c = Mul(x, y);
            a = Add(a, c);
            StoreNumber(a, b + 4);
            a = RecallNumber(b + 5);
            a = Add(a, S_ONE);
            StoreNumber(a, b + 5);
            StoreNumber(a, R_X);
        }
    }
}

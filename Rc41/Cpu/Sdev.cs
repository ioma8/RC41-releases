using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Sdev()
        {
            int b;
            int ofs;
            Number a;
            string tmp;
            double x, x2, meanx, n;
            b = ram[REG_C + 2] << 4;
            b |= ((ram[REG_C + 1] >> 4) & 0xf);
            ofs = ram[REG_C + 6] << 4;
            ofs |= ((ram[REG_C + 5] >> 4) & 0xf);
            b += ofs;
            a = RecallNumber(R_X);
            StoreNumber(a, R_L);

            a = RecallNumber(b + 0);
            tmp = NtoA(a);
            x = Convert.ToDouble(tmp);
            a = RecallNumber(b + 1);
            tmp = NtoA(a);
            x2 = Convert.ToDouble(tmp);
            a = RecallNumber(b + 5);
            tmp = NtoA(a);
            n = Convert.ToDouble(tmp);
            if (n == 1)
            {
                Message("DATA ERROR");
                Error();
                return;
            }
            meanx = x / n;
            x = Math.Sqrt((x2 - (x * meanx)) / (n - 1));
            tmp = $"{x:e12}";
            a = AtoN(tmp);
            StoreNumber(a, R_X);
            a = RecallNumber(b + 2);
            tmp = NtoA(a);
            x = Convert.ToDouble(tmp);
            a = RecallNumber(b + 3);
            tmp = NtoA(a);
            x2 = Convert.ToDouble(tmp);
            if (n == 1)
            {
                Message("DATA ERROR");
                Error();
                return;
            }
            meanx = x / n;
            x = Math.Sqrt((x2 - (x * meanx)) / (n - 1));
            tmp = $"{x:e12}";
            a = AtoN(tmp);
            StoreNumber(a, R_Y);
        }
    }
}

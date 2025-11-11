using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        int Dse(byte post)
        {
            int i;
            int p;
            int e;
            int count;
            int final;
            int inc;
            Number x;
            Number y;
            string tmp;
            x = Rcl(post);
            e = x.exponent[0] * 10 + x.exponent[1];
            if (x.esign == 9) e = -e;
            count = 0;
            final = 0;
            inc = 0;
            p = 0;
            while (e >= 0)
            {
                count *= 10;
                if (p < 10) count += x.mantissa[p++];
                e--;
            }
            for (i = 0; i < 3; i++)
            {
                final = (final * 10);
                if (p < 10) final += x.mantissa[p++];
            }
            for (i = 0; i < 2; i++)
            {
                inc = (inc * 10);
                if (p < 10) inc += x.mantissa[p++];
            }
            if (inc == 0) inc = 1;
            if (x.sign == 9) count = -count;
            count -= inc;
            tmp = $"{count:d}.{final:d3}{inc:d2}";
            y = AtoN(tmp);
            if (count < 0) y.sign = 9;
            Sto(y, post);
            if (count <= final) return -1;
            return 0;
        }
    }
}

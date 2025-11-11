using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Sin(Number a)
        {
            int i;
            bool flag;
            DNumber x;
            DNumber fact;
            DNumber one;
            DNumber sum;
            DNumber sqr;
            DNumber num;
            DNumber den;
            DNumber term;
            DNumber last;
            bool addsub;
            x = NumberToDNumber(a);
            one = D_ONE;
            sum = x;
            den = one;
            fact = D_TWO;
            num = x;
            sqr = D_Mul(x, x);
            addsub = true;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, sqr);
                den = D_Mul(den, fact);
                fact = D_Add(fact, one);
                den = D_Mul(den, fact);
                fact = D_Add(fact, one);
                term = D_Div(num, den);
                term.sign = (byte)((addsub) ? 9 : 0);
                addsub = (addsub) ? false : true;
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 12; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }
    }
}

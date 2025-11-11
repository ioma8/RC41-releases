using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Ex(Number a)
        {
            int i;
            bool flag;
            DNumber one;
            DNumber x;
            DNumber sum;
            DNumber num;
            DNumber den;
            DNumber term;
            DNumber last;
            DNumber fact;
            x = NumberToDNumber(a);
            one = D_ONE;
            sum = D_Add(x, one);
            num = x;
            den = one;
            fact = one;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, x);
                fact = D_Add(fact, one);
                den = D_Mul(den, fact);
                term = D_Div(num, den);
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 15; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Ln1PlusX(Number a)
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
            bool addsub;
            x = NumberToDNumber(a);
            one = new DNumber();
            one.sign = 0;
            one.esign = 0;
            one.mantissa[0] = 1;
            for (i = 1; i < 20; i++) one.mantissa[i] = 0;
            for (i = 0; i < 3; i++) one.exponent[i] = 0;
            sum = x;
            num = x;
            den = one;
            addsub = true;
            flag = true;
            while (flag)
            {
                last = sum;
                num = D_Mul(num, x);
                den = D_Add(den, one);
                term = D_Div(num, den);
                if (addsub)
                {
                    sum = D_Sub(sum, term);
                    addsub = false;
                }
                else
                {
                    sum = D_Add(sum, term);
                    addsub = true;
                }
                flag = false;
                for (i = 0; i < 15; i++)
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
            }
            return DNumberToNumber(sum);
        }
    }
}

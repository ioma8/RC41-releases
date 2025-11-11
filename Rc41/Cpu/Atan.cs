using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Atan(Number a)
        {
            int i;
            bool flag;
            bool addsub;
            DNumber x;
            DNumber xsqr;
            DNumber sum;
            DNumber den;
            DNumber last;
            DNumber term;
            x = NumberToDNumber(a);
            xsqr = D_Mul(x, x);
            addsub = true;
            if (x.esign != 0)
            {
                den = D_TWO;
                den = D_Add(den, D_ONE);
                sum = x;
                flag = true;
                while (flag)
                {
                    last = sum;
                    x = D_Mul(x, xsqr);
                    term = D_Div(x, den);
                    if (addsub) sum = D_Sub(sum, term);
                    else sum = D_Add(sum, term);
                    addsub = (addsub) ? false : true;
                    den = D_Add(den, D_TWO);
                    flag = false;
                    for (i = 0; i < 12; i++)
                        if (last.mantissa[i] != sum.mantissa[i]) flag = true;
                }
            }
            else
            {
                x.sign = 0;
                sum = D_PI2;
                den = D_ONE;
                flag = true;
                while (flag)
                {
                    last = sum;
                    term = D_Mul(den, x);
                    term = D_Div(D_ONE, term);
                    if (addsub) sum = D_Sub(sum, term);
                    else sum = D_Add(sum, term);
                    addsub = (addsub) ? false : true;
                    den = D_Add(den, D_TWO);
                    x = D_Mul(x, xsqr);
                    flag = false;
                    for (i = 0; i < 12; i++)
                        if (last.mantissa[i] != sum.mantissa[i]) flag = true;
                }
                sum.sign = a.sign;
            }
            return DNumberToNumber(sum);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Ln(Number a)
        {
            int i;
            bool flag;
            DNumber one;
            DNumber two;
            DNumber n;
            DNumber x;
            DNumber sum;
            DNumber num;
            DNumber term;
            DNumber b;
            DNumber last;
            if (IsZero(a))
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            if (a.sign != 0)
            {
                Message("DATA ERROR");
                Error();
                return a;
            }
            x = NumberToDNumber(a);
            one = D_ONE;
            two = D_TWO;
            sum = D_Div(D_Sub(x, one), D_Add(x, one));
            n = D_Mul(sum, sum);
            b = sum;
            num = one;
            flag = true;
            while (flag)
            {
                last = sum;
                b = D_Mul(b, n);
                num = D_Add(num, two);
                term = D_Div(one, num);
                term = D_Mul(term, b);
                sum = D_Add(sum, term);
                flag = false;
                for (i = 0; i < 15; i++)
                {
                    if (last.mantissa[i] != sum.mantissa[i]) flag = true;
                }
            }
            sum = D_Add(sum, sum);
            return DNumberToNumber(sum);
        }
    }
}

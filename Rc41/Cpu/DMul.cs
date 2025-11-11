using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        DNumber D_Mul(DNumber a, DNumber b)
        {
            int i;
            int t;
            int j;
            int c;
            int ea, eb;
            byte[] temp = new byte[41];
            int p;
            DNumber result;
            result = new DNumber();
            p = 9;
            ea = (a.exponent[0] * 100) + (a.exponent[1] * 10) + a.exponent[2];
            eb = (b.exponent[0] * 100) + (b.exponent[1] * 10) + b.exponent[2];
            if (a.esign != 0) ea = -ea;
            if (b.esign != 0) eb = -eb;
            result.sign = (byte)((a.sign == b.sign) ? 0 : 9);
            ea += eb;
            for (i = 0; i < 40; i++) temp[i] = 0;
            for (p = 19; p >= 0; p--)
            {
                for (i = 0; i < b.mantissa[p]; i++)
                {
                    c = 0;
                    for (j = 19; j >= 0; j--)
                    {
                        temp[p + j + 2] += (byte)((a.mantissa[j] + c));
                        if (temp[p + j + 2] >= 10)
                        {
                            temp[p + j + 2] -= 10;
                            c = 1;
                        }
                        else c = 0;
                    }
                    if (c != 0)
                    {
                        temp[p + 1]++;
                    }
                }
            }
            p = 1;
            ea++;
            t = 0;
            for (i = 0; i < 19; i++) t += temp[i];
            if (t == 0) return D_ZERO;
            while (temp[p] == 0)
            {
                p++;
                ea--;
            }
            for (i = 0; i < 19; i++)
                if (p + i < 19) result.mantissa[i] = temp[p + i];
                else result.mantissa[i] = 0;
            result.esign = (byte)((ea < 0) ? 9 : 0);
            if (ea < 0) ea = -ea;
            result.exponent[0] = (byte)((ea / 100));
            ea = ea % 100;
            result.exponent[1] = (byte)(ea / 10);
            result.exponent[2] = (byte)(ea % 10);
            return result;
        }
    }
}

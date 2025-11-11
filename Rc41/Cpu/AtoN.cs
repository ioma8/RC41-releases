using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public Number AtoN(string number)
        {
            int i;
            int p;
            int e;
            int dp;
            int x;
            int sx;
            int ps;
            Number ret = new Number();
            ret.sign = 0;
            ret.esign = 0;
            ps = 0;
            for (i = 0; i < 10; i++) ret.mantissa[i] = 0;
            for (i = 0; i < 2; i++) ret.exponent[i] = 0;
            while (number[ps] == ' ') ps++;
            if (number[ps] == '+')
            {
                ret.sign = 0;
                ps++;
            }
            else if (number[ps] == '-')
            {
                ret.sign = 9;
                ps++;
            }
            while (ps < number.Length && number[ps] == '0') ps++;
            if (ps < number.Length && number[ps] == '.')
            {
                p = 0;
                e = -1;
                ps++;
                while (ps < number.Length && number[ps] == '0')
                {
                    e--;
                    ps++;
                }
                while (ps < number.Length && number[ps] >= '0' && number[ps] <= '9')
                {
                    if (p < 10)
                    {
                        ret.mantissa[p++] = (byte)(number[ps] - '0');
                    }
                    ps++;
                }
            }
            else
            {
                dp = 0;
                e = -1;
                p = 0;
                while (ps < number.Length && ((number[ps] >= '0' && number[ps] <= '9') || number[ps] == '.'))
                {
                    if (number[ps] != '.') if (p < 10) ret.mantissa[p++] = (byte)(number[ps] - '0');
                    if (number[ps] == '.') dp = 1;
                    if (dp == 0) e++;
                    ps++;
                }
            }
            if (ps < number.Length && (number[ps] == 'E' || number[ps] == 'e'))
            {
                ps++;
                sx = 1;
                if (number[ps] == '+')
                {
                    sx = 1;
                    ps++;
                }
                if (number[ps] == '-')
                {
                    sx = -1;
                    ps++;
                }
                x = 0;
                while (ps < number.Length && number[ps] >= '0' && number[ps] <= '9')
                {
                    x = (x * 10) + (number[ps] - '0');
                    ps++;
                }
                x *= sx;
                e += x;
            }
            if (e >= 0) ret.esign = 0; else ret.esign = 9;
            if (e < 0) e = -e;
            ret.exponent[0] = (byte)(e / 10);
            ret.exponent[1] = (byte)(e % 10);
            return ret;
        }
    }
}

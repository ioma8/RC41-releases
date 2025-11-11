using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        Number Hms(Number a)
        {
            Number b;
            string tmp;
            int m;
            int s;
            int h;
            int f;
            double y;
            string n;
            n = NtoA(a);
            y = Convert.ToDouble(n);
            h = (int)y;
            y -= h;
            y *= 3600;
            m = (int)(y / 60.0);
            y -= m * 60;
            s = (int)y;
            y -= (int)y;
            y *= 100;
            f = (int)y;
            tmp = $"{h:d2}.{m:d2}{s:d2}{f:d}";
            b = AtoN(tmp);
            return b;
        }
    }
}

using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void anum()
        {
            int p;
            bool flag;
            bool gotDot;
            char dot;
            char sep;
            string alpha = cpu.GetAlpha();
            string num;
            Number x;
            if (cpu.FlagSet(28))
            {
                dot = '.';
                sep = (cpu.FlagSet(29)) ? ',' : (char)0;
            }
            else
            {
                dot = ',';
                sep = (cpu.FlagSet(29)) ? '.' : (char)0;
            }
            gotDot = false;
            p = 0;
            flag = true;

            while (flag)
            {
                if (p >= alpha.Length)
                {
                    return;
                }
                else
                {
                    if (alpha[p] >= '0' && alpha[p] <= '9') flag = false;
                    else if (alpha[p] == '-' && p < alpha.Length - 1 && alpha[p + 1] >= '0' && alpha[p + 1] <= '9') flag = false;
                    else if (alpha[p] == dot && p < alpha.Length - 1 && alpha[p + 1] >= '0' && alpha[p + 1] <= '9') flag = false;
                    else p++;
                }
            }
            num = "";
            if (alpha[p] == '-') { num = "-"; p++; }
            while (p < alpha.Length &&
                ((alpha[p] >= '0' && alpha[p] <= '9') ||
                (alpha[p] == dot && gotDot == false) ||
                alpha[p] == sep))
            {
                if (alpha[p] == dot) gotDot = true;
                if (alpha[p] == dot) num += '.';
                else if (alpha[p] >= '0' && alpha[p] <= '9')
                    num += alpha[p];
                p++;
            }
            x = cpu.AtoN(num);
            cpu.StoreNumber(x, Cpu.R_X);
        }
    }
}

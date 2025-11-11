using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void d_tens(int[] n)
        {
            int i;
            int c;
            for (i = 0; i < 20; i++) n[i] = 9 - n[i];
            c = 1;
            for (i = 19; i >= 0; i--)
            {
                n[i] += c;
                if (n[i] > 9)
                {
                    n[i] -= 10;
                    c = 1;
                }
                else c = 0;
            }
        }
    }
}

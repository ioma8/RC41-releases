using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void SetAlpha(string s)
        {
            int i;
            int p;
            for (i = REG_M; i < REG_P + 3; i++)
                ram[i] = 0;
            p = REG_M;
            i = s.Length - 1;
            while (i >= 0)
            {
                ram[p++] = (byte)s[i--];
            }
        }
    }
}

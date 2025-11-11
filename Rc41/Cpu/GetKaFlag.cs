using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public bool GetKaFlag(byte flag)
        {
            int i;
            int f;
            int b;
            int o;
            i = 0;
            while (keys[i].keycode != 0xff && keys[i].keycode != flag && keys[i].skeycode != flag) i++;
            if (keys[i].keycode == 0xff) return false;
            f = keys[i].bit - 1;
            b = 6 - (f >> 3);
            o = 7 - (f & 0x07);
            i = flag & 0x0f;
            if (i >= 1 && i <= 8)
            {                 /* unshifted */
                if ((ram[REG_R + b] & (1 << o)) != 0) return true;
            }
            else
            {                                  /* shifted */
                if ((ram[REG_E + b] & (1 << o)) != 0) return true;
            }
            return false;
        }
    }
}

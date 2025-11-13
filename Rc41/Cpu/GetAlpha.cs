using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public string GetAlpha()
        {
            int m;
            string buffer;
            m = REG_P + 2;
            while (m >= REG_M && ram[m] == 0x00) m--;
            buffer = "";
            while (m >= REG_M)
            {
                if (ram[m] != 0x00) buffer += ((char)ram[m]).ToString();
                m--;
            }
            return buffer;
        }
    }
}

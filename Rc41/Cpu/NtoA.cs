using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        string NtoA(Number a)
        {
            int p;
            int i;
            string buffer;
            buffer = "";
            p = 0;
            if (a.sign != 0) buffer += "-";
            buffer += ((char)(a.mantissa[0] + '0')).ToString();
            buffer += ".";
            for (i = 1; i < 10; i++)
            {
                buffer += ((char)(a.mantissa[i] + '0')).ToString();
            }
            buffer += "E";
            buffer += (a.esign != 0) ? "-" : "+";
            buffer += ((char)(a.exponent[0] + '0')).ToString();
            buffer += ((char)(a.exponent[1] + '0')).ToString();
            return buffer;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void View(byte post)
        {
            string tmp;
            Number a;
            a = Rcl(post);
            if (errFlag == false)
            {
                tmp = Format(a);
                Message(tmp);
                SetFlag(50);
                if (printer.power)
                {
                    if (!running || FlagSet(21)) printer.Print(tmp, 'R');
                }
            }
        }
    }
}

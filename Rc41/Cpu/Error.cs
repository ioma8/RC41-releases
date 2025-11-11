using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Error()
        {
            if (FlagSet(25))
            {
                ClearFlag(25);
                ClearFlag(50);
                window.Display(Display(), false);
                return;
            }
            errFlag = true;
            window.Display(Display(), false);
            running = false;
        }
    }
}

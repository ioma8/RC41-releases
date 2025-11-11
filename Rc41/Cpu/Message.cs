using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Message(string msg)
        {
            display = msg;
            SetFlag(50);
        }
    }
}

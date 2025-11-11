using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Rc41.T_Cpu;

namespace Rc41.T_CardReader
{
    public partial class CardReader
    {
        Form1 window;
        Cpu cpu;

        public CardReader(Cpu c, Form1 w)
        {
            window = w;
            cpu = c;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void SwShift()
        {
            swShift = !swShift;
            window.Shift(swShift);
        }
    }
}

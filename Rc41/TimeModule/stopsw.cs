using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void stopsw()
        {
            if (swRunning != 'Y') return;
            swAccumulated += (DateTime.Now.ToOADate() - swStart);
            swRunning = 'N';
        }
    }
}

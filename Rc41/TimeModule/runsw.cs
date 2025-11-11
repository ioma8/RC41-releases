using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void runsw()
        {
            if (swRunning == 'Y') return;
            swStart = DateTime.Now.ToOADate();
            swRunning = 'Y';
        }
    }
}

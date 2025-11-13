using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rc41.T_Cpu;
using Rc41.Core.Interfaces;

namespace Rc41.T_CardReader
{
    public partial class CardReader
    {
        ICalculatorUI ui;
        Cpu cpu;

        public CardReader(Cpu c, ICalculatorUI calculatorUI)
        {
            ui = calculatorUI;
            cpu = c;
        }

    }
}

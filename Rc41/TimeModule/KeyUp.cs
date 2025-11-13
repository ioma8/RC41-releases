using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void KeyUp(string tag)
        {
            int b;
            int numkey;
            if (enteringNum && tag.Equals("44"))              // BS
            {
                if (numEntry > 0)
                {
                    number = 0;
                    numEntry--;
                }
                return;
            }

            numkey = -1;
            if (tag.Equals("17")) numkey = 1;
            if (tag.Equals("27")) numkey = 2;
            if (tag.Equals("37")) numkey = 3;
            if (tag.Equals("16")) numkey = 4;
            if (tag.Equals("26")) numkey = 5;
            if (tag.Equals("36")) numkey = 6;
            if (tag.Equals("15")) numkey = 7;
            if (tag.Equals("25")) numkey = 8;
            if (tag.Equals("35")) numkey = 9;
            if (tag.Equals("18")) numkey = 0;
            if (numkey >= 0)
            {
                if (!enteringNum)
                {
                    number = 0;
                    numEntry = 0;
                    enteringNum = true;
                }
                number = (number * 10) + numkey;
                numEntry++;
                if (numEntry == 2)
                {
                    enteringNum = false;
                    if (swMode == 'S') nextSplit = number;
                    else nextRcl = number;
                }
                return;
            }
            if (enteringNum) return;
            if (tag.Equals("04"))                   // ENTER
            {
                swHold = false;
                nextSplit++;
            }
            if (tag.Equals("24"))                   // CHS
            {
                deltaMode = !deltaMode;
            }
            if (tag.Equals("33"))                   // RCL
            {
                swMode = (swMode == 'S') ? 'R' : 'S';
            }
            if (tag.Equals("34"))                   // EEX
            {
                suppressReg = !suppressReg;
            }
            if (tag.Equals("38"))                   // R/S
            {
                if (swRunning == 'Y')
                {
                    stopsw();
                }
                else
                {
                    runsw();
                }
            }
            if (tag.Equals("43"))                    // SST
            {
                if (swShift)
                {
                    if (swMode == 'S')
                    {
                        if (nextSplit > 0) nextSplit--;
                    }
                    else
                    {
                        if (nextRcl > 0) nextRcl--;
                    }
                    swShift = false;
                    ui.Shift(false);
                }
                else
                {
                    if (swMode == 'S') nextSplit++;
                    else
                    {
                        nextRcl++;
                        b = cpu.ram[Cpu.REG_C + 2] << 4;
                        b |= ((cpu.ram[Cpu.REG_C + 1] >> 4) & 0xf);
                        b += nextRcl;
                        if (b > 0x1ff)
                        {
                            cpu.Message("NONEXISTENT");
                            EndSw();
                            return;
                        }
                    }
                }
                SwDisplay();
                ui.Display(cpu.Display(), true);
                return;
            }
            if (tag.Equals("44"))                     // BS
            {
                if (swShift)
                {
                    cpu.ClearFlag(50);
                    ui.Shift(false);
                    EndSw();
                    return;
                }
                if (swRunning != 'Y')
                {
                    swAccumulated = 0;
                    SwDisplay();
                    ui.Display(cpu.Display(), false);
                }
            }
        }
    }
}

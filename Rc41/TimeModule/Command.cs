using Rc41.T_Cpu;
using Rc41;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        public void Command(byte function)
        {

            switch (function)
            {
                case 1:                                       // ADATE
                    adate();
                    break;
                case 4:                                       // ATIME
                    atime();
                    break;
                case 5:                                       // ATIME24
                    atime24();
                    break;
                case 6:                                       // CLK12
                    clkmode = 12;
                    break;
                case 7:                                       // CLK24
                    clkmode = 24;
                    break;
                case 8:                                       // CLKT
                    clktd = 'T';
                    break;
                case 9:                                       // CLKTD
                    clktd = 'D';
                    break;
                case 10:                                      // CLOCK
                    clock();
                    break;
                case 11:                                      // CORRECT
                    setime();
                    af = 0;
                    break;
                case 12:                                      // DATE
                    date();
                    break;
                case 13:                                      // DATE+
                    dateplus();
                    break;
                case 14:                                      // DDAYS
                    ddays();
                    break;
                case 15:                                      // DMY
                    cpu.SetFlag(31);
                    break;
                case 16:                                      // DOW
                    dow();
                    break;
                case 17:                                      // MDY
                    cpu.ClearFlag(31);
                    break;
                case 18:                                      // RCLAF
                    rclaf();
                    break;
                case 19:                                      // RCLSW
                    rclsw();
                    break;
                case 20:                                      // RUNSW
                    runsw();
                    break;
                case 21:                                      // SETAF
                    setaf();
                    break;
                case 22:                                      // SETDATE
                    setDate();
                    break;
                case 23:                                      // SETIME
                    setime();
                    break;
                case 24:                                      // SETSW
                    setsw();
                    break;
                case 25:                                      // STOPSW
                    stopsw();
                    break;
                case 26:                                      // SW
                    sw();
                    break;
                case 27:                                      // T+X
                    tplusx();
                    break;
                case 28:                                      // TIME
                    time();
                    break;

            }
        }
    }
}

using Rc41.T_Cpu;
using Rc41.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TimeModule
{
    public partial class TimeModule
    {
        Cpu cpu;
        ICalculatorUI ui;
        double offset;
        int clkmode;
        char clktd;
        double swAccumulated;
        double swStart;
        char swRunning;
        char swMode;
        double af;
        int nextSplit;
        int nextRcl;
        public bool swHold;
        double swHoldValue;
        bool swShift;
        bool suppressReg;
        bool deltaMode;
        bool enteringNum;
        int numEntry;
        int number;

        public TimeModule(Cpu c, ICalculatorUI calculatorUI)
        {
            cpu = c;
            this.ui = calculatorUI;
            offset = 0;
            clkmode = 12;
            clktd = 'T';
            swRunning = 'N';
            swStart = 0;
            swAccumulated = 0;
            af = 0;
            nextSplit = 0;
            nextRcl = 0;
            swMode = 'S';
            swHold = false;
            swShift = false;
            suppressReg = false;
            deltaMode = false;
            enteringNum = false;
            numEntry = -1;
        }

        public void Load(BinaryReader file)
        {
            offset = file.ReadDouble();
            clkmode = file.ReadInt32();
            clktd = file.ReadChar();
            swRunning = file.ReadChar();
            swAccumulated = file.ReadDouble();
            swStart = file.ReadDouble();
            af = file.ReadDouble();
        }


        public void Save(BinaryWriter file)
        {
            file.Write(offset);
            file.Write(clkmode);
            file.Write(clktd);
            file.Write(swRunning);
            file.Write(swAccumulated);
            file.Write(swStart);
            file.Write(af);
        }

    }
}

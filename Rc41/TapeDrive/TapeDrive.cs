using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rc41.T_Cpu;
using Rc41.Core.Interfaces;

namespace Rc41.T_TapeDrive;

public partial class TapeDrive
{
    ICalculatorUI ui;
    Cpu cpu;
    Stream? tapefile;
    byte[] sector = new byte[256];
    public List<string> dir;
    public int dirPos;
    int sectorNumber;
    bool written;
    int file_rec;
    int file_regs;
    int file_reg;
    int file_pos;
    int file_flags;
    int sfree;
    int dfree;



    public TapeDrive(Cpu c, ICalculatorUI calculatorUI)
    {
        ui = calculatorUI;
        cpu = c;
        dir = new List<string>();
        sfree = 0;
        dfree = 0;
    }

}


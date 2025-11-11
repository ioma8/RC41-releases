using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public void Prp(string name, int lines)
        {
            int lineNumber;
            int address;
            int end;
            string line;
            string tline;
            string oline;
            int i;
            if (power == false) return;
            if (name.Length == 0)
            {
                address = (cpu.ram[Cpu.REG_B + 1] & 0xff) << 8 | cpu.ram[Cpu.REG_B + 0];
                address = cpu.FromPtr(address);

            }
            else
            {
                address = cpu.FindGlobal(name);
                if (address == 0) return;
            }
            if (lines < 0)
            {
                address = cpu.FindStart(address);
                lineNumber = 1;
            }
            else
            {
                lineNumber = cpu.ram[Cpu.REG_E] | ((cpu.ram[Cpu.REG_E+1] & 0x0f) << 8);
                address -= cpu.isize(address);
            }
            end = cpu.FindEnd(address);
            Print("", 'L');
            tline = "";
            while (cpu.ram[address] == 0x00) address--;
            while (address >= end)
            {
                line = cpu.ProgramList(lineNumber, address);
                oline = line;
                i = 0;
                while (line[i] >= '0' && line[i] <= '9') i++;
                if (i < 3) { line = " " + line; i++; }
                if (line[3] == '\"') line = line.Substring(0, 3) + " " + line.Substring(3);
                if (line[i + 1] == 'L' && line[i + 2] == 'B' && line[i + 3] == 'L')
                {
                    line = line.Substring(0, i) + "\x0a" + line.Substring(i + 1);
                }
                if (line.IndexOf('\"') > 0)
                {
                    line += "\"";
                }

                if (printerMode == 'M') Print(line, 'L');
                else if (printerMode == 'N')
                {
                    while (line.Length < 12) line += " ";
                    Print(line, 'R');
                }
                else if (printerMode == 'T')
                {
                    if (line.IndexOf("\x0aLBL") > 0)
                    {
                        if (tline.Length > 0)
                        {
                            Print(tline, 'L');
                            tline = "";
                        }
                        Print("", 'L');
                        Print(line, 'L');
                    }
                    else
                    {
                        if (line[0] == ' ') line = line.Substring(1);
                        while (line[0] >= '0' && line[0] <= '9') line = line.Substring(1);
                        if (line[0] == ' ') line = line.Substring(1);
                        if (tline.Length == 0) tline = line;
                        else if (tline.Length + line.Length + 2 <= 24) tline += "  " + line;
                        else
                        {
                            Print(tline, 'L');
                            tline = line;
                        }
                    }
                }
                lineNumber++;
                address -= cpu.isize(address);
                while (cpu.ram[address] == 0x00) address--;
                if (lines > 0)
                {
                    lines--;
                    if (lines == 0) return;
                }
            }
            if (printerMode == 'T' && tline.Length > 0) Print(tline, 'L');
        }
    }
}

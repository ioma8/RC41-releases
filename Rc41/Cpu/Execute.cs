using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Execute()
        {
            int i;
            int addr;
            int n;
            string buffer;


            if (ram[REG_R + 1] == 0x00 && ram[REG_R + 0] == CMD_PACK)
            {
                Pack();
                ui.Display(Display(), true);
                return;
            }

            if (ram[REG_R + 1] == 0x30)                    //  STO
            {
                if (ram[REG_R + 0] > 15)
                {
                    ram[REG_R + 1] = 0x91;
                }
                else ram[REG_R + 1] |= ram[REG_R + 0];
            }

            if (ram[REG_R + 1] == 0x20)                    //  RCL
            {
                if (ram[REG_R + 0] > 15)
                {
                    ram[REG_R + 1] = 0x90;
                }
                else ram[REG_R + 1] |= ram[REG_R + 0];
            }

            if (FlagSet(F_PRGM))
            {
                if (ram[REG_R + 1] == 0x54 && FlagSet(F_SYS) && !FlagSet(F_ALPHA)) ram[REG_R + 1] = 0x1c;
                if (ram[REG_R + 1] == 0x01)                    //  LBL
                {
                    if (ram[REG_R + 0] > 14)
                    {
                        ram[REG_R + 1] = 0xcf;
                    }
                    else ram[REG_R + 1] = (byte)((ram[REG_R + 0] + 1));
                }
                if (ram[REG_R + 1] == 0xb1)                    //  GTO
                {
                    if (ram[REG_R + 0] > 14)
                    {
                        n = ram[REG_R + 0];
                        ram[REG_R + 1] = 0xd0;
                        ram[REG_R + 0] = 0;
                        ram[REG_E + 2] &= 0xf0;
                        ram[REG_E + 2] |= (byte)(((n & 0x70) >> 4));
                        ram[REG_E + 1] &= 0x0f;
                        ram[REG_E + 1] |= (byte)(((n & 0x0f) << 4));
                    }
                    else
                    {
                        ram[REG_R + 1] += ram[REG_R + 0];
                        ram[REG_R + 0] = 0x00;
                    }
                }
                if (ram[REG_R + 1] == 0xe0)                    //  XEQ
                {
                    n = ram[REG_R + 0];
                    ram[REG_R + 1] = 0xe0;
                    ram[REG_R + 0] = 0;
                    ram[REG_E + 2] &= 0xf0;
                    ram[REG_E + 2] |= (byte)(((n & 0x70) >> 4));
                    ram[REG_E + 1] &= 0x0f;
                    ram[REG_E + 1] |= (byte)(((n & 0x0f) << 4));
                }
                if (ram[REG_R + 1] == 0xc1)                    //  LBL
                {
                    n = ram[REG_R + 0];
                    ram[REG_R + 1] = 0xc1;
                    ram[REG_R + 0] = 0;
                    ram[REG_E + 2] &= 0xf0;
                    ram[REG_E + 2] |= (byte)(((n & 0x70) >> 4));
                    ram[REG_E + 1] &= 0x0f;
                    ram[REG_E + 1] |= (byte)(((n & 0x0f) << 4));
                }
                if (ram[REG_R + 1] >= 0x10 && ram[REG_R + 1] <= 0x1c)
                {
                    AddNumber((char)(ram[REG_R + 1] - 0x10));
                    ui.Display(Display(), true);
                    return;
                }
                ProgramStep("");
                ui.Display(Display(), true);
                return;
            }

            if (ui.PrinterMode() != 'M' && ui.PrinterOn())
            {
                if (ram[REG_R + 1] < 0x10 || ram[REG_R + 1] > 0x1c)
                {
                    if (FlagSet(F_SYS) && !FlagSet(F_ALPHA)) EndNumber();
                    buffer = Postfix(ram[REG_R + 1], ram[REG_R + 0]);
                    printer.Print(buffer, 'R');
                }
            }


            if (ram[REG_R + 1] == 0x1d)
            {
                buffer = "";
                i = REG_Q;
                while (i <= REG_Q + 6 && ram[i] != 0x00) buffer += (char)ram[i++];
                addr = FindGlobal(buffer);
                if (addr != 0)
                {
                    addr = ToPtr(addr);
                    ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                    ram[REG_B + 0] = (byte)(addr & 0xff);
                    ram[REG_E + 1] |= 0x0f;
                    ram[REG_E + 0] = 0xff;
                    ram[REG_E + 0] = 0xff;
                    ram[REG_E + 1] |= 0x0f;
                    ui.Display(Display(), true);
                    return;
                }
            }

            if (ram[REG_R + 1] == 0x1e)
            {
                buffer = "";
                i = REG_Q;
                while (i <= REG_Q + 6 && ram[i] != 0x00) buffer += (char)ram[i++];

                addr = FindGlobal(buffer);
                if (addr != 0)
                {

                    for (i = 0; i < 7; i++) ram[REG_B + i] = 0x00;
                    for (i = 0; i < 7; i++) ram[REG_A + i] = 0x00;
                    addr = ToPtr(addr);
                    ram[REG_B + 1] = (byte)((addr >> 8) & 0xff);
                    ram[REG_B + 0] = (byte)(addr & 0xff);
                    ram[REG_E + 1] |= 0x0f;
                    ram[REG_E + 0] = 0xff;
                    ClearFlag(F_SST);
                    running = true;
                    ram[REG_E + 0] = 0xff;
                    ram[REG_E + 1] |= 0x0f;
                    ui.RunTimerEnabled(true);
                    goose = "\x81           ";
                    ui.Display(goose, true);
                    return;
                }

            }

            if (ram[REG_R + 1] == 0xc1)
            {
                ui.Display(Display(), true);
                Annunciators();
                return;
            }
            if (ram[REG_R + 1] == 0xb1)
            {
                ram[REG_E + 0] = 0xff;
                ram[REG_E + 1] |= 0x0f;
                if (ram[REG_R + 0] > 14)
                {
                    ram[REG_R + 1] = 0xd0;
                }
                else
                {
                    ram[REG_R + 1] += ram[REG_R + 0];
                    ram[REG_R + 0] = 0x00;
                }
            }

            if (ram[REG_R + 1] == 0xe0)
            {
                ram[REG_E + 0] = 0xff;
                ram[REG_E + 1] |= 0x0f;
            }
            Exec(71);
            ui.Display(Display(), true);
            Annunciators();
        }
    }
}

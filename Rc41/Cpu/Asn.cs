using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public void Asn(string command, byte keycode)
        {
            int i;
            bool flag;
            int entry;
            int cat;
            int addr;
            int b1;
            int b2;
            addr = 0;
            b1 = 0;
            b2 = 0;
            if (command.Equals(""))
            {
                cat = 4;
            }
            else
            {
                entry = 0;
                flag = true;
                cat = 0;
                while (flag)
                {
                    if (command.Equals(catalog[entry].name, StringComparison.OrdinalIgnoreCase)) flag = false;
                    else if (catalog[++entry].flags == 0xff) flag = false;
                }
                if (catalog[entry].flags != 0xff)
                {
                    cat = 1;
                    if ((catalog[entry].post) != 0)
                    {
                        b1 = catalog[entry].cmd;
                        b2 = catalog[entry].post;
                    }
                    else
                    {
                        b1 = 0x04;
                        b2 = catalog[entry].cmd;
                        if (b2 == 0x30) b2 = 0x91;
                        if (b2 == 0x20) b2 = 0x90;
                    }
                }
                if (cat == 0)
                {
                    addr = FindGlobal(command);
                    if (addr != 0)
                    {
                        addr--;
                        cat = 3;
                    }
                }
                if (cat == 0)
                {
                    Message("NONEXISTENT");
                    Error();
                    return;
                }
            }
            UnAsn(keycode, 3);
            if (cat == 4) return;
            if (cat == 3)
            {
                ram[addr - 3] = keycode;
                SetKaFlag(keycode, true);
                return;
            }
            addr = 0x0c0 * 7;
            flag = true;
            while (flag)
            {
                if (ram[addr + 6] != 0xf0) flag = false;
                if (ram[addr + 5] == 0x00) flag = false;
                if (ram[addr + 2] == 0x00) flag = false;
                if (flag) addr += 7;
            }
            if (ram[addr + 6] != 0xf0)
            {
                for (i = 0; i < 6; i++) ram[addr + i] = 0x00;
                ram[addr + 6] = 0xf0;
            }
            if (ram[addr + 2] == 0x00)
            {
                ram[addr + 2] = (byte)b1;
                ram[addr + 1] = (byte)b2;
                ram[addr + 0] = keycode;
            }
            else
            {
                ram[addr + 5] = (byte)b1;
                ram[addr + 4] = (byte)b2;
                ram[addr + 3] = keycode;
            }
            SetKaFlag(keycode, true);
        }
    }
}

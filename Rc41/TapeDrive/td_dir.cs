using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_dir()
        {
            bool flag;
            int s;
            int r;
            int i;
            int p;
            string line;
            dfree = 0;
            sfree = 0;
            dir = new List<string>();
            s = 2;
            if (written) WriteSector(sectorNumber);
            ReadSector(0);
            for (i = 0; i < 256; i++) if (sector[i] == 0x00) sfree++;
            ReadSector(1);
            for (i = 0; i < 256; i++) if (sector[i] == 0x00) sfree++;
            flag = true;
            while (flag)
            {
                ReadSector(s);
                p = 0;
                while (p < 256)
                {
                    if (sector[p] != 0 && sector[p] != 0xff)
                    {
                        line = "";
                        for (i = 0; i < 7; i++)
                            if (sector[p + i + 1] != 0) line += ((char)sector[p + i + 1]).ToString();
                            else line += " ";
                        line += " ";
                        if (sector[p] == 'P') line += "PR";
                        else if (sector[p] == 'D') line += "DA";
                        else if (sector[p] == 'K') line += "KE";
                        else if (sector[p] == 'S') line += "ST";
                        else if (sector[p] == 'A') line += "WA";
                        else if (sector[p] != 0x00)
                        {
                            cpu.Message("NO MEDM");
                            cpu.Error();
                            return;
                        }
                        if (sector[p + 9] != 0)
                        {
                            switch (sector[p + 9])
                            {
                                case 1: line += ",S "; break;
                                case 2: line += ",P "; break;
                                case 3: line += ",PS"; break;
                            }
                        }
                        else line += "   ";
                        line += "   ";
                        r = sector[p + 10] << 8 | sector[p + 11];
                        line += $"{r:d4}";
                        dir.Add(line);
//                        window.ToPrinter(line, 'L');
                    }
                    else if (sector[p] == 0x00) dfree++;
                    else if (sector[p] == 0xff)
                    {
                        flag = false;
                    }
                    p += 32;
                }
                if (p >= 256)
                {
                    p = 0;
                    s++;
                    if (s > 511)
                    {
                        flag = false;
                        cpu.Message("NO MEDM");
                        cpu.Error();
                    }
                    else ReadSector(s);
                }
            }

            window.Print("", 'L');
            window.Print("NAME    TYPE    REGS", 'L');
            dirPos = 0;
            cpu.dirMode = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_readp()
        {
            int i;
            int fp;
            int rec;
            int size;
            int p;
            int end;
            int adr;
            int sadr;
            int nabc;
            byte b;
            string filename;
            filename = cpu.GetAlpha();
            if (filename.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            fp = FindFile(filename);
            if (fp >= 0 && sector[fp] != 'P')
            {
                cpu.Message("FL TYPE ERR");
                cpu.Error();
                return;
            }
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            rec = sector[fp + 14] << 8 | sector[fp + 15];
            size = sector[fp + 12] << 8 | sector[fp + 13];
            end = (cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8 | cpu.ram[Cpu.REG_C + 0];
            end = end * 7 + 2;
            adr = cpu.FindStart(end);
            sadr = adr;
            for (i = end + 1; i <= adr; i++) cpu.ram[i] = 0x00;
            p = 0;
            ReadSector(rec);
            while (size > 0)
            {
                nabc = cpu.ToPtr(adr--);
                cpu.ram[Cpu.REG_B + 1] = (byte)(nabc >> 8 & 0xff);
                cpu.ram[Cpu.REG_B + 0] = (byte)(nabc & 0xff);
                cpu.ProgramByte(sector[p++]);
                if (p == 256)
                {
                    rec++;
                    ReadSector(rec);
                    p = 0;
                }
                size--;
            }
            cpu.ReLink();

            end = (cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8 | cpu.ram[Cpu.REG_C + 0];
            end = end * 7 + 2;
            while (cpu.ram[sadr] == 0x00) sadr--;
            while (cpu.ram[sadr] < 0xc0 || cpu.ram[sadr] > 0xcd || cpu.ram[sadr - 2] >= 0xf0)
            {
                if (cpu.ram[sadr] >= 0xc0 && cpu.ram[sadr] <= 0xcd && cpu.ram[sadr - 2] >= 0xf0)
                {
                    if (cpu.ram[sadr - 3] != 0x00)
                    {
                        b = cpu.ram[sadr - 3];
                        cpu.UnAsn(b, 3);
                        cpu.ram[sadr - 3] = b;
                        cpu.SetKaFlag(b, true);
                    }
                }
                sadr -= cpu.isize(sadr);
            }
        }
    }
}

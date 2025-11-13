using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_CardReader
{
    public partial class CardReader
    {
        public void Rprg(string filename)
        {
            Stream file;
            int address;
            int sadr;
            int nabc;
            int len;
            //  int regs;
            byte b;
            byte[] buffer = new byte[1];
            byte[] card = new byte[7];
            file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            if (file == null)
            {
                cpu.Message("CARD ERR");
                cpu.Error();
                MessageBox.Show($"Could not open card file: {filename}");
                return;
            }
            file.ReadExactly(card, 0, 7);
            len = card[5] << 8 | card[6];
            //    regs = (len + 6) / 7;
            cpu.GtoEnd();
            address = (cpu.ram[Cpu.REG_C + 1] & 0x0f) << 8 | cpu.ram[Cpu.REG_C + 0];
            address *= 7;
            address += 2;
            sadr = address;
            while (len > 0)
            {
                nabc = cpu.ToPtr(address);
                cpu.ram[Cpu.REG_B + 1] = (byte)(nabc >> 8 & 0xff);
                cpu.ram[Cpu.REG_B + 0] = (byte)(nabc & 0xff);
                file.ReadExactly(buffer);
                cpu.ProgramByte(buffer[0]);
                address--;
                len--;
            }
            file.Close();
            cpu.ReLink();

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

            cpu.Pack();

        }
    }
}

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
        public void Rsts(string filename)
        {
            int i;
            int j;
            Stream file;
            int adr;
            int regs;
            int old_r00;
            int new_r00;
            byte[] card = new byte[5];
            byte[] buffer = new byte[7];
            file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            if (file == null)
            {
                MessageBox.Show($"Could not open card file: {filename}");
                return;
            }
            file.ReadExactly(card, 0, 5);
            regs = card[3] * 256 + card[4];
            adr = 0;
            for (i = 0; i < 9; i++)
            {
                file.ReadExactly(buffer, 0, 7);
                for (j = 0; j < 7; j++) cpu.ram[adr + 6 - j] = buffer[j];
                adr += 7;
            }
            file.ReadExactly(buffer, 0, 7);

            cpu.ram[Cpu.REG_C + 6] = buffer[0];
            cpu.ram[Cpu.REG_C + 5] &= 0x0f;
            cpu.ram[Cpu.REG_C + 5] |= (byte)(buffer[1] & 0xf0);
            new_r00 = buffer[4] << 4 | (buffer[5] & 0xf0) >> 4;
            file.ReadExactly(buffer, 0, 7);
            for (i = 0; i < 6; i++) cpu.ram[Cpu.REG_D + 6 - i] = buffer[i];
            cpu.ram[Cpu.REG_D + 1] &= 0x0f;
            cpu.ram[Cpu.REG_D + 1] |= (byte)(buffer[5] & 0xf0);
            adr = 0x0c0 * 7;
            while (cpu.ram[adr + 6] == 0xf0)
            {
                for (i = 0; i <= 6; i++) cpu.ram[adr + i] = 0;
                adr += 6;
            }
            adr = 0x0c0 * 7;
            regs -= 11;
            while (regs > 0)
            {
                file.ReadExactly(buffer, 0, 7);
                for (i = 0; i <= 6; i++) cpu.ram[adr + 6 - i] = buffer[i];
                adr += 7;
                regs--;
            }
            file.Close();
            old_r00 = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
            cpu.Resize(old_r00, new_r00);
            cpu.SetKaFlags();
        }
    }
}

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
        public void Wprg(string filename)
        {
            int i;
            Stream file;
            int address;
            int end;
            int len;
            byte[] card = new byte[7];
            byte[] buffer = new byte[2];
            address = cpu.ram[Cpu.REG_B + 1] << 8 | cpu.ram[Cpu.REG_B + 0];
            address = cpu.FromPtr(address);
            address = cpu.FindStart(address);
            end = cpu.FindEnd(address);
            file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write);

            if (file != null)
            {
                end -= 2;
                len = (address - end) / 7;
                card[0] = (byte)'P';
                card[1] = 0x00;
                card[2] = 0x01;
                i = (len + 15) / 16;
                card[3] = (byte)(i >> 8 & 0xff);
                card[4] = (byte)(i & 0xff);
                len = address - end + 1;
                card[5] = (byte)(len >> 8 & 0xff);
                card[6] = (byte)(len & 0xff);
                file.Write(card, 0, 7);
                while (len > 0)
                {
                    buffer[0] = cpu.ram[address];
                    file.Write(buffer, 0, 1);
                    address--;
                    len--;
                }
                file.Close();
            }
            else
            {
                cpu.Message("CARD ERR");
                cpu.Error();
            }
        }
    }
}

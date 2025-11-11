using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        Cpu cpu;
        Form1 window;
        public byte[] ram;
        public int currentFile;

        public Extended(Cpu c, Form1 window)
        {
            cpu = c;
            this.window = window;
            ram = new byte[4200];
            for (var i = 0; i < 4200; i++) ram[i] = 0;
            ram[599 * 7 + 6] = 0xff;
        }

        public void RamClear()
        {
            for (var i = 0; i < 4200; i++) ram[i] = 0;
            ram[599 * 7 + 6] = 0xff;
            currentFile = 0;
        }

        public int FindEnd()
        {
            int addr;
            addr = 599;
            while (ram[addr * 7 + 6] != 0xff) addr--;
            return addr;
        }

        public int FindFile(string name)
        {
            int i;
            int s;
            int addr;
            bool found;
            if (name.Length > 7) name = name.Substring(0, 7);
            while (name.Length < 7) name += " ";
            addr = 599;
            while (ram[addr * 7 + 6] != 0xff)
            {
                found = true;
                for (i = 0; i < 7; i++) if (ram[addr * 7 + 6 - i] != name[i]) found = false;
                if (found)
                {
                    return addr;
                }
                addr--;
                s = ram[addr + 5] * 256 + ram[addr + 4];
                addr -= s;
            }
            return -1;
        }

        public int RecordCount()
        {
            int i;
            int recs;
            i = (currentFile - 2) * 7 + 6;
            recs = 0;
            while (ram[i] != 0xff)
            {
                recs++;
                i -= (ram[i] + 1);
            }
            return recs;
        }

        public int CurrentRecord()
        {
            int addr;
            int rec;
            addr = currentFile - 1;
            rec = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            addr = (addr * 7) - 1;
            while (ram[addr] != 0xff && rec > 0)
            {
                addr -= (ram[addr] + 1);
                rec--;
            }
            return addr;
        }

        public Number ReadRegister(int addr)
        {
            Number x;
            x = new Number(0);
            x.sign = (byte)((ram[addr * 7 + 6] >> 4) & 0x0f);
            x.mantissa[0] = (byte)((ram[addr * 7 + 6] & 0x0f));
            x.mantissa[1] = (byte)((ram[addr * 7 + 5] >> 4) & 0x0f);
            x.mantissa[2] = (byte)((ram[addr * 7 + 5] & 0x0f));
            x.mantissa[3] = (byte)((ram[addr * 7 + 4] >> 4) & 0x0f);
            x.mantissa[4] = (byte)((ram[addr * 7 + 4] & 0x0f));
            x.mantissa[5] = (byte)((ram[addr * 7 + 3] >> 4) & 0x0f);
            x.mantissa[6] = (byte)((ram[addr * 7 + 3] & 0x0f));
            x.mantissa[7] = (byte)((ram[addr * 7 + 2] >> 4) & 0x0f);
            x.mantissa[8] = (byte)((ram[addr * 7 + 2] & 0x0f));
            x.mantissa[9] = (byte)((ram[addr * 7 + 1] >> 4) & 0x0f);
            x.esign = (byte)((ram[addr * 7 + 1] & 0x0f));
            x.exponent[0] = (byte)((ram[addr * 7 + 0] >> 4) & 0x0f);
            x.exponent[1] = (byte)((ram[addr * 7 + 0] & 0x0f));
            return x;
        }
        public void SaveRegister(int addr, Number x)
        {
            ram[addr * 7 + 6] = (byte)((x.sign << 4) | x.mantissa[0]);
            ram[addr * 7 + 5] = (byte)((x.mantissa[1] << 4) | x.mantissa[2]);
            ram[addr * 7 + 4] = (byte)((x.mantissa[3] << 4) | x.mantissa[4]);
            ram[addr * 7 + 3] = (byte)((x.mantissa[5] << 4) | x.mantissa[6]);
            ram[addr * 7 + 2] = (byte)((x.mantissa[7] << 4) | x.mantissa[8]);
            ram[addr * 7 + 1] = (byte)((x.mantissa[9] << 4) | x.esign);
            ram[addr * 7 + 0] = (byte)((x.exponent[0] << 4) | x.exponent[1]);
        }

        public void Load(BinaryReader file)
        {
            file.Read(ram);
            currentFile = file.ReadInt32();
        }

        public void Save(BinaryWriter file)
        {
            file.Write(ram);
            file.Write(currentFile);
        }
    }
}

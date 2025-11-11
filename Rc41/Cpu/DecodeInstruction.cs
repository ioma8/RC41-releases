using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        string DecodeInstruction(int addr)
        {
            int i;
            byte b1, b2, b3;
            b1 = ram[addr];
            b2 = ram[addr - 1];
            string line = "";
            if (b1 == 0x1e)
            {
                line = "XEQ\"";
                addr--;
                for (i = 0; i < (b2 & 0x0f) - 1; i++)
                {
                    b3 = ram[addr - i - 1];
                    if (b3 == 0x00) b3 = (byte)'_';
                    else if (b3 == 0x7f) b3 = (byte)'|';
                    else if (b3 < ' ' || b3 > 126) b3 = (byte)'#';
                    line += (char)b3;
                }
                return line;
            }
            if (b1 >= 0xf0)
            {
                addr--;
                line = "\"";
                for (i = 0; i < (b1 & 0x0f) - 1; i++)
                {
                    b2 = ram[addr - i];
                    if (b2 == 0x00) b2 = (byte)'_';
                    else if (b2 == 0x7f) b2 = (byte)'|';
                    else if (b2 < ' ' || b2 > 126) b2 = (byte)'#';
                    line += (char)b2;
                }
                return line;
            }
            if (b1 >= 0xc0 && b1 <= 0xcd)
            {
                addr -= 2;
                if (ram[addr] >= 0xf0)
                {
                    line = "LBL\"";
                    b1 = (byte)((ram[addr--] & 0x0f) - 1);
                    addr--;
                    for (i = 0; i < (b1 & 0x0f); i++) line += (char)ram[addr - i];
                    return line;
                }
                line = "END";
                return line;
            }
            if (b1 >= 0x10 && b1 <= 0x1c)
            {
                while (b1 >= 0x10 && b1 <= 0x1c)
                {
                    if (b1 >= 0x10 && b1 <= 0x19) line += (char)(b1 - 0x10 + '0');

                    b1 = ram[--addr];
                }
                return line;
            }
            if (b1 >= 0xa0 && b1 <= 0xa7)
            {
                i = FindCommand(b1, b2);
                if (i >= 0) line += catalog[i].name;
                else line += $"XROM {(b1 & 0x0f) << 2 | ((b2 & 0xc0) >> 6):d2},{b2 & 0x3f:d2}";
                return line;
            }
            if (b1 < 0x10)
            {
                i = 0;
                while (reverse[i].cmd != 0xff &&
                       reverse[i].cmd != b2) i++;
                if (reverse[i].cmd == 0xff) line += $"XROM {(b1 & 0x0f) << 2 | ((b2 & 0xc0) >> 6):d2},{b2 & 0x3f:d2}";
                else line += reverse[i].name;
                return line;
            }

            i = 0;
            while (reverse[i].cmd != 0xff &&
                   reverse[i].cmd != b1) i++;
            if (reverse[i].cmd == 0xff)
            {
                line += $"XROM {(b1 & 0x0f) << 2 | ((b2 & 0xc0) >> 6):d2},{b2 & 0x3f:d2}";
                return line;
            }
            line += reverse[i].name + " ";

            if (reverse[i].size < 2) return line;
            if (b2 >= 0x80) line += "IND ";
            b2 &= 0x7f;
            if (b2 < 0x66) line += b2.ToString("d02");
            else
                switch (b2)
                {
                    case 0x66: line += "A"; break;
                    case 0x67: line += "B"; break;
                    case 0x68: line += "C"; break;
                    case 0x69: line += "D"; break;
                    case 0x6a: line += "E"; break;
                    case 0x6b: line += "F"; break;
                    case 0x6c: line += "G"; break;
                    case 0x6d: line += "H"; break;
                    case 0x6e: line += "I"; break;
                    case 0x6f: line += "J"; break;
                    case 0x70: line += "T"; break;
                    case 0x71: line += "Z"; break;
                    case 0x72: line += "Y"; break;
                    case 0x73: line += "X"; break;
                    case 0x74: line += "L"; break;
                    case 0x75: line += "M"; break;
                    case 0x76: line += "N"; break;
                    case 0x77: line += "O"; break;
                    case 0x78: line += "P"; break;
                    case 0x79: line += "Q"; break;
                    case 0x7a: line += "|-"; break;
                    case 0x7b: line += "a"; break;
                    case 0x7c: line += "b"; break;
                    case 0x7d: line += "c"; break;
                    case 0x7e: line += "d"; break;
                    case 0x7f: line += "e"; break;
                }
            return line;
        }

    }
}

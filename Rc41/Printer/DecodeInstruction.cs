using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        string DecodeInstruction(byte b1, byte b2)
        {
            int i;
            string line = "";
            if (b1 == 0x04 && b2 < 0x10)
            {
                switch (b2)
                {
                    case 0x00: return "CAT";
                    case 0x01: return "@c";
                    case 0x02: return "DEL";
                    case 0x03: return "COPY";
                    case 0x04: return "CLP";
                    case 0x05: return "R/S";
                    case 0x06: return "SIZE";
                    case 0x07: return "BST";
                    case 0x08: return "SST";
                    case 0x09: return "ON";
                    case 0x0a: return "PACK";
                    case 0x0b: return "<--";
                    case 0x0c: return "ALPHA";
                    case 0x0d: return "2__";
                    case 0x0e: return "SHIFT";
                    case 0x0f: return "ASN";
                    default: return "";
                }
            }
            if (b1 >= 0xa0 && b1 <= 0xa7)
            {
                i = cpu.FindCommand(b1, b2);
                if (i >= 0) line += cpu.catalog[i].name;
                else line += $"XROM {(b1 & 0x0f) << 2 | (b2 & 0xc0) >> 6:d2},{b2 & 0x3f:d2}";
                return line;
            }
            if (b1 < 0x10)
            {
                i = 0;
                while (cpu.reverse[i].cmd != 0xff &&
                       cpu.reverse[i].cmd != b2) i++;
                if (cpu.reverse[i].cmd == 0xff) line += $"XROM {(b1 & 0x0f) << 2 | (b2 & 0xc0) >> 6:d2},{b2 & 0x3f:d2}";
                else line += cpu.reverse[i].name;
                return line;
            }

            i = cpu.FindCommand(b1, 0);
            if (i >= 0) 
            {
                line += cpu.catalog[i].name + " ";
            }
            else
            {
                line += $"XROM {(b1 & 0x0f) << 2 | (b2 & 0xc0) >> 6:d2},{b2 & 0x3f:d2}";
                return line;
            }
//            i = 0;
//            while (cpu.reverse[i].cmd != 0xff &&
//                   cpu.reverse[i].cmd != b1) i++;
//            if (cpu.reverse[i].cmd == 0xff)
//            {
//                line += $"XROM {(b1 & 0x0f) << 2 | (b2 & 0xc0) >> 6:d2},{b2 & 0x3f:d2}";
//                return line;
//            }
//            line += cpu.reverse[i].name + " ";

            if (b2 >= 0x80) line += "IND ";
            b2 &= 0x7f;
            if (b2 < 0x70) line += b2.ToString("d02");
            else
                switch (b2)
                {
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

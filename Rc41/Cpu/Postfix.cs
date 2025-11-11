using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        string Postfix(byte b1, byte b2)
        {
            int i;
            byte c;
            string buffer;
            if (b1 < 0xa0 || b1 > 0xa7) buffer = $"{reverse[b1].name}";
            else
            {
                i = FindCommand(b1, b2);
                if (i >= 0) buffer = catalog[i].name;
                else buffer = $"XROM {((b1 & 0x0f) << 2) | ((b2 >> 6) & 0x03):d2},{b2 & 0x3f:d2}";
                return buffer;
            }
            if ((reverse[b1].size & 0xf0) == 0x40) return buffer;
            if ((reverse[b1].size & 0x0f) == 0x01) return buffer;
            buffer += " ";
            if (b2 >= 0x80) buffer += "IND ";
            b2 &= 0x7f;
            if ((reverse[b1].size & 0xf0) == 0x90)
            {
                if (b2 < 100)
                {
                    buffer += $"{b2:d2}";
                }
                if (b2 >= 102 && b2 <= 111)
                {
                    c = (byte)(b2 - 102 + 'A');
                    buffer += ((char)c).ToString();
                }
                if (b2 >= 123 && b2 <= 127)
                {
                    c = (byte)(b2 - 123 + 'a');
                    buffer += ((char)c).ToString();
                }
            }
            else if (b2 == 112) buffer += "T";
            else if (b2 == 113) buffer += "Z";
            else if (b2 == 114) buffer += "Y";
            else if (b2 == 115) buffer += "X";
            else if (b2 == 116) buffer += "L";
            else if (b2 == 117) buffer += "M";
            else if (b2 == 118) buffer += "N";
            else if (b2 == 119) buffer += "O";
            else if (b2 == 120) buffer += "P";
            else if (b2 == 121) buffer += "Q";
            else if (b2 == 122) buffer += ((char)0x7f).ToString();
            else if (b2 == 123) buffer += "a";
            else if (b2 == 124) buffer += "b";
            else if (b2 == 125) buffer += "c";
            else if (b2 == 126) buffer += "d";
            else if (b2 == 127) buffer += "e";
            else
            {
                if ((reverse[b1].size & 0xf0) == 0x80)
                {
                    if (b2 < 65)
                    {
                        buffer += $"{b2 % 10:d}";
                    }
                    else
                    {
                        buffer += $"{(char)b2}";
                    }
                }
                else
                {
                    buffer += $"{b2:d2}";
                }
            }
            return buffer;
        }

        public string Post(byte b)
        {
            string ret = " ";
            if (b >= 0x80)
            {
                ret += "IND ";
                b &= 0x7f;
            }
            if (b < 100) ret += $"{b:d2}";
            if (b >= 102 && b <= 111)
            {
                ret += $"{(char)(b - 37)}";
            }
            if (b >= 113 && b <= 127)
            {
                switch (b)
                {
                    case 112: ret += "T"; break;
                    case 113: ret += "Z"; break;
                    case 114: ret += "Y"; break;
                    case 115: ret += "X"; break;
                    case 116: ret += "L"; break;
                    case 117: ret += "M"; break;
                    case 118: ret += "N"; break;
                    case 119: ret += "O"; break;
                    case 120: ret += "P"; break;
                    case 121: ret += "Q"; break;
                    case 122: ret += ((char)0x7f).ToString(); break;
                    case 123: ret += "a"; break;
                    case 124: ret += "b"; break;
                    case 125: ret += "c"; break;
                    case 126: ret += "d"; break;
                    case 127: ret += "e"; break;
                }
            }
            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public string ProgramList(int lineNumber, int adr)
        {
            int i;
            int b;
            int b2;
            int end;
            string buffer = "";
            end = ((ram[REG_C + 1] & 0x0f) << 8) | ram[REG_C + 0];
            while (ram[adr] == 0) adr--;
            if (lineNumber < 100) buffer = $"{lineNumber:d02} ";
            else buffer = $"{lineNumber} ";

            b = ram[adr];
            if (b >= 0xc0 && b < 0xce)
            {
                if (ram[adr - 2] < 0xf0)
                {
                    if ((ram[adr - 2] & 0xf0) == 0x20)
                    {
                        buffer = $".END. REG {end - FindBottom():d}";
                    }
                    else
                    {
                        if (lineNumber < 100) buffer = $"{lineNumber:d02} END";
                        else buffer = $"{lineNumber:D} END";
                    }
                }
                else
                {
                    if (lineNumber < 100) buffer = $"{lineNumber:d02} ";
                    else buffer = $"{lineNumber:d} ";
                    buffer += "LBL\"";
                    adr -= 2;
                    b = ram[adr] - 1;
                    b &= 0xf;
                    adr -= 2;
                    for (i = 0; i < b; i++)
                    {
                        buffer += ((char)ram[adr--]).ToString();

                    }
                }
            }
            else
            {
                if (b >= 0x10 && b <= 0x1c)
                {
                    buffer = $"{lineNumber:d2} " + ProgramNumber(ref adr);
                }
                else if (b == 0xae)
                {
                    b2 = ram[adr - 1];
                    if (b2 >= 0x80)
                    {
                        //                        buffer = $"{lineNumber:d2} XEQ IND {b2 & 0x7f:d02}";
                        buffer = $"{lineNumber:d2} XEQ IND" + Post((byte)(b2 & 0x7f));
                    }
                    else
                    {
                        //                        buffer = $"{lineNumber:d2} GTO IND {b2 & 0x7f:d02}";
                        buffer = $"{lineNumber:d2} GTO IND" + Post((byte)(b2 & 0x7f));
                    }
                }
                else if (b >= 0xa0 && b <= 0xa7)
                {
                    b2 = ram[adr - 1];
                    i = FindCommand((byte)b, (byte)b2);
                    if (i >= 0) buffer = $"{lineNumber:d2} {catalog[i].name}";
                    else
                    {
                        b = ((b & 0x0f) << 2) | ((b2 & 0xc0) >> 6);
                        b2 &= 0x3f;
                        buffer = $"{lineNumber:d2} XROM {b:d2},{b2:d2}";
                    }
                }
                else if (b < 0xf0 && (reverse[b].size & 0x0f) == 1)
                {
                    buffer += $"{reverse[b].name}";
                }
                else if (b < 0xf0 && (reverse[b].size & 0x0f) == 2)
                {
                    buffer += Postfix((byte)b, ram[adr - 1]);
                }
                else if ((reverse[b].size & 0xf0) == 0x60)
                {
                    if (ram[adr - 2] >= 102 && ram[adr - 2] <= 111)
                        buffer += $"{reverse[b].name} {(char)(ram[adr - 2] - 102 + 'A')}";
                    else if (ram[adr - 2] >= 123 && ram[adr - 2] <= 127)
                        buffer += $"{reverse[b].name} {(char)(ram[adr - 2] - 123 + 'a')}";
                    else
                        buffer += $"{reverse[b].name} {ram[adr - 2] & 0x7f:d02}";
                }
                else if ((reverse[b].size & 0xf0) == 0x10)
                {
                    buffer += $"{reverse[b].name}\"";
                    b = ram[--adr] & 0x0f;
                    for (i = 0; i < b; i++)
                    {
                        buffer += ((char)ram[--adr]).ToString();
                    }
                }
                else if (b >= 0xf0)
                {
                    adr--;
                    buffer = buffer.Substring(0, buffer.Length - 1) + "\"";
                    for (i = 0; i < (b & 0x0f); i++)
                    {
                        if (ram[adr] == 0x7f) { buffer += (char)0x7f; adr--; }
                        else if (ram[adr] == 0x00) { buffer += (char)0; adr--; }
                        else if (ram[adr] > 0x7f) { buffer += (char)2; adr--; }
                        else buffer += (char)ram[adr--];
                    }
                }
            }
            return buffer;
        }
    }
}

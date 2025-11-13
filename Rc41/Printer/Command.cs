using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_Printer
{
    public partial class Printer
    {
        public void Command(byte function)
        {
            //  int    end;
            int i;
            int m;
            int n;
            int p;
            int f;
            int s;
            int e;
            int be;
            int reg;
            int r00;
            char ch;
            int[] r = new int[7];
            Number x;
            string buffer;
            if (power == false) return;
            if (cpu.FlagSet(Cpu.F_PRT_EXIST) == false)
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
                return;
            }
            if (cpu.FlagSet(21) == false) return;

            if (function == 1)
            {                           // ACA
                n = 0;
                m = Cpu.REG_P + 2;
                buffer = "";
                while (m >= Cpu.REG_M)
                {
                    if (cpu.ram[m] == 0 && n != 0) buffer += "_";
                    else if (cpu.ram[m] != 0)
                    {
                        if (cpu.ram[m] < ' ' || cpu.ram[m] > 0x7e) PrintToBuffer((byte)'#');
                        else if (cpu.ram[m] >= 'A' && cpu.ram[m] <= 'Z' && cpu.FlagSet(13) != false)
                            PrintToBuffer((byte)(cpu.ram[m] + 32));
                        else PrintToBuffer(cpu.ram[m]);
                    }
                    if (cpu.ram[m] != 0) n = -1;
                    m--;
                }
            }

            else if (function == 2)
            {                      // ACCHR
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < x.exponent[0] * 10 + x.exponent[1]; n++)
                    {
                        if (n + 1 < 10) m = m * 10 + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("NONEXISTENT");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                PrintToBuffer((byte)m);
            }

            else if (function == 3)
            {                      // ACCOL
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < x.exponent[0] * 10 + x.exponent[1]; n++)
                    {
                        if (n + 1 < 10) m = m * 10 + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("DATA ERROR");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                PrintToBuffer((byte)(m | 0x80));
            }

            else if (function == 4)
            {                      // ACSPEC
                r[6] = (cpu.ram[Cpu.REG_X + 6] & 0x01) << 6 | (cpu.ram[Cpu.REG_X + 5] & 0xfc) >> 2;
                r[5] = (cpu.ram[Cpu.REG_X + 5] & 0x03) << 5 | (cpu.ram[Cpu.REG_X + 4] & 0xf8) >> 3;
                r[4] = (cpu.ram[Cpu.REG_X + 4] & 0x07) << 4 | (cpu.ram[Cpu.REG_X + 3] & 0xf0) >> 4;
                r[3] = (cpu.ram[Cpu.REG_X + 3] & 0x0f) << 3 | (cpu.ram[Cpu.REG_X + 2] & 0xe0) >> 5;
                r[2] = (cpu.ram[Cpu.REG_X + 2] & 0x1f) << 2 | (cpu.ram[Cpu.REG_X + 1] & 0xc0) >> 6;
                r[1] = (cpu.ram[Cpu.REG_X + 1] & 0x3f) << 1 | (cpu.ram[Cpu.REG_X + 0] & 0x80) >> 7;
                r[0] = cpu.ram[Cpu.REG_X + 0] & 0x7f;
                for (i = 6; i >= 0; i--)
                    PrintToBuffer((byte)(r[i] | 0x80));
            }

            else if (function == 5)
            {                      // ACX
                x = cpu.RecallNumber(Cpu.R_X);
                buffer = cpu.Format(x);
                if (x.sign == 1 && cpu.FlagSet(13))
                {
                    buffer = buffer.ToLower();
                }
                for (n = 0; n < buffer.Length; n++)
                {
                    PrintToBuffer((byte)buffer[n]);
                }
            }

            else if (function == 6)
            {                      // BLDSPEC
                cpu.ram[Cpu.LIFT] = (byte)'D';
                cpu.ram[Cpu.PENDING] = (byte)'E';
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    p = x.mantissa[0];
                    for (n = 0; n < x.exponent[0] * 10 + x.exponent[1]; n++)
                    {
                        if (n + 1 < 10) p = p * 10 + x.mantissa[n + 1];
                        else p *= 10;
                    }
                    if (p > 127)
                    {
                        cpu.Message("DATA ERROR");
                        cpu.Error();
                        return;
                    }
                }
                else
                {
                    cpu.Message("DATA ERROR");
                    cpu.Error();
                    return;
                }
                if ((cpu.ram[Cpu.REG_Y + 6] & 0xf0) != 0x10)
                {
                    for (i = Cpu.REG_Y; i < Cpu.REG_Y + 6; i++) cpu.ram[i] = 0x00;
                    cpu.ram[Cpu.REG_Y + 6] = 0x10;
                }
                r[5] = (cpu.ram[Cpu.REG_Y + 5] & 0x03) << 5 | (cpu.ram[Cpu.REG_Y + 4] & 0xf8) >> 3;
                r[4] = (cpu.ram[Cpu.REG_Y + 4] & 0x07) << 4 | (cpu.ram[Cpu.REG_Y + 3] & 0xf0) >> 4;
                r[3] = (cpu.ram[Cpu.REG_Y + 3] & 0x0f) << 3 | (cpu.ram[Cpu.REG_Y + 2] & 0xe0) >> 5;
                r[2] = (cpu.ram[Cpu.REG_Y + 2] & 0x1f) << 2 | (cpu.ram[Cpu.REG_Y + 1] & 0xc0) >> 6;
                r[1] = (cpu.ram[Cpu.REG_Y + 1] & 0x3f) << 1 | (cpu.ram[Cpu.REG_Y + 0] & 0x80) >> 7;
                r[0] = cpu.ram[Cpu.REG_Y + 0] & 0x7f;
                cpu.ram[Cpu.REG_Y + 6] = (byte)(0x10 | (r[5] & 0x40) >> 6);
                cpu.ram[Cpu.REG_Y + 5] = (byte)((r[5] & 0x3f) << 2 | r[4] >> 5 & 0x03);
                cpu.ram[Cpu.REG_Y + 4] = (byte)((r[4] & 0x1f) << 3 | r[3] >> 4 & 0x07);
                cpu.ram[Cpu.REG_Y + 3] = (byte)((r[3] & 0x0f) << 4 | r[2] >> 3 & 0x0f);
                cpu.ram[Cpu.REG_Y + 2] = (byte)((r[2] & 0x07) << 5 | r[1] >> 2 & 0x1f);
                cpu.ram[Cpu.REG_Y + 1] = (byte)((r[1] & 0x03) << 6 | r[0] >> 1 & 0x3f);
                cpu.ram[Cpu.REG_Y + 0] = (byte)((r[0] & 0x01) << 7 | p & 0x7f);
                x = cpu.RecallNumber(Cpu.R_Y); cpu.StoreNumber(x, Cpu.R_X);
                x = cpu.RecallNumber(Cpu.R_Z); cpu.StoreNumber(x, Cpu.R_Y);
                x = cpu.RecallNumber(Cpu.R_T); cpu.StoreNumber(x, Cpu.R_Z);
            }

            else if (function == 7)
            {
                Prp("", arg);
            }

            else if (function == 8)
            {                      // PRA
                if (printBuffer.Length != 0)
                {
                    PrintBuffer(' ');
                }
                n = 0;
                p = 0;
                m = Cpu.REG_P + 2;
                buffer = "";
                while (m >= Cpu.REG_M)
                {
                    if (cpu.ram[m] == 0 && n != 0) buffer += "_";
                    else if (cpu.ram[m] != 0)
                    {
                        if (cpu.ram[m] < ' ' || cpu.ram[m] > 0x7e) buffer += "#";
                        else
                        {
                            ch = (char)cpu.ram[m];
                            if (cpu.FlagSet(13))
                            {
                                if (ch >= 'A' && ch <= 'Z') ch = (char)((byte)ch + 32);
                            }
                            buffer += ch.ToString();
                        }
                    }
                    if (cpu.ram[m] != 0) n = -1;
                    m--;
                }
                Print(buffer, 'L');
            }

            else if (function == 10)
            {                     // PRBUF
                PrintBuffer('L');
            }

            else if (function == 11)
            {                     // PRFLAGS
                Print("", 'L');
                if (printBuffer.Length != 0) PrintBuffer(' ');
                Print("STATUS:", 'L');
                reg = (cpu.ram[Cpu.REG_C + 2] << 4) + (cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f);
                Print($"SIZE= {Cpu.RAMTOP - reg:d03}", 'L');
                reg = cpu.ram[Cpu.REG_C + 6] << 4 | cpu.ram[Cpu.REG_C + 5] >> 4 & 0x0f;
                Print($"\x7e= {reg:d03}", 'L');
                if (cpu.FlagSet(Cpu.F_GRAD) == false && cpu.FlagSet(Cpu.F_RAD) == false) Print("DEG", 'L');
                if (cpu.FlagSet(Cpu.F_GRAD) != false && cpu.FlagSet(Cpu.F_RAD) == false) Print("GRAD", 'L');
                if (cpu.FlagSet(Cpu.F_GRAD) == false && cpu.FlagSet(Cpu.F_RAD) != false) Print("RAD", 'L');
                n = 0;
                if (cpu.FlagSet(36)) n |= 8;
                if (cpu.FlagSet(37)) n |= 4;
                if (cpu.FlagSet(38)) n |= 2;
                if (cpu.FlagSet(39)) n |= 1;
                if (cpu.FlagSet(40) == false && cpu.FlagSet(41) == false) Print($"SCI {n}", 'L');
                if (cpu.FlagSet(40) != false && cpu.FlagSet(41) == false) Print($"FIX {n}", 'L');
                if (cpu.FlagSet(40) == false && cpu.FlagSet(41) != false) Print($"ENG {n}", 'L');
                Print("", 'L');
                Print("FLAGS:", 'L');
                m = Cpu.REG_D + 6;
                f = 0;
                while (m >= Cpu.REG_D)
                {
                    p = cpu.ram[m--];
                    for (n = 0; n < 8; n++)
                    {
                        if ((p & 0x80) != 0) Print($"F {f:d02}  SET", 'L');
                        else Print($"F {f:d02}  CLEAR", 'L');
                        p <<= 1;
                        f++;
                    }
                }
            }

            else if (function == 12)
            {                          // PRKEYS
                prkeys();
            }

            else if (function == 16)
            {                          // PRREG
                if (printBuffer.Length != 0) PrintBuffer(' ');
                n = 0;
                reg = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
                while (reg < Cpu.RAMTOP)
                {
                    x = cpu.RecallNumber(reg);
                    buffer = cpu.Format(x);
                    if (x.sign == 1) buffer = $"\"{buffer}\"";
                    Print($"R{n:d02}= {buffer}", 'L');
                    n++;
                    reg++;
                }
            }

            else if (function == 17)
            {                          // PRREGX
                if (printBuffer.Length != 0) PrintBuffer(' ');
                x = cpu.RecallNumber(Cpu.R_X);
                s = 0;
                e = 0;
                p = 0;
                if (x.esign == 0)
                {
                    s = x.mantissa[p++];
                    n = x.exponent[0] * 10 + x.exponent[1];
                    while (n > 0)
                    {
                        s *= 10;
                        if (p < 10) s += x.mantissa[p++];
                        n--;
                    }
                }
                else
                {
                    n = x.exponent[0] * 10 + x.exponent[1];
                    while (n > 0)
                    {
                        n--;
                        p--;
                    }
                    p++;
                }
                for (i = 0; i < 3; i++)
                {
                    e *= 10;
                    if (p >= 0 && p < 10) e += x.mantissa[p];
                    p++;
                }
                reg = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
                n = s;
                e += reg;
                reg += s;
                if (reg > 0x1ff)
                {
                    cpu.Message("NONEXISTENT");
                    cpu.Error();
                }
                else
                {
                    while (reg <= e)
                    {
                        if (reg > 0xfff)
                        {
                            cpu.Message("NONEXISTENT");
                            cpu.Error();
                            reg = e + 1;
                        }
                        else
                        {
                            x = cpu.RecallNumber(reg);
                            buffer = cpu.Format(x);
                            if (x.sign == 1) buffer = $"\"{buffer}\"";
                            Print($"R{n:d02}= {buffer}", 'L');
                            n++;
                            reg++;
                        }
                    }
                }
            }

            else if (function == 18)
            {                          // PRE
                if (printBuffer.Length != 0) PrintBuffer(' ');
                reg = cpu.ram[Cpu.REG_C + 6] << 4 | cpu.ram[Cpu.REG_C + 5] >> 4 & 0x0f;
                r00 = cpu.ram[Cpu.REG_C + 2] << 4 | cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f;
                x = cpu.RecallNumber(reg + r00);
                buffer = cpu.Format(x);
                Print($"\x7eX  = {buffer}", 'L');
                x = cpu.RecallNumber(reg + r00 + 1);
                buffer = cpu.Format(x);
                Print($"\x7eX^2= {buffer}", 'L');
                x = cpu.RecallNumber(reg + r00 + 2);
                buffer = cpu.Format(x);
                Print($"\x7eY  = {buffer}", 'L');
                x = cpu.RecallNumber(reg + r00 + 3);
                buffer = cpu.Format(x);
                Print($"\x7eY^2= {buffer}", 'L');
                x = cpu.RecallNumber(reg + r00 + 4);
                buffer = cpu.Format(x);
                Print($"\x7eXY = {buffer}", 'L');
                x = cpu.RecallNumber(reg + r00 + 5);
                buffer = cpu.Format(x);
                Print($"N   = {buffer}", 'L');
            }

            else if (function == 19)
            {                          // PRSTK
                Print("", 'L');
                if (printBuffer.Length != 0) PrintBuffer(' ');
                x = cpu.RecallNumber(Cpu.R_T);
                buffer = cpu.Format(x);
                Print($"T= {buffer}", 'L');
                x = cpu.RecallNumber(Cpu.R_Z);
                buffer = cpu.Format(x);
                Print($"Z= {buffer}", 'L');
                x = cpu.RecallNumber(Cpu.R_Y);
                buffer = cpu.Format(x);
                Print($"Y= {buffer}", 'L');
                x = cpu.RecallNumber(Cpu.R_X);
                buffer = cpu.Format(x);
                Print($"X= {buffer}", 'L');
            }

            else if (function == 20)
            {                          // PRX
                if (printBuffer.Length != 0) PrintBuffer(' ');
                x = cpu.RecallNumber(Cpu.R_X);
                buffer = cpu.Format(x);
                if (x.sign == 1 && cpu.FlagSet(13))
                {
                    buffer = buffer.ToLower();
                }
                Print(buffer, 'R');
            }

            else if (function == 21)      // REGPLOT
            {
                double ymin = cpu.Rcl(0).ToDouble();
                double ymax = cpu.Rcl(1).ToDouble();
                double value = cpu.RecallNumber(Cpu.R_X).ToDouble();
                be = cpu.GetBE(cpu.Rcl(2));
                Plot(ymin, ymax, value, be);
            }

            else if (function == 22)
            {                          // SKPCHR
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < x.exponent[0] * 10 + x.exponent[1]; n++)
                    {
                        if (n + 1 < 10) m = m * 10 + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("NONEXISTENT");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                for (i = 0; i < m; i++)
                    PrintToBuffer((byte)' ');
            }

            else if (function == 23)
            {                          // SKPCOL
                x = cpu.RecallNumber(Cpu.R_X);
                if (x.esign >= 0)
                {
                    m = x.mantissa[0];
                    for (n = 0; n < x.exponent[0] * 10 + x.exponent[1]; n++)
                    {
                        if (n + 1 < 10) m = m * 10 + x.mantissa[n + 1];
                        else m *= 10;
                    }
                    if (m > 127)
                    {
                        cpu.Message("NONEXISTENT");
                        cpu.Error();
                        return;
                    }
                }
                else m = 0;
                for (i = 0; i < m; i++)
                    PrintToBuffer(0x80);
            }
            else if (function == 24)
            {                          // STKPLOT
                double ymin = cpu.RecallNumber(Cpu.R_Z).ToDouble();
                double ymax = cpu.RecallNumber(Cpu.R_Y).ToDouble();
                double value = cpu.RecallNumber(Cpu.R_T).ToDouble();
                be = cpu.GetBE(cpu.RecallNumber(Cpu.R_X));
                Plot(ymin, ymax, value, be);
            }
            else
            {
                cpu.Message("NONEXISTENT");
                cpu.Error();
            }

        }
    }
}

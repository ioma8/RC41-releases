using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Extended memory file header
 * | 6 | 5 | 4 | 3 | 2 | 1 | 0 | - Filename, padded with spaces on the right
 * | 6 | 5 | 4 | 3 | 2 | 1 | 0 | - File data
 *                                 6   - File type: 0xff - EOD, 'D' Data, 'P' Program, 'A' Ascii
 *                                 5,4 - File size in registers
 */
namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void Command(byte function)
        {
            string alpha;
            string a2;
            Number x;
            Number y;
            Number z;
            int i;
            int p;
            int j;
            byte c;
            int b;
            int e;
            int d;
            int n;
            byte mask;

            switch (function)
            {
                case 1:                                        // ALENG
                    int len = 0;
                    int addr = Cpu.REG_P + 2;
                    while (addr >= Cpu.REG_M && cpu.ram[addr] == 0x00) addr--;
                    while (addr >= Cpu.REG_M)
                    {
                        addr--;
                        len++;
                    }
                    cpu.StoreNumber(new Number(len), Cpu.R_X);
                    break;

                case 2:                                       // ANUM
                    anum();
                    break;

                case 3:                                       // APPCHR
                    appchr();
                    break;

                case 4:                                       // APPREC
                    apprec();
                    break;

                case 5:                                       // ARCLREC
                    arclrec();
                    break;

                case 6:                                       // AROT
                    p = Cpu.REG_P + 2;
                    while (p >= Cpu.REG_M && cpu.ram[p] == 0x00) p--;
                    if (p < Cpu.REG_M) return;
                    x = cpu.RecallNumber(Cpu.R_X);
                    i = x.Int();
                    while (i > 0)
                    {
                        c = cpu.ram[p];
                        for (j = p; j > Cpu.REG_M; j--) cpu.ram[j] = cpu.ram[j - 1];
                        cpu.ram[Cpu.REG_M] = c;
                        i--;
                    }
                    while (i < 0)
                    {
                        c = cpu.ram[Cpu.REG_M];
                        for (j = Cpu.REG_M; j < p; j++) cpu.ram[j] = cpu.ram[j + 1];
                        cpu.ram[p] = c;
                        i++;
                    }
                    break;

                case 7:                                       // ATOX
                    p = Cpu.REG_P + 2;
                    while (p >= Cpu.REG_M && cpu.ram[p] == 0x00) p--;
                    if (p >= Cpu.REG_M)
                    {
                        cpu.StoreNumber(new Number(cpu.ram[p]), Cpu.R_X);
                        cpu.ram[p] = 0x00;
                    }
                    else cpu.StoreNumber(new Number(0), Cpu.R_X);
                    break;

                case 8:                                       // CLFL
                    clfl();
                    break;

                case 9:                                       // CLKEYS
                    clkeys();
                    break;

                case 10:                                      // CRFLAS
                    crflas();
                    break;

                case 11:                                      // CRFLD
                    crfld();
                    break;

                case 12:                                      // DELCHR
                    delchr();
                    break;

                case 13:                                      // DELREC
                    delrec();
                    break;

                case 14:                                      // EMDIR
                    emdir();
                    break;

                case 15:                                      // FLSIZE
                    flsize();
                    break;

                case 16:                                      // GETAS
                    getas();
                    break;

                case 18:                                      // GETP
                    getp();
                    break;

                case 19:                                      // GETR
                    getr();
                    break;

                case 20:                                     // GETREC
                    getrec();
                    break;

                case 21:                                     // GETRX
                    getrx();
                    break;

                case 22:                                     // GETSUB
                    getsub();
                    break;

                case 23:                                     // GETX
                    getx();
                    break;

                case 24:                                      // INSCHR
                    inschr();
                    break;

                case 25:                                      // INSREC
                    insrec();
                    break;

                case 26:                                      // PASN
                    pasn();
                    break;

                case 27:                                      // PCLPS
                    pclps();
                    break;

                case 28:                                      // POSA
                    posa();
                    break;

                case 29:                                     // POSFL
                    posfl();
                    break;

                case 30:                                     // PSIZE
                    psize();
                    break;

                case 31:                                     // PURFL
                    purfl();
                    break;

                case 32:                                     // RCLFLAG
                    x = new Number(0);
                    x.sign = 0x01;
                    x.mantissa[0] = (byte)((cpu.ram[Cpu.REG_D + 6] & 0xf0) >> 4);   // flags 0-3
                    x.mantissa[1] = (byte)(cpu.ram[Cpu.REG_D + 6] & 0x0f);          // flags 4-7
                    x.mantissa[2] = (byte)((cpu.ram[Cpu.REG_D + 5] & 0xf0) >> 4);   // flags 8-11
                    x.mantissa[3] = (byte)(cpu.ram[Cpu.REG_D + 5] & 0x0f);          // flags 12-15
                    x.mantissa[4] = (byte)((cpu.ram[Cpu.REG_D + 4] & 0xf0) >> 4);   // flags 16-19
                    x.mantissa[5] = (byte)(cpu.ram[Cpu.REG_D + 4] & 0x0f);          // flags 20-23
                    x.mantissa[6] = (byte)((cpu.ram[Cpu.REG_D + 3] & 0xf0) >> 4);   // flags 24-27
                    x.mantissa[7] = (byte)(cpu.ram[Cpu.REG_D + 3] & 0x0f);          // flags 28-31
                    x.mantissa[8] = (byte)((cpu.ram[Cpu.REG_D + 2] & 0xf0) >> 4);   // flags 32-35
                    x.mantissa[9] = (byte)(cpu.ram[Cpu.REG_D + 2] & 0x0f);          // flags 36-39
                    x.esign = (byte)((cpu.ram[Cpu.REG_D + 1] & 0xf0) >> 4);         // flags 40-43
                    cpu.StoreNumber(x, Cpu.R_X);
                    break;

                case 33:                                     // RCLPT
                    rclpt();
                    break;

                case 34:                                     // RCLPTA
                    rclpta();
                    break;

                case 35:                                     // REGMOVE
                    x = cpu.RecallNumber(Cpu.R_X);
                    b = x.Int();
                    e = x.Decimal(0, 3);
                    n = x.Decimal(3, 3);
                    if (n == 0) n = 1;
                    for (i=0; i<n; i++)
                    {
                        y = cpu.Rcl(b + i);
                        cpu.Sto(y, e + i);
                    }
                    break;

                case 36:                                     // REGSWAP
                    x = cpu.RecallNumber(Cpu.R_X);
                    b = x.Int();
                    e = x.Decimal(0, 3);
                    n = x.Decimal(3, 3);
                    if (n == 0) n = 1;
                    for (i = 0; i < n; i++)
                    {
                        y = cpu.Rcl(b + i);
                        z = cpu.Rcl(e + i);
                        cpu.Sto(y, e + i);
                        cpu.Sto(z, b + i);
                    }
                    break;

                case 37:                                     // SAVEAS
                    saveas();
                    break;

                case 38:                                     // SAVEP
                    savep();
                    break;

                case 39:                                     // SAVER
                    saver();
                    break;

                case 40:                                     // SAVERX
                    saverx();
                    break;

                case 41:                                     // SAVEX
                    savex();
                    break;

                case 42:                                     // SEEKPT
                    seekpt();
                    break;

                case 43:                                     // SEEKPTA
                    seekpta();
                    break;

                case 44:                                     // SIZE?
                    i = (cpu.ram[Cpu.REG_C + 2] << 4) + (cpu.ram[Cpu.REG_C + 1] >> 4 & 0x0f);
                    i = Cpu.RAMTOP - i;
                    cpu.StoreNumber(new Number(i), Cpu.R_X);
                    break;

                case 45:                                     // STOFLAG
                    x = cpu.RecallNumber(Cpu.R_X);
                    if (x.sign == 0x01)
                    {
                        cpu.ram[Cpu.REG_D + 6] = (byte)(((x.mantissa[0] & 0x0f) << 4) | (x.mantissa[1] & 0x0f));
                        cpu.ram[Cpu.REG_D + 5] = (byte)(((x.mantissa[2] & 0x0f) << 4) | (x.mantissa[3] & 0x0f));
                        cpu.ram[Cpu.REG_D + 4] = (byte)(((x.mantissa[4] & 0x0f) << 4) | (x.mantissa[5] & 0x0f));
                        cpu.ram[Cpu.REG_D + 3] = (byte)(((x.mantissa[6] & 0x0f) << 4) | (x.mantissa[7] & 0x0f));
                        cpu.ram[Cpu.REG_D + 2] = (byte)(((x.mantissa[8] & 0x0f) << 4) | (x.mantissa[9] & 0x0f));
                        cpu.ram[Cpu.REG_D + 1] = (byte)(((x.esign & 0x0f) << 4) | (cpu.ram[Cpu.REG_D + 1] & 0x0f));
                    }
                    else
                    {
                        b = x.Int();
                        y = cpu.RecallNumber(Cpu.R_Y);
                        e = x.Decimal(0, 2);
                        for (i=b; i<=e; i++)
                        {
                            p = i / 8;
                            mask = (byte)(128 >> (i % 8));
                            if ((y[p] & mask) != 0) cpu.SetFlag((byte)i);
                            else cpu.ClearFlag((byte)i);
                        }
                    }
                    break;

                case 46:                                     // X<>F
                    x = cpu.RecallNumber(Cpu.R_X);
                    i = x.Int();
                    if (i > 255)
                    {
                        cpu.Message("DATA ERROR");
                        cpu.Error();
                        return;
                    }
                    c = (byte)cpu.ram[Cpu.REG_D + 6];
                    cpu.ram[Cpu.REG_D + 6] = 0;
                    if ((i & 1) != 0) cpu.ram[Cpu.REG_D + 6] |= 128;
                    if ((i & 2) != 0) cpu.ram[Cpu.REG_D + 6] |= 64;
                    if ((i & 4) != 0) cpu.ram[Cpu.REG_D + 6] |= 32;
                    if ((i & 8) != 0) cpu.ram[Cpu.REG_D + 6] |= 16;
                    if ((i & 16) != 0) cpu.ram[Cpu.REG_D + 6] |= 8;
                    if ((i & 32) != 0) cpu.ram[Cpu.REG_D + 6] |= 4;
                    if ((i & 64) != 0) cpu.ram[Cpu.REG_D + 6] |= 2;
                    if ((i & 128) != 0) cpu.ram[Cpu.REG_D + 6] |= 1;
                    i = 0;
                    if ((c & 1) != 0) i |= 128;
                    if ((c & 2) != 0) i |= 64;
                    if ((c & 4) != 0) i |= 32;
                    if ((c & 8) != 0) i |= 16;
                    if ((c & 16) != 0) i |= 8;
                    if ((c & 32) != 0) i |= 4;
                    if ((c & 64) != 0) i |= 2;
                    if ((c & 128) != 0) i |= 1;
                    cpu.StoreNumber(new Number(i), Cpu.R_X);
                    break;

                case 47:                                     // XTOA
                    x = cpu.RecallNumber(Cpu.R_X);
                    if (x.sign == 1)
                    {
                        alpha = x.Alpha();
                        foreach (var ch in alpha) cpu.AppendAlpha((byte)ch);
                    }
                    else
                    {
                        cpu.AppendAlpha((byte)x.Int());
                    }
                    break;
            }
        }

    }
}

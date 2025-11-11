using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        public string Format(Number a)
        {
            int i;
            int d;
            int e;
            int mode;
            int decimals;
            string dest;
            int comma;

            dest = "";

            if (a.sign == 0x01)
            {
                if (a.mantissa[1] != 0x00 || a.mantissa[2] != 0x00)
                    dest += ((char)((a.mantissa[1] << 4) | (a.mantissa[2]))).ToString();
                if (a.mantissa[3] != 0x00 || a.mantissa[4] != 0x00)
                    dest += ((char)((a.mantissa[3] << 4) | (a.mantissa[4]))).ToString();
                if (a.mantissa[5] != 0x00 || a.mantissa[6] != 0x00)
                    dest += ((char)((a.mantissa[5] << 4) | (a.mantissa[6]))).ToString();
                if (a.mantissa[7] != 0x00 || a.mantissa[8] != 0x00)
                    dest += ((char)((a.mantissa[7] << 4) | (a.mantissa[8]))).ToString();
                if (a.mantissa[9] != 0x00 || a.esign != 0x00)
                    dest += ((char)((a.mantissa[9] << 4) | a.esign)).ToString();
                if (a.exponent[0] != 0x00 || a.exponent[1] != 0x00)
                    dest += ((char)((a.exponent[0] << 4) | (a.exponent[1]))).ToString();
                return dest;
            }

            if (a.sign != 0) dest += "-"; else dest += " ";
            decimals = 0;
            if (FlagSet(36)) decimals |= 8;
            if (FlagSet(37)) decimals |= 4;
            if (FlagSet(38)) decimals |= 2;
            if (FlagSet(39)) decimals |= 1;

            mode = 0;
            if ((FlagSet(40) == false) && (FlagSet(41) == false)) mode = 0;
            if ((FlagSet(40) == false) && (FlagSet(41) == true)) mode = 1;
            if ((FlagSet(40) == true) && (FlagSet(41) == false)) mode = 2;

            e = a.exponent[0] * 10 + a.exponent[1];

            if (mode == 2 && a.exponent[0] > 0 && decimals != 10 && a.esign == 0x00) mode = 0;

            if (mode == 2)
            {                                               /* FIX */
                if (a.esign != 0)
                {
                    dest += "0";
                    if (decimals != 0 || FlagSet(29))
                    {
                        if (FlagSet(28)) dest += '.'; else dest += ',';
                    }
                    d = 0;
                    for (i = 1; i < a.exponent[1]; i++)
                    {
                        if (d < decimals)
                        {
                            dest += '0';
                            d++;
                        }
                    }
                    for (i = 0; i < 10; i++)
                    {
                        if (d < decimals)
                        {
                            dest += (char)(a.mantissa[i] + '0');
                            d++;
                        }
                    }
                }
                else
                {
                    d = 0;
                    dest += (char)(a.mantissa[0] + '0');
                    d++;
                    comma = e - 3;
                    if (comma > 0) comma = comma % 3;
                    for (i = 0; i < a.exponent[1]; i++)
                    {
                        if (comma == 0 && FlagSet(29))
                        {
                            if (FlagSet(28)) dest += ',';
                            else dest += '.';
                            comma = 3;
                        }
                        if (comma > 0) comma--;
                        dest += (char)(a.mantissa[i + 1] + '0');
                        d++;
                    }
                    if (decimals != 0 || FlagSet(29))
                    {
                        if (FlagSet(28)) dest += '.'; else dest += ',';
                    }
                    for (i = 0; i < decimals; i++)
                    {
                        if (d < 10)
                        {
                            if (d + a.exponent[1] >= 10) dest += '0';
                            else dest += (char)(a.mantissa[d] + '0');
                            d++;
                        }
                    }
                }
            }


            if (mode == 0)
            {                                               /* SCI */
                dest += ((char)(a.mantissa[0] + '0')).ToString();
                if (decimals != 0)
                {
                    if (FlagSet(28)) dest += "."; else dest += ",";
                }
                for (i = 0; i < 7; i++)
                    if (i < decimals) dest += ((char)(a.mantissa[i + 1] + '0')).ToString();
                for (i = 0; i < 7 - decimals; i++)
                    dest += " ";
                if (a.esign != 0) dest += "-"; else dest += " ";
                dest += ((char)(a.exponent[0] + '0')).ToString();
                dest += ((char)(a.exponent[1] + '0')).ToString();
            }

            if (mode == 1)
            {                                               /* ENG */
                e = a.exponent[0] * 10 + a.exponent[1];
                dest += ((char)(a.mantissa[0] + '0')).ToString();
                d = 1;
                while ((e % 3) != 0)
                {
                    dest += ((char)(a.mantissa[d] + '0')).ToString();
                    if (a.esign != 0) e++; else e--;
                    d++;
                }
                if (decimals != 0)
                {
                    if (FlagSet(28)) dest += "."; else dest += ",";
                }
                for (i = 0; i < decimals; i++)
                {
                    if (d < 9)
                    {
                        dest += ((char)(a.mantissa[d] + '0')).ToString();
                        d++;
                    }
                }
                while (d < 9) { dest += " "; d++; }
                if (a.esign != 0) dest = "-"; else dest += " ";
                dest += ((char)((e / 10) + '0')).ToString();
                dest += ((char)((e % 10) + '0')).ToString();
            }

            return dest;
        }
    }
}

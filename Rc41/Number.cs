using Rc41.T_Cpu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Rc41
{
    public struct Number
    {
        public byte sign;
        public byte[] mantissa;
        public byte esign;
        public byte[] exponent;

        public Number()
        {
            mantissa = new byte[10];
            exponent = new byte[2];
            sign = 0;
            esign = 0;
            for (int i=0; i<10; i++) mantissa[i] = 0;
            for (int i=0; i<2; i++) exponent[i] = 0;
        }

        public Number(int n)
        {
            int e;
            int p;
            mantissa = new byte[10];
            exponent = new byte[2];
            sign = 0;
            esign = 0;
            for (int i = 0; i < 10; i++) mantissa[i] = 0;
            for (int i = 0; i < 2; i++) exponent[i] = 0;
            if (n < 0)
            {
                n = -n;
                sign = 9;
            }
            e = 0;
            p = 0;
            string ns = $"{n}";
            while (ns[0] == ' ') ns = ns.Substring(1);
            foreach (var ch in ns)
            {
                if (p < 10) mantissa[p++] = (byte)(ch - '0');
                e++;
            }
            if (e > 0) e--;
            exponent[0] = (byte)(e / 10);
            exponent[1] = (byte)(e % 10);
        }

        public Number(byte s, byte m1, byte m2, byte m3, byte m4, byte m5, byte m6, byte m7, byte m8, byte m9, byte m10, byte es, byte e1, byte e2)
        {
            sign = s;
            esign = es;
            mantissa = new byte[10];
            mantissa[0] = m1;
            mantissa[1] = m2;
            mantissa[2] = m3;
            mantissa[3] = m4;
            mantissa[4] = m5;
            mantissa[5] = m6;
            mantissa[6] = m7;
            mantissa[7] = m8;
            mantissa[8] = m9;
            mantissa[9] = m10;
            exponent = new byte[2];
            exponent[0] = e1;
            exponent[1] = e2;
        }

        public byte this[int i]
        {
            get
            {
                if (i == 0) return sign;
                if (i >= 1 && i <= 10) return mantissa[i - 1];
                if (i == 11) return esign;
                if (i == 12) return exponent[0];
                if (i == 13) return exponent[1];
                return 0;
            }
            set
            {
                if (i == 0) sign = value;
                if (i >= 1 && i <= 10) mantissa[i - 1] = value;
                if (value == 11) esign = value;
                if (value == 12) exponent[0] = value;
                if (value == 13) exponent[1] = value;
            }
        }

        public double ToDouble()
        {
            double d = 0;
            int e;
            for (int i = 0; i < 10; i++) d = (d * 10) + mantissa[i];
            e = exponent[0] * 10 + exponent[1];
            if (esign != 0) e = -e;
            e -= 9;
            while (e < 0)
            {
                d /= 10.0;
                e++;
            }
            while (e > 0)
            {
                d *= 10;
                e--;
            }
            if (sign != 0) d = -d;
            return d;
        }

        public int Int()
        {
            int ret;
            int ex;
            int p;
            ret = 0;
            p = 0;
            if (esign != 0) return 0;
            ex = exponent[0] * 10 + exponent[1];
            while (p < 10 && ex >= 0)
            {
                ret = (ret * 10) + mantissa[p++];
                ex--;
            }
            while (ex >= 0)
            {
                ret *= 10;
                ex--;
            }
            if (sign != 0) ret = -ret;
            return ret;
        }

        public int Decimal(int start, int len)
        {
            int p;
            string d;
            int ex;
            d = "";
            ex = exponent[0] * 10 + exponent[1];
            if (esign != 0) ex = -ex;
            p = 0;
            if (ex >= 0)
            {
                p = ex + 1;
            }
            while (ex < -1)
            {
                d += "0";
                ex++;
            }
            while (p < 10) d += (char)(mantissa[p++] + '0');
            while (start+len > d.Length) { d += '0'; }
            d = d.Substring(start, len);
            p = Convert.ToInt32(d);
            return p;
        }

        public string Alpha()
        {
            bool zeroes;
            string ret;
            byte c;
            if (sign != 0x01) return "";
            zeroes = false;
            ret = "";
            c = (byte)((mantissa[1] << 4) + mantissa[2]);
            if (c != 0) { ret += (char)c; zeroes = true; }
            c = (byte)((mantissa[3] << 4) + mantissa[4]);
            if (c != 0 || zeroes) { ret += (char)c; zeroes = true; }
            c = (byte)((mantissa[5] << 4) + mantissa[6]);
            if (c != 0 || zeroes) { ret += (char)c; zeroes = true; }
            c = (byte)((mantissa[7] << 4) + mantissa[8]);
            if (c != 0 || zeroes) { ret += (char)c; zeroes = true; }
            c = (byte)((mantissa[9] << 4) + esign);
            if (c != 0 || zeroes) { ret += (char)c; zeroes = true; }
            c = (byte)((exponent[0] << 4) + exponent[1]);
            ret += (char)c;
            return ret;
        }

    }
}

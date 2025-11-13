using Rc41.T_Cpu;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public void pasn()
        {
            int i;
            int m;
            int n;
            int kc;
            string alpha = cpu.GetAlpha();
            Number x = cpu.RecallNumber(Cpu.R_X);
            i = x.Int();
            if (i > 0)
            {
                if (i == 45) i = 46;
                if (i == 44) i = 45;
                if (i == 43) i = 44;
                if (i == 42) i = 43;
                m = i / 10;
                n = i % 10;
                kc = ((n - 1) << 4) | m;
            }
            else
            {
                i = -i;
                if (i == 45) i = 46;
                if (i == 44) i = 45;
                if (i == 43) i = 44;
                if (i == 42) i = 43;
                m = i / 10;
                n = i % 10;
                kc = ((n - 1) << 4) | (m + 8);
            }
            if (m < 1 || m > 8 || n == 0 || n > 5 || (m > 4 && n > 4) || (m == 3 && n == 1))
            {
                cpu.Message("KEYCODE ERR");
                cpu.Error();
                return;
            }
            cpu.Asn(alpha, (byte)kc);
        }
    }
}

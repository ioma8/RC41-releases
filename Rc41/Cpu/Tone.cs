using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Cpu
{
    public partial class Cpu
    {
        void Tone(byte n)
        {
            Number a;
            if (n >= 0x80 && n <= 0xef)
            {
                a = Rcl(n & 0x7f);
                if (a.sign != 9 && a.sign != 0)
                {
                    Message("ALPHA DATA");
                    Error();
                    return;
                }
                n = (byte)ToInteger(a);
            }
            else if (n >= 0xf0)
            {
                a = RecallNumber(n & 0x0f);
                n = (byte)ToInteger(a);
                sound.PlayTone(n);
            }
            sound.PlayTone(n);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        public void Command(byte function, int addr)
        {
            if (function == 1)
            {                           /* CREATE */
                td_create();
            }

            if (function == 2)
            {                           /* DIR */
                td_dir();
            }

            if (function == 3)
            {                           /* NEWM */
                if (addr < 1 || addr > 447)
                {
                    cpu.Message("DATA ERR");
                    cpu.Error();
                    return;
                }
                td_newm(addr + 1);
            }

            if (function == 4)
            {                           /* PURGE */
                td_purge();
            }

            if (function == 5)
            {                           /* READA */
                td_reada();
            }

            if (function == 6)
            {                           /* READK */
                td_readk();
            }

            if (function == 7)
            {                           /* READP */
                td_readp();
            }

            if (function == 8)
            {                           /* READR */
                td_readr();
            }
            if (function == 9)
            {                           /* READRX */
                td_readrx();
            }

            if (function == 10)
            {                          /* READS */
                td_reads();
            }

            if (function == 11)
            {                          /* READSUB */
                cpu.GtoEnd();
                td_readp();
            }

            if (function == 12)
            {                          /* RENAME */
                td_rename();
            }

            if (function == 13)
            {                          /* SEC */
                td_sec();
            }

            if (function == 14)
            {                          /* SEEKR */
                td_seekr();
            }

            if (function == 15)
            {                          /* UNSEC */
                td_unsec();
            }

            if (function == 16)
            {                          /* VERIFY */
                td_verify();
            }

            if (function == 17)
            {                          /* WRTA */
                td_wrta();
            }

            if (function == 18)
            {                          /* WRTK */
                td_wrtk();
            }

            if (function == 19)
            {                          /* WRTP */
                td_wrtp(0);
            }

            if (function == 20)
            {                          /* WRTPV */
                td_wrtp(1);
            }

            if (function == 21)
            {                          /* WRTR */
                td_wrtr();
            }

            if (function == 22)
            {                          /* WRTRX */
                td_wrtrx();
            }

            if (function == 23)
            {                          /* WRTS */
                td_wrts();
            }

            if (function == 24)
            {                          /* ZERO */
                td_zero();
            }

        }
    }
}

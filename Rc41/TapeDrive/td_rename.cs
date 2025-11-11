using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        void td_rename()
        {
            int i;
            int fp;
            int comma;
            string oldname;
            string newname;
            oldname = cpu.GetAlpha();
            if (oldname.Length == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            comma = oldname.IndexOf(',');
            if (comma < 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            if (comma == 0)
            {
                cpu.Message("NAME ERR");
                cpu.Error();
                return;
            }
            newname = oldname.Substring(comma + 1);
            oldname = oldname.Substring(0, comma - 1);
            if (newname.Length > 7) newname = newname.Substring(0, 7);
            if (oldname.Length > 7) oldname = oldname.Substring(0, 7);
            fp = FindFile(newname);
            if (fp >= 0)
            {
                cpu.Message("DUP FL NAME");
                cpu.Error();
                return;
            }
            fp = FindFile(oldname);
            if (fp < 0)
            {
                cpu.Message("FL NOT FOUND");
                cpu.Error();
                return;
            }
            if ((sector[fp + 9] & 1) != 0)
            {
                cpu.Message("FL SECURED");
                cpu.Error();
                return;
            }
            for (i = 1; i < 8; i++) sector[fp + i] = 0;
            for (i = 0; i < newname.Length; i++)
                sector[fp + 1 + i] = (byte)newname[i];
            WriteSector(sectorNumber);
        }
    }
}

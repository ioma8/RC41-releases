using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        public bool ShowNextDirEntry()
        {
            if (dirPos >= dir.Count)
            {
                var line = $"FREE: {dfree}/{sfree}";
                ui.Print(line, 'L');
                return false;
            }
            ui.Display(dir[dirPos].Substring(0, 12), false);
            ui.Print(dir[dirPos], 'L');
            cpu.SetAlpha(dir[dirPos++]);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_TapeDrive
{
    public partial class TapeDrive
    {
        public void OpenTapeDrive(string filename)
        {
            if (tapefile != null) tapefile.Close();
            tapefile = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (tapefile == null)
            {
                MessageBox.Show($"Could not open tape drive: {filename}");
                return;
            }
            ReadSector(0);
            file_rec = -1;
            file_regs = -1;
            file_pos = -1;
        }
    }
}

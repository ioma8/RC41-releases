using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rc41.T_Cpu;

namespace Rc41.T_CardReader
{
    public partial class CardReader
    {
        public void Rall(string filename)
        {
            Stream file;
            int len;
            byte[] card = new byte[5];
            file = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            if (file == null)
            {
                cpu.Message("CARD ERR");
                MessageBox.Show($"Could not open card file: {filename}");
                return;
            }
            len = Cpu.RAMTOP;
            len *= 7;
            file.Read(card, 0, 5);
            file.Read(cpu.ram, 5, len);
            file.Close();
        }
    }
}

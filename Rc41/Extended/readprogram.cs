using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rc41.T_Extended
{
    public partial class Extended
    {
        public List<byte> readProgram(int addr)
        {
            int len;
            List<byte> program = new List<byte>();
            addr--;
            len = (ram[addr * 7 + 3] << 8) | ram[addr * 7 + 2];
            addr--;
            addr = addr * 7 + 6;
            while (len > 0)
            {
                program.Add(ram[addr--]);
                len--;
            }
            return program;
        }
    }
}

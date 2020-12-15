using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearScriptAppStudy.ScriptObjects
{
    public class Console
    {
        private readonly Action<string> writeLineAction;

        public Console(Action<string> writeLineAction)
        {
            this.writeLineAction = writeLineAction;
        }

        public void WriteLine(string format)
        {
            writeLineAction(format);
        }
    }
}

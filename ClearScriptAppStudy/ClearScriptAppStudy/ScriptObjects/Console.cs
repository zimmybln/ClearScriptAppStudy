using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearScriptAppStudy.Services;

namespace ClearScriptAppStudy.ScriptObjects
{
    public class Console
    {
        private readonly Action<string, OutputTypes> writeLineAction;

        public Console(Action<string, OutputTypes> writeLineAction)
        {
            this.writeLineAction = writeLineAction;
        }

        public void WriteInfo(string format)
        {
            writeLineAction(format, OutputTypes.Info);
        }

        public void WriteWarning(string format)
        {
            writeLineAction(format, OutputTypes.Warning);
        }

        public void WriteError(string format)
        {
            writeLineAction(format, OutputTypes.Error);
        }
    }
}

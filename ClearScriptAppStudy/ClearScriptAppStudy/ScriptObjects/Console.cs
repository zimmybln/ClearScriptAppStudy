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

        public void log(params object[] args)
        {
            StringBuilder builder = new StringBuilder();

            foreach(object arg in args)
            {
                if (arg == null)
                    continue;

                if (arg.GetType().IsValueType || (arg is String))
                {
                    builder.Append(arg.ToString());
                }
                else
                {
                    builder.Append(GetDescription(arg));
                }
            }

            writeLineAction(builder.ToString(), OutputTypes.Info);
        }

        private string GetDescription(object item)
        {
            StringBuilder builder = new StringBuilder();

            foreach(var property in item.GetType().GetProperties())
            {
                if (!property.CanRead)
                    continue;

                if (builder.Length > 0)
                    builder.Append(", ");

                try
                {
                    builder.AppendFormat("{0}: {1}", property.Name, property.GetValue(item));
                }
                catch (Exception ex)
                {
                                        
                }
            }



            return builder.ToString();
        }
    }
}

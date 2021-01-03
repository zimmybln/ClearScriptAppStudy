using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearScriptAppStudy.Services
{
    public interface IFieldMethods
    {
        Task OnFieldGotFocus(object fieldInstance, string propertyName);

        Task OnFieldLostFocus(object fieldInstance, string propertyName);
    }
}

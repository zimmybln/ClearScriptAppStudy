using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearScriptAppStudy.Types;

namespace ClearScriptAppStudy.Services
{
    public interface IPersonMethods
    {
        Task OnNewPerson(Person person);
    }
}

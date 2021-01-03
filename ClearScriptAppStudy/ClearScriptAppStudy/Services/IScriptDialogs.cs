using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearScriptAppStudy.Types;

namespace ClearScriptAppStudy.Services
{
    public interface IScriptDialogs
    {
        ObservableCollection<OutputLine> Outputs { get; }
        
        ApplicationScript Script { get; set; }

        void ShowScriptDialog();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ClearScriptAppStudy.Dialogs
{
    public abstract class ScriptableDialog : BindableBase, IDialogAware
    {
        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            
        }

        void  IDialogAware.OnDialogOpened(IDialogParameters parameters)
        {
            this.OnDialogOpened(parameters);
            
            // run script here
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }

        public string Title { get; protected set; }
        public event Action<IDialogResult> RequestClose;


        protected void RaiseRequestClose(IDialogResult result)
        {
            RequestClose?.Invoke(result);
        }
    }
}

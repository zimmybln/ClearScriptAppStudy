using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClearScriptAppStudy.Components.Behaviors
{
    public class EventAction
    {
        private bool isActivated = false;
        
        
        public virtual void Activated()
        {
            if (!isActivated)
            {
                isActivated = true;
                FirstTimeActivated();
            }
        }

        public virtual void FirstTimeActivated()
        {
            FirstTimeActivatedAction?.Invoke();
        }
        
        public virtual void GotFocus(UIElement element)
        {

        }

        public virtual void LostFocus(UIElement element)
        {

        }

        public virtual void Loaded(Window window)
        {

        }
        
        public Action FirstTimeActivatedAction { get; set; }
        
        
    }
}

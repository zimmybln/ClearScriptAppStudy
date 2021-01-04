using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using ClearScriptAppStudy.Components;
using ClearScriptAppStudy.Components.Behaviors;
using ClearScriptAppStudy.Services;
using ClearScriptAppStudy.Types;

namespace ClearScriptAppStudy.ViewModels
{
    public class GotFocusToScriptAction<T> : EventAction
        where T: class
    {
        private readonly IFieldMethods scriptService;

        public GotFocusToScriptAction(IFieldMethods scriptService)
        {
            this.scriptService = scriptService;
        }
        
        public override async void GotFocus(UIElement element)
        {
            object dataContext = null;
            string propertyName = null;

            if (DataHelper.TryToGetBindingProperties(element, ref dataContext, ref propertyName))
            {
                T typedContext = dataContext as T;
                await scriptService.OnFieldGotFocus(typedContext, propertyName);
            }
        }

        public override async void LostFocus(UIElement element)
        {
            object dataContext = null;
            string propertyName = null;

            if (DataHelper.TryToGetBindingProperties(element, ref dataContext, ref propertyName))
            {
                T typedContext = dataContext as T;
                await scriptService.OnFieldLostFocus(typedContext, propertyName);
            }
        }

 
    }
}

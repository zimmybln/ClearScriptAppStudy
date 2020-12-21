using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using ClearScriptAppStudy.Components.Behaviors;
using ClearScriptAppStudy.Services;
using ClearScriptAppStudy.Types;

namespace ClearScriptAppStudy.ViewModels
{
    public class GotFocusToScriptAction : EventAction
    {
        private ScriptService scriptService;

        public GotFocusToScriptAction(ScriptService scriptService)
        {
            this.scriptService = scriptService;
        }
        
        public override async void GotFocus(UIElement element)
        {
            if (element is TextBox textBox)
            {
                var textBinding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);

                if (textBinding != null)
                {
                    object dataObject = textBox.DataContext;
                    string bindingPath = textBinding.Path.Path;

                    if (dataObject == null || string.IsNullOrEmpty(bindingPath))
                        return;

                    var pathSegments = bindingPath.Split(".");

                    for (int index = 0; index < pathSegments.Length; index++)
                    {
                        var propertyName = pathSegments[index];
                        
                        if (index == pathSegments.Length - 1) // das letzte benötigen wir
                        {
                            // Script aufrufen: object + propertyName
                            await scriptService.OnFieldGotFocus(dataObject as Person, propertyName);
                        }
                        else
                        {
                            var property = dataObject.GetType().GetProperty(propertyName);

                            if (property == null)
                                break;

                            dataObject = property.GetValue(dataObject);
                        }
                    }




                }
            }
        }
    }
}

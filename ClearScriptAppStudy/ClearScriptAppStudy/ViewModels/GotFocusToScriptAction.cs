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
        private readonly ScriptService scriptService;

        public GotFocusToScriptAction(ScriptService scriptService)
        {
            this.scriptService = scriptService;
        }
        
        public override async void GotFocus(UIElement element)
        {
            object dataContext = null;
            string propertyName = null;

            if (TryToGetBindingProperties(element, ref dataContext, ref propertyName))
            {
                T typedContext = dataContext as T;
                await scriptService.OnFieldGotFocus(typedContext, propertyName);
            }
        }

        private static bool TryToGetBindingProperties(UIElement element, ref object dataContext, ref string propertyName)
        {
            if (element is TextBox textBox)
            {
                var textBinding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);

                if (textBinding != null)
                {
                    object dataObject = textBox.DataContext;
                    string bindingPath = textBinding.Path.Path;

                    if (dataObject == null || string.IsNullOrEmpty(bindingPath))
                        return false;

                    var pathSegments = bindingPath.Split(".");

                    for (int index = 0; index < pathSegments.Length; index++)
                    {
                        var segmentName = pathSegments[index];

                        if (index == pathSegments.Length - 1) // das letzte benötigen wir
                        {
                            dataContext = dataObject;
                            propertyName = segmentName;
                            return true;
                        }
                        else
                        {
                            var property = dataObject.GetType().GetProperty(segmentName);

                            if (property == null)
                                break;

                            dataObject = property.GetValue(dataObject);

                            if (dataObject == null)
                                break;
                        }
                    }
                }
            }


            return false;
        }
    }
}

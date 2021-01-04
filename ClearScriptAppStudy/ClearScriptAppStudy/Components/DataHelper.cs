using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ClearScriptAppStudy.Components
{
    public static class DataHelper
    {
        public static bool TryToGetBindingProperties(UIElement element, ref object dataContext, ref string propertyName)
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

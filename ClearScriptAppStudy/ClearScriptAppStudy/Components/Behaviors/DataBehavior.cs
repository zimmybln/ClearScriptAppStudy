using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClearScriptAppStudy.Components.Behaviors
{
    public static class DataBehavior
    {
        public static DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(DataBehavior), new PropertyMetadata(OnIsFocusedChanged));


        public static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue && element.Focusable)
                {
                    element.Focus();
                }
            }
        }

        public static bool GetIsFocused(DependencyObject item)
        {
            return (bool)item.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject item, bool isFocused)
        {
            item.SetValue(IsFocusedProperty, isFocused);
        }

    }
}

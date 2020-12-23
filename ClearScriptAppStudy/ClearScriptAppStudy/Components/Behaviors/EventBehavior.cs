using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClearScriptAppStudy.Components.Behaviors
{
    public static class EventBehavior
    {

        #region GotFocus
        
        public static DependencyProperty GotFocusProperty =
            DependencyProperty.RegisterAttached("GotFocus", typeof(EventAction), typeof(EventBehavior), new PropertyMetadata(null, OnGotFocusChanged));

        private static void OnGotFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.OldValue != null)
                {
                    element.GotFocus -= OnGotFocus;
                }

                if (e.NewValue != null)
                {
                    element.GotFocus += OnGotFocus;
                }
            }
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element && element.GetValue(GotFocusProperty) is EventAction action)
            {
                action.GotFocus(element);
            }
        }

        public static EventAction GetGotFocus(DependencyObject item)
        {
            return item.GetValue(GotFocusProperty) as EventAction;
        }

        public static void SetGotFocus(DependencyObject item, EventAction action)
        {
            item.SetValue(GotFocusProperty, action);
        }

        #endregion

        #region LostFocus
        
        public static DependencyProperty LostFocusProperty =
            DependencyProperty.RegisterAttached("LostFocus", typeof(EventAction), typeof(EventBehavior), new PropertyMetadata(null, OnLostFocusChanged));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLostFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.OldValue != null)
                {
                    element.LostFocus -= OnLostFocus;
                }

                if (e.NewValue != null)
                {
                    element.LostFocus += OnLostFocus;
                }
            }
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element && element.GetValue(LostFocusProperty) is EventAction action)
            {
                action.LostFocus(element);
            }
        }

        public static EventAction GetLostFocus(DependencyObject item)
        {
            return item.GetValue(LostFocusProperty) as EventAction;
        }

        public static void SetLostFocus(DependencyObject item, EventAction action)
        {
            item.SetValue(LostFocusProperty, action);
        }

#endregion

    }
}

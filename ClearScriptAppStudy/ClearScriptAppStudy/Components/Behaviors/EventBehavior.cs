using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        #region Loaded

        public static DependencyProperty LoadedProperty =
            DependencyProperty.RegisterAttached("Loaded", typeof(EventAction), typeof(EventBehavior),
                new PropertyMetadata(null, OnLoadedChanged));

        private static void OnLoadedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.OldValue != null)
                {
                    element.Loaded -= OnLoaded;
                }

                if (e.NewValue != null)
                {
                    element.Loaded += OnLoaded;
                }
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is DependencyObject item)
            {
                (item.GetValue(LoadedProperty) as EventAction)?.Loaded(item as Window);

            }
        }

        public static EventAction GetLoaded(DependencyObject item)
        {
            return item.GetValue(LoadedProperty) as EventAction;
        }

        public static void SetLoaded(DependencyObject item, EventAction action)
        {
            item.SetValue(LoadedProperty, action);
        }

        #endregion

        #region Activated

        public static DependencyProperty ActivatedProperty = 
            DependencyProperty.RegisterAttached("Activated", typeof(EventAction), typeof(EventBehavior), new PropertyMetadata(null, OnActivatedChanged));

        private static void OnActivatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window element)
            {
                if (e.OldValue != null)
                {
                    element.Activated -= OnActivated;
                }

                if (e.NewValue != null)
                {
                    element.Activated += OnActivated;
                }
            }
        }

        public static EventAction GetActivated(DependencyObject item)
        {
            return item.GetValue(ActivatedProperty) as EventAction;
        }

        public static void SetActivated(DependencyObject item, EventAction action)
        {
            item.SetValue(ActivatedProperty, action);
        }

        private static void OnActivated(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                (window.GetValue(ActivatedProperty) as EventAction)?.Activated();
            }
            
        }

        #endregion
    }
}

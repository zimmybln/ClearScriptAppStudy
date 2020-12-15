using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace ClearScriptAppStudy.Components
{
    public class DataContextConnector : MarkupExtension
    {
        public Type Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
                       
            IProvideValueTarget service = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            DependencyObject targetObject = service.TargetObject as DependencyObject;
                        
            if (DesignerProperties.GetIsInDesignMode(targetObject))
                return null;

            return (App.Current as PrismApplication).Container.Resolve(Type);
        }
    }
}

using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClearScriptAppStudy.Dialogs
{
    /// <summary>
    /// Interaction logic for ScriptDialogView.xaml
    /// </summary>
    public partial class ScriptDialogView : UserControl
    {
        public ScriptDialogView()
        {
            InitializeComponent();


            textEditor.DocumentChanged += OnDocumentChanged;
        }

        private void OnDocumentChanged(object sender, EventArgs e)
        {
            var foldingManager = FoldingManager.Install(textEditor.TextArea);
            var foldingStrategy = new JavaScriptFolderStrategy();



            foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);

        }
    }
}

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
using System.Windows.Threading;

namespace ClearScriptAppStudy.Dialogs
{
    /// <summary>
    /// Interaction logic for ScriptDialogView.xaml
    /// </summary>
    public partial class ScriptDialogView : UserControl
    {
        JavaScriptFolderStrategy foldingStrategy;
        FoldingManager foldingManager;


        public ScriptDialogView()
        {
            InitializeComponent();


            textEditor.DocumentChanged += OnDocumentChanged;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += delegate {
                if (foldingManager != null && textEditor.Document != null)
                {
                    foldingStrategy?.UpdateFoldings(foldingManager, textEditor.Document);
                }
            };
            foldingUpdateTimer.Start();
        }

        private void OnDocumentChanged(object sender, EventArgs e)
        {
            foldingManager = FoldingManager.Install(textEditor.TextArea);

            foldingStrategy = new JavaScriptFolderStrategy();

            foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
        }
    }
}

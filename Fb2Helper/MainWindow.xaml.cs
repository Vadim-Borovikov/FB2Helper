using System.Windows;
using System.Xml.Linq;
using Fb2Helper.Logic;
using Microsoft.Win32;

namespace Fb2Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "FB2|*.fb2"
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            XDocument fb2 = XDocument.Load(openFileDialog.FileName);

            DataManager.Process(fb2);

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "FB2|*.fb2"
            };
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            fb2.Save(saveFileDialog.FileName);
        }
    }
}

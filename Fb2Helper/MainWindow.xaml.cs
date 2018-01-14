using System;
using System.Collections.Generic;
using System.Windows;
using Fb2Helper.Logic;
using Microsoft.Win32;

namespace Fb2Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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

            string content = DataManager.Process(openFileDialog.FileName);

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "FB2|*.fb2"
            };
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            DataManager.Save(saveFileDialog.FileName, content);
        }
    }
}

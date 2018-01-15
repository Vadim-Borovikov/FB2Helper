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

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "FB2|*.fb2"
            };
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            _fb2 = XDocument.Load(openFileDialog.FileName);
            _description = _fb2.GetDescription();

            BookAuthorFirstName.Text = _description.BookAuthorFirstName;
            BookAuthorFamilyName.Text = _description.BookAuthorFamilyName;
            BookTitle.Text = _description.Title;
            FileAuthorFirstName.Text = _description.FileAuthorFirstName;
            FileAuthorFamilyName.Text = _description.FileAuthorFamilyName;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _description.Fill(BookAuthorFirstName.Text, BookAuthorFamilyName.Text, BookTitle.Text,
                              FileAuthorFirstName.Text, FileAuthorFamilyName.Text, Title);
            _fb2.Process(_description);

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "FB2|*.fb2"
            };
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            _fb2.Save(saveFileDialog.FileName);
        }

        private XDocument _fb2;
        private BookDescription _description;
    }
}

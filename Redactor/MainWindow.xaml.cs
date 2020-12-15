using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Redactor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlowDocument flowDocument = new FlowDocument();

        string path = @"C:\temp.txt";

        public MainWindow()
        {
            InitializeComponent();

            FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);

            textRange.Load(fileStream, DataFormats.Text);

            richTextBox.Document = flowDocument;

            TimerCallback tm = new TimerCallback(AutoSave);
            var timer = new Timer(tm, null, 0, 10000);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Format (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                var range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Text);
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Format (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() ?? true)
            {
                var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                var range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Text);
                richTextBox.Document = flowDocument;
            }
        }

        private void RichTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        public void AutoSave(object text)
        {
            flowDocument = (FlowDocument)text;
            
            using (StreamWriter writer = new StreamWriter("temp.txt", false))
            {
                writer.WriteLine(flowDocument);
            }
        }
    }
}
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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace TechnicalDocumentation.Windows
{
    /// <summary>
    /// Логика взаимодействия для wReport1.xaml
    /// </summary>
    public partial class wReport1 : Window
    {
        BDEntities db;
        public wReport1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            DateTime date = DateTime.Now.AddDays(D.dataD3);
            dgogrenci.ItemsSource = db.Documents.Where(item => item.Validity_period >= DateTime.Now && item.Validity_period < date).ToList();
            CountDaysTxt.Text = "Истекающие документы за " + D.dataD3 + " дней к " + date.ToString("d");
        }

        private void FileClick(object sender, RoutedEventArgs e)
        {
            Document u = dgogrenci.SelectedItem as Document;

            if (u.Doc_file != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(u.Doc_file);
                }
                catch
                {
                    MessageBox.Show("Error: файл не найден");
                }
            }
        }

        private void SaveDoc_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".doc"; // Default file extension
            dlg.Filter = "Text documents (.doc)|*.doc"; // Filter files by extension
            
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllText(dlg.FileName, "1234");
                File.AppendAllText(dlg.FileName, "123456");
            }

        }
    }
}

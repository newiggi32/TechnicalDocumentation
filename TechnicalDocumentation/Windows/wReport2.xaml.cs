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

namespace TechnicalDocumentation.Windows
{
    /// <summary>
    /// Логика взаимодействия для wReport2.xaml
    /// </summary>
    public partial class wReport2 : Window
    {
        BDEntities db;
        public wReport2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Documents.Where(item => item.Validity_period < DateTime.Now).ToList();
            CountDaysTxt.Text = "Истекшие документы к " + DateTime.Now.ToString("d");
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

        }
    }
}

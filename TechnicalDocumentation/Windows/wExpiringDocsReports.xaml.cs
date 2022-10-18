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
    public partial class wExpiringDocsReports : Window
    {
        public wExpiringDocsReports()
        {
            InitializeComponent();
        }

        private void Month_Click(object sender, RoutedEventArgs e)
        {
            D.dataD3 = 30;
            new wReport1().ShowDialog();          
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (CountDayTxt.Text != "")
            {
                D.dataD3 = Convert.ToInt32(CountDayTxt.Text);
                new wReport1().ShowDialog();
            }
            else
                MessageBox.Show("Error: Введите количество дней");
        }
    }
}

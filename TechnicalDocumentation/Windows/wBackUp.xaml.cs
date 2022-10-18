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
using System.Data.SqlClient;
using System.IO;

namespace TechnicalDocumentation.Windows
{
    public partial class wBackUp : Window
    {
        public wBackUp()
        {
            InitializeComponent();
        }

        private static void CreateBackup(string connectionString, string databaseName, string backupFilePath)
        {
            string backupCommand = "BACKUP DATABASE @BD TO DISK = @backupFilePath";
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(backupCommand, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@BD", databaseName);
                cmd.Parameters.AddWithValue("@backupFilePath", backupFilePath);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void CreateBackUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtWayDirectory.Text != "")
                {
                    string date = DateTime.Now.ToString().Replace(":", ".").Replace(" ", "_");
                    string pathBackup = txtWayDirectory.Text + "\\DB_" + date + ".bak";
                    CreateBackup(@"Data Source = DESKTOP-L0OVD47;Initial Catalog=BD;Integrated Security=True", //меняется при переносе на другой комп
                       "BD",
                       pathBackup);
                    if (File.Exists(pathBackup) == true)
                        MessageBox.Show("Успешно");
                    else
                        MessageBox.Show("Error");
                }
                else
                    MessageBox.Show("Введите путь к папке");
            }
            catch
            {
                MessageBox.Show("Error: путь не найден");
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = @"Db Files|*.bak";
            dialog.InitialDirectory = @"c:\";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                filename = dialog.FileName;
                txtWayFile.Text = filename;
            }
        }

        private void UseBackUp_Click(object sender, RoutedEventArgs e)
        {
            if (txtWayFile.Text != "")
            {
                string connStr = @"Data Source = DESKTOP-L0OVD47;Initial Catalog=BD;Integrated Security=True";
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                string sql = "USE [master] ALTER DATABASE[BD] SET SINGLE_USER WITH ROLLBACK IMMEDIATE RESTORE DATABASE[BD] FROM DISK = N'" + txtWayFile.Text + "' WITH FILE = 1, NOUNLOAD, STATS = 5 ALTER DATABASE[BD] SET MULTI_USER";
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteScalar();
                conn.Close();
                MessageBox.Show("Успешно");
            }
            else
                MessageBox.Show("Файл не выбран");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtWayDirectory.Text = "C:\\Windows\\Temp";
        }
    }
}

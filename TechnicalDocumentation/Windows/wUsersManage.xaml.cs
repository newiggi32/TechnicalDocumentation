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
    public partial class wUsersManage : Window
    {
        BDEntities db;
        public wUsersManage()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Users.ToList();                    
        }
        
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {            
            new wUserAdd().ShowDialog();
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Users.ToList();
        }

        private void dgogrenci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Удалить запись?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    db.Users.Remove((User)dgogrenci.SelectedItem);
                    db.SaveChanges();                    
                    dgogrenci.ItemsSource = db.Users.ToList();
                }
            }                
        }

        private void dgogrenci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgogrenci.SelectedItem != null)
            {
                User u = dgogrenci.SelectedItem as User;
                D.dataD1 = u.Id.ToString();
                new wUserEdit().ShowDialog();

                db = new BDEntities();
                dgogrenci.ItemsSource = db.Users.ToList();
            }     
        }
    }
}

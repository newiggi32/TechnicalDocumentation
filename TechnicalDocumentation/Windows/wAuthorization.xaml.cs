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
    static class D
    {
        public static string dataD1;
        public static List<int> dataD2 = new List<int>();
        public static int dataD3;
    }

    public partial class wAuthorization : Window
    {
        BDEntities db;
        public wAuthorization()
        {            
            InitializeComponent();
            db = new BDEntities();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text != "" && PasswordBox.Password != "")
            {
                foreach (User item in db.Users)
                {
                    if (item.Login == LoginBox.Text && item.Password == PasswordBox.Password)
                    {
                        if (item.Role_id == 1)
                        {
                            new wAdmin().Show();
                            this.Close();
                            break;
                        }
                        else
                        {
                            new wUser().Show();
                            this.Close();
                            break;
                        }
                    }
                    else if(item.Login == LoginBox.Text)
                    {                        
                        this.PasswordBox.Foreground = Brushes.Red;
                    }
                    else if (item.Password == PasswordBox.Password)
                    {
                        this.LoginBox.Foreground = Brushes.Red;
                    }
                    else
                    {
                        this.LoginBox.Foreground = Brushes.Red;
                        this.PasswordBox.Foreground = Brushes.Red;
                    }
                }
            }
            else
            {                
                this.LoginBox.Foreground = Brushes.Red;
                this.PasswordBox.Foreground = Brushes.Red;
            }                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Document item in db.Documents)
            {
                if (item.Validity_period <= DateTime.Today)
                {
                    item.Relevance = false;
                }
            }
        }
    }
}

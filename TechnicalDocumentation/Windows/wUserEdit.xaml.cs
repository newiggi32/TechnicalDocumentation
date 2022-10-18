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
using System.Text.RegularExpressions;

namespace TechnicalDocumentation.Windows
{
    /// <summary>
    /// Логика взаимодействия для wUserEdit.xaml
    /// </summary>
    public partial class wUserEdit : Window
    {
        BDEntities db;
        public wUserEdit()
        {
            InitializeComponent();
            db = new BDEntities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RoleCombo.ItemsSource = db.Roles.ToList();
            int i = Convert.ToInt32(D.dataD1);
            User u = db.Users.FirstOrDefault(item => item.Id == i);

            LnameTxt.Text = u.Lname;
            FnameTxt.Text = u.Fname;
            PnameTxt.Text = u.Pname;
            LoginTxt.Text = u.Login;
            PasswordTxt.Text = u.Password;
            RoleCombo.Text = u.Role.Name;
        }
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(D.dataD1);
                var user = db.Users.Where(w => w.Id == i).FirstOrDefault();

                var regexWords = new Regex("^[a-zA-Zа-яА-я]*$");
                if (LnameTxt.Text != "" && regexWords.IsMatch(LnameTxt.Text) == true)
                {
                    user.Lname = LnameTxt.Text;
                    if (FnameTxt.Text != "" && regexWords.IsMatch(FnameTxt.Text) == true)
                    {
                        user.Fname = FnameTxt.Text;
                        if (PnameTxt.Text != "" && regexWords.IsMatch(PnameTxt.Text) == true)
                        {
                            user.Pname = PnameTxt.Text;
                            if (LoginTxt.Text != "")
                            {
                                var checklogin = db.Users.FirstOrDefault(w => w.Login == LoginTxt.Text);
                                if (checklogin == null || user.Login == LoginTxt.Text)
                                {
                                    user.Login = LoginTxt.Text;
                                    if (PasswordTxt.Text != "")
                                    {
                                        user.Password = PasswordTxt.Text;
                                        if (RoleCombo.Text != "")
                                        {
                                            user.Role_id = (RoleCombo.SelectedItem as Role).Id;
                                            db.SaveChanges();
                                            this.Close();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Логин сущесвтует");
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Некорректные данные");
            }
        }
        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}

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
    public class WhoKnowList
    {
        public int Departmen_id { get; set; }
        public WhoKnowList(int _Departmen_id)
        {
            Departmen_id = _Departmen_id;
        }
    }

    public class DepartmentExtra1
    {
        public int Idd { get; set; }
        public string Namee { get; set; }
        public DepartmentExtra1(int Id, string Name)
        {
            Idd = Id;
            Namee = Name;
        }
    }

    public class DepartmentExtra
    {
        public int Idd { get; set; }
        public string Namee { get; set; }
        public DepartmentExtra(int Id, string Name)
        {
            Idd = Id;
            Namee = Name;
        }
    }

    public partial class wWhoKnow : Window
    {
        BDEntities db;
        public wWhoKnow()
        {
            InitializeComponent();
        }

        public List<WhoKnowList> whoKnowLists = new List<WhoKnowList>();
        public List<DepartmentExtra> _DepartmentExtra = new List<DepartmentExtra>();
        public List<DepartmentExtra1> _DepartmentExtra1 = new List<DepartmentExtra1>();

        private void Window_Loaded(object sender, RoutedEventArgs e) //закончено
        {
            db = new BDEntities();
            if (D.dataD1 != null)
            {
                foreach (Who_know item in db.Who_know.ToList())
                {
                    if (item.Document_id == Convert.ToInt32(D.dataD1))
                    {
                        whoKnowLists.Add(new WhoKnowList(item.Departmen_id));
                    }
                }
            }

            if (whoKnowLists.Count != 0)
            {
                foreach (Department item in db.Departments.ToList())
                {
                    if (whoKnowLists.Select(itemm => itemm.Departmen_id).Contains(item.Id))
                    {
                        _DepartmentExtra1.Add(new DepartmentExtra1(item.Id, item.Name));
                    }
                    else
                    {
                        _DepartmentExtra.Add(new DepartmentExtra(item.Id, item.Name));
                    }
                }
                dgogrenci.ItemsSource = _DepartmentExtra;
                dgogrenci_1.ItemsSource = _DepartmentExtra1;
            }
            else
            {
                foreach (Department item in db.Departments.ToList())
                {
                    _DepartmentExtra.Add(new DepartmentExtra(item.Id, item.Name));
                }
                dgogrenci.ItemsSource = _DepartmentExtra;
            }
        }

        private void PuchToWhoKnow_Click(object sender, RoutedEventArgs e)
        {
            var selctedRow = (DepartmentExtra)dgogrenci.SelectedItem;

            if (selctedRow != null)
            {
                foreach (DepartmentExtra item in _DepartmentExtra.ToList())
                {
                    if (item.Idd == selctedRow.Idd)
                    {
                        _DepartmentExtra1.Add(new DepartmentExtra1(item.Idd, item.Namee));
                    }

                }
                _DepartmentExtra.Remove(selctedRow);
                dgogrenci.ItemsSource = null; //костыль
                dgogrenci_1.ItemsSource = null;
                dgogrenci.ItemsSource = _DepartmentExtra;
                dgogrenci_1.ItemsSource = _DepartmentExtra1;
            }
        }

        private void PuchBack_Click(object sender, RoutedEventArgs e)
        {
            var selctedRow = (DepartmentExtra1)dgogrenci_1.SelectedItem;

            if (selctedRow != null)
            {
                foreach (DepartmentExtra1 item in _DepartmentExtra1.ToList())
                {
                    if (item.Idd == selctedRow.Idd)
                    {
                        _DepartmentExtra.Add(new DepartmentExtra(item.Idd, item.Namee));
                    }
                }
                _DepartmentExtra1.Remove(selctedRow);
                dgogrenci.ItemsSource = null;
                dgogrenci_1.ItemsSource = null;
                dgogrenci.ItemsSource = _DepartmentExtra;
                dgogrenci_1.ItemsSource = _DepartmentExtra1;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (DepartmentExtra1 item in _DepartmentExtra1.ToList())
            {
                D.dataD2.Add(item.Idd);
            }
            this.Close();
        }
    }
}

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
    public partial class wDocumentEdit : Window
    {
        BDEntities db;
        public wDocumentEdit()
        {
            InitializeComponent();
        }

        private void WhoKnowEdit_Click(object sender, RoutedEventArgs e) //маленький косяк с загрузкой
        {
            D.dataD2.Clear();
            new wWhoKnow().ShowDialog();

            string str = "";
            for (int i = 0; i < D.dataD2.Count(); i++)
            {
                int j = D.dataD2[i];
                var d = db.Departments.Where(w => w.Id == j).FirstOrDefault();
                str = string.Concat(str, d.Name + ", ");
            }
            if (str.Length > 3)
                str = str.Remove(str.Length - 2);
            WhoKnowTxt.Text = str;
        }

        private void EditFile_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = @"All Files|*.txt;*.docx;*.doc;*.pdf*.xls;*.xlsx;*.pptx;*.ppt|Image Files|*.jpg;*.jpeg;*.png|Text Files (.txt)|*.txt|Word File (.docx ,.doc)|*.docx;*.doc|PDF (.pdf)|*.pdf|Spreadsheet (.xls ,.xlsx)| *.xls ;*.xlsx|Presentation (.pptx ,.ppt)|*.pptx;*.ppt";
            dialog.InitialDirectory = @"c:\";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                filename = dialog.FileName;
                FileTxt.Text = filename;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int dataDId = Convert.ToInt32(D.dataD1);
                var uRow = db.Documents.Where(w => w.Id == dataDId).FirstOrDefault();

                if (IndexTxt.Text != null || IndexTxt.Text == uRow.Index)
                {

                    uRow.Index = IndexTxt.Text;
                    if (NameDocTxt != null)
                    {
                        uRow.Name = NameDocTxt.Text;
                        if (NumberOrderTxt != null)
                        {
                            uRow.Number_order = NumberOrderTxt.Text;
                            if (DateOrderPicker.Text != null && ValidityPeriodPicker != null)
                            {
                                if (DateOrderPicker.SelectedDate < ValidityPeriodPicker.SelectedDate)
                                {
                                    uRow.Relevance = true;
                                    uRow.Date_order = DateOrderPicker.SelectedDate.Value;
                                    uRow.Validity_period = ValidityPeriodPicker.SelectedDate.Value;
                                    uRow.Doc_file = FileTxt.Text;
                                    uRow.Save_place_id = (SavePlaceCombo.SelectedItem as Save_place).Id;
                                    uRow.Doc_tipe_id = (TipeDocCombo.SelectedItem as Doc_tipe).Id;
                                    uRow.Developer_id = (DeveloperCombo.SelectedItem as Department).Id;
                                    db.SaveChanges();

                                    //whoknow                                
                                    foreach (Who_know item in db.Who_know.ToList())
                                    {
                                        if (item.Document_id == Convert.ToInt32(D.dataD1))
                                        {
                                            db.Who_know.Remove(item);
                                        }
                                    }
                                    Who_know who = new Who_know();
                                    for (int i = 0; i < D.dataD2.Count(); i++)
                                    {
                                        int DepItem = D.dataD2[i];
                                        who.Departmen_id = DepItem;
                                        who.Document_id = Convert.ToInt32(D.dataD1);
                                        db.Who_know.Add(who);
                                        db.SaveChanges();
                                    }
                                    this.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error: некоректные данные");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            int dataDId = Convert.ToInt32(D.dataD1);
            var docData = db.Documents.Where(w => w.Id == dataDId).FirstOrDefault();

            IndexTxt.Text = docData.Index;
            NumberOrderTxt.Text = docData.Number_order;
            NameDocTxt.Text = docData.Name;
            FileTxt.Text = docData.Doc_file;
            RelevanceCheck.IsChecked = docData.Relevance;

            string who = "";
            foreach (Who_know item in db.Who_know.ToList())
            {
                if (D.dataD1 == item.Document_id.ToString())
                {
                    who = string.Concat(who, item.Department.Name + ", ");
                }
            }
            if (who.Length > 3)
                who = who.Remove(who.Length - 2);

            WhoKnowTxt.Text = who;

            DateOrderPicker.Text = docData.Date_order.ToString();
            ValidityPeriodPicker.Text = docData.Validity_period.ToString();
            SavePlaceCombo.ItemsSource = db.Save_place.ToList();
            SavePlaceCombo.SelectedIndex = docData.Save_place_id - 1;
            TipeDocCombo.ItemsSource = db.Doc_tipe.ToList();
            TipeDocCombo.SelectedIndex = docData.Doc_tipe_id - 1;
            DeveloperCombo.ItemsSource = db.Departments.ToList();
            DeveloperCombo.SelectedIndex = docData.Developer_id - 1;
        }
    }
}

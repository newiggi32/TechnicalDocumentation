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

namespace TechnicalDocumentation.Windows
{
    public class who_know
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public who_know(int Id, string Name)
        {
            RoleId = Id;
            RoleName = Name;
        }
    }
    
    public partial class wDocumentAdd : Window
    {
        BDEntities db;
        SqlConnection sqlConnection = new SqlConnection(@"Data Source = DESKTOP-HSHULSK\SQL;Initial Catalog=BD;Integrated Security=True"); //менять при установке
        public wDocumentAdd()
        {
            InitializeComponent();
        }

        private void WhoKnowEdit_Click(object sender, RoutedEventArgs e)
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

        private int GetMaxId() //получение Id нового документа
        {
            sqlConnection.Open();
            string querty = $"select max(Id) from Documents";
            SqlCommand command = new SqlCommand(querty, sqlConnection);
            object result = command.ExecuteScalar();
            int LastDocId = Convert.ToInt32(result);
            sqlConnection.Close();
            return LastDocId;
        }

        private void AddDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Document doc = new Document();
                var checkindex = db.Documents.FirstOrDefault(w => w.Index == IndexTxt.Text);
                if (IndexTxt.Text != null && checkindex == null)
                {
                    doc.Index = IndexTxt.Text;
                    if (NameDocTxt != null)
                    {
                        doc.Name = NameDocTxt.Text;
                        if (NumberOrderTxt != null)
                        {
                            doc.Number_order = NumberOrderTxt.Text;
                            if (DateOrderPicker.Text != null && ValidityPeriodPicker != null)
                            {
                                if (DateOrderPicker.SelectedDate < ValidityPeriodPicker.SelectedDate)
                                {
                                    doc.Date_order = DateOrderPicker.SelectedDate.Value;
                                    doc.Validity_period = ValidityPeriodPicker.SelectedDate.Value;
                                    if (DeveloperCombo != null && SavePlaceCombo != null && TipeDocCombo != null)
                                    {
                                        doc.Save_place_id = (SavePlaceCombo.SelectedItem as Save_place).Id;
                                        doc.Doc_tipe_id = (TipeDocCombo.SelectedItem as Doc_tipe).Id;
                                        doc.Developer_id = (DeveloperCombo.SelectedItem as Department).Id;
                                        doc.Relevance = true;
                                        doc.Doc_file = FileTxt.Text;
                                        db.Documents.Add(doc);
                                        db.SaveChanges();

                                        //whoknow                                

                                        Who_know who = new Who_know();
                                        for (int i = 0; i < D.dataD2.Count(); i++)
                                        {
                                            int DepItem = D.dataD2[i];
                                            who.Departmen_id = DepItem;
                                            who.Document_id = GetMaxId();
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
            }
            catch
            {
                MessageBox.Show("Error: некоректные данные");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            TipeDocCombo.ItemsSource = db.Doc_tipe.ToList();
            SavePlaceCombo.ItemsSource = db.Save_place.ToList();
            DeveloperCombo.ItemsSource = db.Departments.ToList();
        }
    }
}

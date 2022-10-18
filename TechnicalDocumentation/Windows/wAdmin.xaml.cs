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

namespace TechnicalDocumentation.Windows
{
    public partial class wAdmin : Window
    {
        BDEntities db;
        public wAdmin()
        {
            InitializeComponent();
        }

        private void AddDoc_Click(object sender, RoutedEventArgs e)
        {
            new wDocumentAdd().ShowDialog();
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Documents.ToList();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            new wUsersManage().ShowDialog();
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            new wReports().ShowDialog();
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            new wBackUp().ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new wAuthorization().Show();
            this.Close();
        }

        private void dgogrenci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgogrenci.SelectedItem != null)
            {
                Document doc = dgogrenci.SelectedItem as Document;
                D.dataD1 = doc.Id.ToString();

                new wDocumentEdit().ShowDialog();
                db = new BDEntities();
                dgogrenci.ItemsSource = db.Documents.ToList();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Documents.ToList();
            LoadTypeDocuments();
            LoadPlaceStore();
            chbRelevant.IsChecked = null;
            Find_LostFocus(sender, e);
        }

        private void LoadTypeDocuments()
        {
            List<Doc_tipe> typeDocs = db.Doc_tipe.ToList();
            typeDocs.Insert(0, new Doc_tipe { Id = -1, Name = string.Empty });
            cmbTypeDocuments.ItemsSource = typeDocs.ToList();
        }

        private void LoadPlaceStore()
        {
            List<Save_place> placeStores = db.Save_place.ToList();
            placeStores.Insert(0, new Save_place { Id = -1, Name = string.Empty });
            cmbPlaceStore.ItemsSource = placeStores.ToList();
        }

        private void Find_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void txtIndexSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void cmbTypeDocuments_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void cmbPlaceStore_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void chbRelevant_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void SearchDocuments()
        {
            List<Document> documents = db.Documents.ToList();

            if (!string.IsNullOrEmpty(Find.Text))
            {
                documents = documents.Where(t => t.Name.Contains(Find.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(txtIndexSearch.Text))
            {
                documents = documents.Where(t => t.Index.Contains(txtIndexSearch.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(cmbTypeDocuments.Text))
            {
                documents = documents.Where(t => t.Doc_tipe_id == ((Doc_tipe)cmbTypeDocuments.SelectedValue).Id).ToList();
            }

            if (!string.IsNullOrEmpty(cmbPlaceStore.Text))
            {
                documents = documents.Where(t => t.Save_place_id == ((Save_place)cmbPlaceStore.SelectedValue).Id).ToList();
            }

            if (chbRelevant.IsChecked != null)
            {
                documents = documents.Where(t => t.Relevance == chbRelevant.IsChecked.Value).ToList();
            }

            dgogrenci.ItemsSource = documents.ToList();
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

        private void dgogrenci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Удалить запись?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Document d = dgogrenci.SelectedItem as Document;
                    foreach (Who_know item in db.Who_know.ToList())
                    {
                        if (item.Document_id == d.Id)
                        {
                            db.Who_know.Remove(item);
                        }
                    }
                    db.Documents.Remove((Document)dgogrenci.SelectedItem);
                    db.SaveChanges();
                    dgogrenci.ItemsSource = db.Documents.ToList();
                }
            }
        }
    }
}

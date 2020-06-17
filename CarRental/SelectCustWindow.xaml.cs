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

namespace CarRental
{
    /// <summary>
    /// Interaction logic for SelectCustWindow.xaml
    /// </summary>
    public partial class SelectCustWindow : Window
    {

        List<Customers> customersList;
        
        int idCust;
        public int IdCust{ get; set; }

        public SelectCustWindow()
        {
            InitializeComponent();
            CustomersList();
        }



        public void CustomersList()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                customersList = db.Customers.ToList();
                customersList= customersList.OrderBy(o => o.LastName).ToList();
                ListView_SelCust.ItemsSource = customersList;
            }
        }

        private void ListView_SelCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_SelCust.SelectedIndex >= 0)
            {
                var item = (ListBox)sender;
                var customer = (Customers)item.SelectedItem;
                idCust = customer.Id_Customer;
                Bt_SelectCust.IsEnabled = true;
                
            }
        }

        private void Bt_SelectCust_Click(object sender, RoutedEventArgs e)
        {
            IdCust = idCust;
            Close();
        }
    }
}

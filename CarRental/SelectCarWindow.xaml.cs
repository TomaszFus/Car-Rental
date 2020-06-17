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
    /// Interaction logic for SelectCarWindow.xaml
    /// </summary>
    public partial class SelectCarWindow : Window
    {
        List<Cars> carsList;

        string idCar;
        public string IdCar { get; set; }

        public SelectCarWindow()
        {
            InitializeComponent();
            CarList();
        }


        public void CarList()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                carsList = db.Cars.Where(s=>s.Status=="wolny").ToList();
                ListView_SelCar.ItemsSource = carsList;
            }
        }

        private void ListView_SelCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_SelCar.SelectedIndex >= 0)
            {
                var item = (ListBox)sender;
                var car = (Cars)item.SelectedItem;
                idCar = car.Reg_Number;
                Bt_SelectCust.IsEnabled = true;

            }
        }

        private void Bt_SelectCust_Click(object sender, RoutedEventArgs e)
        {
            IdCar = idCar;
            Close();
        }


    }
}

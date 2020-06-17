using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Interaction logic for AddRentWindow.xaml
    /// </summary>
    public partial class AddRentWindow : Window
    {
        Customers customer = new Customers();
        Rent rent = new Rent();
        Cars car = new Cars();
        int idCust;
        string idCar;
        DateTime endDate;
        DateTime startDate;
        bool EndDateCheck = false;
        double cost;
        public AddRentWindow()
        {
            InitializeComponent();
            TxB_StartDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        //metody
        private void SwitchEnableTxB()
        {
            TxB_CustFirstName.IsEnabled = true;
            TxB_CustLastName.IsEnabled = true;
            TxB_CustPhone.IsEnabled = true;
            TxB_CustAddress.IsEnabled = true;
            TxB_CustCity.IsEnabled = true;
            TxB_CustIdNum.IsEnabled = true;
        }

        private void ClearCustData()
        {
            TxB_CustFirstName.Text = TxB_CustLastName.Text = TxB_CustPhone.Text = TxB_CustAddress.Text = TxB_CustCity.Text = TxB_CustIdNum.Text = null;

        }

        private void SetCustomerData()
        {
            using (CarRentalEntities db=new CarRentalEntities())
            {
                customer = db.Customers.Where(i => i.Id_Customer == idCust).FirstOrDefault();
            }
            if (customer != null)
            {
                TxB_CustFirstName.Text = customer.FirstName;
                TxB_CustLastName.Text = customer.LastName;
                TxB_CustPhone.Text = customer.PhoneNumber.ToString();
                TxB_CustAddress.Text = customer.Address;
                TxB_CustCity.Text = customer.City;
                TxB_CustIdNum.Text = customer.Id_Number;
            }
            else
            {
                Radio_NewCust.IsChecked = true;
            }
        }

        private void SetCar()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                car = db.Cars.Where(i => i.Reg_Number == idCar).FirstOrDefault();
            }
            if (car!=null)
            {
                TxB_CarReg_Number.Text = car.Reg_Number;
            }
            else
            {

            }

        }


        private void Rent()
        {
            bool FNameCheck = false;
            bool LNameCheck = false;
            bool PhoneCheck = false;
            bool AddressCheck = false;
            bool CityCheck = false;
            bool IdNumCheck = false;

            bool carCheck = false;
            bool endDateCheck = false;
            //customer = null;
            if (Radio_NewCust.IsChecked==true)
            {
                

                if (String.IsNullOrWhiteSpace(TxB_CustFirstName.Text))
                {
                    Lb_FNameError.Content = "Podaj imie";
                    Lb_FNameError.Visibility = Visibility.Visible;
                    FNameCheck = false;
                }
                else
                {
                    Lb_FNameError.Visibility = Visibility.Collapsed;
                    customer.FirstName = TxB_CustFirstName.Text;
                    FNameCheck = true;
                }

                if (String.IsNullOrWhiteSpace(TxB_CustLastName.Text))
                {
                    Lb_LNameError.Content = "Podaj nazwisko";
                    Lb_LNameError.Visibility = Visibility.Visible;
                    LNameCheck = false;
                }
                else
                {
                    Lb_LNameError.Visibility = Visibility.Collapsed;
                    customer.LastName = TxB_CustLastName.Text;
                    LNameCheck = true;
                }

                if (String.IsNullOrWhiteSpace(TxB_CustPhone.Text))
                {
                    Lb_PhoneError.Content = "Podaj telefon";
                    Lb_PhoneError.Visibility = Visibility.Visible;
                    PhoneCheck = false;
                }
                else
                {
                    try
                    {
                        Lb_PhoneError.Visibility = Visibility.Collapsed;
                        customer.PhoneNumber = int.Parse(TxB_CustPhone.Text);
                        PhoneCheck = true;
                    }
                    catch (Exception ex)
                    {
                        Lb_PhoneError.Content = ex.Message;
                        Lb_PhoneError.Visibility = Visibility.Visible;
                        PhoneCheck = false;
                        //throw;
                    }
                    
                }

                if (String.IsNullOrWhiteSpace(TxB_CustAddress.Text))
                {
                    Lb_AddressError.Content = "Podaj adres";
                    Lb_AddressError.Visibility = Visibility.Visible;
                    AddressCheck = false;
                }
                else
                {
                    Lb_AddressError.Visibility = Visibility.Collapsed;
                    customer.Address = TxB_CustAddress.Text;
                    AddressCheck = true;
                }

                if (String.IsNullOrWhiteSpace(TxB_CustCity.Text))
                {
                    Lb_CityError.Content = "Podaj ulice";
                    Lb_CityError.Visibility = Visibility.Visible;
                    CityCheck = false;
                }
                else
                {
                    Lb_CityError.Visibility = Visibility.Collapsed;
                    customer.City = TxB_CustCity.Text;
                    CityCheck = true;
                }

                if (String.IsNullOrWhiteSpace(TxB_CustIdNum.Text))
                {
                    Lb_IdNumError.Content = "Podaj numer dowodu";
                    Lb_IdNumError.Visibility = Visibility.Visible;
                    IdNumCheck = false;
                }
                else
                {
                    Lb_IdNumError.Visibility = Visibility.Collapsed;
                    customer.Id_Number = TxB_CustIdNum.Text;
                    IdNumCheck = true;
                }

            }
            else if(Radio_Cust.IsChecked==true)
            {
                
                FNameCheck = LNameCheck = PhoneCheck = AddressCheck = CityCheck = IdNumCheck = true;
                rent.CustomerID = customer.Id_Customer;
            }
            else
            {
                MessageBox.Show("Dokonaj byworu");
            }



            if(String.IsNullOrWhiteSpace(TxB_CarReg_Number.Text))
            {
                Lb_SelectedCarError.Content = "Wybierz samochód";
                Lb_SelectedCarError.Visibility = Visibility.Visible;
                carCheck = false;
            }
            else
            {
                Lb_SelectedCarError.Visibility = Visibility.Collapsed;
                rent.Reg_Number = TxB_CarReg_Number.Text;
                carCheck = true;
            }

            rent.StartDate = startDate.ToString("dd/MM/yyyy");

            if(DatePicker_End.SelectedDate==null)
            {
                Lb_EndDateError.Content = "Wybierz datę";
                Lb_EndDateError.Visibility = Visibility.Visible;
                endDateCheck = false;
            }
            else if (DatePicker_End.SelectedDate <= DateTime.Now)
            {
                Lb_EndDateError.Content = "Błędna data";
                Lb_EndDateError.Visibility = Visibility.Visible;
                EndDateCheck = false;
            }
            else
            {
                Lb_EndDateError.Visibility = Visibility.Collapsed;
                rent.EndDate = endDate.ToString("dd/MM/yyyy");
                endDateCheck = true;
            }

            rent.Cost = cost;

            car.Status = "zajety";


            if (FNameCheck && LNameCheck && PhoneCheck && AddressCheck && CityCheck && IdNumCheck && EndDateCheck && carCheck && endDateCheck==true)
            {
                try
                {
                    using (CarRentalEntities db = new CarRentalEntities())
                    {
                        db.Customers.AddOrUpdate(customer);
                        db.Rent.Add(rent);
                        db.Cars.AddOrUpdate(car);
                        db.SaveChanges();
                        Close();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }


            
        }




    //zdarzenia
        private void Radio_NewCust_Checked(object sender, RoutedEventArgs e)
        {
            ClearCustData();
            SwitchEnableTxB();
        }

        private void Radio_Cust_Checked(object sender, RoutedEventArgs e)
        {
            SelectCustWindow selectCustWindow = new SelectCustWindow();
            selectCustWindow.ShowDialog();
            SwitchEnableTxB();
            TxB_CustFirstName.IsReadOnly = true;
            TxB_CustLastName.IsReadOnly = true;
            idCust = selectCustWindow.IdCust;
            SetCustomerData();



        }

        private void Bt_SelCar_Click(object sender, RoutedEventArgs e)
        {
            SelectCarWindow selectCarWindow = new SelectCarWindow();
            selectCarWindow.ShowDialog();
            idCar = selectCarWindow.IdCar;
            SetCar();



        }

        private void DatePicker_End_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePicker_End.SelectedDate<=DateTime.Now)
            {
                Lb_EndDateError.Content = "Błędna data";
                Lb_EndDateError.Visibility = Visibility.Visible;
                EndDateCheck = false;
            }
            else
            {
                Lb_EndDateError.Visibility = Visibility.Collapsed;
                endDate = DatePicker_End.SelectedDate.Value;
                startDate = DateTime.Today;
                rent.EndDate = endDate.ToString("dd/MM/yyyy");
                EndDateCheck = true;
                TimeSpan days = endDate - startDate;
                int rentTime = days.Days;
                cost= rentTime * car.Price;
                TxB_Cost.Text = cost.ToString();


            }
        }

        private void Bt_Rent_Click(object sender, RoutedEventArgs e)
        {
            Rent();
        }
    }
}

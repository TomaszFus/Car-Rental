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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace CarRental
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Employees employee = new Employees();


        List<Employees> empList;
        List<Cars> carsList;
        List<Rent> rentList;
        Rent rent = new Rent();

        public MainWindow(int userId)
        {
            InitializeComponent();
            using (CarRentalEntities db=new CarRentalEntities())
            {
                employee = db.Employees.Where(i => i.Id_Employee == userId).First();
                if (employee.AdvancePermission==true)
                {
                    Bt_Grid2.IsEnabled = true;
                }
            }
        }

        
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Grid_1.Visibility = Visibility.Visible;
            Grid_2.Visibility = Visibility.Hidden;
            EmpList();
            CarsList();
            RentList();
        }

        private void Bt_Grid1_Click(object sender, RoutedEventArgs e)
        {
            Grid_1.Visibility = Visibility.Visible;
            Grid_2.Visibility = Visibility.Hidden;
            Grid_3.Visibility = Visibility.Hidden;
        }

        private void Bt_Grid2_Click(object sender, RoutedEventArgs e)
        {
            Grid_1.Visibility = Visibility.Hidden;
            Grid_2.Visibility = Visibility.Visible;
            Grid_3.Visibility = Visibility.Hidden;
        }

        private void Bt_Grid3_Click(object sender, RoutedEventArgs e)
        {
            Grid_1.Visibility = Visibility.Hidden;
            Grid_2.Visibility = Visibility.Hidden;
            Grid_3.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Zarzadzanie
        /// </summary>


        //Dodawanie pracownika
        private void Bt_AddEmp_Click(object sender, RoutedEventArgs e)
        {
            CrNewEmpWindow crNewEmpWindow = new CrNewEmpWindow();
            crNewEmpWindow.ShowDialog();
            EmpList();
        }

        //wybieranie z listy
        private void LisView_Emp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LisView_Emp.SelectedIndex>=0)
            {
                var item = (ListBox)sender;
                var emp = (Employees)item.SelectedItem;
                if(employee.Id_Employee==emp.Id_Employee)
                {
                    Bt_DelEmp.IsEnabled = false;
                }
                else
                {
                    Bt_DelEmp.IsEnabled = true;
                    Bt_ModEmp.IsEnabled = true;
                }
                    
                
            }
            
        }

        //modyfikowanie
        private void Bt_ModEmp_Click(object sender, RoutedEventArgs e)
        {
            ModEmp();
            EmpList();
        }



        //zwalnianie
        private void Bt_DelEmp_Click(object sender, RoutedEventArgs e)
        {
            DelEmp();
            EmpList();
            Bt_DelEmp.IsEnabled = false;
        }


        /// <summary>
        /// wylogowanie
        /// </summary>
        private void Bt_LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }



        /// <summary>
        /// samochody
        /// </summary>
        //wybieranie z listy
        private void ListView_Cars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_Cars.SelectedIndex >= 0)
            {
                var item = (ListBox)sender;
                var car = (Cars)item.SelectedItem;
                if (car.Status == "wolny")
                    Bt_DelCar.IsEnabled = true;
                MessageBox.Show("Nr. rejestracyjny: " + car.Reg_Number + "\n" + "Marka: " + car.Mark + "\n" + "Model: " + car.Model + "\n" + "Przebieg: " + car.Course.ToString() + " km" + "\n" + "Klasa: " + car.Class + "\n" + "Cena: " + car.Price.ToString() + " zł" + "\n" + "Status: " + car.Status);
            }
        }

        //usun samochod
        private void Bt_DelCar_Click(object sender, RoutedEventArgs e)
        {
            DelCar();
            CarsList();
        }

        //dodaj samochod
        private void Bt_AddCar_Click(object sender, RoutedEventArgs e)
        {
            AddNewCarWindow addNewCarWindow = new AddNewCarWindow();
            addNewCarWindow.ShowDialog();
            CarsList();
        }


        /// <summary>
        /// wynajecia
        /// </summary>

        //nowe wypozyczenie
        private void Bt_NewRent_Click(object sender, RoutedEventArgs e)
        {
            AddRentWindow addRentWindow = new AddRentWindow();
            addRentWindow.ShowDialog();
            RentList();
            CarsList();
        }


        //wybor wypozyczenia
        private void ListView_Rent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_Rent.SelectedIndex >= 0)
            {
                Bt_EndRent.IsEnabled = true;
                var item = (ListBox)sender;
                rent = (Rent)item.SelectedItem;
                
            }
        }


        //zakoncz wypozycznie

        private void Bt_EndRent_Click(object sender, RoutedEventArgs e)
        {
            EndRent();
        }

        /////////// METODY  //////////////////

        //wyswietlanioe listy pracownikow
        public void EmpList()
        {
            using(CarRentalEntities db=new CarRentalEntities())
            {
                empList = db.Employees.ToList();
                LisView_Emp.ItemsSource = empList;
            }
        }

        //wyswietlanioe listy samochodów
        public void CarsList()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                carsList = db.Cars.ToList();
                ListView_Cars.ItemsSource = carsList;
            }
        }

        //wyswietlanie rezerwacji
        public void RentList()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                rentList = db.Rent.ToList();
                ListView_Rent.ItemsSource = rentList;
            }
        }


        //zwalnianie pracownika
        public void DelEmp()
        {
            if (LisView_Emp.SelectedIndex >= 0)
            {
                Bt_DelEmp.IsEnabled = true;
                var emp = (Employees)LisView_Emp.SelectedItem;

                

                if (MessageBox.Show("Na pewno chcesz zwolnic tego pracownika?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                        try
                        { 
                            using (CarRentalEntities db = new CarRentalEntities())
                            {
                                
                                var loginToDelete= (from item in db.LoginData where item.Id_Emp == emp.Id_Employee select item).First();
                                var empToDelete = (from item in db.Employees where item.Id_Employee == emp.Id_Employee select item).First();
                                if (empToDelete != null)
                                {
                                    db.LoginData.Remove(loginToDelete);
                                    db.Employees.Remove(empToDelete);

                                }
                                db.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            //throw;
                        }
                }


            }
                else
                {

                }
        }
        //modyfikuj pracownika
        public void ModEmp()
        {
           
                
                var emp = (Employees)LisView_Emp.SelectedItem;
                CrNewEmpWindow crNewEmpWindow = new CrNewEmpWindow(emp.Id_Employee);
                crNewEmpWindow.Bt_ModEmp.IsEnabled = true;
                crNewEmpWindow.Bt_CrNewEmp.IsEnabled = false;
                crNewEmpWindow.ShowDialog();
            
        }


        //usun samochod
        public void DelCar()
        {
            if(ListView_Cars.SelectedIndex>=0)
            {
                var selectedCar = (Cars)ListView_Cars.SelectedItem;
                if (MessageBox.Show("Usunąć samochód?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (CarRentalEntities db = new CarRentalEntities())
                        {
                            var carToDel = (from item in db.Cars where item.Reg_Number == selectedCar.Reg_Number select item).First();
                            if (carToDel != null)
                            {
                                db.Cars.Remove(carToDel);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        //throw;
                    }
                    
                }
                else
                {

                }

            }
        }


        private void EndRent()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = DateTime.ParseExact(rent.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (startDate == today)
            {
                MessageBox.Show("Nie można zwrócić. Można wypożyczyć min na jeden dzień");


            }
            else
            {
                int rentId = rent.Id_Rent;
                EndRentWindow endRentWindow = new EndRentWindow(rentId);
                endRentWindow.ShowDialog();
            }
            RentList();
            CarsList();
            Bt_EndRent.IsEnabled = false;
            

        }

        private void Bt_Export_Click(object sender, RoutedEventArgs e)
        {
            string txtEditor;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtEditor = File.ReadAllText(openFileDialog.FileName);
            
        }
    }

        
}


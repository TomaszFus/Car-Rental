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
        List<Customers> customersList;
        Customers customer = new Customers();
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
            CustomersList();
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
            CustomersList();
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


        //wybor klienta
        private void LisView_Customser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LisView_Customser.SelectedIndex >= 0)
            {
                Bt_DelCust.IsEnabled = true;
                var item = (ListBox)sender;
                customer = (Customers)item.SelectedItem;

            }
        }

        //zakoncz wypozycznie

        private void Bt_EndRent_Click(object sender, RoutedEventArgs e)
        {
            EndRent();
        }


        //export klientow
        private void Bt_Export_Click(object sender, RoutedEventArgs e)
        {
            ExportCust();
        }
        //import klientow
        private void Bt_Import_Click(object sender, RoutedEventArgs e)
        {
            ImportCust();
        }


        //usuniecie klienta
        private void Bt_DelCust_Click(object sender, RoutedEventArgs e)
        {
            DelCust();
            CustomersList();
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

        //wyswietlanie klientow
        public void CustomersList()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                customersList = db.Customers.ToList();
                LisView_Customser.ItemsSource = customersList;
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
                            if (carToDel != null && carToDel.Status!="zajęty")
                            {
                                db.Cars.Remove(carToDel);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Nie można usunąć wynajętego samochodu");
                        //throw;
                    }
                    
                }
                else
                {

                }

            }
            Bt_DelCar.IsEnabled = false;
        }

        //zakonczenie wypozyczenia
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


        //export klientow
        private void ExportCust()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Imie,Nazwisko,telefon,Adres,Miejscowość,Nr dowodu");
            foreach (var item in customersList)
            {
                sb.AppendLine($"{item.FirstName},{item.LastName},{item.PhoneNumber},{item.Address},{item.City},{item.Id_Number}");
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());

        }

        //import klientow
        private void ImportCust()
        {
            string fileName=null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                 fileName= openFileDialog.FileName;



            using (var sr = new StreamReader(fileName))
            {
                var line = sr.ReadLine();
                int i = 0;
                while (line!=null)
                {
                    if (i!=0)
                    {
                        string[] splited = line.Split(',');
                        customer.FirstName = splited[0];
                        customer.LastName = splited[1];
                        customer.PhoneNumber = int.Parse(splited[2]);
                        customer.Address = splited[3];
                        customer.City = splited[4];
                        customer.Id_Number = splited[5];
                        using (CarRentalEntities db=new CarRentalEntities())
                        {
                            db.Customers.Add(customer);
                            db.SaveChanges();
                        }

                        //customersList.Add(new Customers()
                        //{
                        //    FirstName=splited[0],
                        //    LastName=splited[1],
                        //    PhoneNumber=int.Parse(splited[2]),
                        //    Address=splited[3],
                        //    City=splited[4],
                        //    Id_Number=splited[5]

                        //});
                    }
                    i++;
                    line = sr.ReadLine();
                }
            }


            CustomersList();
        }



        //usowanie klienta
        public void DelCust()
        {
            bool Check = false;
            if (LisView_Customser.SelectedIndex >= 0)
            {
                Bt_DelCust.IsEnabled = true;
                var cust = (Customers)LisView_Customser.SelectedItem;



                if (MessageBox.Show("Na pewno chceszusunąć klienta?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (CarRentalEntities db = new CarRentalEntities())
                        {

                            var custToDelete = (from item in db.Customers where item.Id_Customer == cust.Id_Customer select item).First();
                            foreach (var item in rentList)
                            {
                                if (item.CustomerID==custToDelete.Id_Customer)
                                {
                                    Check = false;
                                    break;
                                }
                                else
                                    Check = true;
                            }

                            if (custToDelete != null && Check==true)
                            {
                                db.Customers.Remove(custToDelete);
                                
                                db.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("Nie można usunąć klienta, który wypożycza auto");
                            }
                            
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
            Bt_DelCust.IsEnabled = false;
        }

        
    }

        
}


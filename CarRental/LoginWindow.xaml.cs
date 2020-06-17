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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }


        LoginData loginData = new LoginData();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            List<Employees> employeesList;
            using (CarRentalEntities db=new CarRentalEntities())
            {
                employeesList = db.Employees.ToList();
            }
            if (employeesList.Count==0)
            {
                CrNewEmpWindow newEmpWindow = new CrNewEmpWindow();
                newEmpWindow.CheckB_Perm.IsChecked = true;
                newEmpWindow.CheckB_Perm.IsEnabled = false;
                newEmpWindow.ShowDialog();
            }        
 
        }


        public void LogIn()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                loginData = db.LoginData.FirstOrDefault(p => p.Login == TxB_Login.Text);
                if (loginData is null)
                {
                    Lb_LoginError.Content = "Błędny login";
                    Lb_LoginError.Visibility = Visibility.Visible;
                }
                else
                {
                    Lb_LoginError.Visibility = Visibility.Collapsed;
                    if (loginData.Password == PassB_Passowrd.Password)
                    {
                        Employees user = new Employees();
                        user = db.Employees.First(i => i.Id_Employee == loginData.Id_Emp);  

                        Lb_PassError.Visibility = Visibility.Collapsed;
                        MessageBox.Show("Witaj "+user.FirstName);
                        MainWindow mainWindow = new MainWindow(user.Id_Employee);
                        mainWindow.Show();
                        Close();

                    }
                    else
                    {
                        Lb_PassError.Content = "Błędne hasło";
                        Lb_PassError.Visibility = Visibility.Visible;
                    }
                }
            }

        }
















        private void Bt_NewEmp_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEmpWindow confirmEmpWindow = new ConfirmEmpWindow();
            confirmEmpWindow.ShowDialog();
        }

        private void Bt_LogIn_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }
    }
}

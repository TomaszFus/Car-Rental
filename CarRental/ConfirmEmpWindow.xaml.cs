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
    /// Interaction logic for ConfirmEmpWindow.xaml
    /// </summary>
    public partial class ConfirmEmpWindow : Window
    {
        public ConfirmEmpWindow()
        {
            InitializeComponent();
            
        }
        LoginData loginData = new LoginData();
        int id;
        
        public void Find()
        {
            
            string lastName = TxB_LastName.Text;
            using (CarRentalEntities db=new CarRentalEntities())
            {
                try
                {
                    var emp = db.Employees.Where(p => p.LastName == lastName).First();
                    id = emp.Id_Employee;
                    //var tmpLoginData = db.LoginData.Where(p => p.Id_Emp == id).First();
                    List<LoginData> loginDatas;
                    loginDatas = db.LoginData.ToList();
                    //loginDataId = tmpLoginData.Id_Emp;
                    foreach (var item in loginDatas)
                    {
                        if (id == item.Id_Emp)
                        {
                            MessageBox.Show("Dane zostały już ustawione");
                            
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Znaleziono");
                            TxB_EmpLogin.IsEnabled = PassBox_Emp.IsEnabled = Bt_Confirm.IsEnabled = true;
                        }
                    }
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Brak");
                    //throw;
                }
                
                
                
            }         
            
        }



        public void SetLoginData()
        {
            bool loginCheck = false;
            bool passCheck = false;

            if (String.IsNullOrWhiteSpace(TxB_EmpLogin.Text))
            {
                Lb_LoginError.Content = "Podaj login";
                Lb_LoginError.Visibility = Visibility.Visible;
                loginCheck = false;
            }
            else
            {
                Lb_LoginError.Visibility = Visibility.Collapsed;
                loginData.Login = TxB_EmpLogin.Text;
                loginCheck = true;
            }

            if (String.IsNullOrWhiteSpace(PassBox_Emp.Password))
            {
                Lb_PassError.Content = "Podaj haslo";
                Lb_PassError.Visibility = Visibility.Visible;
                passCheck = false;
            }
            else
            {
                if (PassBox_Emp.Password.Length<8)
                {
                    Lb_PassError.Content = "Hasło musi mieć conajmniej 8 znaków";
                    Lb_PassError.Visibility = Visibility.Visible;
                    passCheck = false;
                }
                else
                {
                    Lb_PassError.Visibility = Visibility.Collapsed;
                    loginData.Password = PassBox_Emp.Password;
                    passCheck = true;
                }
                
            }


            loginData.Id_Emp = id;
            
            if(loginCheck && passCheck==true)
            {
                try
                {
                    using (CarRentalEntities db = new CarRentalEntities())
                    {
                        db.LoginData.Add(loginData);
                        db.SaveChanges();
                    }
                    Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            
        }










        private void Bt_Find_Click(object sender, RoutedEventArgs e)
        {
            Find();
        }

        private void Bt_Confirm_Click(object sender, RoutedEventArgs e)
        {
            SetLoginData();

        }
    }
}

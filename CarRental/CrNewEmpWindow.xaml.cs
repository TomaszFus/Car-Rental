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
    /// Interaction logic for CrNewEmpWindow.xaml
    /// </summary>
    public partial class CrNewEmpWindow : Window
    {
        Employees employee = new Employees();
        public CrNewEmpWindow()
        {
            InitializeComponent();
        }

        public CrNewEmpWindow(int userId)
        {
            InitializeComponent();
            using (CarRentalEntities db=new CarRentalEntities())
            {
                employee = db.Employees.Where(i => i.Id_Employee == userId).First();
                TxB_EmpFirstName.IsEnabled = false;
                TxB_EmpLastName.IsEnabled = false;
                TxB_EmpFirstName.Text = employee.FirstName;
                TxB_EmpLastName.Text = employee.LastName;
                TxB_EmpPhone.Text = employee.PhoneNumber.ToString();
                if (employee.Id_Employee==1)
                {
                    CheckB_Perm.IsEnabled = false;
                    if (employee.AdvancePermission == true)
                        CheckB_Perm.IsChecked = true;
                }
                TxB_EmpSalary.Text = employee.Salary.ToString();
            }
        }
        
        

        //tworzenie uzytkownika
        public void CreateEmp()
        {
            bool fNameCheck = false;
            bool lNameCheck = false;
            bool phoneCheck = false;
            bool salaryCheck = false;


            if (String.IsNullOrWhiteSpace(TxB_EmpFirstName.Text))
            {
                Lb_EmpFNameError.Content = "Podaj imie";
                Lb_EmpFNameError.Visibility = Visibility.Visible;
                fNameCheck = false;
            }
            else
            {
                Lb_EmpFNameError.Visibility = Visibility.Collapsed;
                employee.FirstName = TxB_EmpFirstName.Text;
                fNameCheck = true;
            }

            if (String.IsNullOrWhiteSpace(TxB_EmpLastName.Text))
            {
                Lb_EmpLNameError.Content = "Podaj nazwisko";
                Lb_EmpLNameError.Visibility = Visibility.Visible;
                lNameCheck = false;
            }
            else
            {
                Lb_EmpLNameError.Visibility = Visibility.Collapsed;
                employee.LastName = TxB_EmpLastName.Text;
                lNameCheck = true;
            }

            if (String.IsNullOrWhiteSpace(TxB_EmpPhone.Text))
            {
                Lb_EmpPhoneError.Content = "Podaj telefon";
                Lb_EmpPhoneError.Visibility = Visibility.Visible;
                phoneCheck = false;
            }
            else
            {
                try
                {
                    Lb_EmpPhoneError.Visibility = Visibility.Collapsed;
                    employee.PhoneNumber = int.Parse(TxB_EmpPhone.Text);
                    phoneCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_EmpPhoneError.Content=ex.Message;
                    Lb_EmpPhoneError.Visibility = Visibility.Visible;
                    phoneCheck = false;
                    //throw;
                }
                
            }

            if(CheckB_Perm.IsChecked==true)
            {
                employee.AdvancePermission = true;
            }
            else
            {
                employee.AdvancePermission = false;
            }


            if (String.IsNullOrWhiteSpace(TxB_EmpSalary.Text))
            {
                Lb_EmpSalaryError.Content = "Podaj kwotę wynagrodzenia";
                Lb_EmpSalaryError.Visibility = Visibility.Visible;
                salaryCheck = false;
            }
            else
            {
                try
                {
                    Lb_EmpSalaryError.Visibility = Visibility.Collapsed;
                    employee.Salary = double.Parse(TxB_EmpSalary.Text);
                    salaryCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_EmpSalaryError.Content = ex.Message;
                    Lb_EmpSalaryError.Visibility = Visibility.Visible;
                    salaryCheck = false;
                    //throw;
                }
            }



            if (fNameCheck && lNameCheck && phoneCheck && salaryCheck==true)
            {
                try
                {
                    using (CarRentalEntities db=new CarRentalEntities())
                    {
                        db.Employees.Add(employee);
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


        //modyfikowanie uzytkownika
        public void ModUser()
        {
            bool phoneCheck = false;
            bool salaryCheck = false;

            if (String.IsNullOrWhiteSpace(TxB_EmpPhone.Text))
            {
                Lb_EmpPhoneError.Content = "Podaj telefon";
                Lb_EmpPhoneError.Visibility = Visibility.Visible;
                phoneCheck = false;
            }
            else
            {
                Lb_EmpPhoneError.Visibility = Visibility.Collapsed;
                
                phoneCheck = true;
            }

            

            if (String.IsNullOrWhiteSpace(TxB_EmpSalary.Text))
            {
                Lb_EmpSalaryError.Content = "Podaj kwotę wynagrodzenia";
                Lb_EmpSalaryError.Visibility = Visibility.Visible;
                salaryCheck = false;
            }
            else
            {
                Lb_EmpSalaryError.Visibility = Visibility.Collapsed;
                
                salaryCheck = true;
            }

            if(phoneCheck && salaryCheck ==true)
            {
                try
                {
                    using (CarRentalEntities db = new CarRentalEntities())
                    {
                        var toMod = db.Employees.SingleOrDefault(i => i.Id_Employee == employee.Id_Employee);
                        if (toMod != null)
                        {
                            toMod.PhoneNumber = int.Parse(TxB_EmpPhone.Text);
                            toMod.Salary = double.Parse(TxB_EmpSalary.Text);
                            if (CheckB_Perm.IsChecked == true)
                                toMod.AdvancePermission = true;
                            else
                                toMod.AdvancePermission = false;
                        }
                        db.SaveChanges();
                        Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Błąd");
                    //throw;
                }
                
            }
        }





        //////////////////////////
        private void Bt_CrNewEmp_Click(object sender, RoutedEventArgs e)
        {
            CreateEmp();
        }

        private void Bt_ModEmp_Click(object sender, RoutedEventArgs e)
        {
            ModUser();
        }
    }
}

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
    /// Interaction logic for AddNewCarWindow.xaml
    /// </summary>
    public partial class AddNewCarWindow : Window
    {
        Cars car = new Cars();
        public AddNewCarWindow()
        {
            InitializeComponent();
        }

        //dodawanie samochodu
        public void AddCar()
        {
            bool RNumberCheck = false;
            bool MarkCheck = false;
            bool ModelCheck = false;
            bool CourseCheck = false;
            bool ClassCheck = false;
            bool PriceCheck = false;


            if (String.IsNullOrWhiteSpace(TxB_RNumber.Text))
            {
                Lb_RNumberError.Content = "Podaj nr. rejestracyjny";
                Lb_RNumberError.Visibility = Visibility.Visible;
                RNumberCheck = false;
            }
            else
            {
                try
                {
                    Lb_RNumberError.Visibility = Visibility.Collapsed;
                    car.Reg_Number = TxB_RNumber.Text;
                    RNumberCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_RNumberError.Content =ex.Message;
                    Lb_RNumberError.Visibility = Visibility.Visible;
                    RNumberCheck = false;
                    //throw;
                }
            }

            if (String.IsNullOrWhiteSpace(TxB_Mark.Text))
            {
                Lb_MarkError.Content = "Podaj markę";
                Lb_MarkError.Visibility = Visibility.Visible;
                MarkCheck = false;
            }
            else
            {
                try
                {
                    Lb_MarkError.Visibility = Visibility.Collapsed;
                    car.Mark = TxB_Mark.Text;
                    MarkCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_MarkError.Content = ex.Message;
                    Lb_MarkError.Visibility = Visibility.Visible;
                    MarkCheck = false;
                    //throw;
                }
            }

            if (String.IsNullOrWhiteSpace(TxB_Model.Text))
            {
                Lb_ModelError.Content = "Podaj model";
                Lb_ModelError.Visibility = Visibility.Visible;
                ModelCheck = false;
            }
            else
            {
                try
                {
                    Lb_ModelError.Visibility = Visibility.Collapsed;
                    car.Model = TxB_Model.Text;
                    ModelCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_ModelError.Content = ex.Message;
                    Lb_ModelError.Visibility = Visibility.Visible;
                    ModelCheck = false;
                    //throw;
                }
            }

            if (String.IsNullOrWhiteSpace(TxB_Course.Text))
            {
                Lb_CourseError.Content = "Podaj przebieg";
                Lb_CourseError.Visibility = Visibility.Visible;
                CourseCheck = false;
            }
            else
            {
                try
                {
                    Lb_CourseError.Visibility = Visibility.Collapsed;
                    car.Course = int.Parse(TxB_Course.Text);
                    CourseCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_CourseError.Content = ex.Message;
                    Lb_CourseError.Visibility = Visibility.Visible;
                    CourseCheck = false;
                    //throw;
                }
            }


            if (ComboB_Class.SelectedIndex == -1)
            {
                Lb_ClassError.Content = "Wybierz klase";
                Lb_ClassError.Visibility = Visibility.Visible;
                ClassCheck = false;
            }
            else
            {
                Lb_ClassError.Visibility = Visibility.Collapsed;
                //car.Class = ComboB_Class.SelectedItem.ToString();
                car.Class = ComboB_Class.Text;
                ClassCheck = true;
            }



            if (String.IsNullOrWhiteSpace(TxB_Price.Text))
            {
                Lb_PriceError.Content = "Podaj cene";
                Lb_PriceError.Visibility = Visibility.Visible;
                PriceCheck = false;
            }
            else
            {
                try
                {
                    Lb_PriceError.Visibility = Visibility.Collapsed;
                    car.Price = double.Parse(TxB_Price.Text);
                    PriceCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_PriceError.Content = ex.Message;
                    Lb_PriceError.Visibility = Visibility.Visible;
                    PriceCheck = false;
                    //throw;
                }
            }

            car.Status = "wolny";


            if(RNumberCheck && MarkCheck && ModelCheck && CourseCheck && ClassCheck && PriceCheck==true)
            {
                try
                {
                    using (CarRentalEntities db=new CarRentalEntities())
                    {
                        db.Cars.Add(car);
                        db.SaveChanges();
                        Close();
                    }
                }
                catch (Exception )
                {
                    //MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }

        private void Bt_AddNewCar_Click(object sender, RoutedEventArgs e)
        {
            AddCar();
        }
    }
}

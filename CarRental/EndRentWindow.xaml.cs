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
    /// Interaction logic for EndRentWindow.xaml
    /// </summary>
    public partial class EndRentWindow : Window
    {
        Rent rent = new Rent();
        Cars car = new Cars();
        Customers customer = new Customers();
        
        public EndRentWindow(int rentId)
        {
            InitializeComponent();
            using (CarRentalEntities db=new CarRentalEntities())
            {
                rent = db.Rent.Where(i => i.Id_Rent == rentId).First();
                car = db.Cars.Where(i => i.Reg_Number == rent.Reg_Number).First();
                customer = db.Customers.Where(i => i.Id_Customer == rent.CustomerID).First();
            }

            TxB_CustFName.Text = customer.FirstName;
            TxB_CustLName.Text = customer.LastName;
            TxB_CarNr.Text = car.Reg_Number;
            TxB_CarMark.Text = car.Mark;
            TxB_CarModel.Text = car.Model;
            TxB_TodayDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            TxB_PlanEndDate.Text = rent.EndDate;
            TxB_EndCost.Text = rent.Cost.ToString();
            EndDateCheck();
        }





        private void EndDateCheck()
        {
            double cost;
            DateTime today = DateTime.Today;
            DateTime startDate = DateTime.ParseExact(rent.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(rent.EndDate, "dd-MM-yyyy",System.Globalization.CultureInfo.InvariantCulture);
            if (endDate==today)
            {
                TxB_ToPay.Text = rent.Cost.ToString();
            }
            

            else 
            {
               
                    TimeSpan timeSpan = today - startDate;
                    int rentTime = timeSpan.Days;
                    cost = rentTime * car.Price;
                    TxB_ToPay.Text = cost.ToString();
               
            }

        }


        private void EndRent()
        {
            bool DistanceCheck = false;
            if(String.IsNullOrWhiteSpace(TxB_Distance.Text))
            {
                Lb_DistanceError.Content = "Podaj przejechany dystans";
                Lb_DistanceError.Visibility=Visibility.Visible;
                DistanceCheck = false;
            }
            else
            {
                car.Course += int.Parse(TxB_Distance.Text);
                DistanceCheck = true;
            }

            
            car.Status = "wolny";
            if (DistanceCheck==true)
            {
                try
                {
                    using (CarRentalEntities db = new CarRentalEntities())
                    {
                        var rentToDel = (from item in db.Rent where item.Id_Rent == rent.Id_Rent select item).First();
                        if (rentToDel != null)
                        {
                            db.Rent.Remove(rentToDel);
                        }
                        db.Cars.AddOrUpdate(car);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                Close();
            }
            
        }

        private void Bt_End_Click(object sender, RoutedEventArgs e)
        {
            EndRent();
        }
    }
}

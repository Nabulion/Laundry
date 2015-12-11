using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
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

namespace AdminLaundry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LbLaundryRooms.ItemsSource = Service.Service.GetLaundryRooms();
            LbUsers.ItemsSource = Service.Service.GetUsers();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryRoom laundryRoom = null;
                if ((LaundryRoom) LbLaundryRooms.SelectedItem != null)
                {
                    laundryRoom = (LaundryRoom) LbLaundryRooms.SelectedItem;
                }
                else
                {
                    throw new Exception("Please pick a laundry");
                }

                LaundryUser laundryUser = Service.Service.CreateLaundryUser(laundryRoom, TextUserName.Text);

                TextException.Text = "User: " + laundryUser.name + " has been created and assigned " +
                                     laundryUser.LaundryRoom1 + " as Laundryroom";
                
            }
            catch (Exception e1)
            {
                TextException.Text = e1.Message;
            }

            LbUsers.ItemsSource = null;
            LbUsers.ItemsSource = Service.Service.GetUsers();


        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryUser user = (LaundryUser)LbUsers.SelectedItem;
                TextWashCost.Text = Service.Service.UserTotalCost(user) + "";
                TextException.Text = "Calculated the washcost for " + user.name;

            }
            catch (Exception e1)
            {

                TextException.Text = "Please select a user";
            }


        }

        private void BtnRent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryUser user = (LaundryUser)LbUsers.SelectedItem;
                Service.Service.UserTotalCostPayed(user);
                TextWashCost.Text = Service.Service.UserTotalCost(user) + "";
                TextException.Text = "Added the washcost for " + user.name + " to rent";
            }
            catch (Exception e1)
            {

                TextException.Text = "Please select a user";
            }
        }
    }
}

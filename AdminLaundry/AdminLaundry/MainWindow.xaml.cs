using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            lbUsers.ItemsSource = Service.Service.GetUsers();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                LaundryRoom laundryRoom = (LaundryRoom)LbLaundryRooms.SelectedItem;
                LaundryUser laundryUser = Service.Service.CreateLaundryUser(laundryRoom, TextUserName.Text);
                TextMessage.Text = "User: " + laundryUser.name + " has been created and assigned " +
                               laundryUser.LaundryRoom1 + " as Laundryroom";
         
                lbUsers.ItemsSource = Service.Service.GetUsers();
            }
            catch (Exception e1)
            {
                TextMessage.Text = e1.Message;
            }
         
            
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryUser user = (LaundryUser)lbUsers.SelectedItem;
                TextWashCost.Text = Service.Service.UserTotalCost(user)+ "";
                TextMessage.Text = "Calculated the washcost for " + user.name;
            }
            catch (Exception e1)
            {

                TextMessage.Text = e1.Message;
            }
            

        }

        private void BtnRent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryUser user = (LaundryUser)lbUsers.SelectedItem;
                Service.Service.UserTotalCostPayed(user);
                TextWashCost.Text = Service.Service.UserTotalCost(user) + "";
                TextMessage.Text = "Added the washcost for " + user.name + " to rent";
            }
            catch (Exception e1)
            {

                TextMessage.Text = e1.Message;
            }
        }
    }
}

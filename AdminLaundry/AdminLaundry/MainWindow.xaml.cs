using System;
using System.Windows;
using System.Windows.Controls;


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
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryRoom laundryRoom = null;
                if ((LaundryRoom)LbLaundryRooms.SelectedItem != null)
                {
                    laundryRoom = (LaundryRoom)LbLaundryRooms.SelectedItem;
                }
                else
                {
                    throw new Exception("Please pick a laundry");
                }

                LaundryUser laundryUser = Service.Service.CreateLaundryUser(laundryRoom, TextUserName.Text);

                TextException.Text = "User: " + laundryUser.name + " has been created and assigned " +
                                     laundryUser.LaundryRoom1 + " as Laundryroom";
                LbUsers.ItemsSource = Service.Service.GetUsers(laundryRoom);

            }
            catch (Exception e1)
            {
                TextException.Text = e1.Message;
            }


        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaundryUser user = (LaundryUser)LbUsers.SelectedItem;
                TextWashCost.Text = user.UnPaidWashes() + "";
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
                TextWashCost.Text = user.UnPaidWashes() + "";
                TextException.Text = "Added the washcost for " + user.name + " to rent";
            }
            catch (Exception e1)
            {

                TextException.Text = "Please select a user";
            }
        }

        private void LbLaundryRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            LaundryRoom laundryRoom = (LaundryRoom)LbLaundryRooms.SelectedItem;

            LbUsers.ItemsSource = Service.Service.GetUsers(laundryRoom);
        }
    }
}

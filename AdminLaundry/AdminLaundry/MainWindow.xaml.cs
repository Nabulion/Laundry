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
            LaundryRoom laundryRoom = (LaundryRoom) LbLaundryRooms.SelectedItem;
            try
            {
                LaundryUser laundryUser = Service.Service.CreateLaundryUser(laundryRoom, TextUserName.Text);
                TextMessage.Text = "User: " + laundryUser.name + " has been created and assigned " +
                               laundryUser.LaundryRoom1 + " as Laundryroom";
            }
            catch (Exception e1)
            {

                TextMessage.Text = "something went wrong " + e1.Message;
            }
         
            
        }
    }
}

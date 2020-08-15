using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TimeTableManager.Models;

namespace TimeTableManager
{
    /// <summary>
    /// Interaction logic for AddNewRoomWindow.xaml
    /// </summary>
    public partial class AddNewRoomWindow : Window
    {
        MyDbContext dbContext1;
        Building SeletedBuilding = new Building();
        Room RoomToEdit = new Room();

        public AddNewRoomWindow(MyDbContext dbContext, Room room)
        {
            RoomToEdit = room;
            dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();

            if (RoomToEdit != null)
            {
                EnableUpdateMode();
            }
        }

        private void GetBuildings()
        {
            CBBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }

        private void EnableUpdateMode()
        {
            TxtRid.Text = RoomToEdit.Rid;
            TxtCapacity.Text = RoomToEdit.Capacity.ToString();
            CBBuilding.SelectedItem = RoomToEdit.BuildingAS;
            CBType.SelectedValue = RoomToEdit.Type;

        }

        private void AddRoom(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Room NewRoom = new Room();

                NewRoom.Rid = TxtRid.Text;
                NewRoom.Capacity = int.Parse(TxtCapacity.Text);
                NewRoom.BuildingAS = (Building)CBBuilding.SelectedItem;
                NewRoom.Type = CBType.Text;

                //insert that object to database
                if (dbContext1.Rooms.Any(r => r.Rid == TxtRid.Text))
                {
                    MessageBox.Show("This ID Already In the System Use a Different ID",
                                "Duplicate Room ID",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                }
                else
                {

                    dbContext1.Rooms.Add(NewRoom);
                    dbContext1.SaveChanges();
                    MessageBox.Show("Successfully Added to the System",
                        "Room Added!!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                    TxtCapacity.Text = "";
                    TxtRid.Text = "";
                    CBBuilding.SelectedIndex = -1;
                    CBType.SelectedIndex = -1;


                }
            }
            else
            {
                MessageBox.Show("Please Complete Room Details to Continue !",
                                    "Incomplete Details",
                                   MessageBoxButton.OK,
                                 MessageBoxImage.Warning);
            }

        }


        private void Close(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (TxtRid.Text.Trim() == "")
            {
                TxtRid.Focus();
                return false;
            }

            if (TxtCapacity.Text.Trim() == "")
            {
                TxtCapacity.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(CBType.Text))
            {
                CBType.Focus();
                return false;
            }

            if (CBBuilding.SelectedIndex == -1)
            {
                CBBuilding.Focus();
                return false;
            }

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for LocationsWindow.xaml
    /// </summary>
    public partial class LocationsWindow : Window
    {
        MyDbContext dbContext1;
        Room SelectedRoom = null;
        public LocationsWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
            GetRooms();

        }

        private void GetRooms()
        {
            RoomsDG.ItemsSource = dbContext1.Rooms.Include(r => r.BuildingAS);

        }

        public void AddNewRoom(Object s, RoutedEventArgs e)
        {

            AddNewRoomWindow addNewRoomWindow = new AddNewRoomWindow(dbContext1,null);
            addNewRoomWindow.Closed += AddClosed;
            addNewRoomWindow.ShowDialog();

        }

        public void UpdateRoom(Object s, RoutedEventArgs e)
        {
            if (SelectedRoom == null) {
                MessageBox.Show("Please Select A Room from the table before Updating!",
                               "No Selection",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
            AddNewRoomWindow addNewRoomWindow = new AddNewRoomWindow(dbContext1,SelectedRoom);
            addNewRoomWindow.Show();
        }

        public void DeleteRoom(Object s, RoutedEventArgs e)
        {
            BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
            buildingsWindow.Show();
        }

        public void ManageBuilding(Object s, RoutedEventArgs e)
        {
            BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
            buildingsWindow.Show();
        }

        private void UpdateSelection(Object s, RoutedEventArgs e)
        {


            if (RoomsDG.SelectedItem != null)
            {
                try
                {
                    SelectedRoom = (Room)RoomsDG.SelectedItem;
                    TxtBuilding.Text = SelectedRoom.BuildingAS.Name;
                    TxtCapacity.Text =  SelectedRoom.Capacity.ToString();
                    TxtRid.Text = SelectedRoom.Rid;
                    TxtType.Text = SelectedRoom.Type;
                }
                catch (Exception ex)
                {
                    SelectedRoom = null;
                }
            }
            else
            {
                TxtBuilding.Text = "";
                TxtCapacity.Text = "";
                TxtRid.Text = "";
                TxtType.Text = "";
            }



        }

        public void AddClosed(object sender, System.EventArgs e)
        {
            //This gets fired off
            GetRooms();
        }


    }
}

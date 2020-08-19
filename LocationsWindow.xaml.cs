using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        ICollection<Room> Rooms;
        public ObservableCollection<Room> RoomCollection { get; set; }

        public LocationsWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
            GetRooms();
            GetBuildings();
        }

        private void GetRooms()
        {
            if (Rooms != null)
            {
                Rooms.Clear();
            }
            Rooms = dbContext1.Rooms.Include(r => r.BuildingAS).ToList();

            RoomsDG.ItemsSource = new ObservableCollection<Room>(Rooms);

        }

        private void GetBuildings()
        {
            CBSearchBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }

        public void AddNewRoom(Object s, RoutedEventArgs e)
        {

            AddNewRoomWindow addNewRoomWindow = new AddNewRoomWindow(dbContext1, null);
            addNewRoomWindow.Closed += AddClosed;
            addNewRoomWindow.ShowDialog();

        }

        public void UpdateRoom(Object s, RoutedEventArgs e)
        {
            if (SelectedRoom == null)
            {
                MessageBox.Show("Please Select A Room from the table before Updating!",
                               "No Selection",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
            else
            {
                AddNewRoomWindow addNewRoomWindow = new AddNewRoomWindow(dbContext1, SelectedRoom);
                addNewRoomWindow.Closed += AddClosed;
                addNewRoomWindow.ShowDialog();
            }
        }

        public void DeleteRoom(Object s, RoutedEventArgs e)
        {
            //insert that object to database
            if (SelectedRoom == null)
            {
                MessageBox.Show("Please Select a Room to Delete",
                                     "No Selection",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
            }
            else
            {

                dbContext1.Rooms.Remove(SelectedRoom);
                dbContext1.SaveChanges();
                GetRooms();
            }


        }



        public void ManageBuilding(Object s, RoutedEventArgs e)
        {
            BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
            buildingsWindow.Closed += AddClosed;
            buildingsWindow.ShowDialog();
        }

        private void UpdateSelection(Object s, RoutedEventArgs e)
        {


            if (RoomsDG.SelectedItem != null)
            {
                try
                {
                    SelectedRoom = (Room)RoomsDG.SelectedItem;
                    TxtBuilding.Text = SelectedRoom.BuildingAS.Name;
                    TxtCapacity.Text = SelectedRoom.Capacity.ToString();
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

        private void SerachById(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            var _itemSourceList = new CollectionViewSource() { Source = Rooms };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // your Filter
            var yourCostumFilter = new Predicate<object>(item => ((Room)item).Rid.ToLower().Contains(txtSearchId.Text.ToLower()));

            //now we add our Filter
            Itemlist.Filter = yourCostumFilter;

            RoomsDG.ItemsSource = Itemlist;

        }


        private void SearchByType(Object s, RoutedEventArgs e)
        {
            if (CBSearchType.SelectedItem != null)
            {
                // Collection which will take your ObservableCollection
                var _itemSourceList = new CollectionViewSource() { Source = Rooms };

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                // your Filter
                var yourCostumFilter = new Predicate<object>(item => ((Room)item).Type.Contains(CBSearchType.Text.ToString()));

                //now we add our Filter
                Itemlist.Filter = yourCostumFilter;

                RoomsDG.ItemsSource = Itemlist;
            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }
        }

        private void SearchByBuilding(Object s, RoutedEventArgs e)

        {
            if (CBSearchBuilding.SelectedItem != null)
            {
                // Collection which will take your ObservableCollection
                var _itemSourceList = new CollectionViewSource() { Source = Rooms };

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                // your Filter
                var yourCostumFilter = new Predicate<object>(item => ((Room)item).BuildingAS == CBSearchBuilding.SelectedItem);

                //now we add our Filter
                Itemlist.Filter = yourCostumFilter;

                RoomsDG.ItemsSource = Itemlist;
            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }


        }


    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
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
    /// Interaction logic for BuildingsWindow.xaml
    /// </summary>
    public partial class BuildingsWindow : Window
    {
        MyDbContext dbContext1;
        Building SeletedBuilding = new Building();
        ICollection<Building> buildings;

        public BuildingsWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();
        }

        private void GetBuildings()
        {
            if (buildings != null)
            {
                buildings.Clear();
            }

            buildings = dbContext1.Buildings.ToList();
            BuildingDG.ItemsSource = buildings;


        }

        private void AddBuilding(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Building NewBuilding = new Building();

                NewBuilding.Bid = txtId.Text;
                NewBuilding.Name = txtName.Text;
                //insert that object to database
                if (dbContext1.Buildings.Any(b => b.Bid == txtId.Text))
                {

                    new MessageBoxCustom("This ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();
                }
                else
                {

                    dbContext1.Buildings.Add(NewBuilding);
                    dbContext1.SaveChanges();
                    GetBuildings();
                }
            }
            else
            {

                new MessageBoxCustom("Please Complete Building Details to Continue !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

            }

        }

        private void UpdateBuilding(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                if (SeletedBuilding == null)
                {
                    new MessageBoxCustom("Select a Building to update !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

                }
                else
                {
                    //insert that object to database
                    if (SeletedBuilding.Bid != txtId.Text && dbContext1.Buildings.Any(b => b.Bid == txtId.Text))
                    {


                        new MessageBoxCustom("This ID Already In the System Use a Different ID !", MessageType.Error, MessageButtons.Ok).ShowDialog();

                    }
                    else
                    {
                        SeletedBuilding.Bid = txtId.Text;
                        SeletedBuilding.Name = txtName.Text;
                        //  var query = $"UPDATE Buildings SET Id = '{txtId.Text}', Name = '{txtName.Text}' WHERE Id='{SeletedBuilding.Id}'";
                        //  BuildingDG.SelectedItem = NewBuilding;

                        // dbContext1.Database.ExecuteSqlRaw(query);
                        dbContext1.Buildings.Update(SeletedBuilding);
                        dbContext1.SaveChanges();
                        GetBuildings();

                    }

                }
            }
            else
            {
                new MessageBoxCustom("Please Complete Building Details to Continue !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

            }
        }

        private void UpdateSelection(Object s, RoutedEventArgs e)
        {


            if (BuildingDG.SelectedItem != null)
            {
                try
                {
                    SeletedBuilding = (Building)BuildingDG.SelectedItem;
                    txtId.Text = SeletedBuilding.Bid;
                    txtName.Text = SeletedBuilding.Name;
                }
                catch (Exception ex)
                {
                    SeletedBuilding = null;
                }
            }
            else
            {
                txtId.Text = "";
                txtName.Text = "";
            }



        }


        private void DeleteBuilding(Object s, RoutedEventArgs e)
        {

            //insert that object to database
            if (SeletedBuilding == null)
            {
                new MessageBoxCustom("Please Select a Builidng to Delete", MessageType.Error, MessageButtons.Ok).ShowDialog();

            }
            else
            {
                bool? Result = new MessageBoxCustom("Are you sure, You want to Delete This Building ? ",
                   MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

                if (Result.Value)
                {
                    dbContext1.Buildings.Remove(SeletedBuilding);
                    dbContext1.SaveChanges();
                    GetBuildings();
                }
            }


        }

        private bool ValidateInput()
        {
            if (txtId.Text.Trim() == "")
            {
                txtId.Focus();
                return false;
            }

            if (txtName.Text.Trim() == "")
            {
                txtName.Focus();
                return false;
            }

            return true;
        }

        private void SerachById(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            var _itemSourceList = new CollectionViewSource() { Source = buildings };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // your Filter
            var yourCostumFilter = new Predicate<object>(item => ((Building)item).Bid.ToLower().Contains(txtSearchId.Text.ToLower()));

            //now we add our Filter
            Itemlist.Filter = yourCostumFilter;

            BuildingDG.ItemsSource = Itemlist;

        }

        private void SerachByName(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            var _itemSourceList = new CollectionViewSource() { Source = buildings };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // your Filter
            var yourCostumFilter = new Predicate<object>(item => ((Building)item).Name.ToLower().Contains(txtSearchName.Text.ToLower()));

            //now we add our Filter
            Itemlist.Filter = yourCostumFilter;

            BuildingDG.ItemsSource = Itemlist;

        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }

    }
}

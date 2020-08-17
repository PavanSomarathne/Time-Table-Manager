using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        myDbContext dbContext1;
        Building SeletedBuilding = new Building();

        public BuildingsWindow(myDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();
        }

        private void GetBuildings()
        {
            BuildingDG.ItemsSource = dbContext1.Buildings.ToList();
  

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
                    MessageBox.Show("This ID Already In the System Use a Different ID",
                                         "Duplicate Building ID",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Error);
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
                MessageBox.Show("Please Complete Building Details to Continue !",
                              "Incomplete Details",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }

        }

        private void UpdateBuilding(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                if (SeletedBuilding == null)
                {
                    MessageBox.Show("Select a Building to update!", "No Selection",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                }
                else
                {
                    //insert that object to database
                    if (SeletedBuilding.Bid != txtId.Text && dbContext1.Buildings.Any(b => b.Bid == txtId.Text))
                    {
                        MessageBox.Show("This ID Already In the System Use a Different ID",
                                             "Duplicate Building ID",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Error);
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
                MessageBox.Show("Please Complete Building Details to Continue !",
                              "Incomplete Details",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
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
                MessageBox.Show("This ID Already In the System Use a Different ID",
                                     "Duplicate Building ID",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
            }
            else
            {

                dbContext1.Buildings.Remove(SeletedBuilding);
                dbContext1.SaveChanges();
                GetBuildings();
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

    }
}

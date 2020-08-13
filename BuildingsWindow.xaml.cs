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
    /// Interaction logic for BuildingsWindow.xaml
    /// </summary>
    public partial class BuildingsWindow : Window
    {
        MyDbContext dbContext1;
        Building NewSchedule = new Building();
        Building SelectedBuilding = new Building();
       
        public BuildingsWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();
        }

        private void GetBuildings()
        {
            BuildingDG.ItemsSource = dbContext1.Buildings.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
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
        myDbContext dbContext1;
        public LocationsWindow(myDbContext dbContext)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
  
        }


    }
}

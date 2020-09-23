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
    /// Interaction logic for ViewNotAvailableLecturersWindow.xaml
    /// </summary>
    public partial class ViewNotAvailableLecturersWindow : Window
    {
        private MyDbContext dbContext1;

        public ViewNotAvailableLecturersWindow()
        {
            InitializeComponent();
        }

        public ViewNotAvailableLecturersWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
        }
    }
}

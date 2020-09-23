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
    /// Interaction logic for ParallelSessionsWindow.xaml
    /// </summary>
    public partial class ParallelSessionsWindow : Window
    {

        private MyDbContext dbContext1;
        public ParallelSessionsWindow()
        {
            InitializeComponent();
        }

        public ParallelSessionsWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();

        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }
    }
}

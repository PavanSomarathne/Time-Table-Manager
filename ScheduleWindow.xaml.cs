using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using TimeTableManager.Models;

namespace TimeTableManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        MyDbContext dbContext1;
        Schedule NewSchedule=new Schedule();
        Schedule SelectedSchedule=new Schedule();
        List<string> allChecked = new List<string>();


        public ScheduleWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetSchedule();

            AddNewScheduleGrid.DataContext = NewSchedule;
        }

        private void GetSchedule() 
        {
            ScheduleDG.ItemsSource = dbContext1.Schedules.ToList();
        }

        private void AddSchedule(Object s, RoutedEventArgs e)
        {
            //Checkbox manipulation
            var checkString = "";
            if (allChecked.Count == 0)
            { 
                MessageBox.Show("No Strings Checked");
            }
            else
            {
                for (int i = 0; i < allChecked.Count; i++)
                {
                    if (i == 0)
                    {
                        checkString = allChecked[i];
                    }
                    else {
                        checkString = checkString + "," + allChecked[i];
                    }
                }
               
            }
           //insert checkbox values to object
            NewSchedule.Working_days = checkString;

            //insert that object to database
            dbContext1.Schedules.Add(NewSchedule);
            dbContext1.SaveChanges();
            GetSchedule();
            NewSchedule = new Schedule();
            AddNewScheduleGrid.DataContext = NewSchedule;
        }
        private void myCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            allChecked.Add(cb.Content.ToString());
        }
        private void myCheckBox_Unchecked(object sender, RoutedEventArgs e)

        {
            CheckBox cb = sender as CheckBox;
            allChecked.Remove(cb.Content.ToString());
        }

        private void UpdateScheduleForEdit(Object s, RoutedEventArgs e)
        {

            SelectedSchedule = (s as FrameworkElement).DataContext as Schedule;
            AddNewScheduleGrid.DataContext = SelectedSchedule;
        }

        private void UpdateSchedule(Object s, RoutedEventArgs e)
        {
            dbContext1.Update(SelectedSchedule);
            dbContext1.SaveChanges();
            GetSchedule();
        }

        private void DeleteSchedule(Object s, RoutedEventArgs e)
        {
            SelectedSchedule = (s as FrameworkElement).DataContext as Schedule;
            dbContext1.Remove(SelectedSchedule);
            dbContext1.SaveChanges();
            GetSchedule();
        }

    }
}

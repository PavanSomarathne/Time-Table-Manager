using System;
using System.Collections;
using System.Collections.Generic;
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
        Schedule NewSchedule = new Schedule();
        Schedule SelectedSchedule = new Schedule();
        List<string> allChecked = new List<string>();


        public ScheduleWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            this.duration.SelectedIndex = 0;
            ResetButton.IsEnabled = false;
            GetSchedule();

            AddNewScheduleGrid.DataContext = NewSchedule;
        }


        private void GetSchedule()
        {
            ScheduleDG.ItemsSource = dbContext1.Schedules.ToList();
        }

        private void AddSchedule(Object s, RoutedEventArgs e)
        {
            Boolean validator=true;
     
            //Checkbox manipulation
            var checkString = "";
            if (allChecked.Count == 0)
            {
                validator = false;
                MessageBox.Show("No working days Checked");
            }
            else
            {
               
                for (int i = 0; i < allChecked.Count; i++)
                {
                    if (i == 0)
                    {
                        checkString = allChecked[i];
                    }
                    else
                    {
                        checkString = checkString + "," + allChecked[i];
                    }
                }

            }

            //validations
            if (allChecked.Count != NewSchedule.Working_days_count) {
                validator = false;
                MessageBox.Show("Please check the same number of days that you have entered in Working days per week count!");
                
            }
            if (NewSchedule.working_time_hrs>14 || NewSchedule.working_time_hrs<1)
            {
                validator = false;
                MessageBox.Show("Working hours out of range");
            }
            if (NewSchedule.Working_time_mins >= 60 || NewSchedule.Working_time_mins< 0)
            {
                validator = false;
                MessageBox.Show("Working minutes out of range");
                
            }
            if (NewSchedule.Working_duration==null)
            {
                validator = false;
                MessageBox.Show("Please select a timeslot duration!");

            }
            if (validator) {
                //insert checkbox values to object
                NewSchedule.Working_days = checkString;
                NewSchedule.Working_duration = duration.Text;

                //insert that object to database
                dbContext1.Schedules.Add(NewSchedule);
                dbContext1.SaveChanges();

                MondayCheckBox.IsChecked = false;
                TuesdayCheckBox.IsChecked = false;
                WednessdayCheckBox.IsChecked = false;
                ThursdayCheckBox.IsChecked = false;
                FridayCheckBox.IsChecked = false;
                SaturdayCheckBox.IsChecked = false;
                SundayCheckBox.IsChecked = false;

                GetSchedule();
                NewSchedule = new Schedule();
                AddNewScheduleGrid.DataContext = NewSchedule;

            }
            
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
            ResetButton.IsEnabled = true;

            AddButton.IsEnabled = false;
            MondayCheckBox.IsChecked = false;
            TuesdayCheckBox.IsChecked = false;
            WednessdayCheckBox.IsChecked = false;
            ThursdayCheckBox.IsChecked = false;
            FridayCheckBox.IsChecked = false;
            SaturdayCheckBox.IsChecked = false;
            SundayCheckBox.IsChecked = false;

            SelectedSchedule = (s as FrameworkElement).DataContext as Schedule;
 
           
            var arr=SelectedSchedule.Working_days.Split(',') ;
            
            
          // _ = MessageBox.Show((string)arr[0]);

            foreach (var item in arr)
            {
                if (item == "Monday")
                {
                    MondayCheckBox.IsChecked = true;                   
                }
                else if (item == "Tuesday")
                {
                    TuesdayCheckBox.IsChecked = true;
                }
                else if (item == "Wednessday")
                {
                    WednessdayCheckBox.IsChecked = true;
                }
                else if (item == "Thursday")
                {
                    ThursdayCheckBox.IsChecked = true;
                }
                else if (item == "Friday")
                {
                    FridayCheckBox.IsChecked = true;
                }
                else if (item == "Saturday")
                {
                    SaturdayCheckBox.IsChecked = true;
                }
                else if (item == "Sunday")
                {
                    SundayCheckBox.IsChecked = true;
                }
            }

          
            AddNewScheduleGrid.DataContext = SelectedSchedule;
        }

        private void UpdateSchedule(Object s, RoutedEventArgs e)
        {
            Boolean validator = true;
            //Checkbox manipulation
            var checkString = "";
            if (allChecked.Count == 0)
            {
                validator = false;
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
                    else
                    {
                        checkString = checkString + "," + allChecked[i];
                    }
                }

            }

            //validations
            if (allChecked.Count != SelectedSchedule.Working_days_count)
            {
                validator = false;
                MessageBox.Show("Please check the same number of days that you have entered in Working days per week count!");

            }
            if (SelectedSchedule.working_time_hrs > 14 || SelectedSchedule.working_time_hrs < 1)
            {
                validator = false;
                MessageBox.Show("Working hours out of range");
            }
            if (SelectedSchedule.Working_time_mins >= 60 || SelectedSchedule.Working_time_mins < 0)
            {
                validator = false;
                MessageBox.Show("Working minutes out of range");

            }
            if (SelectedSchedule.Working_duration == null)
            {
                validator = false;
                MessageBox.Show("Please select a timeslot duration!");

            }

            if (validator) 
            {
                SelectedSchedule.Working_days = checkString;
                SelectedSchedule.Working_duration = duration.Text;

                dbContext1.Update(SelectedSchedule);
                dbContext1.SaveChanges();
                MondayCheckBox.IsChecked = false;
                TuesdayCheckBox.IsChecked = false;
                WednessdayCheckBox.IsChecked = false;
                ThursdayCheckBox.IsChecked = false;
                FridayCheckBox.IsChecked = false;
                SaturdayCheckBox.IsChecked = false;
                SundayCheckBox.IsChecked = false;

                GetSchedule();
                NewSchedule = new Schedule();
                AddNewScheduleGrid.DataContext = NewSchedule;
                AddButton.IsEnabled = false;

            }

            
        }

        private void DeleteSchedule(Object s, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SelectedSchedule = (s as FrameworkElement).DataContext as Schedule;
                dbContext1.Remove(SelectedSchedule);
                dbContext1.SaveChanges();

                MondayCheckBox.IsChecked = false;
                TuesdayCheckBox.IsChecked = false;
                WednessdayCheckBox.IsChecked = false;
                ThursdayCheckBox.IsChecked = false;
                FridayCheckBox.IsChecked = false;
                SaturdayCheckBox.IsChecked = false;
                SundayCheckBox.IsChecked = false;

                GetSchedule();
                NewSchedule = new Schedule();
                AddNewScheduleGrid.DataContext = NewSchedule;
                AddButton.IsEnabled = true;
                ResetButton.IsEnabled = false;
            }
        }

        private void ResetSchedule(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Reset Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                NewSchedule = new Schedule();
                AddNewScheduleGrid.DataContext = NewSchedule;
                MondayCheckBox.IsChecked = false;
                TuesdayCheckBox.IsChecked = false;
                WednessdayCheckBox.IsChecked = false;
                ThursdayCheckBox.IsChecked = false;
                FridayCheckBox.IsChecked = false;
                SaturdayCheckBox.IsChecked = false;
                SundayCheckBox.IsChecked = false;

                AddButton.IsEnabled = true;
                ResetButton.IsEnabled = false;
            }

            
        }
    }
}
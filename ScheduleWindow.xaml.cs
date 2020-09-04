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
        Schedule wholeSchedule = new Schedule();
        Schedule SelectedSchedule = new Schedule();
        List<string> allChecked = new List<string>();


        public ScheduleWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            this.duration.SelectedIndex = 0;
           
            GetSchedule();

            UpdateButton.IsEnabled = false;


            // AddNewScheduleGrid.DataContext = NewSchedule;
        }


        private void GetSchedule()
        {
            ScheduleDG.ItemsSource = dbContext1.Schedules.ToList();
        }

        private void AddSchedule(Object s, RoutedEventArgs e)
        {
            var checkString = "";
            int val=0;
            int val1=0;
            int val2=0;
            int val3=0;
            Boolean validator1 = true;
            Boolean validator2 = true;
            Boolean validator3 = true;
            Boolean validator4 = true;
            Boolean validator5 = true;

            if (int.TryParse(Working_days_count.Text,out val) && int.TryParse(working_hrs.Text, out val1) && int.TryParse(working_mins.Text, out val2) && duration.Text.ToString()!="" )
            {


                //NewSchedule.Working_days_count = int.Parse(Working_days_count.Text);
                //NewSchedule.working_time_hrs = val1;
                //NewSchedule.Working_time_mins = int.Parse(this.working_mins.Text);
                //NewSchedule.Working_duration = duration.Text.ToString();
                
                NewSchedule.Working_days_count = val;
                NewSchedule.working_time_hrs = val1;
                NewSchedule.Working_time_mins = val2;
                NewSchedule.Working_duration = duration.Text.ToString();



                //Checkbox manipulation

                if (allChecked.Count == 0)
                {
                    validator1 = false;
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
                if (allChecked.Count != NewSchedule.Working_days_count)
                {
                    validator2 = false;
                    MessageBox.Show("Please check the same number of days that you have entered in Working days per week count!");

                }
                if (NewSchedule.working_time_hrs > 14 || NewSchedule.working_time_hrs < 1)
                {
                    validator3 = false;
                    MessageBox.Show("Working hours out of range");
                }
                if (NewSchedule.Working_time_mins >= 60 || NewSchedule.Working_time_mins < 0)
                {
                    validator4 = false;
                    MessageBox.Show("Working minutes out of range");

                }
                if (NewSchedule.Working_duration == null)
                {
                    validator5 = false;
                    MessageBox.Show("Please select a timeslot duration!");

                }

                if (validator1 && validator2 && validator3 && validator4 && validator5)
                {
                    //insert checkbox values to object
                    NewSchedule.Working_days = checkString;
                    NewSchedule.Working_duration = duration.Text;

                    //insert that object to database
                    dbContext1.Schedules.Add(NewSchedule);
                    dbContext1.SaveChanges();
                    MessageBox.Show("Schedule Added!");

                    MondayCheckBox.IsChecked = false;
                    TuesdayCheckBox.IsChecked = false;
                    WednessdayCheckBox.IsChecked = false;
                    ThursdayCheckBox.IsChecked = false;
                    FridayCheckBox.IsChecked = false;
                    SaturdayCheckBox.IsChecked = false;
                    SundayCheckBox.IsChecked = false;

                    GetSchedule();

                    AddNewScheduleGrid.DataContext = null;

                }
            }
            else {

                MessageBox.Show("Plese fill the requested fields with correct format!");

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
            UpdateButton.IsEnabled = true;

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
            Boolean validator1 = true;
            Boolean validator2 = true;
            Boolean validator3 = true;
            Boolean validator4 = true;
            Boolean validator5 = true;
            //Checkbox manipulation
            var checkString = "";
            if (allChecked.Count == 0)
            {
                validator1 = false;
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
                validator2 = false;
                MessageBox.Show("Please check the same number of days that you have entered in Working days per week count!");

            }
            if (SelectedSchedule.working_time_hrs > 14 || SelectedSchedule.working_time_hrs < 1)
            {
                validator3 = false;
                MessageBox.Show("Working hours out of range");
            }
            if (SelectedSchedule.Working_time_mins >= 60 || SelectedSchedule.Working_time_mins < 0)
            {
                validator4 = false;
                MessageBox.Show("Working minutes out of range");

            }
            if (SelectedSchedule.Working_duration == null)
            {
                validator5 = false;
                MessageBox.Show("Please select a timeslot duration!");

            }

            if (validator1 && validator2 && validator3 && validator4 && validator5) 
            {
                SelectedSchedule.Working_days = checkString;
                SelectedSchedule.Working_duration = duration.Text;

                dbContext1.Update(SelectedSchedule);
                dbContext1.SaveChanges();
                MessageBox.Show("Schedule Updated!");
                MondayCheckBox.IsChecked = false;
                TuesdayCheckBox.IsChecked = false;
                WednessdayCheckBox.IsChecked = false;
                ThursdayCheckBox.IsChecked = false;
                FridayCheckBox.IsChecked = false;
                SaturdayCheckBox.IsChecked = false;
                SundayCheckBox.IsChecked = false;

                GetSchedule();
                
                AddNewScheduleGrid.DataContext = null;
                AddButton.IsEnabled = true;
                UpdateButton.IsEnabled = false;

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
                MessageBox.Show("Schedule Deleted!");
                MondayCheckBox.IsChecked = false;
                TuesdayCheckBox.IsChecked = false;
                WednessdayCheckBox.IsChecked = false;
                ThursdayCheckBox.IsChecked = false;
                FridayCheckBox.IsChecked = false;
                SaturdayCheckBox.IsChecked = false;
                SundayCheckBox.IsChecked = false;

                GetSchedule();
                
                AddNewScheduleGrid.DataContext = null;
                AddButton.IsEnabled = true;
                ResetButton.IsEnabled = false;
            }
        }

        private void ResetSchedule(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Reset Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                NewSchedule = new Schedule();
                AddNewScheduleGrid.DataContext = null;
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
        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }

    }
}
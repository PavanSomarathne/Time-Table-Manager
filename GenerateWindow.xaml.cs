using LiveCharts.Dtos;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
    public partial class GenerateWindow : Window
    {
        MyDbContext dbContext1;
        Schedule NewSchedule = new Schedule();
        Schedule wholeSchedule = new Schedule();
        Schedule SelectedSchedule = new Schedule();
        List<string> allChecked = new List<string>();
        DataTable dt;

        public GenerateWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            //GetSchedule();
            studenSP.Visibility = Visibility.Collapsed;
            lecturerSP.Visibility = Visibility.Collapsed;
            roomSP.Visibility = Visibility.Collapsed;

           
            // Program.DisplayMember = "Name";
            //comboBox1.ValueMember = "Name";
            // AddNewScheduleGrid.DataContext = NewSchedule;
        }
       
  
        private void GetSchedule()
        {
           // ScheduleDG.ItemsSource = dbContext1.Schedules.ToList();
        }

        private void AddSchedule(Object s, RoutedEventArgs e)
        {
            Schedule AddSchedule = new Schedule();
            var checkString = "";
            int val = 0;
            int val1 = 0;
            int val2 = 0;

            Boolean validator1 = true;
            Boolean validator2 = true;
            Boolean validator3 = true;
            Boolean validator4 = true;
            Boolean validator5 = true;

                //NewSchedule.Working_days_count = int.Parse(Working_days_count.Text);
                //NewSchedule.working_time_hrs = val1;
                //NewSchedule.Working_time_mins = int.Parse(this.working_mins.Text);
                //NewSchedule.Working_duration = duration.Text.ToString();

                AddSchedule.Working_days_count = val;
                AddSchedule.working_time_hrs = val1;
                AddSchedule.Working_time_mins = val2;

                //Checkbox manipulation
                if (allChecked.Count == 0)
                {
                    validator1 = false;
                    MessageBox.Show("No working days Checked !",
                  "", MessageBoxButton.OK, MessageBoxImage.Warning);

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
                if (DateTime.ParseExact(AddSchedule.start_time, "h:mm tt", CultureInfo.InvariantCulture) >
                    DateTime.ParseExact("04:00 PM", "hh:mm tt", CultureInfo.InvariantCulture))
                {
                    validator2 = false;
                    MessageBox.Show("Too late for start time!",
                   "", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                if (allChecked.Count != AddSchedule.Working_days_count)
                {
                    validator2 = false;
                    MessageBox.Show("Please check the same number of days that you have entered in Working days per week count!",
                   "", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                if (AddSchedule.working_time_hrs > 14 || AddSchedule.working_time_hrs < 1)
                {
                    validator3 = false;
                    MessageBox.Show("Working hours out of range!",
                  "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (AddSchedule.Working_time_mins >= 60 || AddSchedule.Working_time_mins < 0)
                {
                    validator4 = false;
                    MessageBox.Show("Working hours out of range!",
                 "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (AddSchedule.Working_duration == null)
                {
                    validator5 = false;
                    MessageBox.Show("Please select a timeslot duration!",
                                     "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (validator1 && validator2 && validator3 && validator4 && validator5)
                {
                    //insert new values
                    //insert checkbox values to object
                    AddSchedule.Working_days = checkString;

                    //insert that object to database
                    dbContext1.Schedules.Add(AddSchedule);
                    dbContext1.SaveChanges();
                    MessageBox.Show("Schedule Added Successfully!",
                                 "", MessageBoxButton.OK, MessageBoxImage.Information);

                    ResetForm();
                    GetSchedule();
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

        private void UpdateSchedule(Object s, RoutedEventArgs e)
        {
            int val = 0;
            int val1 = 0;
            int val2 = 0;

            Boolean validator1 = true;
            Boolean validator2 = true;
            Boolean validator3 = true;
            Boolean validator4 = true;
            Boolean validator5 = true;
                
                SelectedSchedule.Working_days_count = val;
               // SelectedSchedule.start_time = PresetTimePicker.Text.ToString();
                SelectedSchedule.working_time_hrs = val1;
                SelectedSchedule.Working_time_mins = val2;

                //Checkbox manipulation
                var checkString = "";
                if (allChecked.Count == 0)
                {
                    validator1 = false;
                    MessageBox.Show("No working days Checked !",
                     "", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (DateTime.ParseExact(SelectedSchedule.start_time, "h:mm tt", CultureInfo.InvariantCulture) >
                DateTime.ParseExact("04:00 PM", "hh:mm tt", CultureInfo.InvariantCulture))
                {
                    validator2 = false;
                    MessageBox.Show("Too late for start time!",
                   "", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                if (allChecked.Count != SelectedSchedule.Working_days_count)
                {
                    validator2 = false;
                    MessageBox.Show("Please check the same number of days that you have entered in Working days per week count!",
                   "", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                if (SelectedSchedule.working_time_hrs > 14 || SelectedSchedule.working_time_hrs < 1)
                {
                    validator3 = false;
                    MessageBox.Show("Working hours out of range!",
                  "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (SelectedSchedule.Working_time_mins >= 60 || SelectedSchedule.Working_time_mins < 0)
                {
                    validator4 = false;
                    MessageBox.Show("Working hours out of range!",
                 "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (SelectedSchedule.Working_duration == null)
                {
                    validator5 = false;
                    MessageBox.Show("Please select a timeslot duration!",
                                     "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (validator1 && validator2 && validator3 && validator4 && validator5)
                {
                    SelectedSchedule.Working_days = checkString;
                    

                    dbContext1.Update(SelectedSchedule);
                    dbContext1.SaveChanges();
                    MessageBox.Show("Schedule Updated Successfully!",
                                     "", MessageBoxButton.OK, MessageBoxImage.Information);

                    ResetForm();
                    GetSchedule();
                }
        }

        private void DeleteSchedule(Object s, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SelectedSchedule = (s as FrameworkElement).DataContext as Schedule;
                dbContext1.Remove(SelectedSchedule);
                dbContext1.SaveChanges();
                MessageBox.Show("Schedule Deleted Successfully!",
                                 "", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetForm();
                GetSchedule();

            }
        }

        private void ResetSchedule(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Reset Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                NewSchedule = new Schedule();
                AddNewScheduleGrid.DataContext = null;

            }


        }
        //time picker
        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }

        private void ResetForm()
        {
            AddNewScheduleGrid.DataContext = null;
           

            AddNewScheduleGrid.DataContext = null;
        }

       

        private StackPanel GetStudenSP()
        {
            return studenSP;
        }

        private void TypeChange(object sender, EventArgs e)
        {
            if (sType.Text == "Student")
            {
                GetStudenSP().Visibility = Visibility.Visible;
                lecturerSP.Visibility = Visibility.Collapsed;
                roomSP.Visibility = Visibility.Collapsed;
            }
            else if (sType.Text == "Lecturer")
            {
                studenSP.Visibility = Visibility.Collapsed;
                lecturerSP.Visibility = Visibility.Visible;
                roomSP.Visibility = Visibility.Collapsed;
            }
            else if (sType.Text == "Room")
            {
                studenSP.Visibility = Visibility.Collapsed;
                lecturerSP.Visibility = Visibility.Collapsed;
                roomSP.Visibility = Visibility.Visible;
            }

        }

        

        private void lecturer_load(object sender, RoutedEventArgs e)
        {
            List<String> lecString = new List<String>();
            List<LecturerDetails> lecturers = dbContext1.LectureInformation.ToList();

            foreach (var item in lecturers)
            {
                lecString.Add(item.Id + "-" + item.LecName);
            }
            Lecturer.ItemsSource = lecString;
        }

        private void building_load(object sender, RoutedEventArgs e)
        {
            List<String> buildings = dbContext1.Buildings.Select(n => n.Name).ToList();
            Building.ItemsSource = buildings;
        }


        private void room_load(object sender, EventArgs e)
        {
            String build = Building.Text;
            List<String> roomString = new List<String>();
            List<Room> rooms = dbContext1.Rooms.Include("BuildingAS").Where(b => b.BuildingAS.Name == build).ToList();

            foreach (var item in rooms)
            {
                roomString.Add(item.Id + "-" + item.Rid);
            }
            Room.ItemsSource = roomString;
        }
        DataRow dr;
        private void Table_loaded(object sender, RoutedEventArgs e)
        {
            int dura = 0;
            //new schedule object
            Schedule NewSchedule = new Schedule();
            //Table initialization
            dt = new DataTable("emp");
            //Time heading
            DataColumn dc1 = new DataColumn("Time", typeof(string));
            dt.Columns.Add(dc1);

            NewSchedule =dbContext1.Schedules.Find(1);
            DateTime startTime = DateTime.ParseExact(NewSchedule.start_time, "h:mm tt", CultureInfo.InvariantCulture);
            int worktimeHrs= NewSchedule.working_time_hrs;
            int worktimeMins = NewSchedule.Working_time_mins;
            String duration = NewSchedule.Working_duration;

            //Time slots
            DateTime endTime= startTime.AddHours(worktimeHrs);
            endTime = endTime.AddMinutes(worktimeMins);

            DateTime timeVal=startTime;

            if (duration == "One Hour")
            {
                dura = 60;
            }

            else if (duration == "Thirty Minutes" )
            {
                dura = 30;
            }

                while (timeVal.AddMinutes(dura) <= endTime)
                {

                dr = dt.NewRow();
                timeVal = timeVal.AddMinutes(dura);
                dr["Time"] = timeVal.ToString("h:mm tt");
                dt.Rows.Add(dr);
 
            }



            var arr = NewSchedule.Working_days.Split(',');

            
           

            String[] arr1 = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            foreach (var data in arr1)
            {
                foreach (var item in arr)
                {
                    if (data == item)
                    {
                        dt.Columns.Add(new DataColumn(data, typeof(string)));
                    }           
                }
            }
            

            


            

            //TimeTableDG.ItemsSource = dt.DefaultView;

           // dr = dt.NewRow();

          // dr["Time"] = "Hello";
           // dr["Time"] = "Hello";
           // foreach (DataRow dr in dt.Rows)
           // {
             //   dr["Time"] = "Hello";
           // }
            //  dr[1] = "hello";
            //dr[2] = "pavanpabs";
            //   dr[3] = "Kurunegala";
            //dr[4] = "hello";
            // dr[5] = "pavanpabs";
            // dr[6] = "Kurunegala";
            // dr[7] = "pavanpabs";

           // dt.Rows.Add(dr);

            TimeTableDG.ItemsSource = dt.DefaultView;

            
        }

        private void program_load(object sender, EventArgs e)
        {
            String year = Year.Text;
            
            List<String> students = dbContext1.Students.Where(n => n.accYrSem == year).Select(n=>n.programme).ToList();

            
            Program.ItemsSource = students;
        }

        private void group_load(object sender, EventArgs e)
        {
            String program = Program.Text;

            List<String> groups = dbContext1.Students.Where(n => n.programme == program).Select(n => n.groupId).ToList();


            Group.ItemsSource = groups;
        }
    }
}
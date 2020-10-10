using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
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
using System.Printing;
using System.Windows.Documents;
using MaterialDesignThemes.Wpf;

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
        DataTable dt1;

        public GenerateWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            //GetSchedule();
            studenSP.Visibility = Visibility.Collapsed;
            lecturerSP.Visibility = Visibility.Collapsed;
            roomSP.Visibility = Visibility.Collapsed;
            List<Session> sessionList1 = dbContext1.Sessions.Include(s => s.subjectDSA).Include(s => s.tagDSA).ToList();

            // Program.DisplayMember = "Name";
            //comboBox1.ValueMember = "Name";
            // AddNewScheduleGrid.DataContext = NewSchedule;
        }


        private void GetSchedule()
        {
            // ScheduleDG.ItemsSource = dbContext1.Schedules.ToList();
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
                lecString.Add(item.LecName);
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
                roomString.Add(item.Rid);
            }
            Room.ItemsSource = roomString;
        }
        DataRow dr;
        private void Table_loaded(object sender, RoutedEventArgs e)
        {
            int dura = 0;
            int noOfSlots = 0;
            String type = sType.Text;
            String searchVal = "";
            String searchString= "";
            String[] arr1 = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            if (type == "Student") {
                searchVal = Group.Text;
                searchString = "GroupOrsubgroupForDisplay";
            } else if (type == "Lecturer") 
            {
                searchVal = Lecturer.Text;
                searchString = "lecturesLstByConcadinating";
            }

            else
            {
                searchVal = Room.Text;
                searchString = "RoomId";
            }
            //new schedule object
            Schedule NewSchedule = new Schedule();

            //Table initialization
            dt = new DataTable("emp");

            //Time heading
            //tableName.Content = "Test";
            DataColumn dc1 = new DataColumn("Time", typeof(string));
            dt.Columns.Add(dc1);

            NewSchedule = dbContext1.Schedules.Find(1);
            DateTime startTime = DateTime.ParseExact(NewSchedule.start_time, "h:mm tt", CultureInfo.InvariantCulture);
            int worktimeHrs = NewSchedule.working_time_hrs;
            int worktimeMins = NewSchedule.Working_time_mins;
            String duration = NewSchedule.Working_duration;

            //getting working days
            var workingDays = NewSchedule.Working_days.Split(',');
            //getting sessions
            //Session list re arrange for the consecutive

            //sessions1 copy
           List<ConsecutiveSession> consecutives= dbContext1.ConsecutiveSessions
                .Include(r => r.firstSession)
                .Include(r => r.secondSession)
                .ToList();
            
            var sessions1 = dbContext1.Sessions.Include(n => n.Room).Include(y=>y.studentDSA).ToList(); ;
           
            var existingIndex1 = 0;
            var existingIndex2 = 0;
            if (consecutives.Count() > 0)
            {

                foreach (var con in consecutives)
                {
                    foreach (var session in sessions1)
                    {
                        if ( con.firstSession.SessionId == session.SessionId)
                        {
                            existingIndex1 = sessions1.IndexOf(session);

                        }
                    }
                    foreach (var session in sessions1)
                    {
                        if (con.secondSession.SessionId == session.SessionId && session!=null)
                        {
                            existingIndex2 = sessions1.IndexOf(session);
                        }
                    }
                    

                    //getting the next value after the first variable
                    

                        if (existingIndex1 - 1 > 0)
                        {
                            if (existingIndex1 + 1 != existingIndex2)
                            {
                                Session ses1 = sessions1[existingIndex1];
                                sessions1.RemoveAt(existingIndex1);
                                Session ses = sessions1[existingIndex1 - 1];
                                sessions1.RemoveAt(existingIndex1 - 1);
                                Session ses2 = sessions1[existingIndex2];
                                sessions1.RemoveAt(existingIndex2);

                                sessions1[existingIndex1 - 1] = ses1;
                                sessions1[existingIndex1] = ses2;
                                sessions1[existingIndex2] = ses;
                            }
                        }
                        else {

                           

                        }

                    
                }
                

            }
            List<Session> sessionList = sessions1;
            List<Session> sessionList_copy = dbContext1.Sessions.ToList();

            //colums
            foreach (var data in arr1)
            {
                foreach (var item in workingDays)
                {
                    if (data == item)
                    {
                        dt.Columns.Add(new DataColumn(data, typeof(string)));
                    }
                }
            }



            //Time slots
            DateTime endTime = startTime.AddHours(worktimeHrs);
            endTime = endTime.AddMinutes(worktimeMins);

            DateTime timeVal = startTime;

            if (duration == "One Hour")
            {
                dura = 60;
            }

            else if (duration == "Thirty Minutes")
            {
                dura = 30;
            }

            while (timeVal.AddMinutes(dura) <= endTime)
            {
                noOfSlots++;
                dr = dt.NewRow();
                timeVal = timeVal.AddMinutes(dura);
                dr["Time"] = timeVal.ToString("h:mm tt");
                dt.Rows.Add(dr);

            }
            //2d array
            List<Session>[,] arr2d = new List<Session>[NewSchedule.Working_days_count, noOfSlots];
            //same sid array
            List<Session> sameSidArr = new List<Session>();
            //Session slots

            int sessionCnt = 0;

            int l = sessionList.Count();
            int val = 0;
            int infiniteTracker = 1000;
            int value = 0;

            for (int i = 0; i < NewSchedule.Working_days_count; i++)
            {
                for (int j = 0; j < noOfSlots; j++)
                {


                    arr2d[i, j] = new List<Session>();

                }

            }


            do
            {
                for (int i = 0; i < NewSchedule.Working_days_count; i++)
                {

                    for (int j = 0; j < noOfSlots; j++)
                    {
                        if (sessionCnt < sessionList.Count())
                        {

                            if (arr2d[i, j].Count() > 0)//checking aray has values
                            {
                                Boolean test = true;
                                while (test)
                                {
                                    foreach (var item in arr2d[i, j])//chechking same element
                                    {
                                        if (sessionCnt < sessionList.Count() && item.GroupOrsubgroupForDisplay.Equals(sessionList[sessionCnt].GroupOrsubgroupForDisplay))
                                        {
                                            if (j + 1 < noOfSlots)
                                            {
                                                j++;
                                            }
                                            else {
                                                break;
                                            }
                                           
                                        }
                                        else
                                        {
                                            test = false;
                                        }

                                    }
                                }
                                if (sessionCnt < sessionList.Count())
                                {

                                    val = sessionList[sessionCnt].durationinHours / dura;
                                    //for half sessions
                                    Boolean test1 = true;
                                    while (test1)
                                    {

                                        if ((j + val) > noOfSlots)
                                        {
                                            //date check for half sessions
                                            if (i + 1 < NewSchedule.Working_days_count)
                                            {
                                                i++;
                                            }
                                           
                                            j = 0;
                                        }
                                        else
                                        {
                                            test1 = false;
                                        }


                                    }
                                    //empty session copy
                                    if (sessionList_copy.Contains(sessionList[sessionCnt]))
                                    {
                                        int index = sessionList_copy.IndexOf(sessionList[sessionCnt]);
                                        sessionList_copy.RemoveAt(index);
                                    }
                                    for (int y = 0; y < val; y++)
                                    {

                                        if ((j + y) < noOfSlots)
                                        {
                                            arr2d[i, j + y].Add(sessionList[sessionCnt]);
                                        }
                                        else
                                        {

                                            break;
                                        }


                                    }
                                    j = j + val - 1;


                                    sessionCnt++;



                                }

                            }
                            else
                            {

                                int val1 = 0;
                                //empty session copy
                                if (sessionList_copy.Contains(sessionList[sessionCnt]))
                                {
                                    int index = sessionList_copy.IndexOf(sessionList[sessionCnt]);
                                    sessionList_copy.RemoveAt(index);
                                }
                                val1 = sessionList[sessionCnt].durationinHours / dura;
                                //for half sessions
                                Boolean test1 = true;
                                while (test1)
                                {

                                    if ((j + val1) > noOfSlots)
                                    {
                                        //date check for half sessions
                                        if (i + 1 < NewSchedule.Working_days_count)
                                        {
                                            i++;
                                        }

                                        j = 0;
                                    }
                                    else
                                    {
                                        test1 = false;
                                    }


                                }
                                for (int y = 0; y < val1; y++)
                                {


                                    if ((j + y) < noOfSlots)
                                    {
                                        arr2d[i, j + y].Add(sessionList[sessionCnt]);
                                    }



                                }
                                j = j + val1 - 1;

                                sessionCnt++;

                            }

                        }
                        else
                        {
                            break;
                        }
                    }

                }
                infiniteTracker--;


            } while (sessionList_copy.Count() > 0 && infiniteTracker>0);

            dt1 = new DataTable("emp1");
            DataColumn dc2new= new DataColumn("Conflicts", typeof(string));
            dt1.Columns.Add(dc2new);
            DataRow drow;
            

            sessionList_copy = dbContext1.Sessions.ToList();
           
                for (int i = 0; i < NewSchedule.Working_days_count; i++)
                {

                    for (int j = 0; j < noOfSlots; j++)
                    {

                    if (arr2d[i, j].Count() > 0)//checking aray has values
                    {

                        for (int v = 0; v < arr2d[i, j].Count(); v++)
                        {
                            if (v + 1 < arr2d[i, j].Count())
                            {
                                if (arr2d[i, j][v].studentDSA.groupId == arr2d[i, j][v + 1].studentDSA.groupId)
                                {
                                    drow = dt1.NewRow();
                                    drow["Conflicts"] = arr2d[i, j][v].studentDSA.groupId + "of subject" + arr2d[i, j][v].subjectDSA.SubjectName + " and " + arr2d[i, j][v+1].subjectDSA.SubjectName;

                                    dt1.Rows.Add(drow);
                                }
                            }

                        }



                    }
                        
                    }

                }
              

            l = value;
            ConflictDG.ItemsSource = dt1.DefaultView;

            for (int i = 0; i < NewSchedule.Working_days_count; i++)
            {
                for (int j = 0; j < noOfSlots; j++)
                {


                    String session = "";
                    String[] valval;
                    foreach (var item in arr2d[i, j])
                    {
                        valval = item.lecturesLstByConcadinating.Split(',');
                       
                       // dbContext1.Sessions.Include(b => b.studentDSA).ToList();
                        if (type == "Student" && item.studentDSA.groupId == searchVal) 
                        {

                            session = session + " " + item.getSessionStu();
                            MainTitle.Content = "Group Id- "+searchVal;
                        }
                        else if (type == "Lecturer" && valval.Contains(searchVal+" ") )
                        {
                            session = session + " " + item.getSessionLec();
                            MainTitle.Content = "Mr/Mrs/Ms. " + searchVal;
                        }
                        else if (type == "Room" && item.Room != null )
                        {
                            if (item.Room.Rid == searchVal)
                            {
                                session = session + " " + item.getSessionRoom();
                                MainTitle.Content = "Room Id- "+ searchVal;
                            }
                            
                        }





                    }
                    dt.Rows[j][workingDays[i]] = session;


                }

            }


            TimeTableDG.ItemsSource = dt.DefaultView;

        }

        private void program_load(object sender, EventArgs e)
        {
            String year = Year.Text;

            List<String> students = dbContext1.Students.Where(n => n.accYrSem == year).Select(n => n.programme).ToList();


            Program.ItemsSource = students;
        }

        private void group_load(object sender, EventArgs e)
        {
            String program = Program.Text;

            List<String> groups = dbContext1.Students.Where(n => n.programme == program).Select(n => n.groupId).ToList();


            Group.ItemsSource = groups;
        }

        private void PrintTable(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                tableArea.Measure(pageSize);
                tableArea.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));

                Printdlg.PrintVisual(tableArea, Title);
            }


        }
    }
}
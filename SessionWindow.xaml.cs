using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        

        List<LecturerDetails> LeLISTT = new List<LecturerDetails>();

        List<LecturerDetails> LoadLec = new List<LecturerDetails>();
        List<Session> sesinList = new List<Session>();
        Session newSessionDl = new Session();
        MyDbContext dbContext1;
        //   List<SubjectDetails> subjects;

        public List<Tag> tagList { get; set; }
        public SessionWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
            getlecturers();
            getTags();
            LoadSessions();

        }

        public void LoadSessions()
        {
            SessionDGg.ItemsSource = dbContext1.Sessions.Include(r => r.subjectDSA);
        }


        public void getlecturers()
        {
            LecturerDrpn.ItemsSource = dbContext1.LectureInformation.ToList();


        }

        public void getTags()
        {
            tagList = dbContext1.Tags.ToList();
            CBTagsNames.ItemsSource = dbContext1.Tags.ToList();

        }



        public void getMainGroupdetails(String yrr)
        {

            String Mainstyr;

            if (yrr.Equals("1st Year"))
            {
                Mainstyr = "Y1" + "%";
            }
            else if (yrr.Equals("2nd Year"))
            {
                Mainstyr = "Y2" + "%";
            }
            else if (yrr.Equals("3rd Year"))
            {
                Mainstyr = "Y3" + "%";

            }
            else
            {
                Mainstyr = "Y4" + "%";
            }

            selectMainGroup.ItemsSource = dbContext1.Students.Where(c => EF.Functions.Like(c.accYrSem, Mainstyr)).ToList();


        }

        public void getSubgroupdetails(String acdYrr)
        {

            String Subgrp;


            if (acdYrr.Equals("1st Year"))
            {
                Subgrp = "Y1" + "%";
            }
            else if (acdYrr.Equals("2nd Year"))
            {
                Subgrp = "Y2" + "%";
            }
            else if (acdYrr.Equals("3rd Year"))
            {
                Subgrp = "Y3" + "%";

            }
            else
            {
                Subgrp = "Y4" + "%";
            }


            selectSubgrp.ItemsSource = dbContext1.Students.Where(c => EF.Functions.Like(c.accYrSem, Subgrp)).ToList();


        }




        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();

            this.Close();

        }
        private void getSubjects(String year)
        {


            selectsubjects.ItemsSource = dbContext1.SubjectInformation.Where(s => s.OfferedYear == year).ToList();



        }

        private void yeardropdwn(Object s, RoutedEventArgs e)

        {
            String ContentOfItemOne = (CByearselect.Items[CByearselect.SelectedIndex] as ComboBoxItem).Content.ToString();


            //  rankDetail.Text = ContentOfItemOne;




            getMainGroupdetails(ContentOfItemOne);
            getSubgroupdetails(ContentOfItemOne);
            getSubjects(ContentOfItemOne);



        }



        private void NumberValidationForStudentCount(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void NumberValidationForDuration(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddSessionDetails(object s, RoutedEventArgs e)

        {
            Tag tgggg = (Tag)CBTagsNames.SelectedItem;
            String tagname = tgggg.tags;






            newSessionDl.StdntCount = int.Parse(StdntCnt.Text);
            newSessionDl.durationinHours = int.Parse(Duration.Text);
            newSessionDl.tagDSA = (Tag)CBTagsNames.SelectedItem;
            newSessionDl.subjectDSA = (SubjectDetails)selectsubjects.SelectedItem;



            Student getGrpsub; ;

            if (tagname.Equals("Lecture") || tagname.Equals("Tutorial"))
            {

                newSessionDl.studentDSA = (Student)selectMainGroup.SelectedItem;
                getGrpsub = (Student)selectMainGroup.SelectedItem;
                newSessionDl.GroupOrsubgroupForDisplay = getGrpsub.groupId;

            }
            else
            {
                newSessionDl.studentDSA = (Student)selectSubgrp.SelectedItem;
                getGrpsub = (Student)selectSubgrp.SelectedItem;
                newSessionDl.GroupOrsubgroupForDisplay = getGrpsub.subGroupId;

            }


            dbContext1.Sessions.Add(newSessionDl);

       int condition=  dbContext1.SaveChanges();

          //  MessageBox.Show(condition.ToString());



        //    if (condition > 0)
       //    { }

                foreach (LecturerDetails lec in LeLISTT)
                {


                    SessionLecturer sessionlc = new SessionLecturer
                    {

                        Ssssion = newSessionDl,
                        Lecdetaiils = lec

                    };

                    dbContext1.SessionLecturers.Add(sessionlc);
                    dbContext1.SaveChanges();


                }









            getGrpsub = null;


            LoadSessions();





            // newSessionDl.studentDSA = (Student)selectsubjects.SelectedItem;

            //  if((Tag)CBTagsNames.SelectedItem.)




            //  NewLecDL.LecName = LecturerName.Text;
            // NewLecDL.EmpId = LecIdName.Text;
            //  NewLecDL.Faculty = LecturerFaculty.Text;
            //  NewLecDL.Department = LecturerDepartment.Text;
            //  NewLecDL.Center = LecturerCenter.Text;
            // // NewLecDL.BuildinDSA = (Building)CBBuilding.SelectedItem;
            // NewLecDL.EmpLevel = int.Parse(LecLevel.Text);
            // NewLecDL.Rank = LecLevel.SelectedIndex + 1.ToString() + "." + LecIdName.Text.ToString();
            // Addlecbtn.Content = "Add lecture";
            // dbContext1.LectureInformation.Add(NewLecDL);
            // NewLecDL = new LecturerDetails();







        }




        private void SelectLectureritem(Object s, RoutedEventArgs e)
        {
            LecturerDetails TrytoAddLecture = (LecturerDetails)LecturerDrpn.SelectedItem;

            newSessionDl.lecturesLstByConcadinating += TrytoAddLecture.LecName + " , ";


            LeLISTT.Add(TrytoAddLecture);
        }



        private void AssignLectureItemTo(Object s, RoutedEventArgs e)
        {

            LVlecturer.ItemsSource = LeLISTT.ToList();



        }

        private void TagValueChanged(Object s, RoutedEventArgs e)
        {
            Tag CheckingTT = (Tag)CBTagsNames.SelectedItem;
            String checkingTag = CheckingTT.tags;



            if (checkingTag.Equals("Lecture") || checkingTag.Equals("Tutorial"))
            {

                selectMainGroup.IsEnabled = true;
                selectSubgrp.IsEnabled = false;
               

            }

            if (checkingTag.Equals("Practical"))
            {
                selectSubgrp.IsEnabled = true;
                selectMainGroup.IsEnabled = false;

            }

        }


        private void SelectSessionRow(object s,RoutedEventArgs e)
        {
            if (SessionDGg.SelectedItem != null)
            {
                Addlecbtn.IsEnabled = false;
                Sessionupdatebtn.IsEnabled = true;
            }
            else
            {
                Addlecbtn.IsEnabled = true;
                Sessionupdatebtn.IsEnabled = false;
            }


        }



        private void UpdateSessionDetails(Object s, RoutedEventArgs e)
        {
            if (SessionDGg.SelectedItem != null)
            {
                Session selectedSession = (Session)SessionDGg.SelectedItem;

                MessageBox.Show(selectedSession.tagDSA.tags);
             
            }
        }



    }
}
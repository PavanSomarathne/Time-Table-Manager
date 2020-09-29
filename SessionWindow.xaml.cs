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

        Boolean Addseesion = true;
        Boolean settingEmptyValues = false;
        List<LecturerDetails> LeLISTT = new List<LecturerDetails>();

        //  List<LecturerDetails> LoadLec = new List<LecturerDetails>();
        List<Session> sesinList = new List<Session>();
        Session newSessionDl = new Session();
        Session UpdatingSession = new Session();
        MyDbContext dbContext1;
        //   List<SubjectDetails> subjects;

        public List<Tag> tagList { get; set; }
        public SessionWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
            Addseesion = true;
            settingEmptyValues = false;
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
            //MessageBox.Show(CByearselect.Text.ToString());

            if (!settingEmptyValues)
            {

                String ContentOfItemOne = (CByearselect.Items[CByearselect.SelectedIndex] as ComboBoxItem).Content.ToString();


                //  rankDetail.Text = ContentOfItemOne;




                getMainGroupdetails(ContentOfItemOne);
                getSubgroupdetails(ContentOfItemOne);
                getSubjects(ContentOfItemOne);

            }

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

            Addseesion = true;

            Tag tgggg = (Tag)CBTagsNames.SelectedItem;
            String tagname = tgggg.tags;


            //concadinate lecturer names
            newSessionDl.lecturesLstByConcadinating = null;
            foreach (LecturerDetails LL in LeLISTT)
            {

                newSessionDl.lecturesLstByConcadinating += LL.LecName;
            }






            newSessionDl.Year = CByearselect.Text;
            newSessionDl.StdntCount = int.Parse(StdntCnt.Text);
            newSessionDl.durationinHours = int.Parse(Duration.Text);
            newSessionDl.tagDSA = (Tag)CBTagsNames.SelectedItem;
            newSessionDl.subjectDSA = (SubjectDetails)selectsubjects.SelectedItem;



            Student getGrpsub;

            if (tagname.Equals("Lecture") || tagname.Equals("Tutorial"))
            {

                newSessionDl.studentDSA = (Student)selectMainGroup.SelectedItem;
                getGrpsub = (Student)selectMainGroup.SelectedItem;
                newSessionDl.GroupOrsubgroupForDisplay = getGrpsub.groupId;
                newSessionDl.GroupType = "Main Group";
            }
            else
            {
                newSessionDl.studentDSA = (Student)selectSubgrp.SelectedItem;
                getGrpsub = (Student)selectSubgrp.SelectedItem;
                newSessionDl.GroupOrsubgroupForDisplay = getGrpsub.subGroupId;
                newSessionDl.GroupType = "Sub Group";
            }


            dbContext1.Sessions.Add(newSessionDl);

            int condition = dbContext1.SaveChanges();

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

            settingEmptyValues = true;

            CByearselect.Text = " ";
            LVlecturer.ItemsSource = null;
            CBTagsNames.Text = "";
            selectMainGroup.Text = "";
            selectSubgrp.Text = "";
            StdntCnt.Text = "";
            Duration.Text = "";
            LecturerDrpn.Text = "";
            selectsubjects.Text = "";


            newSessionDl = new Session();

            LeLISTT = null;
            LeLISTT = new List<LecturerDetails>();
            getGrpsub = null;

            settingEmptyValues = false;
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




        //   private void SelectLectureritem(Object s, RoutedEventArgs e)

        //  {                      



        //  }



        private void AssignLectureItemTo(Object s, RoutedEventArgs e)
  {

            if (!LecturerDrpn.Text.ToString().Equals(""))
            {

                if (Addseesion)
                { 
                    LecturerDetails TrytoAddLecture = (LecturerDetails)LecturerDrpn.SelectedItem;

                    foreach (LecturerDetails Lliss in LeLISTT)
                    {
                        if (Lliss.Id == TrytoAddLecture.Id)
                        {
                            LecturerDrpn.SelectedIndex = -1;
                            new MessageBoxCustom("This Lecture already You assigned,Can't add again!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                            return;

                        }

                    }

                      LeLISTT.Add(TrytoAddLecture);

                            String test = null;
                            foreach(LecturerDetails ll in LeLISTT)
                            {
                                test += ll.LecName + " ";
                            }
                            MessageBox.Show(test);
                            LVlecturer.ItemsSource = LeLISTT.ToList();
                            LecturerDrpn.SelectedIndex = -1;
                        
                    

                  

                }
                else
                {
                    LecturerDetails lectu = (LecturerDetails)LecturerDrpn.SelectedItem;
                    MessageBox.Show(lectu.Id.ToString());
                    if (dbContext1.SessionLecturers.Any(r => r.SessionrId == UpdatingSession.SessionId && r.LecturerId == lectu.Id))
                     {
                        new MessageBoxCustom("This Lecture already assigned to this Session,Can't add again!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        LecturerDrpn.SelectedIndex = -1;
                     }
                    else
                     {
                         


                            SessionLecturer assgni = new SessionLecturer
                            {

                                Ssssion = UpdatingSession,
                                Lecdetaiils = lectu

                            };

                            dbContext1.SessionLecturers.Add(assgni);
                            dbContext1.SaveChanges();

                        LeLISTT.Add(lectu);
                        LVlecturer.ItemsSource = LeLISTT.ToList();
                        LecturerDrpn.SelectedIndex = -1;

                      }


                }
            

                  
            }
            else
            {
                new MessageBoxCustom("Please Select the lecture before to assign", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }
          

 }

        private void TagValueChanged(Object s, RoutedEventArgs e)
        {
            
            if (!settingEmptyValues)
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
        }


        private void SelectSessionRow(object s, RoutedEventArgs e)
        {
            // MessageBox.Show("Im outer");
            if (SessionDGg.SelectedItem != null)
            {
                //   MessageBox.Show("Im In");
                Addlecbtn.IsEnabled = false;
                Sessionupdatebtn.IsEnabled = true;


                Addseesion = false;
                UpdatingSession = (Session)SessionDGg.SelectedItem;
                //update ekedi lectures la concat net karannaa and year eka concadidate karanna
                CByearselect.Text = UpdatingSession.Year;
                selectsubjects.Text = UpdatingSession.subjectDSA.SubjectName;
                LoadLecturesGivenBySessionId(UpdatingSession.SessionId);
                CBTagsNames.Text = UpdatingSession.tagDSA.tags;

                if (UpdatingSession.GroupType.Equals("Main Group"))
                {
                    selectMainGroup.Text = UpdatingSession.GroupOrsubgroupForDisplay;
                }
                if (UpdatingSession.GroupType.Equals("Sub Group"))
                {
                    selectSubgrp.Text = UpdatingSession.GroupOrsubgroupForDisplay;
                }


                StdntCnt.Text = UpdatingSession.StdntCount.ToString();
                Duration.Text = UpdatingSession.durationinHours.ToString();



            }
            else
            {
                Addlecbtn.IsEnabled = true;
                Sessionupdatebtn.IsEnabled = false;
            }



        }

        private void SelectLectureRooww(object s, RoutedEventArgs e)
        {

            if (LVlecturer.SelectedItem != null)
            {
                TrashlecBtn.IsEnabled = true;
            }
            else
            {
                TrashlecBtn.IsEnabled = false;
            }

        }

        private void Delectlecturec(object s, RoutedEventArgs e)
        {


            if (LVlecturer.SelectedItem != null)
            {

                LecturerDetails lecturer = (LecturerDetails)LVlecturer.SelectedItem;

                if (Addseesion == true)
                {


                    var item = LeLISTT.Find(x => x.Id == lecturer.Id);
                    LeLISTT.Remove(item);



                    LVlecturer.ItemsSource = LeLISTT.ToList();

                    LVlecturer.SelectedIndex = -1;
                }

                else
                {



                    LecturerDetails lecturerrty = (LecturerDetails)LVlecturer.SelectedItem;

                    var item = LeLISTT.Find(x => x.Id == lecturerrty.Id);
                    LeLISTT.Remove(item);


                    if (dbContext1.SessionLecturers.Any(r => r.SessionrId == UpdatingSession.SessionId && r.LecturerId == lecturerrty.Id))
                    {

                        var SesLert = dbContext1.SessionLecturers.First(row => row.SessionrId == UpdatingSession.SessionId && row.LecturerId == lecturerrty.Id);
                        dbContext1.SessionLecturers.Remove(SesLert);
                        dbContext1.SaveChanges();

                      
                        // LoadLecturesGivenBySessionId(UpdatingSession.SessionId);
                    }




                    LVlecturer.ItemsSource = LeLISTT.ToList();
                    LVlecturer.SelectedIndex = -1;
                 


                }


            }
            else
            {
                MessageBox.Show("please select lecture before clicking teh button  ");
            }



        }




        private void UpdateSessionDetails(Object s, RoutedEventArgs e)
        {
            //at last set sesiondg.selected to null///////////////////////
            //   if(SessionDGg.SelectedItem == null)
            //  {

            //    new MessageBoxCustom("Please Select session before the update !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            //  }




            UpdatingSession.lecturesLstByConcadinating = null;
            foreach (LecturerDetails LL in LeLISTT)
            {

                UpdatingSession.lecturesLstByConcadinating += LL.LecName;
            }



            Tag updatingtg = (Tag)CBTagsNames.SelectedItem;
            String updatingtagname = updatingtg.tags;


            UpdatingSession.Year = CByearselect.Text;
            UpdatingSession.StdntCount = int.Parse(StdntCnt.Text);
            UpdatingSession.durationinHours = int.Parse(Duration.Text);
            UpdatingSession.tagDSA = (Tag)CBTagsNames.SelectedItem;
            UpdatingSession.subjectDSA = (SubjectDetails)selectsubjects.SelectedItem;


            Student UpadtinggetGrpsubb;

            if (updatingtagname.Equals("Lecture") || updatingtagname.Equals("Tutorial"))
            {

                UpdatingSession.studentDSA = (Student)selectMainGroup.SelectedItem;
                UpadtinggetGrpsubb = (Student)selectMainGroup.SelectedItem;
                UpdatingSession.GroupOrsubgroupForDisplay = UpadtinggetGrpsubb.groupId;
                UpdatingSession.GroupType = "Main Group";
            }
            else
            {
                UpdatingSession.studentDSA = (Student)selectSubgrp.SelectedItem;
                UpadtinggetGrpsubb = (Student)selectSubgrp.SelectedItem;
                UpdatingSession.GroupOrsubgroupForDisplay = UpadtinggetGrpsubb.subGroupId;
                UpdatingSession.GroupType = "Sub Group";
            }





            dbContext1.Update(UpdatingSession);
            dbContext1.SaveChanges();



            foreach (LecturerDetails lecdetl in LeLISTT)
            {
                MessageBox.Show("updating... outer");
                if (dbContext1.SessionLecturers.Any(r => r.SessionrId == UpdatingSession.SessionId && r.LecturerId == lecdetl.Id))
                    MessageBox.Show("upadaing ...inner");
                {

                    var SesLert = dbContext1.SessionLecturers.First(row => row.SessionrId == UpdatingSession.SessionId && row.LecturerId == lecdetl.Id);
                    dbContext1.SessionLecturers.Update(SesLert);
                    dbContext1.SaveChanges();



                }


            }




            settingEmptyValues = true;

            CByearselect.Text = " ";
            LVlecturer.ItemsSource = null;
            CBTagsNames.Text = "";
            selectMainGroup.Text = "";
            selectSubgrp.Text = "";
            StdntCnt.Text = "";
            Duration.Text = "";
            LecturerDrpn.Text = "";
            selectsubjects.Text = "";


            LeLISTT = null;
            LeLISTT = new List<LecturerDetails>();
            UpdatingSession = null;
            UpdatingSession = new Session();

            settingEmptyValues = false;
            LoadSessions();

            settingEmptyValues = false;

            Addlecbtn.IsEnabled = true;
            Sessionupdatebtn.IsEnabled = false;
            Addseesion = true;
            SessionDGg.SelectedItem = null;
            LoadSessions();
            //antima addsession eka true karann
            // Addseesion = true; //meka true karanna kalin values empty kala yuthu wey,nattama waradi
        }

        public void LoadLecturesGivenBySessionId(int sesin)
        {


            LeLISTT = dbContext1.Sessions
        .Where(p => p.SessionId == sesin)
       .SelectMany(r => r.SessionLecturers)
      .Select(rl => rl.Lecdetaiils).ToList();

            LVlecturer.ItemsSource = LeLISTT;

            String name = null;
            foreach (LecturerDetails kk in LeLISTT)
            {
                name += kk.LecName + ",";
            }

            MessageBox.Show(name);



        }



        private void SessionDelete(object s, RoutedEventArgs e)
        {

            if (SessionDGg.SelectedItem != null)
            {


                bool? Result = new MessageBoxCustom("Are you sure, You want to Delete This Session Detail ? ",
         MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

                if (Result.Value)
                {
                    Session ss11 = (Session)SessionDGg.SelectedItem;
                    dbContext1.Sessions.Remove(ss11);
                    dbContext1.SaveChanges();
                    LoadSessions();
                }



                settingEmptyValues = true;

                CByearselect.Text = " ";
                LVlecturer.ItemsSource = null;
                CBTagsNames.Text = "";
                selectMainGroup.Text = "";
                selectSubgrp.Text = "";
                StdntCnt.Text = "";
                Duration.Text = "";
                LecturerDrpn.Text = "";
                selectsubjects.Text = "";


                LeLISTT = null;
                LeLISTT = new List<LecturerDetails>();
                UpdatingSession = null;
                UpdatingSession = new Session();

                settingEmptyValues = false;
                LoadSessions();

                settingEmptyValues = false;

                Addlecbtn.IsEnabled = true;
                Sessionupdatebtn.IsEnabled = false;
                Addseesion = true;
                SessionDGg.SelectedItem = null;
                LoadSessions();



            }




        }









    }
}
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
        String chkkradiovall = "Lecturer";
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


        private void selectsubjectdrpp(Object s, RoutedEventArgs e)

        {   // if (selectsubjects.Text.ToString().Equals(""))
            // {
            //      MessageBox.Show(selectsubjects.Text.ToString() + "when the subject drop down value empty");
            //  }


            //  if (!settingEmptyValues && !selectsubjects.Text.ToString().Equals(""))
            //   {

            //    SubjectDetails dt = (SubjectDetails)selectsubjects.SelectedItem;

            //  MessageBox.Show(dt.SubjectCode);

            //  MessageBox.Show(selectsubjects.Text.ToString().Equals("").ToString());

            //   MessageBox.Show("when the subject drp down not empty " );


            // }

            //   String ContentOfItemOne = (selectsubjects.Items[selectsubjects.SelectedIndex] as ComboBoxItem).Content.ToString();



            //   MessageBox.Show(selectsubjects.Text.ToString());


            if (!settingEmptyValues)
            {

                 SubjectDetails dd = (SubjectDetails)selectsubjects.SelectedItem;
                 if (dd != null)
                {
                    sucode.Content = dd.SubjectCode;
                }
                else
                {
                    sucode.Content = "";
                }

                dd = null;


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
            if (ValidateSessionInputs())
            {


                //condition
                if (dbContext1.Sessions.Any(b => b.subjectDSA.SubjectName == selectsubjects.Text.Trim() && b.tagDSA.tags == CBTagsNames.Text.Trim() && (b.GroupOrsubgroupForDisplay == selectMainGroup.Text.Trim() || b.GroupOrsubgroupForDisplay == selectSubgrp.Text.Trim())  ))
                {
                    new MessageBoxCustom("This Session is already in the System", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }


                Addseesion = true;

                Tag tgggg = (Tag)CBTagsNames.SelectedItem;
                String tagname = tgggg.tags;


                //concadinate lecturer names
                newSessionDl.lecturesLstByConcadinating = null;
                foreach (LecturerDetails LL in LeLISTT)
                {

                    newSessionDl.lecturesLstByConcadinating += LL.LecName + " ,";
                }






                newSessionDl.Year = CByearselect.Text.Trim();
                newSessionDl.StdntCount = int.Parse(StdntCnt.Text.Trim());
                newSessionDl.durationinHours = int.Parse(Duration.Text.Trim());
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
                sucode.Content = "";

                newSessionDl = new Session();

                LeLISTT = null;
                LeLISTT = new List<LecturerDetails>();
                getGrpsub = null;

                settingEmptyValues = false;
                LoadSessions();





            }
            else
            {
                new MessageBoxCustom("Please Complete  Session   Details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }





        }




    



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

                           // String test = null;
                          //  foreach(LecturerDetails ll in LeLISTT)
                           // {
                            //    test += ll.LecName + " ";
                           // }
                          //  MessageBox.Show(test);
                            LVlecturer.ItemsSource = LeLISTT.ToList();
                            LecturerDrpn.SelectedIndex = -1;
                        
                    

                  

                }
                else
                {
                    LecturerDetails lectu = (LecturerDetails)LecturerDrpn.SelectedItem;
                  //  MessageBox.Show(lectu.Id.ToString());
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
                sucode.Content = UpdatingSession.subjectDSA.SubjectCode;
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
        //remove lectures from list
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
                MessageBox.Show("please select lecture before clicking the button  ");
            }



        }




        private void UpdateSessionDetails(Object s, RoutedEventArgs e)
        {
            //at last set sesiondg.selected to null///////////////////////
            //   if(SessionDGg.SelectedItem == null)
            //  {

            //    new MessageBoxCustom("Please Select session before the update !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            //  }

            if (ValidateSessionInputs())
            {
                bool estingSS = !(UpdatingSession.subjectDSA.SubjectName.Equals(selectsubjects.Text.ToString()) && UpdatingSession.tagDSA.tags.Equals(CBTagsNames.Text.ToString()) && (UpdatingSession.GroupOrsubgroupForDisplay.Equals(selectMainGroup.Text.ToString()) || UpdatingSession.GroupOrsubgroupForDisplay.Equals(selectSubgrp.Text.ToString())));
            

                if (estingSS &&  dbContext1.Sessions.Any(b => b.subjectDSA.SubjectName == selectsubjects.Text.ToString() && b.tagDSA.tags == CBTagsNames.Text.ToString() && (b.GroupOrsubgroupForDisplay == selectMainGroup.Text.ToString() || b.GroupOrsubgroupForDisplay == selectSubgrp.Text.ToString())))
                {
                    new MessageBoxCustom("This session already In the system", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;

                }


                UpdatingSession.lecturesLstByConcadinating = null;
                foreach (LecturerDetails LL in LeLISTT)
                {

                    UpdatingSession.lecturesLstByConcadinating += LL.LecName + " ,";
                }



                Tag updatingtg = (Tag)CBTagsNames.SelectedItem;
                String updatingtagname = updatingtg.tags;


                UpdatingSession.Year = CByearselect.Text.Trim();
                UpdatingSession.StdntCount = int.Parse(StdntCnt.Text.Trim());
                UpdatingSession.durationinHours = int.Parse(Duration.Text.Trim());
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

                    if (dbContext1.SessionLecturers.Any(r => r.SessionrId == UpdatingSession.SessionId && r.LecturerId == lecdetl.Id))

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
                sucode.Content = "";


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

        }

        public void LoadLecturesGivenBySessionId(int sesin)
        {


            LeLISTT = dbContext1.Sessions
        .Where(p => p.SessionId == sesin)
       .SelectMany(r => r.SessionLecturers)
      .Select(rl => rl.Lecdetaiils).ToList();

            LVlecturer.ItemsSource = LeLISTT;

           // String name = null;
           // foreach (LecturerDetails kk in LeLISTT)
           // {
           //     name += kk.LecName + ",";
           // }

          //  MessageBox.Show(name);



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
                sucode.Content = "";


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

        private void Lectueradiobtnclickingg(object sender, RoutedEventArgs e)
        {

            if (lecturechkbtn.IsChecked == true)
            {
                chkkradiovall = "Lecturer";
            }

            sessionserhh.Text = "";
        }


        private void groupcheckbtnclicking(object sender, RoutedEventArgs e)
        {
         

            if(grpsupchkbtn.IsChecked==true)
            {
                chkkradiovall = "Group Or Sub Group";
            }

            sessionserhh.Text = "";
        }


        private void subjectcheckbtnclicking(object sender, RoutedEventArgs e)
        {

            if (subjectchkbtn.IsChecked == true)
            {
                chkkradiovall = "Subject";
            }

            sessionserhh.Text = "";
        }

       
            private void Serchsessionvaluess(object sender, RoutedEventArgs e)
            {      

                   if(sessionserhh.Text.Trim().Equals("") )
                   {
                     LoadSessions();
                   }
      
                   if( !sessionserhh.Text.Trim().Equals("") && !chkkradiovall.Equals(""))
                  {
                    //MessageBox.Show(sessionserhh.Text.ToLower() + chkkradiovall);

                        if(chkkradiovall.Equals("Lecturer"))
                      {
                       advancedSearchlectures(sessionserhh.Text.ToString());

                      }


                       if (chkkradiovall.Equals("Group Or Sub Group"))
                      {
                         advancedgroupOrSubgrpSearch(sessionserhh.Text.ToString());

                      }


                       if(chkkradiovall.Equals("Subject"))
                      {
                        advancedSubjectSearch(sessionserhh.Text.ToString());

                      }
 
                  }


           }


        public void advancedSubjectSearch(String sujct)
        {

            SessionDGg.ItemsSource = dbContext1.Sessions.Where(d => d.subjectDSA.SubjectName .ToLower().StartsWith(sujct.ToLower())).ToList();

        }

        public void advancedgroupOrSubgrpSearch(String grp)
        {

            SessionDGg.ItemsSource = dbContext1.Sessions.Where(d => d.GroupOrsubgroupForDisplay.ToLower().StartsWith(grp.ToLower())).ToList();

        }





           public void advancedSearchlectures(String lec)
        {



            // var names = (from cust in dbContext1.Sessions
            //  where cust.lecturesLstByConcadinating.IndexOf(lec, StringComparison.InvariantCultureIgnoreCase) >= 0
            //  select cust).ToList();

            //  String yy = null;

            ///  foreach(Session ss in names)
            //  {
            //      yy += ss.Year + " ";
            //   }

            //  MessageBox.Show(yy);




            //  SessionDGg.ItemsSource = dbContext1.SessionLecturers.Where(d => d.lecturesLstByConcadinating.ToLower().Contains(lec.ToLower())).ToList();

            SessionDGg.ItemsSource = dbContext1.SessionLecturers.Where(d => d.Lecdetaiils.LecName.ToLower().StartsWith(lec.ToLower())).Select(s => s.Ssssion).ToList();





        }



     

        //return value eka dannn/////////////////////////////////
        private bool ValidateSessionInputs()
        {
            if (CByearselect.Text.Trim() == "")
            {
                CByearselect.Focus();
                return false;
            }




            if (selectsubjects.SelectedIndex == -1)
            {
               // MessageBox.Show(selectsubjects.Text.ToString());
                selectsubjects.Focus();
                return false;
            }

            if (LeLISTT.Count == 0)
            {
              //  new MessageBoxCustom("Assign  Lecturer or lecturers before create the session", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return false;
            }

            if (CBTagsNames.SelectedIndex == -1)
            {
                CBTagsNames.Focus();
                return false;
            }



            if (CBTagsNames.Text.ToString().Equals("Lecture") && selectMainGroup.SelectedIndex == -1)
            {
                selectMainGroup.Focus();
                return false;
            }

            if (CBTagsNames.Text.ToString().Equals("Tutorial") && selectMainGroup.SelectedIndex == -1)
            {
                selectMainGroup.Focus();
                return false;

            }

            if (CBTagsNames.Text.ToString().Equals("Practical") && selectSubgrp.SelectedIndex == -1)
            {
                selectSubgrp.Focus();
                return false;

            }

            if (StdntCnt.Text.Trim() == "")
            {
                StdntCnt.Focus();
                return false;
            }

            if (Duration.Text.Trim() == "")
            {
                Duration.Focus();
                return false;
            }

             return true;
         
        }








    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<Session> parall = new List<Session>();
        Boolean Addseesion = true;
        private MyDbContext dbContext1;
        ParallelSession NewSession = new ParallelSession();
        Session UpdatingSession = new Session();
        ParallelSession selectedSession = new ParallelSession();
        List<Session> SessionLIST = new List<Session>();
        Session newSessionDl = new Session();
        List<LecturerDetails> LeLISTT = new List<LecturerDetails>();
        Boolean settingEmptyValues = false;

        public ParallelSessionsWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();
            //GetSessions();
            Boolean Addseesion = true;
            addUpdateSessionDetailsGrid.DataContext = NewSession;
            getlecturers();
            settingEmptyValues = false;
        }

        //private void GetSessions()
        //{
        //    showDetailsGrid.ItemsSource = dbContext1.ParallelSessions.ToList();
        //}

        private void clear()
        {

            addUpdateSessionDetailsGrid.DataContext = null;
        }

        //public void loadsessions()
        //{
        //    cmb1.ItemsSource = dbContext1.Sessions.ToList();
        //}

        public void getlecturers()
        {
            cmb1.ItemsSource = dbContext1.LectureInformation.ToList();


        }

        private void sessions_load(object sender, RoutedEventArgs e)
        {
            List<String> sesString = new List<String>();
            List<Session> sess = dbContext1.Sessions.ToList();

            foreach (var item in sess)
            {
                sesString.Add(item.lecturesLstByConcadinating + "\n " + item.subjectDSA.SubjectName + "(" + item.subjectDSA.SubjectCode + ")" + " \n " + item.tagDSA.tags + "\n " + item.GroupOrsubgroupForDisplay + "\n" + item.StdntCount + "(" + item.durationinHours + ")");
            }
            cmb1.ItemsSource = sesString;
        }

        private void AddSessionDetails(object s, RoutedEventArgs e)

        {

            Addseesion = true;


            //concadinate lecturer names
            newSessionDl.lecturesLstByConcadinating = null;
            foreach (LecturerDetails LL in LeLISTT)
            {

                newSessionDl.lecturesLstByConcadinating += LL.LecName;
            }



            dbContext1.Sessions.Add(newSessionDl);

            int condition = dbContext1.SaveChanges();

           

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

            LVlecturer.ItemsSource = null;
            cmb1.Text = "";
          

            newSessionDl = new Session();

            LeLISTT = null;
            LeLISTT = new List<LecturerDetails>();
            

            settingEmptyValues = false;
            getlecturers();








        }


        private void AssignSessionItemTo(Object s, RoutedEventArgs e)
        {

            if (!cmb1.Text.ToString().Equals(""))
            {

                if (Addseesion)
                {
                    LecturerDetails TrytoAddLecture = (LecturerDetails)cmb1.SelectedItem;

                    foreach (LecturerDetails Lliss in LeLISTT)
                    {
                        if (Lliss.Id == TrytoAddLecture.Id)
                        {
                            cmb1.SelectedIndex = -1;
                            new MessageBoxCustom("This Lecture already You assigned,Can't add again!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                            return;

                        }

                    }

                    LeLISTT.Add(TrytoAddLecture);

                    String test = null;
                    foreach (LecturerDetails ll in LeLISTT)
                    {
                        test += ll.LecName + " ";
                    }
                    MessageBox.Show(test);
                    LVlecturer.ItemsSource = LeLISTT.ToList();
                    cmb1.SelectedIndex = -1;





                }
                else
                {
                    LecturerDetails lectu = (LecturerDetails)cmb1.SelectedItem;
                    MessageBox.Show(lectu.Id.ToString());
                    if (dbContext1.SessionLecturers.Any(r => r.SessionrId == UpdatingSession.SessionId && r.LecturerId == lectu.Id))
                    {
                        new MessageBoxCustom("This Lecture already assigned to this Session,Can't add again!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        cmb1.SelectedIndex = -1;
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
                        cmb1.SelectedIndex = -1;

                    }


                }



            }
            else
            {
                new MessageBoxCustom("Please Select the lecture before to assign", MessageType.Warning, MessageButtons.Ok).ShowDialog();
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

        //private void AssignSessionItemTo(Object s, RoutedEventArgs e)
        //{

        //    if (!cmb1.Text.ToString().Equals(""))
        //    {



        //       Session TrytoAddSession = (Session)cmb1.SelectedItem;


        //        parall.Add(TrytoAddSession);


        //        LVlecturer.ItemsSource = parall;
        //        cmb1.SelectedIndex = -1;
        //    }
        //}

        //  SessionLIST.Add(TrytoAddSession);

        //  String test = null;
        // foreach (Session ll in SessionLIST)
        //            {
        //                test += ll.SessionId + " ";
        //            }
        //            MessageBox.Show(test);
        //            LVlecturer.ItemsSource = SessionLIST.ToList();
        //            cmb1.SelectedIndex = -1;





        //        }
        //        else
        //        {
        //            Session lectu = (Session)cmb1.SelectedItem;
        //            MessageBox.Show(lectu.SessionId.ToString());
        //            if (dbContext1.SessionLecturers.Any(r => r.SessionrId == UpdatingSession.SessionId && r.LecturerId == lectu.SessionId))
        //            {
        //                new MessageBoxCustom("This Lecture already assigned to this Session,Can't add again!", MessageType.Error, MessageButtons.Ok).ShowDialog();
        //                cmb1.SelectedIndex = -1;
        //            }
        //            else
        //            {



        //                Session assgni = new Session
        //                {

        //                    Ssssion = UpdatingSession,
        //                    Lecdetaiils = lectu

        //                };

        //                dbContext1.Sessions.Add(assgni);
        //                dbContext1.SaveChanges();

        //                SessionLIST.Add(lectu);
        //                LVlecturer.ItemsSource = SessionLIST.ToList();
        //                cmb1.SelectedIndex = -1;

        //            }


        //        }



        //    }
        //    else
        //    {
        //        new MessageBoxCustom("Please Select parallel sessions before to adding", MessageType.Warning, MessageButtons.Ok).ShowDialog();
        //    }


        //}

        //private void AddSession(object s, RoutedEventArgs e)
        //{
        //    if (ValidateInput())
        //    {
        //        NewSession.parallelId = txtParallelSession.Text;
        //        NewSession.firstSession = cmb1.Text;
        //        NewSession.secondSession = cmb2.Text;
        //        NewSession.thirdSession = cmb3.Text;

        //        dbContext1.ParallelSessions.Add(NewSession);
        //        dbContext1.SaveChanges();

        //        NewSession = new ParallelSession();
        //        addUpdateSessionDetailsGrid.DataContext = NewSession;

        //        new MessageBoxCustom("Successfully added Parallel Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

        //        clear();
        //        GetSessions();

        //    }
        //    else
        //    {

        //        new MessageBoxCustom("Please complete Parallel Session  details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
        //    }


        //}


        //private void updateSessionsForEdit(object s, RoutedEventArgs e)
        //{
        //    selectedSession = (s as FrameworkElement).DataContext as ParallelSession;

        //    txtParallelSession.Text = selectedSession.parallelId;
        //    cmb1.Text = selectedSession.firstSession;
        //    cmb2.Text = selectedSession.secondSession;
        //    cmb3.Text = selectedSession.thirdSession;

        //}



        //private void UpdateSession(object s, RoutedEventArgs e)
        //{
        //    if (ValidateInput())
        //    {
        //        selectedSession.parallelId = txtParallelSession.Text;
        //        selectedSession.firstSession = cmb1.Text;
        //        selectedSession.secondSession = cmb2.Text;
        //        selectedSession.thirdSession = cmb3.Text;
        //        dbContext1.Update(selectedSession);
        //        dbContext1.SaveChanges();

        //        new MessageBoxCustom("Successfully updated Parallel Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
        //        clear();
        //        GetSessions();
        //    }
        //    else
        //    {

        //        new MessageBoxCustom("Please complete Parallel Session details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
        //    }

        //}

        //private void deleteSession(object s, RoutedEventArgs e)
        //{
        //    bool? Result = new MessageBoxCustom("Are you sure, you want to delete this session detail ? ",
        //     MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

        //    if (Result.Value)
        //    {

        //        var sessionToBeDeleted = (s as FrameworkElement).DataContext as ParallelSession;
        //        dbContext1.ParallelSessions.Remove(sessionToBeDeleted);
        //        dbContext1.SaveChanges();
        //        GetSessions();
        //    }

        //}

        //private void resetSessions(object s, RoutedEventArgs e)
        //{
        //    clear();
        //    ParallelSession NewSession = new ParallelSession();
        //    addUpdateSessionDetailsGrid.DataContext = NewSession;
        //    GetSessions();
        //}

        //private bool ValidateInput()
        //{
        //    if (string.IsNullOrEmpty(txtParallelSession.Text))
        //    {
        //        txtParallelSession.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(cmb1.Text))
        //    {
        //        cmb1.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(cmb2.Text))
        //    {
        //        cmb2.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(cmb3.Text))
        //    {
        //        cmb3.Focus();
        //        return false;
        //    }


        //    return true;
        //}

        private void GoBack(Object s, RoutedEventArgs e)
            {
                MainWindow mainWindow = new MainWindow(dbContext1);
                mainWindow.Show();
                this.Close();

            }


        }
    }

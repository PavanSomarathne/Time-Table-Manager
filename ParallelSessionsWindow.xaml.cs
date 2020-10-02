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
            GetSessions();
            Boolean Addseesion = true;
            addUpdateSessionDetailsGrid.DataContext = NewSession;
            getlecturers();
            loadsessions();
            settingEmptyValues = false;
        }

        private void GetSessions()
        {
            showDetailsGrid.ItemsSource = dbContext1.ParallelSessions.ToList();
        }

        private void clear()
        {

            cmb1.Text = null;
            LVlecturer.ItemsSource = null;
        }

        public void loadsessions()
        {
            cmb1.ItemsSource = dbContext1.Sessions.ToList();
        }

        public void getlecturers()
        {
            cmb1.ItemsSource = dbContext1.LectureInformation.ToList();


        }

    


        private void AssignSessionItemTo(Object s, RoutedEventArgs e)
        {

            if (cmb1.SelectedItem != null)
            {

                parall.Add((Session)cmb1.SelectedItem);
                LVlecturer.ItemsSource = parall.ToList();


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



        }



        private void AddSession(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
               
                ParallelSession parallelSession = new ParallelSession();

                dbContext1.ParallelSessions.Add(NewSession);
                dbContext1.SaveChanges();


                var lastShowPieceId = dbContext1.ParallelSessions.Max(x => x.Id);
                ParallelSession lastinserted = dbContext1.ParallelSessions.FirstOrDefault(x => x.Id == lastShowPieceId);

                foreach (Session session1 in parall) {

                    //dbContext1.Sessions.Update(session1);

                    session1.par = lastinserted;
                    dbContext1.Sessions.Update(session1);
                    dbContext1.SaveChanges();
                  
                } 

                new MessageBoxCustom("Successfully added Parallel Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                GetSessions();

            }
            else
            {

                new MessageBoxCustom("Please complete Parallel Session  details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }


        private bool ValidateInput()
        {

            if (string.IsNullOrEmpty(cmb1.Text))
            {
                cmb1.Focus();
                return false;
            }


            return true;
        }



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




        //private void sessions_load(object sender, RoutedEventArgs e)
        //{
        //    List<String> sesString = new List<String>();
        //    List<Session> sess = dbContext1.Sessions.ToList();

        //    foreach (var item in sess)
        //    {
        //        sesString.Add(item.lecturesLstByConcadinating + "\n " + item.subjectDSA.SubjectName + "(" + item.subjectDSA.SubjectCode + ")" + " \n " + item.tagDSA.tags + "\n " + item.GroupOrsubgroupForDisplay + "\n" + item.StdntCount + "(" + item.durationinHours + ")");
        //    }
        //    cmb1.ItemsSource = sesString;
        //}

        //private void AddSessionDetails(object s, RoutedEventArgs e)

        //{

        //    Addseesion = true;


        //    //concadinate lecturer names
        //    newSessionDl.lecturesLstByConcadinating = null;
        //    foreach (LecturerDetails LL in LeLISTT)
        //    {

        //        newSessionDl.lecturesLstByConcadinating += LL.LecName;
        //    }



        //    dbContext1.Sessions.Add(newSessionDl);

        //    int condition = dbContext1.SaveChanges();



        //    foreach (LecturerDetails lec in LeLISTT)
        //    {


        //        SessionLecturer sessionlc = new SessionLecturer
        //        {

        //            Ssssion = newSessionDl,
        //            Lecdetaiils = lec

        //        };

        //        dbContext1.SessionLecturers.Add(sessionlc);
        //        dbContext1.SaveChanges();


        //    }

        //    settingEmptyValues = true;

        //    LVlecturer.ItemsSource = null;
        //    cmb1.Text = "";


        //    newSessionDl = new Session();

        //    LeLISTT = null;
        //    LeLISTT = new List<LecturerDetails>();


        //    settingEmptyValues = false;
        //    getlecturers();








        //}
        private void GoBack(Object s, RoutedEventArgs e)
            {
                MainWindow mainWindow = new MainWindow(dbContext1);
                mainWindow.Show();
                this.Close();

            }


        }
    }

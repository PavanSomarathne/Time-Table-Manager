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
        //List<LecturerDetails> LeLISTT = new List<LecturerDetails>();
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
            SESSION.ItemsSource = null;
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

                Session TrytoAddSession = (Session)cmb1.SelectedItem;

                System.Collections.IList list = parall;
                for (int i = 0; i < list.Count; i++)
                {
                    Session Lliss = (Session)list[i];
                    if (Lliss.SessionId == TrytoAddSession.SessionId)
                    {
                        cmb1.SelectedIndex = -1;
                        new MessageBoxCustom("This Session already You assigned,Can't add again!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        return;

                    }
                   
                }

                parall.Add((Session)cmb1.SelectedItem);
                SESSION.ItemsSource = parall.ToList();


            }
            else
            {
                new MessageBoxCustom("Please Select Sessions before adding", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }


        private void SelectLectureRooww(object s, RoutedEventArgs e)
        {

            if (SESSION.SelectedItem != null)
            {
                TrashlecBtn.IsEnabled = true;
            }
            else
            {
                TrashlecBtn.IsEnabled = false;
            }

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

                foreach (Session session1 in parall)
                {

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





       


        private void resetSessions(object s, RoutedEventArgs e)
        {
            clear();
            ParallelSession NewSession = new ParallelSession();
            GetSessions();
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }


      

    }
}

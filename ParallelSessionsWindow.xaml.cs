using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private MyDbContext dbContext1;

        ParallelSession NewSession = new ParallelSession();
        ParallelSession selectedSession = new ParallelSession();


        public ParallelSessionsWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();

            loadsessions();
            GetSessions();

            addUpdateSessionDetailsGrid.DataContext = NewSession;
        }

        public void loadsessions()
        {
            p1.ItemsSource = dbContext1.Sessions.Include(s => s.subjectDSA).Include(s => s.tagDSA).ToList();
            p2.ItemsSource = dbContext1.Sessions.Include(s => s.subjectDSA).Include(s => s.tagDSA).ToList();


        }

        private void GetSessions()
        {
            show.ItemsSource = dbContext1.ParallelSessions
                .Include(r => r.first)
                .Include(r => r.second)
                .ToList();
        }

        private void clear()
        {
            p1.Text = null;
            p2.Text = null;

            //addUpdateSessionDetailsGrid.DataContext = null;
        }

        private void AddSession(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                //NewSession.consecutiveId = txtSessionName.Text;
                NewSession.first = (Session)p1.SelectedItem;
                NewSession.second = (Session)p2.SelectedItem;


                if ((Session)p1.SelectedItem == (Session)p2.SelectedItem)
                {
                    new MessageBoxCustom("Can't add same session, Please select Parallel sessions!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;

                }


                if (NewSession.first.tagDSA.tags == NewSession.second.tagDSA.tags)
                {
                    new MessageBoxCustom("Those are not Parallel, Both of the selections can't have same tag!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                if (NewSession.first.subjectDSA.SubjectCode != NewSession.second.subjectDSA.SubjectCode)
                {
                    new MessageBoxCustom("Those are not Parallel, Please use same subject code to create Parallel sessions!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }



                dbContext1.ParallelSessions.Add(NewSession);
                dbContext1.SaveChanges();

                NewSession = new ParallelSession();
                addUpdateSessionDetailsGrid.DataContext = NewSession;

                new MessageBoxCustom("Successfully added Parallel Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                GetSessions();

            }
            else
            {

                new MessageBoxCustom("Please complete  Parallel Session  details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }


        private void updateSessionsForEdit(object s, RoutedEventArgs e)
        {
            selectedSession = (s as FrameworkElement).DataContext as ParallelSession;

            //txtSessionName.Text = selectedSession.consecutiveId;
            p1.SelectedItem = selectedSession.first;
            p2.SelectedItem = selectedSession.second;

        }



        private void UpdateSession(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                //selectedSession.consecutiveId = txtSessionName.Text;
                selectedSession.first = (Session)p1.SelectedItem;
                selectedSession.second = (Session)p2.SelectedItem;

                if ((Session)p1.SelectedItem == (Session)p2.SelectedItem)
                {
                    new MessageBoxCustom("Can't add same session, Please select Parallel sessions!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;

                }


                if (selectedSession.first.tagDSA.tags == selectedSession.second.tagDSA.tags)
                {
                    new MessageBoxCustom("Those are not Parallel, Both of the selections can't have same tag!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                if (selectedSession.first.subjectDSA.SubjectCode != selectedSession.second.subjectDSA.SubjectCode)
                {
                    new MessageBoxCustom("Those are not Parallel, Please use same subject code to create Parallel sessions!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                dbContext1.Update(selectedSession);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated Parallel Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetSessions();
            }
            else
            {

                new MessageBoxCustom("Please complete Parallel Session details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteSession(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this session detail ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var sessionToBeDeleted = (s as FrameworkElement).DataContext as ParallelSession;
                dbContext1.ParallelSessions.Remove(sessionToBeDeleted);
                dbContext1.SaveChanges();
                GetSessions();
            }

        }

        private void resetSessions(object s, RoutedEventArgs e)
        {
            clear();
            ParallelSession NewSession = new ParallelSession();
            addUpdateSessionDetailsGrid.DataContext = NewSession;
            GetSessions();
        }

        private void sessions_load(object sender, RoutedEventArgs e)
        {
            List<String> sesString = new List<String>();
            List<Session> sess = dbContext1.Sessions
                    .Where(r => r.SessionId != -1)
                    .Include(r => r.subjectDSA)
                    .Include(r => r.tagDSA).ToList();
            foreach (var item in sess)
            {
                sesString.Add(item.lecturesLstByConcadinating + "\n " + item.subjectDSA.SubjectName + "(" + item.subjectDSA.SubjectCode + ")" + " \n " + item.tagDSA.tags + "\n " + item.GroupOrsubgroupForDisplay + "\n" + item.StdntCount + "(" + item.durationinHours + ")");
            }
            p1.ItemsSource = sesString;
            p2.ItemsSource = sesString;
        }

        private bool ValidateInput()
        {
            //if (string.IsNullOrEmpty(txtSessionName.Text))
            //{
            //    txtSessionName.Focus();
            //    return false;
            //}

            if (string.IsNullOrEmpty(p1.Text))
            {
                p1.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(p2.Text))
            {
                p2.Focus();
                return false;
            }



            return true;
        }

        private void load_consecfirst(object sender, RoutedEventArgs e)
        {
            List<String> sesString = new List<String>();
            List<ParallelSession> sess = dbContext1.ParallelSessions.Include(r => r.first)
                .Include(r => r.second).ToList();

            foreach (var item in sess)
            {
                sesString.Add(item.first);
            }
            p1.ItemsSource = sesString;
        }

        private void load_consecsecond(object sender, RoutedEventArgs e)
        {
            List<String> sesStrings = new List<String>();
            List<ParallelSession> sessS = dbContext1.ParallelSessions.Include(r => r.first)
                .Include(r => r.second).ToList();

            foreach (var item in sessS)
            {
                sesStrings.Add(item.second);
            }
            p2.ItemsSource = sesStrings;
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }
    }
}

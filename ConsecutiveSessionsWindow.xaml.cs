using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ConsecutiveSessionsWindow.xaml
    /// </summary>
    public partial class ConsecutiveSessionsWindow : Window
    {
        private MyDbContext dbContext1;

        ConsecutiveSession NewSession = new ConsecutiveSession();
        ConsecutiveSession selectedSession = new ConsecutiveSession();


        public ConsecutiveSessionsWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();
           
            loadsessions();
            GetSessions();

            addUpdateSessionDetailsGrid.DataContext = NewSession;
        }

        public void loadsessions()
        {
            cmb1.ItemsSource = dbContext1.Sessions.Include(s => s.subjectDSA).Include(s => s.tagDSA).ToList();
            cmb2.ItemsSource = dbContext1.Sessions.Include(s => s.subjectDSA).Include(s => s.tagDSA).ToList();


        }

        private void GetSessions()
        {
            showDetailsGrid.ItemsSource = dbContext1.ConsecutiveSessions
                .Include(r => r.firstSession)
                .Include(r => r.secondSession)
                .ToList();
        }

        private void clear()
        {
            cmb1.Text = null;
            cmb2.Text = null;

            //addUpdateSessionDetailsGrid.DataContext = null;
        }

        private void AddSession(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                //NewSession.consecutiveId = txtSessionName.Text;
                NewSession.firstSession = (Session)cmb1.SelectedItem;
                NewSession.secondSession = (Session)cmb2.SelectedItem;

                dbContext1.ConsecutiveSessions.Add(NewSession);
                dbContext1.SaveChanges();

                NewSession = new ConsecutiveSession();
                addUpdateSessionDetailsGrid.DataContext = NewSession;

                new MessageBoxCustom("Successfully added Consecutive Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                GetSessions();

            }
            else
            {

                new MessageBoxCustom("Please complete  Consecutive Session  details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }


        private void updateSessionsForEdit(object s, RoutedEventArgs e)
        {
            selectedSession = (s as FrameworkElement).DataContext as ConsecutiveSession;

            //txtSessionName.Text = selectedSession.consecutiveId;
            cmb1.SelectedItem = selectedSession.firstSession;
            cmb2.SelectedItem = selectedSession.secondSession;

        }



        private void UpdateSession(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                //selectedSession.consecutiveId = txtSessionName.Text;
                selectedSession.firstSession = (Session)cmb1.SelectedItem;
                selectedSession.secondSession = (Session)cmb2.SelectedItem;
                dbContext1.Update(selectedSession);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated Consecutive Session details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetSessions();
            }
            else
            {

                new MessageBoxCustom("Please complete Consecutive Session details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteSession(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this session detail ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var sessionToBeDeleted = (s as FrameworkElement).DataContext as ConsecutiveSession;
                dbContext1.ConsecutiveSessions.Remove(sessionToBeDeleted);
                dbContext1.SaveChanges();
                GetSessions();
            }

        }

        private void resetSessions(object s, RoutedEventArgs e)
        {
            clear();
            ConsecutiveSession NewSession = new ConsecutiveSession();
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
            cmb1.ItemsSource = sesString;
            cmb2.ItemsSource = sesString;
        }

        private bool ValidateInput()
        {
            //if (string.IsNullOrEmpty(txtSessionName.Text))
            //{
            //    txtSessionName.Focus();
            //    return false;
            //}

            if (string.IsNullOrEmpty(cmb1.Text))
            {
                cmb1.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(cmb2.Text))
            {
                cmb2.Focus();
                return false;
            }

            return true;
        }

        private void load_consecfirst(object sender, RoutedEventArgs e)
        {
            List<String> sesString = new List<String>();
            List<ConsecutiveSession> sess = dbContext1.ConsecutiveSessions.Include(r => r.firstSession)
                .Include(r => r.secondSession).ToList();

            foreach (var item in sess)
            {
                sesString.Add(item.firstSession);
            }
            cmb1.ItemsSource = sesString;
        }

        private void load_consecsecond(object sender, RoutedEventArgs e)
        {
            List<String> sesStrings = new List<String>();
            List<ConsecutiveSession> sessS = dbContext1.ConsecutiveSessions.Include(r => r.firstSession)
                .Include(r => r.secondSession).ToList();

            foreach (var item in sessS)
            {
                sesStrings.Add(item.secondSession);
            }
            cmb2.ItemsSource = sesStrings;
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }
    }
}

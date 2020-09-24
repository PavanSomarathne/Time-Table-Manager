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
    /// Interaction logic for ViewNotAvailableSessionsWindow.xaml
    /// </summary>
    public partial class ViewNotAvailableSessionsWindow : Window
    {
        MyDbContext dbContext1;
        Sessions_NotAvailable selectedSessions = new Sessions_NotAvailable();
        public ViewNotAvailableSessionsWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetSessions();
        }

        private void GetSessions()
        {
            showGroupsGrid.ItemsSource = dbContext1.Sessions_NotAvailables.ToList();
        }

        private void updateSessionsForEdit(object s, RoutedEventArgs e)
        {
            selectedSessions = (s as FrameworkElement).DataContext as Sessions_NotAvailable;

            cmb1.Text = selectedSessions.notAvailableSession;
            date1.Text = selectedSessions.notAvailableSessionDate;
            st1.Text = selectedSessions.notAvailableSessionStAt;
            et1.Text = selectedSessions.notAvailableSessionEndAt;
        }

        private void clear()
        {
            cmb1.Text = null;
            date1.Text = null;
            st1.Text = null;
            et1.Text = null;
        }

        private void UpdateSessions(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                selectedSessions.notAvailableSession = cmb1.Text;
                selectedSessions.notAvailableSessionDate = date1.Text.ToString();
                selectedSessions.notAvailableSessionStAt = st1.Text.ToString();
                selectedSessions.notAvailableSessionEndAt = et1.Text.ToString();
                dbContext1.Update(selectedSessions);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated Not Available Sessions details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetSessions();
            }
            else
            {

                new MessageBoxCustom("Please complete Not Available Sessions details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteSessions(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var SessionToBeDeleted = (s as FrameworkElement).DataContext as Sessions_NotAvailable;
                dbContext1.Sessions_NotAvailables.Remove(SessionToBeDeleted);
                dbContext1.SaveChanges();
                GetSessions();
            }

        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(cmb1.Text))
            {
                cmb1.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(date1.Text))
            {
                date1.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(st1.Text))
            {
                st1.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(et1.Text))
            {
                et1.Focus();
                return false;
            }


            return true;
        }

        //private void sessions_load(object sender, RoutedEventArgs e)
        //{
        //    List<String> grpString = new List<String>();
        //    List<Student> grps = dbContext1.Students.ToList();

        //    foreach (var item in grps)
        //    {
        //        grpString.Add(item.groupId);
        //    }
        //    cmb1.ItemsSource = grpString;
        //}

        private void Home(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            NotAvailableDetailsWindow notAvailableDetails = new NotAvailableDetailsWindow(dbContext1);
            notAvailableDetails.Show();
            this.Close();

        }
    }
}

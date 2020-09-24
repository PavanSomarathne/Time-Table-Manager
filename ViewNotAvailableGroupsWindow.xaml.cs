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
    /// Interaction logic for ViewNotAvailableGroupsWindow.xaml
    /// </summary>
    public partial class ViewNotAvailableGroupsWindow : Window
    {

        MyDbContext dbContext1;
        Groups_NotAvailable selectedGroups = new Groups_NotAvailable();
        public ViewNotAvailableGroupsWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetGroups();


        }

        private void GetGroups()
        {
            showGroupsGrid.ItemsSource = dbContext1.Groups_NotAvailables.ToList();
        }

        private void updateGroupsForEdit(object s, RoutedEventArgs e)
        {
            selectedGroups = (s as FrameworkElement).DataContext as Groups_NotAvailable;

            cmb1.Text = selectedGroups.notAvailableGroupId;
            date1.Text = selectedGroups.notAvailableGroupIdDate;
            st1.Text = selectedGroups.notAvailableGroupIdStAt;
            et1.Text = selectedGroups.notAvailableGroupIdEndAt;
        }

        private void clear()
        {
            cmb1.Text = null;
            date1.Text = null;
            st1.Text = null;
            et1.Text = null;
        }

        private void UpdateGroups(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                selectedGroups.notAvailableGroupId = cmb1.Text;
                selectedGroups.notAvailableGroupIdDate = date1.Text.ToString();
                selectedGroups.notAvailableGroupIdStAt = st1.Text.ToString();
                selectedGroups.notAvailableGroupIdEndAt = et1.Text.ToString();
                dbContext1.Update(selectedGroups);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated Not Available Groups details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetGroups();
            }
            else
            {

                new MessageBoxCustom("Please complete Not Available Groups details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteGroups(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var GroupToBeDeleted = (s as FrameworkElement).DataContext as Groups_NotAvailable;
                dbContext1.Groups_NotAvailables.Remove(GroupToBeDeleted);
                dbContext1.SaveChanges();
                GetGroups();
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

        private void groups_load(object sender, RoutedEventArgs e)
        {
            List<String> grpString = new List<String>();
            List<Student> grps = dbContext1.Students.ToList();

            foreach (var item in grps)
            {
                grpString.Add(item.groupId);
            }
            cmb1.ItemsSource = grpString;
        }

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

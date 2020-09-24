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
    /// Interaction logic for ViewNotAvailableSubGroupsWindow.xaml
    /// </summary>
    public partial class ViewNotAvailableSubGroupsWindow : Window
    {

        private MyDbContext dbContext1;
        SubGroups_NotAvailable selectedSubGroups = new SubGroups_NotAvailable();

        public ViewNotAvailableSubGroupsWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();
            GetSubGroups();
        }

        private void GetSubGroups()
        {
            showGroupsGrid.ItemsSource = dbContext1.SubGroups_NotAvailables.ToList();
        }

        private void updateSubGroupsForEdit(object s, RoutedEventArgs e)
        {
            selectedSubGroups = (s as FrameworkElement).DataContext as SubGroups_NotAvailable;

            cmb1.Text = selectedSubGroups.notAvailableSubGroupId;
            date1.Text = selectedSubGroups.notAvailableSubGroupIdDate;
            st1.Text = selectedSubGroups.notAvailableSubGroupIdStAt;
            et1.Text = selectedSubGroups.notAvailableSubGroupIdEndAt;
        }

        private void clear()
        {
            cmb1.Text = null;
            date1.Text = null;
            st1.Text = null;
            et1.Text = null;
        }

        private void UpdateSubGroups(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                selectedSubGroups.notAvailableSubGroupId = cmb1.Text;
                selectedSubGroups.notAvailableSubGroupIdDate = date1.Text.ToString();
                selectedSubGroups.notAvailableSubGroupIdStAt = st1.Text.ToString();
                selectedSubGroups.notAvailableSubGroupIdEndAt = et1.Text.ToString();
                dbContext1.Update(selectedSubGroups);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated Not Available Sub Groups details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetSubGroups();
            }
            else
            {

                new MessageBoxCustom("Please complete Not Available Sub Groups details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteSubGroups(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var SubGroupToBeDeleted = (s as FrameworkElement).DataContext as SubGroups_NotAvailable;
                dbContext1.SubGroups_NotAvailables.Remove(SubGroupToBeDeleted);
                dbContext1.SaveChanges();
                GetSubGroups();
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

        private void SubGroups_load(object sender, RoutedEventArgs e)
        {
            List<String> subString = new List<String>();
            List<Student> subs = dbContext1.Students.ToList();

            foreach (var item in subs)
            {
                subString.Add(item.subGroupId);
            }
            cmb1.ItemsSource = subString;
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

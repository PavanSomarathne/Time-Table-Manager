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
    /// Interaction logic for ViewNotAvailableLecturersWindow.xaml
    /// </summary>
    public partial class ViewNotAvailableLecturersWindow : Window
    {
        private MyDbContext dbContext1;
        Lecturers_NotAvailable selectedLecturers = new Lecturers_NotAvailable();

        public ViewNotAvailableLecturersWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();
            GetLecturers();
        }
        private void GetLecturers()
        {
            showGroupsGrid.ItemsSource = dbContext1.Lecturers_NotAvailables.ToList();
        }

        private void updateLecturersForEdit(object s, RoutedEventArgs e)
        {
            selectedLecturers = (s as FrameworkElement).DataContext as Lecturers_NotAvailable;

            cmb1.Text = selectedLecturers.notAvailableLecturerName;
            date1.Text = selectedLecturers.notAvailableLecturerDate;
            st1.Text = selectedLecturers.notAvailableLecturerStAt;
            et1.Text = selectedLecturers.notAvailableLecturerEndAt;
        }

        private void clear()
        {
            cmb1.Text = null;
            date1.Text = null;
            st1.Text = null;
            et1.Text = null;
        }

        private void UpdateLecturers(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                selectedLecturers.notAvailableLecturerName = cmb1.Text;
                selectedLecturers.notAvailableLecturerDate = date1.Text.ToString();
                selectedLecturers.notAvailableLecturerStAt = st1.Text.ToString();
                selectedLecturers.notAvailableLecturerEndAt = et1.Text.ToString();
                dbContext1.Update(selectedLecturers);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated Not Available Lecturers details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetLecturers();
            }
            else
            {

                new MessageBoxCustom("Please complete Not Available Lecturers details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteLecturers(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var LecturerToBeDeleted = (s as FrameworkElement).DataContext as Lecturers_NotAvailable;
                dbContext1.Lecturers_NotAvailables.Remove(LecturerToBeDeleted);
                dbContext1.SaveChanges();
                GetLecturers();
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

        private void lecturers_load(object sender, RoutedEventArgs e)
        {
            List<String> lecString = new List<String>();
            List<LecturerDetails> lecs = dbContext1.LectureInformation.ToList();

            foreach (var item in lecs)
            {
                lecString.Add(item.LecName);
            }
            cmb1.ItemsSource = lecString;
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

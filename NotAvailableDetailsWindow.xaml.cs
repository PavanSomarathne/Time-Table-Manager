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
    /// Interaction logic for NotAvailableDetailsWindow.xaml
    /// </summary>
    public partial class NotAvailableDetailsWindow : Window
    {
        private MyDbContext dbContext1;
        Lecturers_NotAvailable NewNotAvailable = new Lecturers_NotAvailable();
        Groups_NotAvailable groups_NotAvailable = new Groups_NotAvailable();
        SubGroups_NotAvailable subGroups_NotAvailable = new SubGroups_NotAvailable();
        public NotAvailableDetailsWindow()
        {
            InitializeComponent();
        }

        public NotAvailableDetailsWindow(MyDbContext dbContext1)
        {
            this.dbContext1 = dbContext1;
            InitializeComponent();
           

        }

        private void lecturer_load(object sender, RoutedEventArgs e)
        {
            List<String> lecString = new List<String>();
            List<LecturerDetails> lecturers = dbContext1.LectureInformation.ToList();

            foreach (var item in lecturers)
            {
                lecString.Add(item.LecName);
            }
            cmb1.ItemsSource = lecString;
        }

        private void groups_load(object sender, RoutedEventArgs e)
        {
            List<String> grpString = new List<String>();
            List<Student> grps = dbContext1.Students.ToList();

            foreach (var item in grps)
            {
                grpString.Add(item.groupId);
            }
            cmb3.ItemsSource = grpString;
        }

        private void sub_groups_load(object sender, RoutedEventArgs e)
        {
            List<String> subgrpString = new List<String>();
            List<Student> subgrps = dbContext1.Students.ToList();

            foreach (var item in subgrps)
            {
                subgrpString.Add(item.subGroupId);
            }
            cmb4.ItemsSource = subgrpString;
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }

        private void clear()
        {
            cmb1.Text = null;
            date1.Text = null;
            st1.Text = null;
            et1.Text = null;
        }

        private void AddNotAvailableLecturers(object s, RoutedEventArgs e)
        {
            if (ValidateInputForLecturers())
            {
                NewNotAvailable.notAvailableLecturerName = cmb1.Text;
                NewNotAvailable.notAvailableLecturerDate = date1.Text.ToString();
                NewNotAvailable.notAvailableLecturerStAt = st1.Text.ToString();
                NewNotAvailable.notAvailableLecturerEndAt = et1.Text.ToString();
                dbContext1.Lecturers_NotAvailables.Add(NewNotAvailable);
                dbContext1.SaveChanges();

                NewNotAvailable = new Lecturers_NotAvailable();
                addNotAvailableDetailsGrid.DataContext = NewNotAvailable;

                new MessageBoxCustom("Successfully added not available details for lecturers!", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                //GetStudents();

            }
            else
            {

                new MessageBoxCustom("Please complete not available details for lecturers correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }

        private void AddNotAvailableGroups(object s, RoutedEventArgs e)
        {
            if (ValidateInputForGroups())
            {
                groups_NotAvailable.notAvailableGroupId = cmb3.Text;
                groups_NotAvailable.notAvailableGroupIdDate = date3.Text.ToString();
                groups_NotAvailable.notAvailableGroupIdStAt = st3.Text.ToString();
                groups_NotAvailable.notAvailableGroupIdEndAt = et3.Text.ToString();
                dbContext1.Groups_NotAvailables.Add(groups_NotAvailable);
                dbContext1.SaveChanges();

                groups_NotAvailable = new Groups_NotAvailable();
                addNotAvailableDetailsGrid.DataContext = groups_NotAvailable;

                new MessageBoxCustom("Successfully added not available details for groups!", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                //GetStudents();

            }
            else
            {

                new MessageBoxCustom("Please complete not available details for groups correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }

        private void AddNotAvailableSubGroups(object s, RoutedEventArgs e)
        {
            if (ValidateInputForSubGroups())
            {
                subGroups_NotAvailable.notAvailableSubGroupId = cmb4.Text;
                subGroups_NotAvailable.notAvailableSubGroupIdDate = date4.Text.ToString();
                subGroups_NotAvailable.notAvailableSubGroupIdStAt = st4.Text.ToString();
                subGroups_NotAvailable.notAvailableSubGroupIdEndAt = et4.Text.ToString();
                dbContext1.SubGroups_NotAvailables.Add(subGroups_NotAvailable);
                dbContext1.SaveChanges();

                subGroups_NotAvailable = new SubGroups_NotAvailable();
                addNotAvailableDetailsGrid.DataContext = subGroups_NotAvailable;

                new MessageBoxCustom("Successfully added not available details for sub groups!", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                //GetStudents();

            }
            else
            {

                new MessageBoxCustom("Please complete not available details for sub groups correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }


        }

        private bool ValidateInputForLecturers()
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

        private bool ValidateInputForGroups()
        {
            if (string.IsNullOrEmpty(cmb3.Text))
            {
                cmb3.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(date3.Text))
            {
                date3.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(st3.Text))
            {
                st3.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(et3.Text))
            {
                et3.Focus();
                return false;
            }



            return true;
        }

        private bool ValidateInputForSubGroups()
        {
            if (string.IsNullOrEmpty(cmb4.Text))
            {
                cmb4.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(date4.Text))
            {
                date4.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(st4.Text))
            {
                st4.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(et4.Text))
            {
                et4.Focus();
                return false;
            }



            return true;
        }


        public void GotoNotAvailableGroupView(Object s, RoutedEventArgs e)
        {
            ViewNotAvailableGroupsWindow viewG = new ViewNotAvailableGroupsWindow(dbContext1);
            //viewL.Closed += AddClosed;
            viewG.ShowDialog();
        }


        //public void AddClosed(object sender, System.EventArgs e)
        //{
        //    //This gets fired off
        //    GetRooms();
        //    GetBuildings();

        //}


        //public void GotoNotAvailableLecturerView(Object s, RoutedEventArgs e)
        //{
        //    BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
        //    buildingsWindow.Closed += AddClosed;
        //    buildingsWindow.ShowDialog();
        //}

        //public void GotoNotAvailableLecturerView(Object s, RoutedEventArgs e)
        //{
        //    BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
        //    buildingsWindow.Closed += AddClosed;
        //    buildingsWindow.ShowDialog();
        //}

        //public void GotoNotAvailableLecturerView(Object s, RoutedEventArgs e)
        //{
        //    BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
        //    buildingsWindow.Closed += AddClosed;
        //    buildingsWindow.ShowDialog();
        //}
    }
}

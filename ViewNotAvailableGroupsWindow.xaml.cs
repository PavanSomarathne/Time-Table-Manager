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
    }
}

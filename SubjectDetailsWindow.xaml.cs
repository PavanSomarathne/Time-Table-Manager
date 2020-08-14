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
    /// Interaction logic for SubjectDetailsWindow.xaml
    /// </summary>
    public partial class SubjectDetailsWindow : Window
    {
        MyDbContext dbContext1;
        bool update = false;
        SubjectDetails NewSubDL = new SubjectDetails();

        public SubjectDetailsWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetSubjectdetail();

            AddSubjectdetails.DataContext = NewSubDL;
        }


        private void GetSubjectdetail()
        {
            SubjectDG.ItemsSource = dbContext1.SubjectInformation.ToList();
        }

        private void AddSubjectDetails(object s, RoutedEventArgs e)
        {
            if (update)
            {
                dbContext1.Update(selectedSubject);
                ADbuttn.Content = "Add Subject";
            }
            else
            {
                dbContext1.SubjectInformation.Add(NewSubDL);
            }
           
            dbContext1.SaveChanges();
            GetSubjectdetail();
            NewSubDL = new SubjectDetails();
            update = false;
            AddSubjectdetails.DataContext = NewSubDL;
        }

        SubjectDetails selectedSubject = new SubjectDetails();
        private void UpdateSubjecttForEdit(object s,RoutedEventArgs e)
        {
            ADbuttn.Content = "Update Subject";
            update = true;
            selectedSubject = (s as FrameworkElement).DataContext as SubjectDetails;
            AddSubjectdetails.DataContext = selectedSubject;
        }


        private void DeleteSubjectForEdit(object s, RoutedEventArgs e)
        {
            var subjectToBeDeleted = (s as FrameworkElement).DataContext as SubjectDetails;
            dbContext1.SubjectInformation.Remove(subjectToBeDeleted);
            dbContext1.SaveChanges();
            GetSubjectdetail();
        }
    }
}

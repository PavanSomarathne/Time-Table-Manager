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
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        MyDbContext dbContext1;

        Student NewStudent = new Student();
        Student selectedStudent = new Student();

        public StudentWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetStudents();

            addUpdateStudentDetailsGrid.DataContext = NewStudent;
        }

        private void GetStudents()
        {
            showDetailsGrid.ItemsSource = dbContext1.Students.ToList();
        }

        


        private void clear()
        {
            //cmb1.Text = null;
            //cmb2.Text = null;
            //txtGroupNo.Text=null;
            //txtGroupID.Text = null;
            //txtSubGroupNo.Text = null;
            //txtSubGroupID.Text = null;
            addUpdateStudentDetailsGrid.DataContext = null;
        }


      

        private void AddStudent(object s, RoutedEventArgs e)
        {
            //NewStudent1 = GetStudentBeforeAdd(NewStudent);
            NewStudent.groupId = NewStudent.accYrSem + "." + NewStudent.programme + "." + NewStudent.groupNo;
            NewStudent.subGroupId = NewStudent.accYrSem + "." + NewStudent.programme + "." + NewStudent.groupNo + "." + NewStudent.subGroupNo;
            dbContext1.Students.Add(NewStudent);
            dbContext1.SaveChanges();
           
            NewStudent = new Student();
            addUpdateStudentDetailsGrid.DataContext = NewStudent;
            //dbContext.Update(selectedStudent);
            clear();
            GetStudents();
        }

        
        private void updateSdudentForEdit(object s, RoutedEventArgs e)
        {
            selectedStudent = (s as FrameworkElement).DataContext as Student;
            addUpdateStudentDetailsGrid.DataContext = selectedStudent;
        }

       

        private void UpdateStudent(object s, RoutedEventArgs e)
        {
           
            selectedStudent.groupId = selectedStudent.accYrSem + "." + selectedStudent.programme + "." + selectedStudent.groupNo;
            selectedStudent.subGroupId = selectedStudent.accYrSem + "." + selectedStudent.programme + "." + selectedStudent.groupNo + "." + selectedStudent.subGroupNo;
            dbContext1.Update(selectedStudent);
            dbContext1.SaveChanges();
            clear();
            GetStudents();
            
        }

        private void deleteSdudent(object s, RoutedEventArgs e)
        {
            var studentToBeDeleted = (s as FrameworkElement).DataContext as Student;
            dbContext1.Students.Remove(studentToBeDeleted);
            dbContext1.SaveChanges();
            GetStudents();
        }

        private void resetStudent(object s, RoutedEventArgs e)
        {
            clear();
            Student NewStudent = new Student();
            addUpdateStudentDetailsGrid.DataContext = NewStudent;
            GetStudents();
        }

    
        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }


    }
}

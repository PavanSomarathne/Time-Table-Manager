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

        Student NewStudent1 = new Student();


        private void clear()
        {
            //cmb1.Text = "";
            //cmb2.Text = "";
            txtGroupNo.Clear();
            txtGroupID.Clear();
            txtSubGroupNo.Clear();
            txtSubGroupID.Clear();
        }


        private Student GetStudentBeforeAdd(Student student)
        {
            NewStudent.Id = student.Id;
            NewStudent.accYrSem = student.accYrSem;
            NewStudent.programme = student.programme;
            NewStudent.groupNo = student.groupNo;
            NewStudent.groupId = student.accYrSem + "." + student.programme + "." + student.groupNo;
            NewStudent.subGroupNo = student.subGroupNo;
            NewStudent.subGroupId = student.accYrSem + "." + student.programme + "." + student.groupNo + "." + student.subGroupNo;

            return NewStudent;
        }



        private void AddStudent(object s, RoutedEventArgs e)
        {
            NewStudent1 = GetStudentBeforeAdd(NewStudent);
            dbContext1.Students.Add(NewStudent1);
            dbContext1.SaveChanges();
            GetStudents();
            NewStudent = new Student();
            addUpdateStudentDetailsGrid.DataContext = NewStudent;
            //dbContext.Update(selectedStudent);
            clear();
        }

        Student selectedStudent = new Student();
        private void updateSdudentForEdit(object s, RoutedEventArgs e)
        {
            selectedStudent = (s as FrameworkElement).DataContext as Student;
            addUpdateStudentDetailsGrid.DataContext = selectedStudent;
        }

        Student NewStd = new Student();
        Student NewStudent2 = new Student();

        private Student GetStudentBeforeUpdate(Student std)
        {
            NewStd.Id = std.Id;
            NewStd.accYrSem = std.accYrSem;
            NewStd.programme = std.programme;
            NewStd.groupNo = std.groupNo;
            NewStd.groupId = std.accYrSem + "." + std.programme + "." + std.groupNo;
            NewStd.subGroupNo = std.subGroupNo;
            NewStd.subGroupId = std.accYrSem + "." + std.programme + "." + std.groupNo + "." + std.subGroupNo;

            return NewStd;
        }

        private void UpdateStudent(object s, RoutedEventArgs e)
        {
            NewStudent2 = GetStudentBeforeUpdate(NewStd);
            dbContext1.Update(NewStudent2);
            dbContext1.SaveChanges();
            GetStudents();
            clear();
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
        }

        private void showDetailsGrid_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}

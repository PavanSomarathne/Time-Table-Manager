using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
           
            addUpdateStudentDetailsGrid.DataContext = null;
        }


      

        private void AddStudent(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                NewStudent.accYrSem = cmb1.Text;
                NewStudent.programme = cmb2.Text;
                NewStudent.groupNo = int.Parse(txtGroupNo.Text);
                NewStudent.groupId = NewStudent.accYrSem + "." + NewStudent.programme + "." + NewStudent.groupNo;
                NewStudent.subGroupNo = int.Parse(txtSubGroupNo.Text);
                NewStudent.subGroupId = NewStudent.accYrSem + "." + NewStudent.programme + "." + NewStudent.groupNo + "." + NewStudent.subGroupNo;
                dbContext1.Students.Add(NewStudent);
                dbContext1.SaveChanges();

                NewStudent = new Student();
                addUpdateStudentDetailsGrid.DataContext = NewStudent;
                
                new MessageBoxCustom("Successfully added student details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
               
                clear();
                GetStudents();

            }
            else
            {

                new MessageBoxCustom("Please complete  student  details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }
            

            }

        
        private void updateSdudentForEdit(object s, RoutedEventArgs e)
        {
            selectedStudent = (s as FrameworkElement).DataContext as Student;
            addUpdateStudentDetailsGrid.DataContext = selectedStudent;
            //cmb1.Text = selectedStudent.accYrSem;
            //cmb2.Text = selectedStudent.programme;
            //txtGroupNo.Text = selectedStudent.groupNo.ToString();
            //txtSubGroupID.Text = selectedStudent.subGroupNo.ToString();
        }

       

        private void UpdateStudent(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                selectedStudent.accYrSem = cmb1.Text;
                selectedStudent.programme = cmb2.Text;
                selectedStudent.groupNo = int.Parse(txtGroupNo.Text);
                selectedStudent.groupId = selectedStudent.accYrSem + "." + selectedStudent.programme + "." + selectedStudent.groupNo;
                selectedStudent.subGroupNo = int.Parse(txtSubGroupNo.Text);
                selectedStudent.subGroupId = selectedStudent.accYrSem + "." + selectedStudent.programme + "." + selectedStudent.groupNo + "." + selectedStudent.subGroupNo;
                dbContext1.Update(selectedStudent);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated student details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetStudents();
            }
            else
            {

                new MessageBoxCustom("Please complete  student  details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }

        private void deleteSdudent(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this student detail ? ",
             MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {

                var studentToBeDeleted = (s as FrameworkElement).DataContext as Student;
                dbContext1.Students.Remove(studentToBeDeleted);
                dbContext1.SaveChanges();
                GetStudents();
            }

        }

        private void resetStudent(object s, RoutedEventArgs e)
        {
            clear();
            Student NewStudent = new Student();
            addUpdateStudentDetailsGrid.DataContext = NewStudent;
            GetStudents();
        }

        private bool ValidateInput()
        {
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


            if (txtGroupNo.Text.Trim() == "")
            {
                txtGroupNo.Focus();
                return false;
            }

            if (txtSubGroupNo.Text.Trim() == "")
            {
                txtSubGroupNo.Focus();
                return false;
            }


            return true;
        }

        private void NumberValidationForGroupNO(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationForSubGroupNO(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }


    }
}

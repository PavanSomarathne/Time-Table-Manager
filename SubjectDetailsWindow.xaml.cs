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
    /// Interaction logic for SubjectDetailsWindow.xaml
    /// </summary>
    public partial class SubjectDetailsWindow : Window
    {
        MyDbContext dbContext1;
        bool update = false;
        SubjectDetails NewSubDL = new SubjectDetails();
        SubjectDetails selectedSubject = new SubjectDetails();

        public SubjectDetailsWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetSubjectdetail();
            update = false;

       
        }


        private void GetSubjectdetail()
        {
            SubjectDG.ItemsSource = dbContext1.SubjectInformation.ToList();
        }

        private void AddSubjectDetails(object s, RoutedEventArgs e)
        {


            if (update)
            {

                if (ValidateInput())
                {

                    selectedSubject.SubjectName = SubName.Text;
                    selectedSubject.SubjectCode = SubCoDe.Text;
                    selectedSubject.OfferedYear = SubOfferYrr.Text;
                    selectedSubject.OfferedSemester = SubOfferSemmm.Text;
                    selectedSubject.LecHours = int.Parse( LecHrForSub.Text);
                    selectedSubject.TutorialHours = int.Parse(TuteHrForSub.Text);
                    selectedSubject.LabHours = int.Parse(LabHrForSub.Text);
                    selectedSubject.EvalHours =int.Parse(EvalHrForSub.Text);





                    dbContext1.Update(selectedSubject);

                    dbContext1.SaveChanges();

                    GetSubjectdetail();
                    ADbuttn.Content = "Add Subject";

                    MessageBox.Show("Updating the System",
               "Update subject Details!!",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information);

                    update = false;
                    SubName.Text = "";
                    SubCoDe.Text = "";
                    SubOfferYrr.Text = "";
                    SubOfferSemmm.Text = "";
                    LecHrForSub.Text = "";
                    TuteHrForSub.Text = "";
                    LabHrForSub.Text = "";
                    EvalHrForSub.Text = "";
                 


                }
                else
                {

                    MessageBox.Show("Please Complete Subject  Details correctly !",
                             "Input Values not Valid to update",
                             MessageBoxButton.OK,
                             MessageBoxImage.Warning);
                }

            }

            else
            {
                

                if (ValidateInput())
                {

                    NewSubDL.SubjectName = SubName.Text;
                    NewSubDL.SubjectCode = SubCoDe.Text;
                    NewSubDL.OfferedYear = SubOfferYrr.Text; 
                    NewSubDL.OfferedSemester = SubOfferSemmm.Text;
                    NewSubDL.LecHours = int.Parse(LecHrForSub.Text);
                    NewSubDL.TutorialHours = int.Parse(TuteHrForSub.Text);
                    NewSubDL.LabHours = int.Parse(LabHrForSub.Text);
                    NewSubDL.EvalHours = int.Parse(EvalHrForSub.Text);
                    ADbuttn.Content = "Add Subject";
                    dbContext1.SubjectInformation.Add(NewSubDL);
                    NewSubDL = new SubjectDetails();




                    dbContext1.SaveChanges();

                    GetSubjectdetail();

                    MessageBox.Show("Adding subject details Successfully",
                      "subject detail added!!",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);


                    update = false;
                    SubName.Text = "";
                    SubCoDe.Text = "";
                    SubOfferYrr.Text = "";
                    SubOfferSemmm.Text = "";
                    LecHrForSub.Text = "";
                    TuteHrForSub.Text = "";
                    LabHrForSub.Text = "";
                    EvalHrForSub.Text = "";

                }
                else
                {

                    MessageBox.Show("Please Complete subject  Details correctly !",
                            "Input Values not Valid to Add",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }



            }






        }


        private void UpdateSubjecttForEdit(object s,RoutedEventArgs e)
        {
            ADbuttn.Content = "Update Subject";
            update = true;
            selectedSubject = (s as FrameworkElement).DataContext as SubjectDetails;


            SubName.Text = selectedSubject.SubjectName;
            SubCoDe.Text = selectedSubject.SubjectCode;
            SubOfferYrr.Text = selectedSubject.OfferedYear;
            SubOfferSemmm.Text = selectedSubject.OfferedSemester;
            LecHrForSub.Text = selectedSubject.LecHours.ToString();
            TuteHrForSub.Text = selectedSubject.TutorialHours.ToString();
            LabHrForSub.Text = selectedSubject.LabHours.ToString();
            EvalHrForSub.Text = selectedSubject.EvalHours.ToString();



        }


        private void DeleteSubjectForEdit(object s, RoutedEventArgs e)
        {
            var subjectToBeDeleted = (s as FrameworkElement).DataContext as SubjectDetails;
            dbContext1.SubjectInformation.Remove(subjectToBeDeleted);
            dbContext1.SaveChanges();
            GetSubjectdetail();
        }

        private void NumberValidationForLecHours(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void NumberValidationForTuteHours(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationForLabHours(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void NumberValidationForEvaluHours(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }




        private bool ValidateInput()
        {
            if (SubName.Text.Trim() == "")
            {
                SubName.Focus();
                return false;
            }

            if (SubCoDe.Text.Trim() == "")
            {
                SubCoDe.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(SubOfferYrr.Text))
            {
                SubOfferYrr.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(SubOfferSemmm.Text))
            {
                SubOfferSemmm.Focus();
                return false;
            }



            if (LecHrForSub.Text.Trim() == "")
            {
                LecHrForSub.Focus();
                return false;
            }


            if (TuteHrForSub.Text.Trim() == "")
            {
                TuteHrForSub.Focus();
                return false;
            }



            if (LabHrForSub.Text.Trim() == "")
            {
                LabHrForSub.Focus();
                return false;
            }

            if (EvalHrForSub.Text.Trim() == "")
            {
                LabHrForSub.Focus();
                return false;
            }


            return true;
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }

    }
}

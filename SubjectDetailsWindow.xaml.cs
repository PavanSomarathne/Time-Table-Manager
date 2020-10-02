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

                    if (!(selectedSubject.SubjectCode.Equals(SubCoDe.Text.Trim())) && dbContext1.SubjectInformation.Any(r => r.SubjectCode == SubCoDe.Text.Trim()))
                    {
                        new MessageBoxCustom("This Subject code is Already In the System Use a Different Subject Code", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        return;

                    }


                    selectedSubject.SubjectName = SubName.Text.Trim();
                    selectedSubject.SubjectCode = SubCoDe.Text.Trim();
                    selectedSubject.OfferedYear = SubOfferYrr.Text.Trim();
                    selectedSubject.OfferedSemester = SubOfferSemmm.Text.Trim();
                    selectedSubject.LecHours = int.Parse( LecHrForSub.Text.Trim());
                    selectedSubject.TutorialHours = int.Parse(TuteHrForSub.Text.Trim());
                    selectedSubject.LabHours = int.Parse(LabHrForSub.Text.Trim());
                    selectedSubject.EvalHours =int.Parse(EvalHrForSub.Text.Trim());





                    dbContext1.Update(selectedSubject);

                    dbContext1.SaveChanges();

                    GetSubjectdetail();
                    ADbuttn.Content = "Add Subject";

                    new MessageBoxCustom("Successfully Updated Subject details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

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

                    new MessageBoxCustom("Please Complete  subject  Details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                }

            }

            else
            {
                

                if (ValidateInput())
                {
                    if (dbContext1.SubjectInformation.Any(b => b.SubjectCode == SubCoDe.Text.Trim()))
                    {
                        new MessageBoxCustom("This Subject code is Already In the System Use a Different Subject Code", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        return;
                    }


                    NewSubDL.SubjectName = SubName.Text.Trim();
                    NewSubDL.SubjectCode = SubCoDe.Text.Trim();
                    NewSubDL.OfferedYear = SubOfferYrr.Text.Trim(); 
                    NewSubDL.OfferedSemester = SubOfferSemmm.Text.Trim();
                    NewSubDL.LecHours = int.Parse(LecHrForSub.Text.Trim());
                    NewSubDL.TutorialHours = int.Parse(TuteHrForSub.Text.Trim());
                    NewSubDL.LabHours = int.Parse(LabHrForSub.Text.Trim());
                    NewSubDL.EvalHours = int.Parse(EvalHrForSub.Text.Trim());
                    ADbuttn.Content = "Add Subject";
                    dbContext1.SubjectInformation.Add(NewSubDL);
                    NewSubDL = new SubjectDetails();




                    dbContext1.SaveChanges();

                    GetSubjectdetail();

              

                    new MessageBoxCustom("Successfully Added Subject details !", MessageType.Success, MessageButtons.Ok).ShowDialog();


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

                 

                    new MessageBoxCustom("Please Complete  subject  Details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();


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

            bool? Result = new MessageBoxCustom("Are you sure, You want to Delete this Subject Detail ? ",
                    MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {
              
             var subjectToBeDeleted = (s as FrameworkElement).DataContext as SubjectDetails;

               if( checksessionhasSubject(subjectToBeDeleted))
                {
                    new MessageBoxCustom("This subject Is already assigned Session,Before Delete Subject,delete the session", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                dbContext1.SubjectInformation.Remove(subjectToBeDeleted);
                dbContext1.SaveChanges();
                GetSubjectdetail();
            }



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
                EvalHrForSub.Focus();
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


        private bool checksessionhasSubject(SubjectDetails SuBBb)
        {
            bool ty = dbContext1.Sessions.Any(r => r.subjectDSA.Id== SuBBb.Id);

            return ty;
        }

    }
}

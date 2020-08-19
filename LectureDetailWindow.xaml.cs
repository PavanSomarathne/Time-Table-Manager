using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for LectureDetailWindow.xaml
    /// </summary>
    public partial class LectureDetailWindow : Window

    {
        bool updatebtnclicked = false;
        bool lecidSelected = false;
        bool lecLevelSelected = false;
        MyDbContext dbContext1;
        LecturerDetails NewLecDL = new LecturerDetails();
        LecturerDetails selectedLecturedtls = new LecturerDetails();

        public LectureDetailWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();
            getLectureDetai();
            lecidSelected = false;
            lecLevelSelected = false;
            updatebtnclicked = false;
            // AddNewLecturedetails.DataContext = NewLecDL;
        }

        private void GetBuildings()
        {
            CBBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }


        private void getLectureDetai()
        {
            LectureDG.ItemsSource = dbContext1.LectureInformation.Include(r => r.BuildinDSA);


        }

        private void AddLecturerDt(object s, RoutedEventArgs e)
        {




            if (updatebtnclicked)
            {

                if (ValidateInput())
                {


                    selectedLecturedtls.LecName = LecturerName.Text;
                    selectedLecturedtls.EmpId = LecIdName.Text;
                    selectedLecturedtls.Faculty = LecturerFaculty.Text;
                    selectedLecturedtls.Department = LecturerDepartment.Text;
                    selectedLecturedtls.Center = LecturerCenter.Text;
                    selectedLecturedtls.BuildinDSA = (Building)CBBuilding.SelectedItem;
                    selectedLecturedtls.EmpLevel = int.Parse(LecLevel.Text);
                    selectedLecturedtls.Rank = LecLevel.SelectedIndex + 1.ToString() + "." + LecIdName.Text.ToString();





                    dbContext1.Update(selectedLecturedtls);

                    dbContext1.SaveChanges();

                    getLectureDetai();

                    MessageBox.Show("Updating the System",
                    "Update Lecture Details!!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                    Addlecbtn.Content = "Add Lecture";
                    lecidSelected = false;
                    lecLevelSelected = false;
                    updatebtnclicked = false;
                    LecturerName.Text = "";
                    LecIdName.Text = "";
                    LecturerFaculty.Text = "";
                    LecturerDepartment.Text = "";
                    LecturerCenter.Text = "";
                    CBBuilding.Text = "";
                    LecLevel.SelectedIndex = -1;
                    rankDetail.Text = "";


                }
                else
                {

                    MessageBox.Show("Please Complete Lecture  Details correctly !",
                             "Input Values not Valid to update",
                             MessageBoxButton.OK,
                             MessageBoxImage.Warning);
                }

            }

            else
            {

                if (ValidateInput())
                {

                    NewLecDL.LecName = LecturerName.Text;
                    NewLecDL.EmpId = LecIdName.Text;
                    NewLecDL.Faculty = LecturerFaculty.Text;
                    NewLecDL.Department = LecturerDepartment.Text;
                    NewLecDL.Center = LecturerCenter.Text;
                    NewLecDL.BuildinDSA = (Building)CBBuilding.SelectedItem;
                    NewLecDL.EmpLevel = int.Parse(LecLevel.Text);
                    NewLecDL.Rank = LecLevel.SelectedIndex + 1.ToString() + "." + LecIdName.Text.ToString();
                    Addlecbtn.Content = "Add lecture";
                    dbContext1.LectureInformation.Add(NewLecDL);
                    NewLecDL = new LecturerDetails();




                    dbContext1.SaveChanges();

                    getLectureDetai();

                    MessageBox.Show("Adding Lecture details Successfully",
                      "Lecture detail added!!",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);


                    lecidSelected = false;
                    lecLevelSelected = false;
                    updatebtnclicked = false;
                    LecturerName.Text = "";
                    LecIdName.Text = "";
                    LecturerFaculty.Text = "";
                    LecturerDepartment.Text = "";
                    LecturerCenter.Text = "";
                    CBBuilding.Text = "";
                    LecLevel.SelectedIndex = -1;
                    rankDetail.Text = "";

                }
                else
                {

                    MessageBox.Show("Please Complete Lecture  Details correctly !",
                            "Input Values not Valid to Add",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }



            }






        }


        private void LecIdSelectChnged(object sender, TextChangedEventArgs e)
        {
            lecidSelected = true;
            if (lecidSelected && lecLevelSelected)
            {
                int lLvl = LecLevel.SelectedIndex + 1;

                rankDetail.Text = lLvl.ToString() + "." + LecIdName.Text.ToString();
                NewLecDL.Rank = rankDetail.Text.ToString();
            }

        }

        private void LecLevel_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            lecLevelSelected = true;

            if (lecidSelected && lecLevelSelected)
            {
                int lLvl = LecLevel.SelectedIndex + 1;

                rankDetail.Text = lLvl.ToString() + "." + LecIdName.Text.ToString();
                NewLecDL.Rank = rankDetail.Text.ToString();

            }
        }


        private void UpdateLectureDetailsIn(object s, RoutedEventArgs e)
        {
            selectedLecturedtls = (s as FrameworkElement).DataContext as LecturerDetails;
            updatebtnclicked = true;
            Addlecbtn.Content = "update lecture";

            LecturerName.Text = selectedLecturedtls.LecName;
            LecIdName.Text = selectedLecturedtls.EmpId;
            LecturerFaculty.Text = selectedLecturedtls.Faculty;
            LecturerDepartment.Text = selectedLecturedtls.Department;
            LecturerCenter.Text = selectedLecturedtls.Center;
            Building buildinDSA = selectedLecturedtls.BuildinDSA;
            CBBuilding.Text = buildinDSA.Name;
            LecLevel.Text = selectedLecturedtls.EmpLevel.ToString();
            rankDetail.Text = selectedLecturedtls.Rank;



        }

        private void DeleteLectureDetailsIn(object s, RoutedEventArgs e)
        {

            var lectureTobeDeleted = (s as FrameworkElement).DataContext as LecturerDetails;
            dbContext1.LectureInformation.Remove(lectureTobeDeleted);
            dbContext1.SaveChanges();
            getLectureDetai();
        }



        private bool ValidateInput()
        {
            if (LecturerName.Text.Trim() == "")
            {
                LecturerName.Focus();
                return false;
            }

            if (LecIdName.Text.Trim() == "")
            {
                LecIdName.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(LecturerFaculty.Text))
            {
                LecturerFaculty.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(LecturerDepartment.Text))
            {
                LecturerFaculty.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(LecturerCenter.Text))
            {
                LecturerCenter.Focus();
                return false;
            }

            if (CBBuilding.SelectedIndex == -1)
            {
                CBBuilding.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(LecLevel.Text))
            {
                LecLevel.Focus();
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

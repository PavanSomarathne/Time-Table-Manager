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
        bool lecidSelected = false;
        bool lecLevelSelected = false;
        MyDbContext dbContext1;
        LecturerDetails NewLecDL = new LecturerDetails();

        public LectureDetailWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            getLectureDetai();
            lecidSelected = false;
            lecLevelSelected = false;
            AddNewLecturedetails.DataContext = NewLecDL;
        }


        private void getLectureDetai()
        {
            LectureDG.ItemsSource = dbContext1.LectureInformation.ToList();
        }

        private void AddLecturerDt(object s, RoutedEventArgs e)
        {
            dbContext1.LectureInformation.Add(NewLecDL);
            dbContext1.SaveChanges();


            getLectureDetai();
            NewLecDL = new LecturerDetails();
            rankDetail.Text = " ";
            AddNewLecturedetails.DataContext = NewLecDL;
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

        private void LecLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lecLevelSelected = true;

            if(lecidSelected && lecLevelSelected)
            {
                int lLvl = LecLevel.SelectedIndex + 1;

                rankDetail.Text = lLvl.ToString() + "." + LecIdName.Text.ToString();
               
            }
        }
    }

}

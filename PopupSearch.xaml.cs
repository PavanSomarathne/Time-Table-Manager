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
    /// Interaction logic for PopupSearch.xaml
    /// </summary>
    public partial class PopupSearch : Window
    {
        MyDbContext dbContext1;
        String type;
        Room room;

        public PopupSearch(MyDbContext dbContext, String type, Room room)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
            this.type = type;
            this.room = room;
            if (type.Equals("lec"))
            {
                loadLecturers();
            }
            else if (type.Equals("sub"))
            {
                loadSubjects();
            }

        }


        private void loadLecturers()

        {
            LBL1.Content = "Name";
            LBL2.Content = "EMP ID";
            LBL3.Content = "Department";

            lbl0.Content = "Select Lecturer";
            CB1.ItemsSource = dbContext1.LectureInformation.ToList();
        }

        private void loadSubjects()

        {
            LBL1.Content = "Code";
            LBL2.Content = "Name";
            LBL3.Content = "Offered Year";

            lbl0.Content = "Select Subject";
            CB1.ItemsSource = dbContext1.SubjectInformation.ToList();
            CB1.DisplayMemberPath = "SubjectCode";
        }


        private void SelectItem(Object s, RoutedEventArgs e)
        {
            if (type.Equals("lec"))
            {
                LecturerDetails lecturer = (LecturerDetails)CB1.SelectedItem;
                TXT1.Text = lecturer.LecName;
                TXT2.Text = lecturer.EmpId;
                TXT3.Text = lecturer.Department;
            }
        }

        private void AssignItem(Object s, RoutedEventArgs e)
        {
            if (type.Equals("lec"))
            {
                //var roomToAdd = this.dbContext1.Rooms.FirstOrDefault(s => s.Id == 1);

                //LecturerDetails lc1 = this.dbContext1.LectureInformation.FirstOrDefault(s => s.Id == 2);
                LecturerDetails lecturer = (LecturerDetails)CB1.SelectedItem;

                if (!(dbContext1.RoomLecturers.Any(r => r.RoomId == this.room.Id && r.LecturerId == lecturer.Id)))
                {
                    RoomLecturer roomLecturer = new RoomLecturer
                    {
                        Room = this.room,
                        Lecturer = lecturer
                    };

                    dbContext1.RoomLecturers.Add(roomLecturer);
                    dbContext1.SaveChanges();

                    new MessageBoxCustom("Lecturer Assigned Successfully !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    new MessageBoxCustom("This Lecturer is Already Assigned to this Room !", MessageType.Error, MessageButtons.Ok).ShowDialog();

                }
            }else if (type.Equals("sub"))
            {
                //var roomToAdd = this.dbContext1.Rooms.FirstOrDefault(s => s.Id == 1);

                //LecturerDetails lc1 = this.dbContext1.LectureInformation.FirstOrDefault(s => s.Id == 2);
                SubjectDetails subject = (SubjectDetails)CB1.SelectedItem;

                if (!(dbContext1.RoomSubjects.Any(r => r.RoomId == this.room.Id && r.SubjectId == subject.Id)))
                {
                    RoomSubject roomSubject = new RoomSubject
                    {
                        Room = this.room,
                        Subject = subject
                    };

                    dbContext1.RoomSubjects.Add(roomSubject);
                    dbContext1.SaveChanges();

                    new MessageBoxCustom("Subject Assigned Successfully !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    new MessageBoxCustom("This Subject is Already Assigned to this Room !", MessageType.Error, MessageButtons.Ok).ShowDialog();

                }
            }
        }
    }
}

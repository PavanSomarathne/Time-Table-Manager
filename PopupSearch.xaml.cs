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
            else if (type.Equals("ses"))
            {
                loadSessions();
            }
            else if (type.Equals("grp"))
            {
                loadGroups();
            }

        }


        private void loadLecturers()

        {
            LBL1.Content = "Name";
            LBL2.Content = "EMP ID";
            LBL3.Content = "Department";

            lbl0.Content = "Select Lecturer";
            CB1.ItemsSource = dbContext1.LectureInformation.ToList();
            CB1.DisplayMemberPath = "LecName";
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

        private void loadSessions()

        {
            LBL1.Content = "ID";
            LBL2.Content = "Subject";
            LBL3.Content = "Tag";

            lbl0.Content = "Select Session";
            warningMSG.Visibility = Visibility.Visible;

            if (room.Type.Equals("LectureRoom"))
            {
                CB1.ItemsSource = dbContext1.Sessions
                    .Where(r => r.tagDSA.tags.Equals("Lecture") || r.tagDSA.tags.Equals("Tutorial"))
                    .Include(s => s.subjectDSA)
                    .Include(s => s.tagDSA)
                    .Include(s => s.Room)
                    .ToList();

                warningTxt.Text = "Selected Room is a Lecture Hall Showing only Lectures and Tutorial Sessions !";
            }
            else
            {
                CB1.ItemsSource = dbContext1.Sessions
                .Where(r => r.tagDSA.tags.Equals("Practical"))
                .Include(s => s.subjectDSA)
                .Include(s => s.tagDSA)
                .Include(s => s.Room)
                .ToList();

                warningTxt.Text = "Selected Room is a Lab Showing only Practical and Evaluation Sessions !";
            }



        }


        private void loadGroups()

        {
            LBL1.Content = "";
            LBL2.Content = "";
            LBL3.Content = "";
            TXT1.Visibility = Visibility.Hidden;
            TXT2.Visibility = Visibility.Hidden;
            TXT3.Visibility = Visibility.Hidden;

            lbl0.Content = "Select a Group";

            List<String> data1 = dbContext1.Students.Select(r => r.groupId).Distinct().ToList();
            List<String> data = dbContext1.Students.Select(r => r.subGroupId).Distinct().ToList();

            foreach (String s in data)
            {
                data1.Add(s);
            }
            CB1.ItemsSource = data1;
        }


        private void SelectItem(Object s, RoutedEventArgs e)
        {
            if (CB1.SelectedItem != null)
            {
                if (type.Equals("lec"))
                {
                    LecturerDetails lecturer = (LecturerDetails)CB1.SelectedItem;
                    TXT1.Text = lecturer.LecName;
                    TXT2.Text = lecturer.EmpId;
                    TXT3.Text = lecturer.Department;
                }
                else if (type.Equals("ses"))
                {
                    Session session = (Session)CB1.SelectedItem;
                    TXT1.Text = session.SessionId.ToString();
                    TXT2.Text = session.subjectDSA.SubjectCode;
                    TXT3.Text = session.tagDSA.tags;
                }
            }
        }

        private void Close(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AssignItem(Object s, RoutedEventArgs e)
        {
            if (CB1.SelectedItem != null)
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
                }
                else if (type.Equals("sub"))
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
                else if (type.Equals("ses"))
                {
                    //var roomToAdd = this.dbContext1.Rooms.FirstOrDefault(s => s.Id == 1);

                    //LecturerDetails lc1 = this.dbContext1.LectureInformation.FirstOrDefault(s => s.Id == 2);
                    Session session = (Session)CB1.SelectedItem;


                    if (!(dbContext1.Sessions.Any(r => r.Room.Id == this.room.Id && r.SessionId == session.SessionId)))
                    {

                        if (session.Room != null)
                        {
                            bool? Result = new MessageBoxCustom("This Session is Already Assigned to Room "+session.Room.Rid
                                +", Do you want to continue? ",
                            MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

                            if (!Result.Value)
                            {
                                return;
                            }
                        }

                            ConsecutiveSession conSession;
                        Session secondSession;
                        if (dbContext1.ConsecutiveSessions.Any(r => r.firstSession.SessionId == session.SessionId
                        || r.secondSession.SessionId == session.SessionId))
                        {
                            conSession = dbContext1.ConsecutiveSessions
                            .Where(j => j.firstSession.SessionId == session.SessionId || j.secondSession.SessionId == session.SessionId)
                            .FirstOrDefault();

                            if (conSession.firstSession.SessionId == session.SessionId)
                            {
                                secondSession = conSession.secondSession;
                            }
                            else
                            {
                                secondSession = conSession.firstSession;
                            }

                            new MessageBoxCustom("Found A Consecutive Session ! \n" + secondSession.ToString(), MessageType.Warning, MessageButtons.Ok).ShowDialog();

                            session.Room = this.room;
                            dbContext1.Sessions.Update(session);

                            secondSession.Room = this.room;
                            dbContext1.Sessions.Update(secondSession);

                            dbContext1.SaveChanges();

                            new MessageBoxCustom("Both Sessions Assigned to " + this.room.Rid + " Successfully !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                        }
                        else
                        {
                            session.Room = this.room;
                            dbContext1.Sessions.Update(session);
                            dbContext1.SaveChanges();

                            new MessageBoxCustom("Session Assigned Successfully !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("This Session is Already Assigned to this Room !", MessageType.Error, MessageButtons.Ok).ShowDialog();

                    }
                }
                else if (type.Equals("grp"))
                {

                    RoomGroup roomGroup = new RoomGroup
                    {
                        room = this.room,
                        Group = CB1.Text

                    };

                    if (dbContext1.RoomGroups.Any(r => r.Group == roomGroup.Group && r.room.Id == roomGroup.room.Id))
                    {
                        new MessageBoxCustom("This Group has alreday Assigned to this room!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    }
                    else
                    {

                        dbContext1.RoomGroups.Add(roomGroup);
                        dbContext1.SaveChanges();

                        new MessageBoxCustom("Group Assigned Successfully !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                    }
                }
            }
            else
            {
                new MessageBoxCustom("No Selection!", MessageType.Error, MessageButtons.Ok).ShowDialog();

            }
        }
    }
}

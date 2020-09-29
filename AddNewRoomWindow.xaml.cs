using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for AddNewRoomWindow.xaml
    /// </summary>
    public partial class AddNewRoomWindow : Window
    {
        MyDbContext dbContext1;
        Building SeletedBuilding = new Building();
        Room RoomToEdit = new Room();

        public AddNewRoomWindow(MyDbContext dbContext, Room room)
        {
            RoomToEdit = room;
            dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();

            if (RoomToEdit != null)
            {

                EnableUpdateMode();
            }
        }

        private void GetBuildings()
        {
            CBBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }

        private void EnableUpdateMode()
        {
            TxtTitle.Content = "Change Room Details";
            TxtRid.Text = RoomToEdit.Rid;
            TxtCapacity.Text = RoomToEdit.Capacity.ToString();
            CBBuilding.SelectedItem = RoomToEdit.BuildingAS;
            CBType.Text = RoomToEdit.Type;

            BtnSave.Content = "Update";
            BtnSave.Click -= AddRoom;
            BtnSave.Click += UpdateRoom;

            LoadLecturers();
            LoadSubjects();
            LoadNATS();
            LoadSessions();
            LoadGroups();
        }

        private void LoadLecturers()
        {
            LVlecturer.ItemsSource = dbContext1.Rooms
            .Where(p => p.Id == RoomToEdit.Id)
            .SelectMany(r => r.RoomLecturers)
            .Select(rl => rl.Lecturer).ToList();
        }

        private void LoadSubjects()
        {
            LVSubjects.ItemsSource = dbContext1.Rooms
            .Where(p => p.Id == RoomToEdit.Id)
            .SelectMany(r => r.RoomSubjects)
            .Select(rl => rl.Subject).ToList();
        }

        private void LoadNATS()
        {

            LVNAT.ItemsSource = dbContext1.RoomNATs
            .Where(r => r.room.Id == RoomToEdit.Id)
            .ToList();
        }

        private void LoadSessions()
        {

            SessionDG.ItemsSource = dbContext1.Sessions
            .Where(p => p.Room.Id == RoomToEdit.Id).ToList();
        }

        private void LoadGroups()
        {

            LVGroups.ItemsSource = dbContext1.RoomGroups
            .Where(p => p.room.Id == RoomToEdit.Id).ToList();
        }

        private void AddRoom(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Room NewRoom = new Room();

                NewRoom.Rid = TxtRid.Text;
                NewRoom.Capacity = int.Parse(TxtCapacity.Text);
                NewRoom.BuildingAS = (Building)CBBuilding.SelectedItem;
                NewRoom.Type = CBType.Text;

                //insert that object to database
                if (dbContext1.Rooms.Any(r => r.Rid == TxtRid.Text))
                {
                    new MessageBoxCustom("This ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();

                }
                else
                {

                    dbContext1.Rooms.Add(NewRoom);
                    dbContext1.SaveChanges();
                    new MessageBoxCustom("Successfully Added to the System !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                    TxtCapacity.Text = "";
                    TxtRid.Text = "";
                    CBBuilding.SelectedIndex = -1;
                    CBType.SelectedIndex = -1;


                }
            }
            else
            {
                new MessageBoxCustom("Please Complete Room Details to Continue !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

            }

        }

        private void UpdateRoom(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {


                //insert that object to database
                if (!(RoomToEdit.Rid.Equals(TxtRid.Text.Trim())) && dbContext1.Rooms.Any(r => r.Rid == TxtRid.Text))
                {
                    new MessageBoxCustom("This ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();

                }
                else
                {
                    RoomToEdit.Rid = TxtRid.Text;
                    RoomToEdit.Type = CBType.Text;
                    RoomToEdit.BuildingAS = (Building)CBBuilding.SelectedItem;
                    RoomToEdit.Capacity = int.Parse(TxtCapacity.Text);
                    //  var query = $"UPDATE Buildings SET Id = '{txtId.Text}', Name = '{txtName.Text}' WHERE Id='{SeletedBuilding.Id}'";
                    //  BuildingDG.SelectedItem = NewBuilding;

                    // dbContext1.Database.ExecuteSqlRaw(query);
                    dbContext1.Rooms.Update(RoomToEdit);
                    dbContext1.SaveChanges();
                    this.Close();

                }


            }
            else
            {
                new MessageBoxCustom("Please Complete Room Details to Continue !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

            }
        }

        private void CloseWindow(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddLec(Object s, RoutedEventArgs e)
        {
            PopupSearch popup = new PopupSearch(dbContext1, "lec", RoomToEdit);
            popup.Closed += RefreshLec;
            popup.ShowDialog();
        }

        private void DelLec(Object s, RoutedEventArgs e)
        {
            //Lvlecturer list name
            if (LVlecturer.SelectedItem != null)
            {
                LecturerDetails lecturer = (LecturerDetails)LVlecturer.SelectedItem;
                RoomLecturer roomLecturer = new RoomLecturer
                {
                    Lecturer = lecturer,
                    Room = RoomToEdit
                };

                if (dbContext1.RoomLecturers.Any(r => r.RoomId == this.RoomToEdit.Id && r.LecturerId == lecturer.Id))
                {
                    //dbContext1.Entry(roomLecturer).State = EntityState.Detached;
                    var roomLec = dbContext1.RoomLecturers.First(row => row.RoomId == RoomToEdit.Id && row.LecturerId == lecturer.Id);
                    dbContext1.RoomLecturers.Remove(roomLec);
                    dbContext1.SaveChanges();

                    LVlecturer.SelectedIndex = -1;
                    LoadLecturers();
                }
            }
        }

        private void LecDelActive(Object s, RoutedEventArgs e)
        {
            if (LVlecturer.SelectedItem != null)
            {
                BTNdelLec.IsEnabled = true;
            }
            else
            {
                BTNdelLec.IsEnabled = false;
            }
        }


        private void AddNAT(Object s, RoutedEventArgs e)
        {
            PopupTimeSelect popup = new PopupTimeSelect(dbContext1, RoomToEdit);
            popup.Closed += RefreshNAT;
            popup.ShowDialog();
        }

        private void DelNAT(Object s, RoutedEventArgs e)
        {
            if (LVNAT.SelectedItem != null)
            {
                RoomNAT roomNAT = (RoomNAT)LVNAT.SelectedItem;

                if (dbContext1.RoomNATs.Any(r => r.room.Id == this.RoomToEdit.Id && r.Id == roomNAT.Id))
                {
                    //dbContext1.Entry(roomLecturer).State = EntityState.Detached;
                    var rrn = dbContext1.RoomNATs.First(row => row.Id == roomNAT.Id);
                    dbContext1.RoomNATs.Remove(rrn);
                    dbContext1.SaveChanges();

                    LVNAT.SelectedIndex = -1;
                    LoadNATS();
                }
            }
        }

        private void NATDelActive(Object s, RoutedEventArgs e)
        {
            if (LVNAT.SelectedItem != null)
            {
                BTNdelNAT.IsEnabled = true;
            }
            else
            {
                BTNdelNAT.IsEnabled = false;
            }
        }

        private void AddSession(Object s, RoutedEventArgs e)
        {
            PopupSearch popup = new PopupSearch(dbContext1, "ses", RoomToEdit);
            popup.Closed += RefreshSes;
            popup.ShowDialog();
        }

        private void DelSession(Object s, RoutedEventArgs e)
        {
            if (SessionDG.SelectedItem != null)
            {
                Session session = (Session)SessionDG.SelectedItem;

                if (dbContext1.Sessions.Any(r => r.Room.Id == this.RoomToEdit.Id && r.SessionId == session.SessionId))
                {
                    //dbContext1.Entry(roomLecturer).State = EntityState.Detached;
                    session.Room = null;
                    dbContext1.Update(session);
                    dbContext1.SaveChanges();

                    SessionDG.SelectedIndex = -1;
                    LoadSessions();
                }
            }
        }

        private void SeesionDelAct(Object s, RoutedEventArgs e)
        {
            if (SessionDG.SelectedItem != null)
            {
                BTNDelSession.IsEnabled = true;
            }
            else
            {
                BTNDelSession.IsEnabled = false;
            }
        }

        private void AddSub(Object s, RoutedEventArgs e)
        {
            PopupSearch popup = new PopupSearch(dbContext1, "sub", RoomToEdit);
            popup.Closed += RefreshSub;
            popup.ShowDialog();
        }

        private void SubDelActive(Object s, RoutedEventArgs e)
        {
            if (LVSubjects.SelectedItem != null)
            {
                BTNdelSub.IsEnabled = true;
            }
            else
            {
                BTNdelSub.IsEnabled = false;
            }
        }

        private void DelSub(Object s, RoutedEventArgs e)
        {
            if (LVSubjects.SelectedItem != null)
            {
                SubjectDetails subject = (SubjectDetails)LVSubjects.SelectedItem;
                RoomSubject roomSubject = new RoomSubject
                {
                    Subject = subject,
                    Room = RoomToEdit
                };

                if (dbContext1.RoomSubjects.Any(r => r.RoomId == this.RoomToEdit.Id && r.SubjectId == subject.Id))
                {
                    //dbContext1.Entry(roomLecturer).State = EntityState.Detached;
                    var roomSub = dbContext1.RoomSubjects.First(row => row.RoomId == RoomToEdit.Id && row.SubjectId == subject.Id);
                    dbContext1.RoomSubjects.Remove(roomSub);
                    dbContext1.SaveChanges();

                    LVSubjects.SelectedIndex = -1;
                    LoadSubjects();
                }
            }
        }

        private void AddGRP(Object s, RoutedEventArgs e)
        {
            PopupSearch popup = new PopupSearch(dbContext1, "grp", RoomToEdit);
            popup.Closed += RefreshGRP;
            popup.ShowDialog();
        }

        private void DelGRP(Object s, RoutedEventArgs e)
        {
            if (LVGroups.SelectedItem != null)
            {
                RoomGroup roomGroup = (RoomGroup)LVGroups.SelectedItem;

                if (dbContext1.RoomGroups.Any(r => r.room.Id == this.RoomToEdit.Id && r.Id == roomGroup.Id))
                {
                    //dbContext1.Entry(roomLecturer).State = EntityState.Detached;
                    var rrn = dbContext1.RoomGroups.First(row => row.Id == roomGroup.Id);
                    dbContext1.RoomGroups.Remove(rrn);
                    dbContext1.SaveChanges();

                    LVGroups.SelectedIndex = -1;
                    LoadGroups();
                }
            }
        }

        private void GRPDelActive(Object s, RoutedEventArgs e)
        {
            if (LVGroups.SelectedItem != null)
            {
                BTNdelGRP.IsEnabled = true;
            }
            else
            {
                BTNdelGRP.IsEnabled = false;
            }
        }

        private bool ValidateInput()
        {
            if (TxtRid.Text.Trim() == "")
            {
                TxtRid.Focus();
                return false;
            }

            if (TxtCapacity.Text.Trim() == "")
            {
                TxtCapacity.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(CBType.Text))
            {
                CBType.Focus();
                return false;
            }

            if (CBBuilding.SelectedIndex == -1)
            {
                CBBuilding.Focus();
                return false;
            }

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {

            this.Close();

        }


        public void RefreshLec(object sender, System.EventArgs e)
        {
            //This gets fired off
            LVlecturer.ItemsSource = null;
            LoadLecturers();

        }

        public void RefreshSub(object sender, System.EventArgs e)
        {
            //This gets fired off
            LVlecturer.ItemsSource = null;
            LoadSubjects();

        }

        public void RefreshNAT(object sender, System.EventArgs e)
        {
            //This gets fired off
            LVNAT.ItemsSource = null;
            LoadNATS();

        }

        public void RefreshSes(object sender, System.EventArgs e)
        {
            //This gets fired off
            SessionDG.ItemsSource = null;
            LoadSessions();

        }

        public void RefreshGRP(object sender, System.EventArgs e)
        {
            //This gets fired off
            SessionDG.ItemsSource = null;
            LoadGroups();

        }
    }
}
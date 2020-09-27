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
    /// Interaction logic for PopupTimeSelect.xaml
    /// </summary>
    public partial class PopupTimeSelect : Window
    {
        MyDbContext dbContext1;
        String type;
        Room roomToEdit;

        public PopupTimeSelect(MyDbContext dbContext, Room room)
        {
            this.dbContext1 = dbContext;
            this.roomToEdit = room;
            InitializeComponent();
        }

        private void CB1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void AssignItem(Object s, RoutedEventArgs e)
        {
            if (Validate())
            {
                RoomNAT roomNAT = new RoomNAT
                {
                    room = roomToEdit,
                    Day = CB1.Text,
                    StartTime = TM1.Text,
                    EndTime = TM2.Text
                };

                if (dbContext1.RoomNATs.Any(r => r.Day == roomNAT.Day
                && r.StartTime == roomNAT.StartTime && r.EndTime == roomNAT.EndTime && r.room.Id == roomNAT.room.Id))
                {
                    new MessageBoxCustom("This Time has alreday set as not availble for this room!", MessageType.Error, MessageButtons.Ok).ShowDialog();
                }
                else
                {

                    dbContext1.RoomNATs.Add(roomNAT);
                    dbContext1.SaveChanges();

                    new MessageBoxCustom("Time Assigned Successfully !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                }
            }
            else
            {
                new MessageBoxCustom("Please Provide A Valid Time!", MessageType.Error, MessageButtons.Ok).ShowDialog();

            }



        }


        private bool Validate()
        {

            if (TM1.Text == null || TM1.Text.Trim().Equals("") )
                return false;
        
            if (TM2.Text == null || TM2.Text.Trim().Equals("") )
                return false;

            if (CB1.SelectedItem == null || CB1.SelectedIndex ==-1)
                return false;

            return true;
        }

        private void close(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

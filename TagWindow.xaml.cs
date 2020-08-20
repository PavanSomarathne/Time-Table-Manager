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
    /// Interaction logic for TagWindow.xaml
    /// </summary>
    public partial class TagWindow : Window
    {
        MyDbContext dbContext1;

        Tag NewTag = new Tag();
        Tag selectedTags = new Tag();
        public TagWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetTags();

            addUpdateTagsDetailsGrid.DataContext = NewTag;
        }

        private void GetTags()
        {
            showTagsDetailsGrid.ItemsSource = dbContext1.Tags.ToList();
        }

        private void clear()
        {

            addUpdateTagsDetailsGrid.DataContext = null;
        }

        private void AddTags(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                NewTag.tags = cmb1.Text;
                dbContext1.Tags.Add(NewTag);
                dbContext1.SaveChanges();
           
                NewTag = new Tag();
                addUpdateTagsDetailsGrid.DataContext = NewTag;

                new MessageBoxCustom("Successfully added tag details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                clear();
                GetTags();
            }
            else
            {

                new MessageBoxCustom("Please complete  tag  details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }

        }


        private void updateTagsForEdit(object s, RoutedEventArgs e)
        {
            selectedTags = (s as FrameworkElement).DataContext as Tag;
            addUpdateTagsDetailsGrid.DataContext = selectedTags;
        }

        private void UpdateTags(object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                selectedTags.tags = cmb1.Text;
                dbContext1.Update(selectedTags);
                dbContext1.SaveChanges();

                new MessageBoxCustom("Successfully updated tag details !", MessageType.Success, MessageButtons.Ok).ShowDialog();
                clear();
                GetTags();

            }
            else
            {

                new MessageBoxCustom("Please complete  tag  details correctly  !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }
        }

        private void deleteTags(object s, RoutedEventArgs e)
        {
            bool? Result = new MessageBoxCustom("Are you sure, you want to delete this tag detail ? ",
            MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {
                var tagsToBeDeleted = (s as FrameworkElement).DataContext as Tag;
                dbContext1.Tags.Remove(tagsToBeDeleted);
                dbContext1.SaveChanges();
                GetTags();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(cmb1.Text))
            {
                cmb1.Focus();
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

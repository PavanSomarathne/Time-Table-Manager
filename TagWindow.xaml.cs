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

        private void AddTags(object s, RoutedEventArgs e)
        {

            dbContext1.Tags.Add(NewTag);
            dbContext1.SaveChanges();
            GetTags();
            NewTag = new Tag();
            addUpdateTagsDetailsGrid.DataContext = NewTag;
            dbContext1.Update(selectedTags);
        }

        Tag selectedTags = new Tag();
        private void updateTagsForEdit(object s, RoutedEventArgs e)
        {
            selectedTags = (s as FrameworkElement).DataContext as Tag;
            addUpdateTagsDetailsGrid.DataContext = selectedTags;
        }

        private void UpdateTags(object s, RoutedEventArgs e)
        {
            dbContext1.Update(selectedTags);
            dbContext1.SaveChanges();
            GetTags();
        }

        private void deleteTags(object s, RoutedEventArgs e)
        {
            var tagsToBeDeleted = (s as FrameworkElement).DataContext as Tag;
            dbContext1.Tags.Remove(tagsToBeDeleted);
            dbContext1.SaveChanges();
            GetTags();
        }
    }
}

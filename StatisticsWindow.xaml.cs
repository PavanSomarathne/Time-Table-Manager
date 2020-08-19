using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using TimeTableManager.Models;

namespace TimeTableManager
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
       
    {
        MyDbContext dbContext1;
        public StatisticsWindow(MyDbContext dbContext)
        {
            dbContext1 = dbContext;
            InitializeComponent();
            LoadStudentStats();
        }

        public void LoadStudentStats()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Students According to Subject",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //also adding values updates and animates the chart automatically

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            DataContext = this;

            //Second Chart

            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Students According to Subject",
                    Values = new ChartValues<double> { 50, 56, 37, 50 }
                }
            };

            //also adding values updates and animates the chart automatically

            Labels2 = new[] { "CSN", "IM", "New Subjetc 567", "Datamobile 65678 sjj" };
            Formatter2 = value => value.ToString("N");

            Func<ChartPoint, string> labelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            pieChart1.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Maria",
                    Values = new ChartValues<double> {3},
                    PushOut = 15,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<double> {4},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<double> {6},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frederic",
                    Values = new ChartValues<double> {2},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };

            pieChart2.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Maria",
                    Values = new ChartValues<double> {3},
                    PushOut = 15,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<double> {4},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<double> {6},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frederic",
                    Values = new ChartValues<double> {2},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };

        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }



        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels2 { get; set; }
        public Func<double, string> Formatter2 { get; set; }


    }
}

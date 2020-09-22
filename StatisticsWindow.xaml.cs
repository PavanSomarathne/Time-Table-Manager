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
using System.Windows.Threading;
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
        int StudentCount;
        int LecturerCount;
        int SubjectsCount;
        public StatisticsWindow(MyDbContext dbContext)
        {
            dbContext1 = dbContext;
            InitializeComponent();
            LoadNumbers();
            LoadStudentStats(null,null);

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            txtDateTime.Content = DateTime.Now.ToString();
        }

        public void LoadNumbers()
        {
            StudentCount = dbContext1.Students.Count();
            LecturerCount = dbContext1.LectureInformation.Count();
            SubjectsCount = dbContext1.SubjectInformation.Count();

            txtNoStudents.Content = StudentCount.ToString();
            txtNoLecturers.Content = LecturerCount.ToString();
            txtNoSubjects.Content = SubjectsCount.ToString();
        }

        public void LoadStudentStats(Object s, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection();
            txtChart1.Content = "Student According to Semester";
            txtC1X.Title = "Semester";
            txtC1Y.Title = "Number Of Students";

            C1Values = new ChartValues<double>();
            Labels = new List<string>();
            Labels.Clear();
            dbContext1.Students.GroupBy(s => s.accYrSem).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                C1Values.Add(r.sum);
                Labels.Add(r.name);
            });

            SeriesCollection.Clear();

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Students According to Subject",
                Values = C1Values

            });

            Formatter = value => value.ToString("N");

            DataContext = this;

            CH1.Series = SeriesCollection;
            txtC1X.Labels = Labels;

            //Second Chart
            txtChart2.Content = "Students According to Group";
            txtC2X.Title = "Student Group";
            txtC2Y.Title = "Number Of Students";


            C2Values = new ChartValues<double>();
            Labels2 = new List<string>();

            dbContext1.Students.GroupBy(s => s.groupNo).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                C2Values.Add(r.sum);
                Labels2.Add(r.name.ToString());
            });

            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Students According to Group",
                    Values = C2Values
                }
            };

            Formatter2 = value => value.ToString("N");

            Func<ChartPoint, string> labelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            CH2.Series = SeriesCollection2;
            txtC2X.Labels = Labels2;

            //Student Pie Chart1

            P1Series = new SeriesCollection();
            txtP1.Text = "Students According to Programme";

            dbContext1.Students.GroupBy(s => s.programme).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                P1Series.Add(new PieSeries
                {
                    Title = r.name,
                    Values = new ChartValues<double> { r.sum },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            });

            pieChart1.Series = P1Series;

            //Pie Chart 2

            txtP2.Text = "Students Amount Compared to Lecturers";

            pieChart2.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Students",
                    Values = new ChartValues<double> {StudentCount},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Lecturers",
                    Values = new ChartValues<double> {LecturerCount},
                    DataLabels = true,
                    PushOut = 15,
                    LabelPoint = labelPoint
                },
            };

        }

        public void LoadSubjectsStats(Object s, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection();

            txtChart1.Content = "Subjects with highest Lab hours";
            txtC1X.Title = "Subject Code";
            txtC1Y.Title = "Number Of Lab Hours";

            SeriesCollection.Clear();
            
            C1Values = new ChartValues<double>();
            Labels = new List<string>();
            Labels.Clear();
            dbContext1.SubjectInformation.OrderByDescending(x => x.LabHours).Take(7)
            .ToList().ForEach(r =>
            {
                C1Values.Add(r.LabHours);
                Labels.Add(r.SubjectCode);
            });

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Subjects with highest Lab hours",
                Values = C1Values

            });
            Formatter = value => value.ToString("N");

            DataContext = this;

            CH1.Series = SeriesCollection;
            txtC1X.Labels = Labels;



            //Second Chart
            txtChart2.Content = "Subjects with highest Lecture hours";
            txtC2X.Title = "Subject Code";
            txtC2Y.Title = "Number Of Lecture Hours";


            C2Values = new ChartValues<double>();
            Labels2 = new List<string>();
            Labels2.Clear();

            SeriesCollection2.Clear();

            dbContext1.SubjectInformation.OrderByDescending(x => x.LecHours).Take(7)
             .ToList().ForEach(r =>
             {
                 C2Values.Add(r.LecHours);
                 Labels2.Add(r.SubjectCode);
             });

            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Subjects with highest Lecture hours",
                    Values = C2Values
                }
            };

            Formatter2 = value => value.ToString("N");

            Func<ChartPoint, string> labelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            CH2.Series = SeriesCollection2;
            txtC2X.Labels = Labels2;


            //Student Pie Chart1

            P1Series = new SeriesCollection();
            txtP1.Text = "Subjects According to Year";


            dbContext1.SubjectInformation.GroupBy(s => s.OfferedYear).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                P1Series.Add(new PieSeries
                {
                    Title = r.name,
                    Values = new ChartValues<double> { r.sum },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            });

            pieChart1.Series = P1Series;

            //Pie Chart 2

            txtP2.Text = "Subject Amount Compared to Lecturers";

            pieChart2.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Subjects",
                    Values = new ChartValues<double> {SubjectsCount},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Lecturers",
                    Values = new ChartValues<double> {LecturerCount},
                    DataLabels = true,
                    PushOut = 15,
                    LabelPoint = labelPoint
                },
            };

        }

        public void LoadLecturerStats(Object s, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection();
            txtChart1.Content = "Lecturers Amount According to Faculty";
            txtC1X.Title = "Faculty";
            txtC1Y.Title = "Number Of Lecturers";

            C1Values = new ChartValues<double>();
            Labels = new List<string>();
            Labels.Clear();
            dbContext1.LectureInformation.GroupBy(s => s.Faculty).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                C1Values.Add(r.sum);
                Labels.Add(r.name);
            });

            SeriesCollection.Clear();

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Lecturers According to Faculty",
                Values = C1Values

            });

            Formatter = value => value.ToString("N");

            DataContext = this;

            CH1.Series = SeriesCollection;
            txtC1X.Labels = Labels;

            //Second Chart
            txtChart2.Content = "Lecturers According to Department";
            txtC2X.Title = "Department";
            txtC2Y.Title = "Number Of Lecturers";


            C2Values = new ChartValues<double>();
            Labels2 = new List<string>();

            dbContext1.LectureInformation.GroupBy(s => s.Department).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                C2Values.Add(r.sum);
                Labels2.Add(r.name);
            });

            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Lecturers According to Department",
                    Values = C2Values
                }
            };

            Formatter2 = value => value.ToString("N");

            Func<ChartPoint, string> labelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            CH2.Series = SeriesCollection2;
            txtC2X.Labels = Labels2;

            //Student Pie Chart1

            P1Series = new SeriesCollection();
            txtP1.Text = "Lecturers According to Center";

            dbContext1.LectureInformation.GroupBy(s => s.Center).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                P1Series.Add(new PieSeries
                {
                    Title = r.name,
                    Values = new ChartValues<double> { r.sum },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            });

            pieChart1.Series = P1Series;

            //Pie Chart 2
            P2Series = new SeriesCollection();

            txtP2.Text = "Lecturers According to Level";

            dbContext1.LectureInformation.GroupBy(s => s.EmpLevel).Select(o => new
            {
                name = o.Key,
                sum = o.Count()
            }).ToList().ForEach(r =>
            {
                P2Series.Add(new PieSeries
                {
                    Title = "Level "+r.name.ToString(),
                    Values = new ChartValues<double> { r.sum },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            });

            pieChart2.Series = P2Series;
        }


        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }



        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public Func<double, string> Formatter2 { get; set; }

        public ChartValues<double> C1Values { get; set; }

        public ChartValues<double> C2Values { get; set; }

        public List<string> Labels2 { get; set; }


        public SeriesCollection P1Series { get; set; }
        public SeriesCollection P2Series { get; set; }

    }
}

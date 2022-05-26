﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GolfReview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string DATA_FOLDER = "GolfReview.Data";
        string SCORES_DATA_PATH = DATA_FOLDER + ".Scores.json";
        string AUGUSTA_CARD = DATA_FOLDER + ".AugustaNationalCard.json";
        string AUGUSTA_HOLE_FOLER = "GolfReview.Images.AugustaNationalGolfCourse";

        int currentHoleToDisplay = 1;
        new List<System.Windows.Media.Brush> listBrushes = new List<System.Windows.Media.Brush>() { System.Windows.Media.Brushes.LightSteelBlue, System.Windows.Media.Brushes.Black };

        List<Player> playersList;

        Assembly assembly = Assembly.GetExecutingAssembly();

        public MainWindow()
        {
            InitializeComponent();

            LoadPlayers();
            playersList.Insert(0, GetParPlayer());
            dataGridScores.ItemsSource = playersList;

            ChangeHole();

            //List<PieChartItem> piechartList = new List<PieChartItem>()
            //{
            //    new PieChartItem() {Name="Par", Number=3},
            //    new PieChartItem() {Name="Bogey", Number=1},
            //    new PieChartItem() {Name="Birdie", Number=1}
            //};

            ((PieSeries)pieChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int> ("Par", 3),
                new KeyValuePair<string, int> ("Bogey", 1),
                new KeyValuePair<string, int> ("Birdie", 1)
            };

            //((PieSeries)pieChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
            //    new KeyValuePair<string, int>("Project Manager", 12),  
            //    new KeyValuePair<string, int>("CEO", 25),  
            //    new KeyValuePair<string, int>("Software Engg.", 5),  
            //    new KeyValuePair<string, int>("Team Leader", 6),  
            //    new KeyValuePair<string, int>("Project Leader", 10),  
            //    new KeyValuePair<string, int>("Developer", 4)
            //};
            //DrawHoleOnScreen();
            //DrawPlayersShots();

            //    Line myLine = new Line();
            //    myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            //    myLine.X1 = 0;
            //    myLine.X2 = 50;
            //    myLine.Y1 = 0;
            //    myLine.Y2 = 50;
            //    myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //    myLine.VerticalAlignment = VerticalAlignment.Center;
            //    myLine.StrokeThickness = 2;
            //    canvasHoleMap.Children.Add(myLine);
        }

        public void DrawPlayersShots()
        {
            for(int j = 1; j < playersList.Count(); j++)
            {
                for (int i = 0; i < playersList[j].Holes[currentHoleToDisplay].Shots.Count; i++)
                {
                    
                    Line tempLine = new Line();
                    tempLine.Stroke = listBrushes[j-1];
                    tempLine.X1 = ConvertXPoint(playersList[j].Holes[currentHoleToDisplay].Shots[i].X1);
                    tempLine.X2 = ConvertXPoint(playersList[j].Holes[currentHoleToDisplay].Shots[i].X2);
                    tempLine.Y1 = ConvertYPoint(playersList[j].Holes[currentHoleToDisplay].Shots[i].Y1);
                    tempLine.Y2 = ConvertYPoint(playersList[j].Holes[currentHoleToDisplay].Shots[i].Y2);
                    tempLine.HorizontalAlignment = HorizontalAlignment.Left;
                    tempLine.VerticalAlignment = VerticalAlignment.Center;
                    tempLine.StrokeThickness = 2;
                    canvasHoleMap.Children.Add(tempLine);
                }
            }
        }

        public int ConvertXPoint(float x)
        {
            return (int)(canvasHoleMap.Width * x);
        }

        public int ConvertYPoint(float y)
        {
            return (int)(canvasHoleMap.Height * y);
        }

        public void DrawHoleOnScreen()
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Hole" + (currentHoleToDisplay + 1).ToString() + ".jpg", UriKind.RelativeOrAbsolute));
            canvasHoleMap.Background = ib;
        }

        public void LoadPlayers()
        {
            //Read the Data JSON File
            JObject scoresJSON = null;
            using(StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(SCORES_DATA_PATH)))
            {
                scoresJSON = JObject.Parse(reader.ReadToEnd());
            }

            playersList = ConvertJSONToPlayers(scoresJSON);
        }

        private Player GetParPlayer()
        {
            JObject parCardJSON = null;
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(AUGUSTA_CARD)))
            {
                parCardJSON = JObject.Parse(reader.ReadToEnd());
            }

            return new Player(parCardJSON);
        }

        private List<Player> ConvertJSONToPlayers(JObject jobject)
        {
            List<Player> playersList = new List<Player>();
            JArray jArray = (JArray)jobject["Players"];
            foreach (var i in jArray)
            {

                playersList.Add(new Player((JObject)i));
            }

            return playersList;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = Mouse.GetPosition(canvasHoleMap);
            double x = p.X / canvasHoleMap.Width;
            double y = p.Y / canvasHoleMap.Height;

            Console.WriteLine(x + ", " + y);
        }

        private void dataGridScores_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // TODO: Add event handler implementation here.
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            // iteratively traverse the visual tree
            while ((dep != null) &&
                    !(dep is DataGridCell) &&
                    !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            //if (dep == null)
            //    return;

            if (dep is DataGridColumnHeader)
            {
                DataGridColumnHeader columnHeader = dep as DataGridColumnHeader;
                // do something
                currentHoleToDisplay = int.Parse(columnHeader.Column.Header.ToString()) - 1;
                ChangeHole();
            }

            //if (dep is DataGridCell)
            //{
            //    DataGridCell cell = dep as DataGridCell;
            //    // do something
            //}
        }

        public void ChangeHole()
        {
            canvasHoleMap.Children.Clear();
            DrawHoleOnScreen();
            DrawPlayersShots();
        }
    }
}

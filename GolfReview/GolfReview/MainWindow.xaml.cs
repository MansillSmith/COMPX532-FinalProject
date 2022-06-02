using Newtonsoft.Json.Linq;
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

        int currentHoleToDisplay = 0;
        List<System.Windows.Media.Brush> listBrushes = new List<System.Windows.Media.Brush>() { 
            System.Windows.Media.Brushes.LightSteelBlue, 
            System.Windows.Media.Brushes.Black,
            System.Windows.Media.Brushes.LightSalmon,
            System.Windows.Media.Brushes.Brown,
            System.Windows.Media.Brushes.MediumPurple,
            System.Windows.Media.Brushes.Khaki
        };

        List<Player> playersList;

        Assembly assembly = Assembly.GetExecutingAssembly();

        List<int> parScores = new List<int>();

        public MainWindow()
        {
            InitializeComponent();

            LoadPlayers();
            playersList.Insert(0, GetParPlayer());
            dataGridScores.ItemsSource = playersList;

            ChangeHole();
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

            Player p = new Player(parCardJSON);
            for(int i = 0; i < p.Holes.Count(); i++)
            {
                parScores.Add(p.Holes[i].Score);
            }

            return p;
        }

        private List<Player> ConvertJSONToPlayers(JObject jobject)
        {
            List<Player> playersList = new List<Player>();
            JArray jArray = (JArray)jobject["Players"];
            int count = 0;
            foreach (var i in jArray)
            {
                playersList.Add(new Player((JObject)i, listBrushes[count]));
                count++;
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

                //If the selected coloumn is a hole
                try
                {
                    currentHoleToDisplay = int.Parse(columnHeader.Column.Header.ToString()) - 1;
                    ChangeHole();
                }
                catch
                {

                }
            }

            //if (dep is DataGridCell)
            //{
            //    DataGridCell cell = dep as DataGridCell;
            //    // do something
            //}
        }

        public void SetPieChart()
        {
            //((PieSeries)pieChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[]
            //{
            //    new KeyValuePair<string, int> ("Par", 3),
            //    new KeyValuePair<string, int> ("Bogey", 1),
            //    new KeyValuePair<string, int> ("Birdie", 1)
            //};

            int albatross = 0;
            int eagle = 0;
            int birdie = 0;
            int par = 0;
            int bogey = 0;
            int higherBogey = 0;

            //KeyValuePair<string, int>[] array = new KeyValuePair<string, int>[playersList.Count() - 1];
            for (int i = 1; i < playersList.Count(); i++)
            {
                int diff = parScores[currentHoleToDisplay] - playersList[i].Holes[currentHoleToDisplay].Score;

                if(diff == -3)
                {
                    albatross++;
                }
                else if(diff == -2)
                {
                    eagle++;
                }
                else if (diff == -1)
                {
                    birdie++;
                }
                else if (diff == 0)
                {
                    par++;
                }
                else if (diff == 1)
                {
                    bogey++;
                }
                else
                {
                    higherBogey++;
                }
            }

            List<KeyValuePair<string, int>> l = new List<KeyValuePair<string, int>>();
            if(albatross != 0)
            {
                l.Add(new KeyValuePair<string, int>("Albatross", albatross));
            }
            if(eagle != 0)
            {
                l.Add(new KeyValuePair<string, int>("Eagle", eagle));
            }
            if (birdie != 0)
            {
                l.Add(new KeyValuePair<string, int>("Birdie", birdie));
            }
            if (par != 0)
            {
                l.Add(new KeyValuePair<string, int>("Par", par));
            }
            if (bogey != 0)
            {
                l.Add(new KeyValuePair<string, int>("Bogey", bogey));
            }
            if(higherBogey != 0)
            {
                l.Add(new KeyValuePair<string, int>("Double Bogey +", higherBogey));
            }

             ((PieSeries)pieChart.Series[0]).ItemsSource = l.ToArray();
        }

        public void ChangeHole()
        {
            canvasHoleMap.Children.Clear();
            DrawHoleOnScreen();
            DrawPlayersShots();
            SetPieChart();
        }
    }
}

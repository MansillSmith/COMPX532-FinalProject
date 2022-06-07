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

        const int MAX_HOLE_TO_DISPLAY = 6;

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
            AddIconsToButtons();
            ChangeHole();
        }

        public void AddIconsToButtons()
        {
            stackPanelLeftButton.Children.Add(new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Images/arrowLeft.png", UriKind.RelativeOrAbsolute)) });
            stackPanelRightButton.Children.Add(new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Images/arrowRight.png", UriKind.RelativeOrAbsolute)) });
        }

        public void DrawPlayersShots()
        {
            for(int j = 1; j < playersList.Count(); j++)
            {
                if (playersList[j].Selected)
                {
                    for (int i = 0; i < playersList[j].Holes[currentHoleToDisplay].Shots.Count; i++)
                    {

                        Line tempLine = new Line();
                        tempLine.Stroke = listBrushes[j - 1];
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
                    int clickedHole = int.Parse(columnHeader.Column.Header.ToString()) - 1;
                    if (clickedHole != currentHoleToDisplay && clickedHole <= (MAX_HOLE_TO_DISPLAY - 1))
                    {
                        currentHoleToDisplay = clickedHole;
                        ChangeHole();
                    }
                }
                catch
                {

                }
            }
            else
            {
                canvasHoleMap.Children.Clear();
                DrawPlayersShots();
            }


            //if (dep is DataGridCell)
            //{
            //    DataGridCell cell = dep as DataGridCell;
            //    // do something
            //}
        }

        public void SetPieChartPutts()
        {
            int noPutts = 0;
            int onePutt = 0;
            int twoPutt = 0;
            int threePutt = 0;
            int fourPuttPlus = 0;

            for(int i = 1; i < playersList.Count(); i++)
            {
                int numPutts = 0;
                for(int j = 0; j < playersList[i].Holes[currentHoleToDisplay].Shots.Count(); j++)
                {
                    if(playersList[i].Holes[currentHoleToDisplay].Shots[j].Club == "Putter")
                    {
                        numPutts++;
                    }
                }

                if(numPutts == 0)
                {
                    noPutts++;
                }
                else if(numPutts == 1)
                {
                    onePutt++;
                }
                else if (numPutts == 2)
                {
                    twoPutt++;
                }
                else if (numPutts == 3)
                {
                    threePutt++;
                }
                else
                {
                    fourPuttPlus++;
                }
            }

            List<KeyValuePair<string, int>> l = new List<KeyValuePair<string, int>>();
            if(noPutts != 0)
            {
                l.Add(new KeyValuePair<string, int>("0 Putts", noPutts));
            }
            if (onePutt != 0)
            {
                l.Add(new KeyValuePair<string, int>("1 Putt", onePutt));
            }
            if (twoPutt != 0)
            {
                l.Add(new KeyValuePair<string, int>("2 Putt", twoPutt));
            }
            if (threePutt != 0)
            {
                l.Add(new KeyValuePair<string, int>("3 Putt", threePutt));
            }
            if (fourPuttPlus != 0)
            {
                l.Add(new KeyValuePair<string, int>("4 Putt +", fourPuttPlus));
            }

            ((PieSeries)pieChartPutts.Series[0]).ItemsSource = l.ToArray();
        }

        public void SetPieChartScores()
        {

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

             ((PieSeries)pieChartScores.Series[0]).ItemsSource = l.ToArray();
        }

        private void SetPieChartGreenInRegulation()
        {
            int greenInRegulation = 0;
            int holePar = playersList[0].Holes[currentHoleToDisplay].Score;
            for(int i = 1; i < playersList.Count(); i++)
            {
                for(int j = 0; j < playersList[i].Holes[currentHoleToDisplay].Shots.Count(); j++)
                {
                    // If the player is using a putter in or before GIR
                    if(playersList[i].Holes[currentHoleToDisplay].Shots[j].Club == "Putter" && j <= holePar -2 )
                    {
                        greenInRegulation++;
                    }
                }
            }

            List<KeyValuePair<string, int>> l = new List<KeyValuePair<string, int>>() {
                new KeyValuePair<string, int>("GIR", greenInRegulation),
                new KeyValuePair<string, int>("Not GIR", playersList.Count() - 1 - greenInRegulation)
            };

            ((PieSeries)pieChartGreensInRegulation.Series[0]).ItemsSource = l.ToArray();
        }

        public void ChangeHole()
        {
            canvasHoleMap.Children.Clear();
            DrawHoleOnScreen();
            DrawPlayersShots();
            SetPieChartScores();
            SetPieChartPutts();
            SetPieChartGreenInRegulation();
            textBlockHoleText.Text = "Hole " + (currentHoleToDisplay + 1).ToString();
        }

        private void buttonLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            if(currentHoleToDisplay >= 1)
            {
                currentHoleToDisplay--;
                ChangeHole();
            }
        }

        private void buttonRightArrow_Click(object sender, RoutedEventArgs e)
        {
            if(currentHoleToDisplay < (MAX_HOLE_TO_DISPLAY - 1))
            {
                currentHoleToDisplay++;
                ChangeHole();
            }
        }
    }
}

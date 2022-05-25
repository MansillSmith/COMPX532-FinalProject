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

        List<Player> playersList;

        Assembly assembly = Assembly.GetExecutingAssembly();

        public MainWindow()
        {
            InitializeComponent();

            LoadPlayers();
            playersList.Insert(0, GetParPlayer());
            dataGridScores.ItemsSource = playersList;

            DrawHoleOnScreen();
            DrawPlayersShots();

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
            for (int i = 0; i < playersList[1].Holes[0].Shots.Count; i++)
            {
                Line tempLine = new Line();
                tempLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                tempLine.X1 = playersList[1].Holes[0].Shots[i].X1;
                tempLine.X2 = playersList[1].Holes[0].Shots[i].X2;
                tempLine.Y1 = playersList[1].Holes[0].Shots[i].Y1;
                tempLine.Y2 = playersList[1].Holes[0].Shots[i].Y2;
                tempLine.HorizontalAlignment = HorizontalAlignment.Left;
                tempLine.VerticalAlignment = VerticalAlignment.Center;
                tempLine.StrokeThickness = 2;
                canvasHoleMap.Children.Add(tempLine);
            }
        }

        public void DrawHoleOnScreen()
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Hole1.jpg", UriKind.RelativeOrAbsolute));
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
            Console.WriteLine(Mouse.GetPosition(canvasHoleMap));
        }
    }
}

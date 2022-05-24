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

            BitmapImage holeImage = new BitmapImage();
            holeImage.BeginInit();
            holeImage.UriSource = new Uri("/GolfReview;component/Images/AugustaNationalGolfCourse/Hole1.jpg", UriKind.Relative);
            holeImage.EndInit();
            imageHoleMap.Source = holeImage;

            //Bitmap image = new Bitmap(assembly.GetManifestResourceStream(AUGUSTA_HOLE_FOLER));
            //imageHoleMap.Source = (ImageSource)image;
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

    }
}

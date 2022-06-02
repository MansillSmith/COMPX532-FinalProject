using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GolfReview
{
    public class Player
    {
        private string _name;
        /// <summary>
        /// The name of the player
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _nationality;
        /// <summary>
        /// The nationality of the player
        /// </summary>
        public string Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }

        private List<Hole> _holes;
        /// <summary>
        /// The list of holes the player had
        /// </summary>
        public List<Hole> Holes
        {
            get { return _holes; }
            set { _holes = value; }
        }

        private int _hole1;
        public int Hole1
        {
            get { return _hole1; }
        }

        private int _hole2;
        public int Hole2
        {
            get { return _hole2; }
        }

        private int _hole3;
        public int Hole3
        {
            get { return _hole3; }
        }

        private int _hole4;
        public int Hole4
        {
            get { return _hole4; }
        }

        private int _hole5;
        public int Hole5
        {
            get { return _hole5; }
        }

        private int _hole6;
        public int Hole6
        {
            get { return _hole6; }
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
        }

        private System.Windows.Media.Brush _brush;

        public System.Windows.Media.Brush Brush
        {
            get { return _brush; }
        }

        private void Constructor(JObject jObject)
        {
            if (jObject.ContainsKey(nameof(Player.Name)))
            {
                Name = (string)jObject[nameof(Player.Name)];
                Nationality = (string)jObject[nameof(Player.Nationality)];
            }
            else
            {
                Nationality = "Par";
            }

            Holes = new List<Hole>();

            foreach (var i in jObject[nameof(Player.Holes)])
            {
                Holes.Add(new Hole((JObject)i));
            }

            this._hole1 = Holes[0].Score;
            this._hole2 = Holes[1].Score;
            this._hole3 = Holes[2].Score;
            this._hole4 = Holes[3].Score;
            this._hole5 = Holes[4].Score;
            this._hole6 = Holes[5].Score;

            if (this.Name != null)
            {
                string[] names = this.Name.Split(' ');

                this._image = new BitmapImage(new Uri("pack://application:,,,/Images/player" + names[0] + names[1] + ".jpg", UriKind.RelativeOrAbsolute));
            }
        }

        public Player (JObject jObject, System.Windows.Media.Brush brush)
        {
            Constructor(jObject);
            this._brush = brush;
        }


        public Player (JObject jObject)
        {
            Constructor(jObject);
        }
    }
}

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

        private int _hole7;
        public int Hole7
        {
            get { return _hole7; }
        }

        private int _hole8;
        public int Hole8
        {
            get { return _hole8; }
        }

        private int _hole9;
        public int Hole9
        {
            get { return _hole9; }
        }

        private int _hole10;
        public int Hole10
        {
            get { return _hole10; }
        }

        private int _hole11;
        public int Hole11
        {
            get { return _hole11; }
        }

        private int _hole12;
        public int Hole12
        {
            get { return _hole12; }
        }

        private int _hole13;
        public int Hole13
        {
            get { return _hole13; }
        }

        private int _hole14;
        public int Hole14
        {
            get { return _hole14; }
        }

        private int _hole15;
        public int Hole15
        {
            get { return _hole15; }
        }

        private int _hole16;
        public int Hole16
        {
            get { return _hole16; }
        }

        private int _hole17;
        public int Hole17
        {
            get { return _hole17; }
        }

        private int _hole18;
        public int Hole18
        {
            get { return _hole18; }
        }

        private int _total = 0;
        public int Total
        {
            get { return _total; }
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

        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
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
                Hole h = new Hole((JObject)i);
                this._total += h.Score;
                Holes.Add(h);
            }



            this._hole1 = Holes[0].Score;
            this._hole2 = Holes[1].Score;
            this._hole3 = Holes[2].Score;
            this._hole4 = Holes[3].Score;
            this._hole5 = Holes[4].Score;
            this._hole6 = Holes[5].Score;
            this._hole7 = Holes[6].Score;
            this._hole8 = Holes[7].Score;
            this._hole9 = Holes[8].Score;
            this._hole10 = Holes[9].Score;
            this._hole11 = Holes[10].Score;
            this._hole12 = Holes[11].Score;
            this._hole13 = Holes[12].Score;
            this._hole14 = Holes[13].Score;
            this._hole15 = Holes[14].Score;
            this._hole16 = Holes[15].Score;
            this._hole17 = Holes[16].Score;
            this._hole18 = Holes[17].Score;
            this.Selected = true;

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

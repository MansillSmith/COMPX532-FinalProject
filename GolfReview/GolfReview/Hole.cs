using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfReview
{
    public class Hole
    {
        private int _holeNumber;
        /// <summary>
        /// The hole number of the hole
        /// </summary>
        public int HoleNumber
        {
            get { return _holeNumber; }
            set { _holeNumber = value; }
        }

        private int _score;
        /// <summary>
        /// The score the player got on this hole
        /// </summary>
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        private List<Shot> _shots;

        public List<Shot> Shots
        {
            get { return _shots; }
        }

        private bool _gir;
        public bool GIR
        {
            get { return _gir; }
        }

        public Hole (JObject jObject)
        {
            HoleNumber = (int)jObject[nameof(HoleNumber)];
            Score = (int)jObject[nameof(Score)];
            _shots = new List<Shot>();

            if (jObject.ContainsKey(nameof(GIR)))
            {
                _gir = (bool)jObject[nameof(GIR)];
            }


            if (jObject.ContainsKey(nameof(Shots)))
            {
                foreach (var i in jObject[nameof(Shots)])
                {
                    _shots.Add(new Shot((JObject)i));
                }

            }
        }
    }
}

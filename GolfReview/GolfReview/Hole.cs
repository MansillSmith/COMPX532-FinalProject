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

        public Hole (JObject jObject)
        {
            HoleNumber = (int)jObject[nameof(HoleNumber)];
            Score = (int)jObject[nameof(Score)];
        }
    }
}

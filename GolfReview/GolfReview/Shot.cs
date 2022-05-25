using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfReview
{
    public class Shot
    {
        private string _club;

        public string Club
        {
            get { return _club; }
        }

        private float _distance;

        public float Distance
        {
            get { return _distance; }
        }

        private int _x1;

        public int X1
        {
            get { return _x1; }
        }

        private int _x2;

        public int X2
        {
            get { return _x2; }
        }

        private int _y1;

        public int Y1
        {
            get { return _y1; }
        }

        private int _y2;

        public int Y2
        {
            get { return _y2; }
        }

        public Shot(JObject jObject)
        {
            _club = (string)jObject[nameof(Club)];
            _distance = (int)jObject[nameof(Distance)];
            _x1 = (int)jObject[nameof(X1)];
            _x2 = (int)jObject[nameof(X2)];
            _y1 = (int)jObject[nameof(Y1)];
            _y2 = (int)jObject[nameof(Y2)];
        }
    }
}

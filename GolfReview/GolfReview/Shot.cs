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

        private float _x1;

        public float X1
        {
            get { return _x1; }
        }

        private float _x2;

        public float X2
        {
            get { return _x2; }
        }

        private float _y1;

        public float Y1
        {
            get { return _y1; }
        }

        private float _y2;

        public float Y2
        {
            get { return _y2; }
        }

        public Shot(JObject jObject)
        {
            _club = (string)jObject[nameof(Club)];
            _distance = (int)jObject[nameof(Distance)];
            _x1 = (float)jObject[nameof(X1)];
            _x2 = (float)jObject[nameof(X2)];
            _y1 = (float)jObject[nameof(Y1)];
            _y2 = (float)jObject[nameof(Y2)];
        }
    }
}

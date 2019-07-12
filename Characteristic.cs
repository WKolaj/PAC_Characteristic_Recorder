using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_Characterstic_Recorder
{
    public class Characterstic
    {
        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            private set
            {
                this._startTime = value;
            }
        }

        public Characterstic()
        {
        }

        public void SetStartTime(DateTime time)
        {
            this.StartTime = time;
        }

        public void AddPoint(DateTime date, Double value)
        {
            var ms = (date - this.StartTime).TotalMilliseconds / 1000;
            this.Points.Add(new CharacteristicPoint(ms, value));
        }

        private List<CharacteristicPoint> _points = new List<CharacteristicPoint>();
        public List<CharacteristicPoint> Points
        {
            get
            {
                return _points;
            }
        }

        public void Clear()
        {
            Points.Clear();
        }

    }
}

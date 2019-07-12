using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_Characterstic_Recorder
{
    public class CharacteristicPoint
    {
        private Double _x;
        public Double X
        {
            get
            {
                return _x;
            }

            private set
            {
                _x = value;
            }
        }

        private Double _y;
        public Double Y
        {
            get
            {
                return _y;
            }

            private set
            {
                _y = value;
            }
        }

        public CharacteristicPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

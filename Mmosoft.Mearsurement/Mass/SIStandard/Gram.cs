using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mmosoft.Measurement.Mass.SIStandard
{
    public class Gram : BaseMeasurement, ISIStandardMass
    {
        public Gram()
        {

        }

        public Gram(decimal value) 
        {
            this.Value = value;
        }
    }
}

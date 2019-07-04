using Mmosoft.Measurement.Mass.USStandard;
using System;
using System.Linq;
namespace Mmosoft.Measurement.Mass.SIStandard
{
    public class Kilogram : BaseMeasurement, ISIStandardMass 
    {
        public static implicit operator Kilogram(int val)
        {
            return new Kilogram() {Value = val};
        }

        public static implicit operator Kilogram(Gram val)
        {
            return MassConvertor.Convert<Gram, Kilogram>(val);
        }

        public static implicit operator Pound(Kilogram kg)
        {
            return MassConvertor.Convert<Kilogram, Pound>(kg);
        }


    }
}

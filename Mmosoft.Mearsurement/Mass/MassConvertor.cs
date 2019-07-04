using Mmosoft.Measurement.Mass.SIStandard;
using Mmosoft.Measurement.Mass.USStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mmosoft.Measurement.Mass
{
    public static class MassConvertor
    {
        // 
        private static Dictionary<Type, decimal> _PerGram = new Dictionary<Type, decimal>
        {
            { typeof(Yoctogram), (decimal)Math.Pow(10, -24) },
            { typeof(Zeptogram), (decimal)Math.Pow(10, -21) },
            { typeof(Attogram ), (decimal)Math.Pow(10, -18) },
            { typeof(Femtogram), (decimal)Math.Pow(10, -15) },
            { typeof(Picogram ), (decimal)Math.Pow(10, -12) },
            { typeof(Nanogram ), (decimal)Math.Pow(10, -9) },
            { typeof(Microgram), (decimal)Math.Pow(10, -6) },
            { typeof(Milligram), (decimal)Math.Pow(10, -3) },
            { typeof(Centigram), (decimal)Math.Pow(10, -2) },
            { typeof(Decigram ), (decimal)Math.Pow(10, -1) },
            { typeof(Gram),      (decimal)Math.Pow(10, 0) },
            { typeof(Decagram),  (decimal)Math.Pow(10, 1) },
            { typeof(Hectogram), (decimal)Math.Pow(10, 2) },
            { typeof(Kilogram),  (decimal)Math.Pow(10, 3) },
            { typeof(Megagram),  (decimal)Math.Pow(10, 6) },
            { typeof(Tonne),     (decimal)Math.Pow(10, 6) },
            { typeof(Gigagram),  (decimal)Math.Pow(10, 9) },
            { typeof(Teragram),  (decimal)Math.Pow(10, 12) },
            { typeof(Petagram),  (decimal)Math.Pow(10, 15) },
            { typeof(Exagram),   (decimal)Math.Pow(10, 18) },
            { typeof(Zettagram), (decimal)Math.Pow(10, 21) },
            { typeof(Yottagram), (decimal)Math.Pow(10, 24) },
            
            // 1 oz = 28.349523125 
            { typeof(Ounce), 28.349523125m }
        };

        private static Dictionary<Type, decimal> _PerOunce = new Dictionary<Type, decimal>
        {
            { typeof(Grain), 1m/7000 * 16  }, // 1 gr = 1/7000 lb = 1/7000 * 16oz
            { typeof(Dram),  1m/16 }, // 1 dr = 1 / 16 oz
            { typeof(Ounce), 1 }, // 1 oz = 1 oz
            { typeof(Pound), 16 }, // 1 lb = 16 oz
            { typeof(USHundredWeight), 100 * 16 }, // 1 UShundred (cwt) = 100 Pound (lb) = 100 * 16 oz
            { typeof(LongHundredWeight), 112 * 16 }, // 1 long cwt = 112 lb = 112 * 16 oz
            { typeof(Ton), 2000 * 16 }, // = 2000 lb = 2000 * 16 oz
            { typeof(LongTon), 2240 * 16 }, // 20 long cwt = 2240 lb = 2240 * 16 oz
            //
            { typeof(Gram), 1m/28.349523125m }
        };

        public static TMassTo Convert<TMassFrom, TMassTo>(TMassFrom from)
            where TMassFrom : IMeasurement, IMass, new()
            where TMassTo   : IMeasurement, IMass, new()
        {
            Type fromType = typeof(TMassFrom);
            Type[] interfaceTypeOfType1 = fromType.GetInterfaces();

            Type toType = typeof(TMassTo);
            Type[] interfaceTypeOfType2 = toType.GetInterfaces();

            //
            Type massSI = typeof(ISIStandardMass);
            Type massUS = typeof(IUSMassStandard);

            if (interfaceTypeOfType1.Contains(massSI))
            {
                if (interfaceTypeOfType2.Contains(massSI))
                {
                    return new TMassTo() { Value = From_SI_To_SI(from.Value, fromType, toType) };
                }
                else if (interfaceTypeOfType2.Contains(massUS))
                {
                    return new TMassTo() { Value = From_SI_To_US(from.Value, fromType, toType) };
                }
            }
            else if (interfaceTypeOfType1.Contains(massUS))
            {
                if (interfaceTypeOfType2.Contains(massSI))
                {
                    return new TMassTo() { Value = From_US_To_SI(from.Value, fromType, toType) };
                }
                else if (interfaceTypeOfType2.Contains(massUS))
                {
                    return new TMassTo() { Value = From_US_To_US(from.Value, fromType, toType) };
                }
            }

            throw new Exception();
        }

        private static decimal From_SI_To_SI(decimal fromSIValue, Type fromSI, Type toSI)
        {
            // fromSI to how many gram
            decimal grams = fromSIValue * _PerGram[fromSI];
            // how many gram -> gram ratio => to SI
            return grams / _PerGram[toSI];
        }

        private static decimal From_SI_To_US(decimal SIValue, Type fromSI, Type toUS)
        {
            // from SI to gram
            decimal grams = SIValue * _PerGram[fromSI];
            // gram per ounce
            decimal gramPerOunce = _PerOunce[typeof(Gram)];
            // from SI to ounce
            decimal ounce = grams / gramPerOunce;
            // from SI to US
            return ounce / _PerOunce[toUS];
        }

        private static decimal From_US_To_SI(decimal USValue, Type fromUS, Type toSI)
        {
            decimal ounces = USValue * _PerOunce[fromUS];
            decimal ouncesPerGram = _PerGram[typeof(Ounce)];
            decimal grams = ounces * ouncesPerGram;
            decimal toPerGram = _PerGram[toSI];
            return grams / toPerGram;
        }

        private static decimal From_US_To_US(decimal fromUSValue, Type fromUS, Type toUS)
        {
            decimal ounces = fromUSValue * _PerOunce[fromUS];
            return ounces / _PerOunce[toUS];
        }
    }
}

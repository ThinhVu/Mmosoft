using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mmosoft.Measurement.Mass;
using Mmosoft.Measurement.Mass.SIStandard;
using Mmosoft.Measurement.Mass.USStandard;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // all possible but require a lot of code for converting
            Gram g = 5000.g();                              // 5kg
            Kilogram kg1 = 5000.g();                        // 5kg
            Kilogram kg2 = new Kilogram() { Value = 5 };    // 5kg
            Kilogram kg3 = new Gram(5000);                  // 5kg
            Kilogram kg4 = 5;                               // 5kg
            var kg5 = 5.kg();                               // 5kg
        }
    }
}

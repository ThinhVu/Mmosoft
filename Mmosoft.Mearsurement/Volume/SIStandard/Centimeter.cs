
namespace Mmosoft.Measurement.Volume.SIStandard
{
    // cm^3
    public class Centimeter : BaseMeasurement, ISIVolume {}
    // Commonly name of Centimeter: 1cm^3 = 1mL
    public class Milliliter : Centimeter, ISIVolume { }
}

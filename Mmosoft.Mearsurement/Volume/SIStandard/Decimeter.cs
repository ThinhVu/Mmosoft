
namespace Mmosoft.Measurement.Volume.SIStandard
{
    // dm^3 = 1000 cm^3
    public class Decimeter : BaseMeasurement, ISIVolume
    {
       
    }

    // 1dm^3 = 1L
    public class Liter : Decimeter, ISIVolume
    {
       
    }
}

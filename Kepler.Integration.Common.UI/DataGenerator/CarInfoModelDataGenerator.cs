namespace Kepler.Integration.Common.UI.DataGenerator
{
    public class CarInfoModelDataGenerator
    {
        public static CarInfoModel CreateCarInfoModel()
        {
            return new CarInfoModel
            {
                BrandName = "bmw",
                Year = "2016",
                CarModel = "3er",
                BodyType = "Седан",
                EngineCapacity = "3.0 л.",
                TransmissionType = "Автоматическая",
                CarPower = "306 л.с."
            };
        }
    }
}
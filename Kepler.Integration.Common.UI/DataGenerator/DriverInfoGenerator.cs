namespace Kepler.Integration.Common.UI.DataGenerator
{
    public static class DriverInfoGenerator
    {
        public static DriverInfoModel CreateDriverInfoModel()
        {
            return new DriverInfoModel
            {
                DriverAge = "40",
                DrivingExperienceByYears = "10",
                DriverSex = "Мужской",
                IsMarried = true,
                NumberOfChildren = "1",
                DriverName = "Test",
                DriverPhoneNumber = "0000000000"
            };
        }
    }
}
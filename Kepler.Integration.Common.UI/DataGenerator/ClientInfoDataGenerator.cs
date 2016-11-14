using NLog;
using Ploeh.AutoFixture;

namespace Kepler.Integration.Common.UI.DataGenerator
{
    public class ClientInfoDataGenerator
    {
        private static Fixture _fixture = new Fixture();
        private static Logger LOG = LogManager.GetLogger(typeof (ClientInfoDataGenerator).Name);

        public static ClientInfoModel CreateClientInfoModel(string email, bool generateName = false)
        {
            var name = "тест";
            var phone = "999 999-99-99";

            if (generateName)
                name = _fixture.Create("Имя_");
            var password = _fixture.Create("").Substring(0, 10);

            var clientModel = new ClientInfoModel()
            {
                Email = email,
                Name = name,
                Phone = phone,
                Password = password
            };

            LOG.Info("Generated user data: " + clientModel);

            return clientModel;
        }
    }
}
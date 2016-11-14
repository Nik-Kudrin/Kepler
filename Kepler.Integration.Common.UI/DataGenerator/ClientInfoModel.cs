using Ploeh.AutoFixture;

namespace Kepler.Integration.Common.UI.DataGenerator
{
    public class ClientInfoModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public void RegeneratePassword()
        {
            var fixture = new Fixture();
            Password = fixture.Create("").Substring(0, 10);
        }

        public override string ToString()
        {
            return string.Format("User info: Email={0}; Name={1}; Phone={2}; Password={3}", Email, Name, Phone, Password);
        }
    }
}
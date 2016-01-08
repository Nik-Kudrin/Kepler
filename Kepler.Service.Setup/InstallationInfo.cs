namespace Kepler.Service.Setup
{
    public class InstallationInfo
    {
        public string InstallationDir { get; set; }
        public string SQLInstanceName { get; set; }
        public string DBName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long Port { get; set; }
    }
}
namespace WebAppMVC.Utils
{
    //ConfigReader wird erstellt
    public class ConfigReader : IConfigReader
    {
        private readonly IConfiguration _configuration;

        public ConfigReader(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string ReadConnectionString()
        {
            return _configuration.GetConnectionString("MariaDB");
        }
    }

    public interface IConfigReader
    {
        string ReadConnectionString();

    }
}

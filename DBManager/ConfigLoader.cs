using System.IO;
using System.Xml.Serialization;

namespace DBManager
{
    public class ConfigLoader
    {
        public static Configurations LoadConfigs()
        {
            if (!File.Exists(DbClient.SqlConfigFilePath))
                return CreateNewSqlConfigFile();

            return DeserializeConfigFile();
        }

        private static Configurations DeserializeConfigFile()
        {
            var ser = new XmlSerializer(typeof(Configurations));
            var sr = new StreamReader(DbClient.SqlConfigFilePath);
            return ser.Deserialize(sr) as Configurations;
        }

        private static Configurations CreateNewSqlConfigFile()
        {
            var configObj = new Configurations()
            {
                SQLAddress = "DESKTOP-G7L0IKL",
                SQLDataBase = "cakeFactory",
                SQLUser = "sa",
                SQLPass = "tre543tre",
                CSV_SEPARATOR = ";", 
                EnableDebugInfo = false
            };

            var ser = new XmlSerializer(typeof(Configurations));
            var sr = new StreamWriter(DbClient.SqlConfigFilePath);
            ser.Serialize(sr, configObj);

            return configObj;
        }

    }
}

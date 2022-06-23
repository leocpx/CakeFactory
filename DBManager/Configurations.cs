using eLongMuSQL;

namespace DBManager
{
    public class Configurations : IConfigurations
    {
        public bool EnableDebugInfo{ get; set; }
        public string SQLAddress{ get; set; }
        public string SQLDataBase{ get; set; }
        public string SQLPass{ get; set; }
        public string SQLUser{ get; set; }
        public string CSV_SEPARATOR{ get; set; }
    }

}

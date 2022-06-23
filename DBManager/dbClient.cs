using DBManager.Tables;
using eLongMuSQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public static class DbClient
    {
        #region -- PROPERTIES --

        #region ---- PRIVATE --
        public static string _sqlConfigFilePath { get; set; } = "SqlConfig.ini";
        public static string DefaultAdminPassword { get; set; } = "cakeFactory";
        public static string DefaultUser { get; set; } = "admin";
        public static string SqlConfigFilePath => Path.Combine(Directory.GetCurrentDirectory(), DbClient._sqlConfigFilePath);
        private static MuSQL _muSql { get; set; } = new MuSQL(ConfigLoader.LoadConfigs());
        #endregion

        #endregion

        #region -- FUNCTIONS --

        #region ---- PUBLIC --
        public static Users CheckUser(string user, string pass)
        {
            CheckTable<Users>();

            var usersTable = _muSql.GetTable<Users>();
            if (usersTable.Count == 0)
            {
                CreateDefaultUser();
                usersTable = _muSql.GetTable<Users>();
            }
            

            return usersTable.FirstOrDefault(u=>u._user == user && u._pass == GetHASH256(pass));
        }

        private static void CreateDefaultUser()
        {

            var newUser = new Users()
            {
                id = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                _user = DefaultUser,
                _pass = GetHASH256(DefaultAdminPassword),
                _level = 1
            };

            _muSql.InsertEntry(newUser);
        }
        #endregion

        #region ---- PRIVATE --

        private static void CheckTable<T>() where T : new()
        {
            if (!_muSql.TableExists<T>())
            {
                _muSql.CreateTable<T>();
            }
        }

        #region ---- helpers --
        public static string GetHASH256(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            byte[] resultedData;
            SHA256 shaM = new SHA256Managed();
            resultedData = shaM.ComputeHash(data);

            var result = BitConverter.ToString(resultedData);

            return result;
        }
        #endregion
        #endregion
        #endregion
    }
}

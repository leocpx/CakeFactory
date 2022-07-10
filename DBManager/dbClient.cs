using Core.Interfaces;
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
        #region -- PUBLIC --
        #endregion

        #region ---- PRIVATE --
        private static MuSQL _muSql { get; set; } = new MuSQL(ConfigLoader.LoadConfigs()); 
        public static string _sqlConfigFilePath { get; set; } = "SqlConfig.ini";
        public static string DefaultAdminPassword { get; set; } = "cakeFactory";
        public static string DefaultUser { get; set; } = "admin";
        public static string SqlConfigFilePath => Path.Combine(Directory.GetCurrentDirectory(), "SqlConfig.ini");
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


            return usersTable.FirstOrDefault(u => u._user == user && u._pass == GetHASH256(pass));
        }


        public static List<Users> GetUserList()
        {
            return _muSql.GetTable<Users>();
        }

        public static List<RawGoodsInfo> GetRawGoodsInfoList()
        {
            if (!_muSql.TableExists<RawGoodsInfo>())
                _muSql.CreateTable<RawGoodsInfo>();

            return _muSql.GetTable<RawGoodsInfo>();
        }
        #endregion

        #region ---- PRIVATE --
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
        public static void RegisterNewUser(Users user)
        {
            _muSql.InsertEntry(user);
        }

        public static void RegisterNewRawGoodInfo(RawGoodsInfo rawGood)
        {
            if (!_muSql.TableExists<RawGoodsInfo>())
                _muSql.CreateTable<RawGoodsInfo>();

            _muSql.InsertEntry(rawGood);
        }

        public static FinishedGoodsInfo GetFinishedGoodInfo(long finishedGoodId)
        {
            return _muSql.GetTableWithCondition<FinishedGoodsInfo>($"where id='{finishedGoodId}'").FirstOrDefault();

        }

        public static void CompleteProductionOrder(IOrder order)
        {
            order._completed = true;
            var productionOrder = order as ProductionOrders;
            _muSql.UpdateEntry(productionOrder);
        }

        public static List<FinishedGoodsDetails> GetFinishedGoodDetails(long finishedGoodId)
        {
            return _muSql.GetTableWithCondition<FinishedGoodsDetails>($"where _finishedGoodId='{finishedGoodId}'");
        }

        public static void RegisterNewFinishedGoodInfo(FinishedGoodsInfo finishedGood)
        {
            if (!_muSql.TableExists<FinishedGoodsInfo>())
                _muSql.CreateTable<FinishedGoodsInfo>();

            _muSql.InsertEntry(finishedGood);
        }

        public static Users GetUser(long workerId)
        {
            return _muSql.GetTableWithCondition<Users>($"where id='{workerId}'").FirstOrDefault();
        }

        public static void RegisterFinishedGoodsDetails(List<FinishedGoodsDetails> finishedGoodDetails)
        {
            if (!_muSql.TableExists<FinishedGoodsDetails>())
                _muSql.CreateTable<FinishedGoodsDetails>();

            _muSql.InsertEntries(finishedGoodDetails);
        }

        //public static void RegisterNewProductionOrder(ProductionOrders newOrder)
        public static void RegisterNewProductionOrder(IOrder order)
        {
            if (!_muSql.TableExists<ProductionOrders>())
                _muSql.CreateTable<ProductionOrders>();

            var newOrder = order as ProductionOrders;
            _muSql.InsertEntry(newOrder);
        }

        public static List<PackagingOrders> GetPackagingOrders()
        {
            return _muSql.GetTable<PackagingOrders>();
        }
        public static PackagingOrders GetPackagingOrder(long _workerId, long startTime)
        {
            var todayID = DateTime.Now.ToString("yyyyMMdd000000000");

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            var tomorrowID = new DateTime(year, month, day, 23, 59, 59).ToString("yyyyMMddHHmmssfff");


            var condition = $"where id>='{todayID}' and id<='{tomorrowID}' and _workerId ='{_workerId}' and _startTime = '{startTime}' ;";
            var result = _muSql.GetTableWithCondition<PackagingOrders>(condition).FirstOrDefault();

            return result;
        }

        public static void RegisterNewPackagingOrder(IOrder order)
        {
            if (!_muSql.TableExists<PackagingOrders>())
                _muSql.CreateTable<PackagingOrders>();

            var newOrder = order as PackagingOrders;
            _muSql.InsertEntry(newOrder);
        }

        public static RawGoodsInfo GetRawGoodsInfo(long _rawGoodInfoId)
        {
            return _muSql.GetTableWithCondition<RawGoodsInfo>($"where id='{_rawGoodInfoId}'").FirstOrDefault();
        }

        public static List<ProductionOrders> GetCompletedProductOrders()
        {
            return _muSql.GetTable<ProductionOrders>().Where(_order => _order._completed).ToList();
        }



        public static FinishedGoodsInfo GetFinishedGoodOrder(long workerId, long startTime)
        {

            var todayID = DateTime.Now.ToString("yyyyMMdd000000000");

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            var tomorrowID = new DateTime(year, month, day, 23, 59, 59).ToString("yyyyMMddHHmmssfff");


            var condition = $"where id>='{todayID}' and id<='{tomorrowID}' and _workerId ='{workerId}' and _startTime = '{startTime}' ;";
            var result = _muSql.GetTableWithCondition<ProductionOrders>(condition).FirstOrDefault();

            if (result == null) return null;

            var fgi = _muSql.GetTableWithCondition<FinishedGoodsInfo>($"where id='{result._finishedGoodId}'").FirstOrDefault();
            return fgi;
        }

        public static IOrder GetOrder(FinishedGoodsInfo fgi, long workerId, string startTime)
        {
            var productionOrder = _muSql.GetTableWithCondition<ProductionOrders>($"where _finishedGoodId='{fgi.id}' and _workerId='{workerId}' and _startTime='{startTime}'").FirstOrDefault();
            var packagingOrder = _muSql.GetTableWithCondition<PackagingOrders>($"where _finishedGoodId='{fgi.id}' and _workerId='{workerId}' and _startTime='{startTime}'").FirstOrDefault();

            if (productionOrder != null) return productionOrder;

            return packagingOrder;
        }

        public static ProductionOrders GetProductionOrder(long workerId, long startTime)
        {
            if (!_muSql.TableExists<ProductionOrders>())
                _muSql.CreateTable<FinishedGoodsDetails>();

            var todayID = DateTime.Now.ToString("yyyyMMdd000000000");

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            var tomorrowID = new DateTime(year, month, day, 23, 59, 59).ToString("yyyyMMddHHmmssfff");


            var condition = $"where id>='{todayID}' and id<='{tomorrowID}' and _workerId ='{workerId}' and _startTime = '{startTime}' ;";
            var result = _muSql.GetTableWithCondition<ProductionOrders>(condition).FirstOrDefault();

            return result;
        }

        public static void DeleteOrder(IOrder order)
        {
            var todayID = DateTime.Now.ToString("yyyyMMdd000000000");
            
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            var tomorrowID = new DateTime(year, month, day, 23, 59, 59).ToString("yyyyMMddHHmmssfff");

            var orderEntry1 = _muSql.GetTableWithCondition<ProductionOrders>($"where id>='{todayID}' and id<='{tomorrowID}' and _workerId='{order._workerId}' and _startTime='{order._startTime}'").FirstOrDefault();
            var orderEntry2 = _muSql.GetTableWithCondition<PackagingOrders>($"where id>='{todayID}' and id<='{tomorrowID}' and _workerId='{order._workerId}' and _startTime='{order._startTime}'").FirstOrDefault();
            
            _muSql.DeleteEntry(orderEntry1);
            _muSql.DeleteEntry(orderEntry2);
        }

        public static List<FinishedGoodsInfo> GetFinishedGoodInfoList()
        {
            if (!_muSql.TableExists<FinishedGoodsDetails>())
                _muSql.CreateTable<FinishedGoodsDetails>();

            return _muSql.GetTable<FinishedGoodsInfo>();
        }
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

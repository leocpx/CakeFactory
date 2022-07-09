using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eLongMuSQL
{
    public class MuSQL
	{
		#region -- PROPERTIES --
		#region -- PRIVATE --
		private int commandTimeOut = 3;
		private SqlConnection MyConnection { get; set; }
		private static object SQLLocker = new object();
		private IConfigurations Configuration;
		#endregion
		#endregion

		#region -- CONSTRUCTOR --
		public MuSQL(IConfigurations Configuration)
		{

			this.Configuration = Configuration;
			CreateNewConnection();

		}
		#endregion

		#region -- FUNCTIONS --
		#region -- PUBLIC --
		#region -- CORE --
		public void TruncateTable(string TableName)
		{
			var sqlcmd = $"TRUNCATE TABLE {TableName};";
			using (var myCommand = new SqlCommand(sqlcmd, MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public void TruncateTable<T>() where T:new()
        {
			TruncateTable(GetTableName<T>());
        }
		public bool TableExists(string TableName)
		{
			bool result = false;

			using (var myCommand = new SqlCommand($"select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='{TableName}'", MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				result = CheckTable(myCommand);
			}

			return result;
		}
		public bool TableExists<T>() where T:new()
        {
			return TableExists(GetTableName<T>());
        }
		public void DeleteEntry<T>(string TableName, T entry) where T : new()
		{
			if (!HasPrimaryKey(entry)) return;

			var sqlcmd = GetDeleteSQLquery(TableName, entry);
			using (var myCommand = new SqlCommand(sqlcmd, MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public void DeleteEntry<T>(string TableName, string OtherCondition, T entry) where T : new()
		{
			if (!HasPrimaryKey(entry)) return;

			var sqlcmd = GetDeleteSQLquery(TableName, OtherCondition, entry);
			using (var myCommand = new SqlCommand(sqlcmd, MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public void DeleteEntry<T>(T entry) where T:new()
        {
			DeleteEntry<T>(GetTableName<T>(), entry);
        }
		public void Connect()
		{
		}
		public void UpdateEntry<T>(string TableName, T entry) where T : new()
		{
			if (!HasPrimaryKey(entry)) return;

			var sqlcmd = GetUpdateSQLquery(TableName, entry);
			using (var myCommand = new SqlCommand(sqlcmd, MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public void UpdateEntry<T>(T entry) where T:new()
        {
			UpdateEntry<T>(GetTableName<T>(), entry);
        }
		public void UpdateEntries<T>(string TableName, List<T> entries) where T : new()
		{
			foreach (var entry in entries)
				UpdateEntry<T>(TableName, entry);
		}
		public void UpdateEntries<T>(List<T> entries) where T:new()
        {
			UpdateEntries<T>(GetTableName<T>(), entries);
        }
		public string InsertEntry<T>(string TableName, T entry) where T : new()
		{
			return InsertEntries<T>(TableName, new List<T> { entry });
		}
		public string InsertEntry<T>(T entry) where T:new()
        {
			return InsertEntry<T>(GetTableName<T>(), entry);
        }
		public string InsertEntries<T>(string TableName, List<T> entries) where T : new()
		{
			var cols = GetColumnsFrom<T>();
			var query = "";
			foreach (var entry in entries)
			{
				var sqlCreateQuery = GetInsertSQLQuery(TableName, cols, entry);
				query = sqlCreateQuery;
				using (var myCommand = new SqlCommand(sqlCreateQuery, MyConnection))
				{
					myCommand.CommandTimeout = commandTimeOut;
					ExecuteCommandNonQuery(myCommand);
				}
			}
			return query;
		}
		public string InsertEntries<T>(List<T> entries) where T:new()
        {
			return InsertEntries<T>(GetTableName<T>(), entries);
        }
		public void CreateTable<T>(string TableName) where T : new()
		{
			if (!IsTableMarked(typeof(T))) { FailedTableCreationHappened(); return; }

			var columnsAtr = GetColumnsFrom<T>();
			var sqlCreateQuery = GetCreateTableSQLQuery(TableName, columnsAtr);
			using (var myCommand = new SqlCommand(sqlCreateQuery, MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public void CreateTable<T>() where T:new()
        {
			CreateTable<T>(GetTableName<T>());
        }
		public List<T> GetTable<T>(string TableName) where T : new()
		{
			List<T> result = new List<T>();

			if (!IsTableMarked(typeof(T))) return new List<T>();

			using (SqlCommand myCommand = new SqlCommand($"select * from {TableName}", MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				result = ExecuteCommandReader<T>(myCommand);
			}

			CheckForTableLinks(result);

			return result;
		}




        public List<T> GetTable<T>() where T: new()
        {
			//var TableName = (typeof(T).GetCustomAttributes().Where(a => a.GetType() == typeof(SQLTableAttribute)).Select(a => (SQLTableAttribute)a).First().TableName);
			//List<T> result = GetTable<T>(TableName);
			//return result;

			return GetTable<T>(GetTableName<T>());
		}
		public List<T> GetTableWithCondition<T>(string TableName, string WithCondition) where T : new()
		{
			List<T> result = new List<T>();
			if (!IsTableMarked(typeof(T))) return result;

			using (var myCommand = new SqlCommand($"select * from {TableName} {WithCondition};", MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				result = ExecuteCommandReader<T>(myCommand);
			}
			CheckForTableLinks(result);
			return result;
		}
		public List<T> GetTableWithCondition<T>(string condition) where T:new()	
        {
			var TableName = (typeof(T).GetCustomAttributes().Where(a => a.GetType() == typeof(SQLTableAttribute)).Select(a => (SQLTableAttribute)a).First().TableName);
			List<T> result = GetTableWithCondition<T>(TableName, condition);
			CheckForTableLinks(result);
			return result;
		}
		public string ExecStorageProcedure(string procedureName, string parameters)
		{
			var query = $"EXEC { procedureName}{ parameters}";
			using (var myCommand = new SqlCommand($"EXEC {procedureName} {parameters}", MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}

			return query;
		}
		public object ExecuteGenericSQLCmd(string sqlCmd)
		{
			var myCommand = new SqlCommand(sqlCmd, MyConnection);
			myCommand.CommandTimeout = commandTimeOut;
			var result = new List<object>();

			lock (SQLLocker)
			{
				if (MyConnection.State != System.Data.ConnectionState.Open)
					try
					{
						//MyConnection.Open();
						CreateNewConnection();
					}
					catch (Exception ex)
					{
						LogMessage($"opening sql connection - connection state:{MyConnection.State.ToString()}");
					}

				using (var SqlReader = myCommand.ExecuteReader())
				{
					try
					{
						while (SqlReader.Read())
						{
							var row = new List<object>();
							for (int i = 0; i < SqlReader.FieldCount; i++)
								row.Add(SqlReader[i]);
							result.Add(row);
						}
						SqlReader.Close();
					}
					catch (Exception ex)
					{
						LogMessage($"failed to parse query result information {ex.ToString()}");

					}
				}

				return result;

				#region -- OLD --
				SqlDataReader myReader = null;
				try
				{
					myReader = myCommand.ExecuteReader();
				}
				catch (Exception ex)
				{
					try
					{
						LogMessage($"failed to execute command reader:{myCommand.CommandText} - connection state:{MyConnection.State.ToString()} - exception:{ex.ToString()}");
						//MyConnection.Close();
						//Thread.Sleep(200);
						//MyConnection.Open();
						CreateNewConnection();
						myReader = myCommand.ExecuteReader();
					}
					catch (Exception ex1)
					{
						LogMessage($"failed to open reconnection - connection state:{MyConnection.State.ToString()} - exception:{ex1.ToString()}");
						Console.WriteLine("failed to execute sql command, probably not connected to db");
					}
				}
				try
				{
					while (myReader.Read())
					{
						var row = new List<object>();
						for (int i = 0; i < myReader.FieldCount; i++)
							row.Add(myReader[i]);
						result.Add(row);
					}
					myReader.Close();
				}
				catch (Exception ex)
				{
					LogMessage($"failed to parse query result information {ex.ToString()}");

				}
				#endregion
			}

			return result;
		}
		public void BulkMoveEntries<T>(string sourceTable, string destinationTable, string condition) where T : new()
		{
			var cols = GetColumnsFrom<T>();
			var sqlBulkMoveQuery = GetBulkMoveQuery<T>(cols, sourceTable, destinationTable, condition);
			using (var myCommand = new SqlCommand(sqlBulkMoveQuery, MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public void DeleteFromTable(string table, string condition)
		{
			using (var myCommand = new SqlCommand($"DELETE FROM {table} {condition}", MyConnection))
			{
				myCommand.CommandTimeout = commandTimeOut;
				ExecuteCommandNonQuery(myCommand);
			}
		}
		public string GetTableName<T>() where T:new()
        {
			var TableName = (typeof(T).GetCustomAttributes().Where(a => a.GetType() == typeof(SQLTableAttribute)).Select(a => (SQLTableAttribute)a).First().TableName);
			return TableName;
		}
		#endregion
		#endregion
		#region -- PRIVATE --
		#region -- TIER 1 --
		#region -- TIER 2 --
		private object GetValueOfProperty(SQLColumnAttribute col, string colValue)
		{
			switch (col.ColumnType)
			{
				case SQLColumnType.BigInt:
					try
					{
						return Int64.Parse(colValue);
					}
					catch (Exception)
					{
					}
					return null;

				case SQLColumnType.Int:
					try
					{
						return int.Parse(colValue);
					}
					catch (Exception)
					{
					}
					return null;

				case SQLColumnType.Float:
					try
					{
						return float.Parse(colValue);
					}
					catch (Exception)
					{
					}
					return null;

				case SQLColumnType.Varchar:
					try
					{
						return colValue;
					}
					catch (Exception)
					{
					}
					return null;

				case SQLColumnType.Xml:
					try
					{
						return colValue;
					}
					catch (Exception)
					{
					}
					return null;

				case SQLColumnType.Date:
					break;

				case SQLColumnType.Bit:
					try
					{
						return bool.Parse(colValue);
					}
					catch (Exception)
					{
					}
					return null;

				case SQLColumnType.TimeStamp:
					try
					{
						return double.Parse(colValue);
					}
					catch (Exception)
					{

						throw;
					}
				default:
					break;
			}
			return null;
		}
		private string GetPrimaryKeyName<T>(T entry) where T : new()
		{
			var cols = GetColumnsFrom<T>();

			foreach (var col in cols)
			{
				if (col.isPrimaryKey)
					return col.Name;
			}

			return "";
		}
		private string GetPrimaryKeyValue<T>(T entry) where T : new()
		{
			var cols = GetColumnsFrom<T>();

			foreach (var col in cols)
			{
				if (col.isPrimaryKey)
				{
					var result = typeof(T).GetProperty(col.Name).GetValue(entry);
					return result.ToString();
				}
			}
			return "";
		}
		#endregion
		private void LogMessage(string message)
		{
			var path = Environment.SpecialFolder.CommonApplicationData+"/eLongMuSQ";

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			try
			{
				var sw = new StreamWriter($"{path}\\queries_log.txt", true);
				var timestamp = DateTime.Now.ToString("[ dd/MM/yy HH:mm:ss ] ");
				sw.WriteLine($"{timestamp}{message}");
				sw.WriteLine();
				sw.Close();
			}
			catch (Exception ex)
			{
				// LOL 
				// LogMessage(ex);
			}
		}
		private SQLColumnAttribute GetSqlTableAtr(PropertyInfo prop)
		{
			var result = prop.GetCustomAttributes(true).Where(x => x.GetType() == typeof(SQLColumnAttribute)).ToList()[0] as SQLColumnAttribute;
			return result;
		}
		private bool IsSQLTableMarked(PropertyInfo prop)
		{
			var result = prop.CustomAttributes.Where(x => x.AttributeType == typeof(SQLColumnAttribute)).Any();
			return result;
		}
		private string CommaHandler(string query)
		{
			var last = query.Last();
			var result = query.Last() == '(' ? "" : ",";
			return result;
		}
		private string GetPrimaryKey(SQLColumnAttribute col)
		{
			return col.isPrimaryKey ? "PRIMARY KEY" : "";
		}
		private string GetColType(SQLColumnAttribute col)
		{
			switch (col.ColumnType)
			{
				case SQLColumnType.Int:
					return "int";
				case SQLColumnType.Float:
					return "float";
				case SQLColumnType.Varchar:
					return "varchar(MAX)";
				case SQLColumnType.Xml:
					return "xml";
				case SQLColumnType.Date:
					return "datetime";
				case SQLColumnType.BigInt:
					return "bigint";
				case SQLColumnType.Bit:
					return "bit";
				case SQLColumnType.TimeStamp:
					return "timestamp";

				default:
					return "";
			}
		}
		private string GetNotNULL(SQLColumnAttribute col)
		{
			return col.NotNULL ? " NOT NULL" : "";
		}
		private List<PropertyInfo> GetTableLinkProperties<T>()
        {
			return typeof(T).GetProperties().Where(_p => _p.GetCustomAttribute(typeof(SQLTableLinkAttribute)) != null).ToList();
        }
		#endregion
		private void CheckForTableLinks<T>(List<T> table) where T : new()
		{
			var linkedProperties = GetTableLinkProperties<T>();

			if (linkedProperties !=null)
            {
                foreach (var row in table)
                {
					foreach (var property in linkedProperties)
					{
						var _attr = property.GetCustomAttribute(typeof(SQLTableLinkAttribute)) as SQLTableLinkAttribute;
						var id1 = _attr.id1;
						var id2 = _attr.id2;
						var _type = _attr._TableType;

						var id1Property = row.GetType().GetProperty(id1);
						var id1Value = id1Property.GetValue(row, null);

						var method = GetType().GetMethods().FirstOrDefault(
							x => x.Name.Equals(nameof(GetTableWithCondition), StringComparison.OrdinalIgnoreCase) &&
							x.IsGenericMethod && x.GetParameters().Length == 1);

						MethodInfo generic = method.MakeGenericMethod(_type);
						var rez = generic.Invoke(this,new object[] { $"where {id2}='{id1Value}'" });

						property.SetValue(row, rez);
					}
                }
            }
		}
		private void CreateNewConnection()
		{
			if (MyConnection != null)
			{
				MyConnection.Close();
				MyConnection.Dispose();
			}

			MyConnection = new SqlConnection($"user id={Configuration.SQLUser};" +
								 $"password={Configuration.SQLPass};server={Configuration.SQLAddress};" +
								 "Trusted_Connection=no;" +
								 $"database={Configuration.SQLDataBase}; " +
								 "Connect Timeout=5;" +
								 "MultipleActiveResultSets=false");

			MyConnection.Open();
		}
		private string GetBulkMoveQuery<T>(List<SQLColumnAttribute> cols, string sourceTable, string destinationTable, string condition)
		{
			//	BEGIN TRANSACTION;
			//	INSERT INTO[Case_Info]
			//([Id]
			//	  ,[ID_Batch]
			//	  ,[ID_Case]
			//	  ,[LastState]
			//	  ,[CODE]
			//	  ,[DateTime])

			//SELECT
			//	   [Id]
			//      ,[ID_Batch]
			//      ,[ID_Case]
			//      ,[LastState]
			//      ,[CODE]
			//      ,[DateTime]
			//	FROM[Case_Info_History] WHERE ID_Batch = '20210303100755465';
			//	DELETE FROM[Case_Info_History] WHERE ID_Batch = '20210303100755465';
			//	COMMIT;
			Func<string, string> removedLast = (str) => str.Remove(str.Length - 1, 1);
			var result = $@"BEGIN TRANSACTION; INSERT INTO {destinationTable} (";
			var columns_str = "";
			foreach (var column in cols)
			{
				if (IsIgnoredColumn<T>(column)) continue;
				result += $"{column.Name},";
				columns_str += $"{column.Name},";
			}
			result = removedLast(result);
			columns_str = removedLast(columns_str);
			result += $")SELECT {columns_str} FROM {sourceTable} {condition};";
			result += "COMMIT;";

			return result;
		}
		private string GetDeleteSQLquery<T>(string TableName, T entry) where T : new()
		{
			var result = $"DELETE FROM {TableName} WHERE ";
			var primaryKname = GetPrimaryKeyName(entry);
			var primaryKvalue = GetPrimaryKeyValue(entry);
			result += $"{primaryKname}='{primaryKvalue}';";

			return result;
		}
		private string GetDeleteSQLquery<T>(string TableName, string OtherConditions, T entry) where T : new()
		{
			var result = $"DELETE FROM {TableName} WHERE ";
			var primaryKname = GetPrimaryKeyName(entry);
			var primaryKvalue = GetPrimaryKeyValue(entry);
			result += $"{primaryKname}='{primaryKvalue}' and {OtherConditions};";

			return result;
		}
		private string GetUpdateSQLquery<T>(string TableName, T entry) where T : new()
		{
			var result = $"UPDATE {TableName} SET (";
			var primaryKname = GetPrimaryKeyName(entry);
			var primaryKvalue = GetPrimaryKeyValue(entry);
			var cols = GetColumnsFrom<T>();

			foreach (var col in cols)
			{
				if (col.Name != primaryKname && !IsIgnoredColumn<T>(col))
				{
					var propvalue = typeof(T).GetProperty(col.Name).GetValue(entry);
					if (propvalue == null) continue;
					var value = typeof(T).GetProperty(col.Name).GetValue(entry).ToString();
					result += $"{CommaHandler(result)}{col.Name}='{value}'";
				}
			}
			result += $") WHERE {primaryKname}='{primaryKvalue}';";

			return result.Replace("(", "").Replace(")", "");
		}
		private bool HasPrimaryKey(object entry)
		{
			if (!IsTableMarked(entry.GetType()))
				return false;

			var columns = entry.GetType().GetProperties().Where(x => IsSQLTableMarked(x)).Select(x => GetSqlTableAtr(x)).ToList();

			//return columns.Any(x => x.isPrimaryKey);

			foreach (var col in columns)
			{
				if (col.isPrimaryKey)
					return true;
			}

			return false;
		}
		private string GetInsertSQLQuery<T>(string TableName, List<SQLColumnAttribute> col, T entry)
		{
			var result = $"INSERT INTO {TableName}(";

			foreach (var column in col)
			{
				if (IsIgnoredColumn<T>(column)) continue;
				result += $"{CommaHandler(result)}{column.Name}";
			}
			result += ") VALUES (";

			foreach (var column in col)
			{
				if (IsIgnoredColumn<T>(column)) continue;
				var value = typeof(T).GetProperty(column.Name).GetValue(entry);
				value = value == null ? "0" : value;
				result += $"{CommaHandler(result)}'{value.ToString()}'";
			}
			result += ");";
			return result;
		}
		private bool CheckTable(SqlCommand command)
		{
			lock (SQLLocker)
			{
				if (MyConnection.State != System.Data.ConnectionState.Open)
					try
					{
						//MyConnection.Open();
						CreateNewConnection();
					}
					catch (Exception ex)
					{
						LogMessage($"failed to open connection - connection state:{MyConnection.State.ToString()} - exception:{ex.ToString()}");
					}

				try
				{
					using (var SqlReader = command.ExecuteReader())
					{
						while (SqlReader.Read())
						{
							SqlReader.Close();
							return true;
						}
					}
				}
				catch (Exception)
				{
					return false;
				}

				return false;
				#region -- OLD --
				SqlDataReader myReader = null;
				try
				{
					//lock (SQLLocker)
					{
						myReader = command.ExecuteReader();
					}
				}
				catch (Exception ex)
				{
					LogMessage($"failed to execute sql reader - connection state:{MyConnection.State.ToString()} - query:{command.CommandText} - exception:{ex.ToString()}");
					//lock (SQLLocker)
					{
						try
						{
							//MyConnection.Close();
							//Thread.Sleep(200);
							//MyConnection.Open();
							CreateNewConnection();

							myReader = command.ExecuteReader();
						}
						catch (Exception ex2)
						{
							LogMessage($"failed to reopen the connection - connection state:{MyConnection.State.ToString()} - exception:{ex2.ToString()}");
							Console.WriteLine($"failed to execute sql command, probably not connected to db:{ex2.Message}");
						}
					}
				}
				try
				{
					while (myReader.Read())
					{
						return true;
					}
					myReader.Close();

				}
				catch (Exception ex)
				{
				}
				#endregion

			}
			return false;
		}
		private List<T> ExecuteCommandReader<T>(SqlCommand command) where T : new()
		{
			var result = new List<T>();
			ExecuteWithDebugInfo(() =>
			{
				lock (SQLLocker)
				{
					CheckConnection();

					try
					{
						using (var SqlReader = command.ExecuteReader())
						{
							var columnList = GetColumnsFrom<T>();
							try
							{
								while (SqlReader.Read())
								{
									var newT = new T();
									foreach (var col in columnList)
									{
										if (IsIgnoredColumn<T>(col)) continue;

										object value = GetValueOfProperty(col, SqlReader[col.Name].ToString()); /*myReader[col.Name].ToString()*/
										typeof(T).GetProperty(col.Name).SetValue(newT, value);
									}
									result.Add(newT);
								}
								SqlReader.Close();
							}
							catch (Exception ex)
							{
								LogMessage($"failed to parse retrieved information from query: {command.CommandText} ");
							}
						}

					}
					catch (Exception queryException)
					{
						LogMessage($"query failed:{command.CommandText} - connection status:{MyConnection.State.ToString()} - exception:{queryException.ToString()}");
						return;
					}
					return;
					#region -- OLD --
					SqlDataReader myReader = null;
					try
					{
						myReader = command.ExecuteReader();
					}
					catch (Exception ex)
					{
						LogMessage($"failed to execute command reader:{command.CommandText} - connection state:{MyConnection.State.ToString()} - exception: {ex.ToString()}");
						try
						{
							//MyConnection.Close();
							//Thread.Sleep(200);
							//MyConnection.Open();
							CreateNewConnection();

							myReader = command.ExecuteReader();
						}
						catch (Exception ex2)
						{
							LogMessage($"failed to execute SQL reader operation: {command.CommandText} - connection state:{MyConnection.State.ToString()} - {ex2.ToString()}");
							Console.WriteLine("failed to execute sql command, probably not connected to db");
						}
					}
					var columns = GetColumnsFrom<T>();
					try
					{
						while (myReader.Read())
						{
							var newT = new T();
							foreach (var col in columns)
							{
								if (IsIgnoredColumn<T>(col)) continue;

								object value = GetValueOfProperty(col, myReader[col.Name].ToString()); /*myReader[col.Name].ToString()*/
								typeof(T).GetProperty(col.Name).SetValue(newT, value);
							}
							result.Add(newT);
						}
						myReader.Close();
					}
					catch (Exception ex)
					{
						LogMessage($"failed to parse retrieved information from query: {command.CommandText} ");
					}
					#endregion
				}
			},
			$"ExecutingCommandReader:{command.CommandText}");

			return result;
		}
		private void CheckConnection()
		{
			if (MyConnection.State != System.Data.ConnectionState.Open)
				try
				{
					//MyConnection.Open();
					CreateNewConnection();
				}
				catch (Exception ex)
				{
					LogMessage($"failed to open connection - connection state:{MyConnection.State.ToString()}");
				}
		}
		private bool IsIgnoredColumn<T>(SQLColumnAttribute col)
		{
			var isIgnored = typeof(T).GetProperty(col.Name).GetCustomAttribute(typeof(SQLUsedOnlyInCreateTableAttribute)) != null;
			return isIgnored;
		}
		private void ExecuteCommandNonQuery(SqlCommand command)
		{
			ExecuteWithDebugInfo(() =>
			{
				lock (SQLLocker)
				{
					if (MyConnection.State != System.Data.ConnectionState.Open)
						try
						{
							//MyConnection.Open();
							CreateNewConnection();
						}
						catch (Exception ex)
						{
							LogMessage($"failed to connect to db - {ex.ToString()}");
							Console.WriteLine("failed to execute sql command, probably not connected to db");
						}

					try
					{
						//lock (SQLLocker)
						{
							command.ExecuteNonQuery();
						}
					}
					catch (Exception ex1)
					{
						LogMessage($"failed to execute nonquery - connection state:{MyConnection.State.ToString()} - command:{command.CommandText}");
						try
						{
							//lock (SQLLocker)
							{
								//MyConnection.Close();
								//MyConnection.Open();
								CreateNewConnection();
								command.ExecuteNonQuery();
							}
						}
						catch (Exception ex2)
						{
							LogMessage($"failed to execute SQL operation: {command.CommandText} - connection state:{MyConnection.State.ToString()} - exception :{ex2.ToString()}");
							//throw new Exception($"Failed to execute SQL operation: {ex2.Message}");
							Console.WriteLine($"failed to execute SQL operation: {ex2.Message}");
						}
						Console.WriteLine(ex1);
					}
				}
			}, $"ExecutingCommandNonQuery:{command.CommandText}");
		}
		private string GetCreateTableSQLQuery(string TableName, List<SQLColumnAttribute> columnsAtr)
		{
			var result = $"CREATE TABLE {TableName} (";

			foreach (var col in columnsAtr)
				result += $"{CommaHandler(result)}{col.Name} {GetColType(col)} {GetPrimaryKey(col)}{GetNotNULL(col)}";

			return result + ");";
		}
		private List<SQLColumnAttribute> GetColumnsFrom<T>() where T : new()
		{
			var result = typeof(T).GetProperties().Where(x => IsSQLTableMarked(x)).Select(x => GetSqlTableAtr(x)).ToList();
			return result;
		}
		private List<SQLColumnAttribute> GetColumnsFrom(Type ColumnType)
		{
			var result = ColumnType.GetProperties().Where(x => IsSQLTableMarked(x)).Select(x => GetSqlTableAtr(x)).ToList();
			return result;
		}
		private void FailedTableCreationHappened()
		{
			Console.WriteLine("failed to create table");
		}
		private bool IsTableMarked(Type TableObject)
		{
			return TableObject.CustomAttributes.Where(x => x.AttributeType == typeof(SQLTableAttribute)).Any();
		}
		private void ExecuteWithDebugInfo(Action _a, string logMsg)
		{
			if (Configuration.EnableDebugInfo)
			{
				try
				{
					var time_started = DateTime.Now;
					_a.Invoke();
					var time_ended = DateTime.Now;
					var elapsed = (time_ended - time_started).TotalMilliseconds;
					if (elapsed > 1000)
						LogMessage($"DEBUG_INFO: executed {logMsg} in {elapsed}ms");
				}
				catch (Exception ex)
				{
					LogMessage($"{logMsg} - {ex.ToString()}");
					throw ex;
				}
			}
			else
			{
				_a?.Invoke();
			}
		}
		#endregion
		#endregion
	}
}
